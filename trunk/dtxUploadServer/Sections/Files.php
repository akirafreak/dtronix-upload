<?php

/**
 * File manangement class.
 */
class Files extends SectionBase{
	
	/**
	 * @var array Contains all the queried directories this session. 
	 */
	private $directories;
	
	/**
	 * @var array Associative array that contains all permissions for a directory.
	 */
	private $directory_properties = array(
		"is_public" => "bool",
		"is_locked" => "bool",
		"password" => "string"
	);

	/**
	 * Outputs user's directory contents.
	 *
	 * @param string $url_id url_id of the directory to retrieve.
	 * @return mixed <code>
	 * directory_contents: Requested files (False on no files).
	 * </code>
	 *
	 * @todo Limit the ammount of files that can be queried this way.
	 */
	public function directoryFiles($url_id){
		$this->validateConnection();
		$owner_id = "";
		
		// Check to see if the user can modify any file.
		$validate = $this->getPermission("manage_uploads");
		if($validate->successful == false || $validate->data == false){
			$owner_id = "owner_id = '{$this->USER["id"]}' AND ";
		}

		$files = $this->SQL->fetchRows("SELECT file_name, upload_date, file_size, last_accessed, url_id, total_views
			FROM files
			WHERE {$owner_id} directory_url_id = '%s'", array(
				$url_id
			));
			
		// Remove the file place holder for the directory.
		for($i = 0; $i < count($files); $i++){
			if($files[$i]["file_size"] == 0){
				unset($files[$i]);
				break;
			}
		}
		
		if(is_assoc($files))
			$files = array_values($files);

		return new ReturnData(true, "directory_files", $files);
	}
	
	/**
	 * Outputs a list of all directories that the user has access to.
	 * 
	 * @return ReturnData <code>
	 * directory_list: JSON Directory list. (False on no directories)
	 * </code>
	 */
	public function directoryList(){
		$this->validateConnection();

		$directories = $this->SQL->fetchRows("SELECT id, url_id, name, is_public, is_locked
			FROM directories
			WHERE owner_id = '%s'", array(
				$this->USER["id"]
			));

		return new ReturnData(true, "directory_list", $directories);
	}
	
	/**
	 * Create a directory for files to be placed inside.
	 *
	 * @param type $dir_name Name of the new directory.
	 * @param array $properties Array that contains all properties to set for the folder.
	 * @return ReturnData <code>
	 * directory_invalid_name
	 * directory_too_long
	 * directory_create_confirmation: JSON directory information.
	 * directory_create_failure
	 * </code>
	 */
	public function directoryCreate($dir_name, $properties = null){
		$this->validateConnection();
		
		$validate = $this->validDirectoryName($dir_name);
		if($validate->successful != true)
			return $validate;
		
		$url_id = $this->createNewId();
		
		$new_dir_array = array(
			"owner_id" => $this->USER["id"],
			"url_id" => $url_id,
			"name" => $dir_name,
			"is_public" => 0,
			"is_locked" => 0
		);
		
		$dir_id = $this->SQL->insert("directories", $new_dir_array);
		
		if($properties != null){
			$set_properties = $this->directorySetProperties($url_id, $properties);
			if($set_properties->successful == false)
				return $set_properties;
		}
		
		$id = $this->SQL->insert("files", array(
			"owner_id" => $this->USER["id"],
			"url_id" => $url_id,
			"directory_url_id" => $url_id
		));

		if($id != false){
			return new ReturnData(true, "directory_create_confirmation", $new_dir_array);
		}else{
			return new ReturnData(false, "directory_create_failure");
		}
	}
	
	
	/**
	 * Delete a directory and optionally the contents too.
	 *
	 * @param type $url_id Url ID of the directory to delete.
	 * @return ReturnData <code>
	 * directory_delete_confirmation
	 * directory_delete_failure
	 * directory_delete_dne
	 * </code>
	 */
	public function directoryDelete($url_id){
		$this->validateConnection();
		$owner_id = "";
		
		// Check to see if the user can modify any file.
		$validate = $this->getPermission("manage_uploads");
		if($validate->successful == false || $validate->data == false){
			$owner_id = "owner_id = '{$this->USER["id"]}' AND ";
		}

		$directory = $this->SQL->fetchRow("SELECT * FROM directories 
			WHERE {$owner_id} url_id = '%s'", array(
			$url_id
		));
			
		if($directory == false)
			return new ReturnData(false, "directory_delete_dne");
		
		// Delete the directory placeholder.
		$this->SQL->query("DELETE FROM files WHERE url_id = '%s'", array($url_id));
		
		// Delete the actual directory.
		$this->SQL->query("DELETE FROM directories WHERE url_id = '%s'", array($url_id));
							
		$files = $this->SQL->fetchRowsEnum("SELECT url_id FROM files
			WHERE {$owner_id} directory_url_id = '%s'", array(
			$url_id
		));
		
		$moving_files = array();
		foreach($files as $file){
			$moving_files[] = $file[0];
		}

		// Check to see if there are any files that need to be moved back into the default directory.
		if(count($moving_files) > 0){
			$successful = $this->SQL->update("files", array(
				"directory_url_id" => "0"
			), "WHERE " . $owner_id . $this->SQL->whereIn("url_id", $moving_files));
			
			if($successful){
				return new ReturnData(true, "directory_delete_confirmation");
			}else{
				return new ReturnData(false, "directory_delete_failure");
			}
		}else{
			return new ReturnData(false, "directory_delete_failure");
		}

		
	}
	
	/**
	 * Method to set the proeprties on a user directory.
	 *
	 * @param string $url_id url_id of the directory to set properties on.
	 * @param array $properties associative array that contains all the properties to change on the directory.
	 * @return ReturnData <code>
	 * directory_set_properties_failure
	 * directory_set_properties_confirmation: $url_id
	 * </code>
	 */
	public function directorySetProperties($url_id, $properties){
		$this->validateConnection();
		$owner_id = "";
		
		if(!is_array($properties))
			return new ReturnData(false, "directory_set_properties_failure");
		
		$property_update = array();
		
			
		foreach($properties as $prop => $value){
			if(array_key_exists($prop, $this->directory_properties) == false)
				continue;
			
			if($this->directory_properties[$prop] == "bool"){
				if($value == true){
					$property_update[$prop] =1;
					
				}else{
					$property_update[$prop] = 0;
				}
				
			}else if($this->directory_properties[$prop] == "string"){
				$property_update[$prop] = (string)$value;
				
			}else if($this->directory_properties[$property] == "int"){
				$property_update[$prop] = (int)$value;
			}
		}
		
		if(count($property_update) == 0)
			return new ReturnData(false, "directory_set_properties_failure");
		
		// Check to see if the user can modify any file.
		$validate = $this->getPermission("manage_uploads");
		if($validate->successful == false || $validate->data == false){
			$owner_id = "owner_id = '{$this->USER["id"]}' AND ";
		}
		
		$successful = $this->SQL->update("directories", $property_update, 
		"WHERE {$owner_id} url_id = '%s'", array(
			$url_id
		));
		
		if($successful === true){
			return new ReturnData(true, "directory_set_properties_confirmation");
		}else{
			return new ReturnData(false, "directory_set_properties_failure");
		}
	}
	
	
	/**
	 * Gets all the associated properties with a url_id
	 *
	 * @param string $url_id url_id of the directory to set properties on.
	 * @param mixed $properties Property or an array of properties to retrieve on a directory.
	 * @return ReturnData <code>
	 * directory_get_properties_failure
	 * directory_get_properties_failure_invalid_properties
	 * directory_get_properties_confirmation
	 * </code>
	 */
	public function directoryGetProperties($url_id, $properties){
		$this->validateConnection();
		
		if(!is_array($properties) && is_string($properties))
			$properties = array($properties);
		
		if(array_key_exists($url_id, $this->directories) == false){

			// Check to see if the user can modify any file.
			$validate = $this->getPermission("manage_uploads");
			if($validate->successful == false || $validate->data == false){
				$owner_id = "owner_id = '{$this->USER["id"]}' AND ";
			}

			$directory = $this->SQL->fetchRow("SELECT * FROM directories 
				WHERE {$owner_id} url_id = '%s'", array(
				$url_id
			));

			if($directory == false)
				return new ReturnData(false, "directory_get_properties_failure");

			$this->directories[$url_id] = $directory;
		}
		$dir = $this->directories[$url_id];
		$return_props = array();
		
		foreach($properties as $property){
			if(array_key_exists($property, $this->directory_properties) == false)
				continue;
			
			if($this->directory_properties[$property] == "bool"){
				if($dir[$property] == 1){
					$return_props[$property] = true;
					
				}else{
					$return_props[$property] = false;
				}
				
			}else if($this->directory_properties[$property] == "string"){
				$return_props[$property] = $dir[$property];
				
			}else if($this->directory_properties[$property] == "int"){
				$return_props[$property] = (int)$dir[$property];
			}
		}

		if(count($return_props) == 0){
			return new ReturnData(false, "directory_get_properties_failure_invalid_properties");
		}else{
			return new ReturnData(true, "directory_get_properties_confirmation", $return_props);
		}
	}
	

	/**
	 * Method to rename an existing directory
	 *
	 * @param string $url_id Directory url_id to rename.
	 * @param string $new_name New name of the directory.
	 * @return ReturnData <code>
	 * directory_invalid_name
	 * directory_too_long
	 * directory_rename_confirmation: array($directory, $new_directory)
	 * directory_rename_failure
	 * </code>
	 */
	public function directoryRename($url_id, $new_name){
		$this->validateConnection();
		$owner_id = "";

		$validate = $this->validDirectoryName($new_name);
		if($validate->successful !== true)
			return $validate;

		// Check to see if the user can modify any file.
		$validate = $this->getPermission("manage_uploads");
		if($validate->successful == false || $validate->data == false){
			$owner_id = "owner_id = '{$this->USER["id"]}' AND";
		}

		$successful = $this->SQL->update("directories", array(
			"name" => $new_name
		), "WHERE {$owner_id} url_id = '%s'", array(
			$url_id
		));

		if($successful){
			return new ReturnData(true, "directory_rename_confirmation", array($url_id, $new_name));
		}else{
			return new ReturnData(false, "directory_rename_failure");
		}
	}

	/**
	 * Method to move files into other directories.
	 *
	 * @param mixed $url_id url_id or array of url_ids of the file(s) to move into a new directory.
	 * @param string $dir_url_id Directory url_id to move the files inside.
	 *
	 * @return ReturnData <code>
	 * 
	 * file_move_confirmation: array($url_ids, new directory name).
	 * file_move_failure
	 * directory_invalid_name
	 * directory_too_long
	 * directory_dne: $dir_url_id
	 * directory_not_owner
	 * </code>
	 */
	public function move($url_ids, $dir_url_id){
		$this->validateConnection();
		$owner_id = "";

		if(is_array($url_ids) && count($url_ids) == 0)
			return new ReturnData(false, "file_move_failure");
		
		$valid_dir_name = $this->validDirectoryName($dir_url_id);
		if($valid_dir_name->successful == false)
			return $valid_dir_name;
		
		// Check to see if the user can modify any file.
		$manage_uploads = $this->getPermission("manage_uploads");
		if($manage_uploads->successful == false || $manage_uploads->data == false){
			$owner_id = "owner_id = '{$this->USER["id"]}' AND";
		}
		
		// Check to see if the files are being moved into the uncategorized dir.
		if($dir_url_id !== "0"){
			$direcory = $this->SQL->fetchRow("SELECT * FROM directories WHERE url_id = '%s'", array(
				$dir_url_id
			));

			if($direcory == false)
				return new ReturnData(false, "directory_dne", $dir_url_id);

			if($direcory["owner_id"] != $this->USER["id"] && $owner_id != "")
				return new ReturnData(false, "directory_not_owner");
		}

		$successful = $this->SQL->update("files", array(
			"directory_url_id" => $dir_url_id
		), "WHERE ". $owner_id . $this->SQL->whereIn("url_id", $url_ids));

		if($successful){
			return new ReturnData(true, "file_move_confirmation", array($url_ids, $dir_url_id));
		}else{
			return new ReturnData(false, "file_move_failure");
		}
	}

	/**
	 * Returnes the fill file info about the queried file.
	 *
	 * @param string $url_id Id of the file to be queried.
	 * 
	 * @return ReturnData <code>
	 * file_info: All file information, False on no info.
	 * </code>
	 */
	public function info($url_id){
		$this->validateConnection();

		$file = $this->SQL->fetchRow("SELECT file_name, upload_date, file_size, last_accessed, url_id
			FROM files
			WHERE owner_id = '%s'
			AND url_id = '%s'
			LIMIT 1;", array(
				$this->USER["id"],
				$url_id
			));

		return new ReturnData(true, "file_info", $file);
	}

	/**
	 * List most recent 60 uncategorized files.
	 *
	 * @param int $start Page to start listing the files on.
	 *
	 * @return ReturnData <code>
	 * files_uploaded_quick: All file information
	 * files_uploaded_no_files: No files exist in the default upload directory.
	 * </code>
	 */
	public function listFiles($start){
		$this->validateConnection();

		// The total pages into the list to go.
		$files_per_page = 60;

		$files = $this->SQL->fetchRows("SELECT url_id, upload_date, file_name, file_size
			FROM files
			WHERE owner_id = '%s' AND is_visible = '1'
			ORDER BY upload_date DESC
			LIMIT %s , %s", array(
				$this->USER["id"],
				$start * $files_per_page,
				($start * $files_per_page) + $files_per_page
			));

		if($files != false){
			return new ReturnData(true, "files_uploaded_quick", $files);
		}else{
			return new ReturnData(false, "files_uploaded_no_files");
		}
	}

	/**
	 * Method to delete user uploaded files.
	 *
	 * @param array $url_ids Array of url IDs of the files to delete.
	 *
	 * @return ReturnData <code>
	 * file_delete_failure_owner
	 * file_delete_failure
	 * file_delete_confirmation
	 * </code>
	 */
	public function delete($url_ids){
		$this->validateConnection();
		
		$build_inner_query = "";
		$query_array = array();

		// Check for file ownership
		$owner_files = $this->isOwner($url_ids);
		
		if($owner_files == false)
			return new ReturnData(false, "file_delete_failure");

		// Start building the query to delete all rows associated with the file.
		foreach($owner_files as $url_id){
			if(file_exists($this->CONFIG["upload_dir"] . $url_id)){
				if(unlink($this->CONFIG["upload_dir"] . $url_id) == false)
					return new ReturnData(false, "file_delete_failure");
			}else{
				// The file does not exist... How did this happen?
			}
		}

		$deleted = $this->SQL->successful("DELETE FROM files
			WHERE ". $this->SQL->whereIn("url_id", $owner_files),
			$query_array);
		
		// Get the user's current file info.
		$update_user_info = $this->SQL->fetchResult("SELECT sum(file_size), count(file_size)
			FROM files
			WHERE owner_id = '%s'",
			array(
				$this->USER["id"]
			));

		// Ensure that the query successfully deleted the associated rows.
		if(!$deleted)
			return new ReturnData(false, "file_delete_failure");

		// Update the user's stats.
		$this->SQL->update("users", array(
			"total_uploaded_filesizes" => $update_user_info[0],
			"total_files_uploaded" => $update_user_info[1]),
			"WHERE id = '%s'", array(
				$this->USER["id"]
			));
		
		return new ReturnData(true, "file_delete_confirmation");
	}

	/**
	 * Method to upload files from the client to the server.
	 *
	 * @tutorial
	 * <code>
	 * upload_failed_exceeded_toal_used_space: User has exceeded the maximul alloted space on the server.
	 * upload_failed_could_not_handle_file: File was uploaded, but culd not be moved from the temp folder to the designated folder.
	 * upload_failed_exceeded_file_size: File exceeds the maximum uploadable file size.
	 * upload_successful: Upload completed successfully.
	 * </code>
	 */
	public function upload(){
		$this->validateConnection();
		
		//callClientMethod("error", $_FILES);
		// Check to see if this file exceeds the maximum size alotted.
		$total_used_space = $this->USER["total_uploaded_filesizes"] + $_FILES["file"]["size"];
		$max_upload_space = $this->getPermission("max_upload_space");
		if($max_upload_space->successful && $total_used_space > $max_upload_space->data)
			return new ReturnData(false, "upload_failed_exceeded_toal_used_space", array($total_used_space, $max_upload_space->data));
			
		$max_upload_size = $this->getPermission("max_upload_size");
		if($max_upload_size->successful && $_FILES["file"]["size"] > $max_upload_size->data)
			return new ReturnData(false, "upload_failed_exceeded_file_size");

		$url_id = $this->createNewId();
		
		if(!move_uploaded_file($_FILES["file"]["tmp_name"], $this->CONFIG["upload_dir"] . $url_id))
			return new ReturnData(false, "upload_failed_could_not_handle_file");

		$file_parts = explode(".", $_FILES["file"]["name"]);
		$file_extention = strtolower($file_parts[count($file_parts) - 1]);
		$file_mime = (array_key_exists($file_extention, $this->mime_types))? $this->mime_types[$file_extention] : $_FILES["file"]["type"];

		$insert_id = $this->SQL->insert("files", array(
			"owner_id" => $this->USER["id"],
			"url_id" => $url_id,
			"upload_date" => time(),
			"is_public" => 1,
			"is_public" => 1,
			"is_visible" => 1,
			"is_disabled" => 0,
			"file_status" => 2,
			"is_encrypted" => 0,
			"file_name" => $_FILES["file"]["name"],
			"file_size" => $_FILES["file"]["size"],
			"file_mime" => $file_mime
		));

		if($insert_id === false)
			return new ReturnData(false, "upload_failed_db_error");

		$this->SQL->update("users", array(
			"total_uploaded_filesizes" => $total_used_space,
			"total_files_uploaded" => $this->USER["total_files_uploaded"] + 1
		), "WHERE id = '%s'", array(
			$this->USER["id"]
		));
		
		return new ReturnData(true, "upload_successful", array(
			"url_id" => $url_id,
			"is_visible" => true,
			"file_status" => 2
		));
	}

	/**
	 * Method called every time somebody views an uploaded file.
	 *
	 * Handles multi-part downloads.
	 *
	 * @param string $url_id Url id of the file to view.
	 */
	public function view($url_id){

		$file = $this->SQL->fetchRow("SELECT * FROM files
			WHERE url_id = '%s'
			LIMIT 1", array($url_id));

		// Determine if the file exists in the database.
		if($file == false)
			die("File does not exist.");

		// Update the stats for the file.
		$this->SQL->update("files", array(
			"last_accessed" => time(),
			"total_views" => $file["total_views"] + 1
		), "WHERE id = '%s'", array(
			$file["id"]
		));

		// Determine if the file exists on the file system.
		if(!file_exists($this->CONFIG["upload_dir"] . $file["url_id"]))
			die("File does not exist.");

		// Set the mime type.
		header("Content-Type: ". $file["file_mime"]);

		// Make sure that the file is not one that should be viewed in the browser.
		if(strpos($file["file_mime"], "image") === false && strpos($file["file_mime"], "text/plain") === false){
			header("Content-Transfer-Encoding: binary");

			$this->outputFile($this->CONFIG["upload_dir"] . $url_id, basename($file["file_name"]), $file["file_mime"], true);
			die();
		}

		$etag = md5($file["upload_date"] + $file["file_size"] + $file["file_name"]);
		
		// Determine if the client's cache is up to date.
		if(isset($_SERVER['HTTP_IF_NONE_MATCH']) && $etag == $_SERVER['HTTP_IF_NONE_MATCH']){
			header("HTTP/1.1 304 Not Modified");
			die();
		}

		// Set caching variables.
		$expires = 60 * 60 * 24 * 360;
		$exp_gmt = gmdate("D, d M Y H:i:s", time() + $expires )." GMT";
		$mod_gmt = gmdate("D, d M Y H:i:s", strtotime($file["upload_date"])) ." GMT";
		header("Expires: ". $exp_gmt);
		header("Last-Modified: ". $mod_gmt);
		header("Cache-Control: public, max-age=". $expires);
		header("ETag: ". substr($etag, 16));


		$this->outputFile($this->CONFIG["upload_dir"] . $url_id, basename($file["file_name"]), $file["file_mime"], false);
		die();

	}

	/**
	 * Internal method to deal with multi-part file downloads.
	 *
	 * @param string $file Uploaded file location.
	 * @param string $uploaded_file_name Original uploaded file name.
	 * @param string $mime File mime type.
	 * @param bool $force_download make the user download the file vs just being able to view the file in the browser.
	 */
	private function outputFile($file, $uploaded_file_name, $mime, $force_download = false){
		$size = filesize($file);
		//workaround for IE filename bug with multiple periods / multiple dots in filename
		//that adds square brackets to filename - eg. setup.abc.exe becomes setup[1].abc.exe
		if(strstr($_SERVER['HTTP_USER_AGENT'], 'MSIE')){
			$filename = preg_replace('/\./', '%2e', $uploaded_file_name, substr_count($uploaded_file_name, '.') - 1);
		}else{
			$filename = $uploaded_file_name;
		}

		if(!empty($_SERVER['HTTP_RANGE'])){
			$used_range = $_SERVER['HTTP_RANGE'];
		}else if(!empty($_SERVER['RANGE'])){
			$used_range = $_SERVER['RANGE'];
		}

		//check if http_range is sent by browser (or download manager)
		if(isset($used_range)){
			list($size_unit, $range_orig) = explode('=', $used_range, 2);

			if ($size_unit == 'bytes'){
				//multiple ranges could be specified at the same time, but for simplicity only serve the first range
				//http://tools.ietf.org/id/draft-ietf-http-range-retrieval-00.txt
				$all_ranges = explode(",", $range_orig, 2);
				$range = $all_ranges[0];
			}
		}

		if(!isset($range)){
			$seek_end = $seek_start = null;
		}else{
			list($seek_start, $seek_end) = explode('-', $range, 2);
		}

		//set start and end based on range (if set), else set defaults
		//also check for invalid ranges.
		$seek_end = (empty($seek_end))? ($size - 1) : min(abs(intval($seek_end)), ($size - 1));
		$seek_start = (empty($seek_start) || $seek_end < abs(intval($seek_start))) ? 0 : max(abs(intval($seek_start)), 0);

		//Only send partial content header if downloading a piece of the file (IE workaround)
		if ($seek_start > 0 || $seek_end < ($size - 1)){
			header('HTTP/1.1 206 Partial Content');
		}

		header('Accept-Ranges: bytes');
		header('Content-Range: bytes '.$seek_start.'-'.$seek_end.'/'.$size);

		header('Content-Type: ' . $mime);

		if($force_download){
			header('Content-Disposition: attachment; filename="' . $filename . '"');
		}else{
			header('Content-Disposition: inline; filename="' . $filename . '"');
		}

		set_time_limit(0);
		$fp = fopen($file, 'rb');
		fseek($fp, $seek_start);

		while(!feof($fp)){
			echo fread($fp, 1024*8);
			ob_flush();
		}

		fclose($fp);
	}

	/**
	 * Check to see if the input items are owned by the current user.
	 *
	 * @property mixed $file_url_ids array(string, [...]) of file's url_id to verify ownership of.
	 * @return mixed Enumerated array with the owned files, false on falure to own any.
	 */
	private function isOwner($file_url_ids){
		$this->validateConnection();
		$owner_check = array();
		
		if(is_string($file_url_ids) || is_int($file_url_ids))
			$file_url_ids = array($file_url_ids);

		$result = $this->SQL->fetchRows("SELECT url_id
			FROM files
			WHERE owner_id = '%s' AND ". $this->SQL->whereIn("url_id", $file_url_ids), array(
			$this->USER["id"]
		));
		
		

		foreach($result as $owner_file){
			$owner_checks[] = $owner_file["url_id"];
		}
		if(count($owner_checks) == 0){
			return false;
		}else{
			return $owner_checks;
		}
	}
	
	/**
	 * Internal method to deal with directory limits.
	 * 
	 * @param string $directory Full new directory name.
	 *
	 * @return ReturnData <code>
	 * directory_invalid_name
	 * directory_too_long
	 * True on success.
	 * </code>
	 */
	private function validDirectoryName($directory){
		$this->validateConnection();
		
		if(preg_match("/[^A-Za-z0-9\s_-]/", $directory) == true)
			return new ReturnData(false, "directory_invalid_name");

		if(strlen($directory) > 128)
			return new ReturnData(false, "directory_too_long");
		
		return new ReturnData(true);
	}

	/**
	 * Generate a new unused url_id for a uploaded file.
	 * 
	 * @return string New incremented value.
	 *
	 * @todo Modify method to account for multiple simultanious uploads.  Currently it can generate the same ID for two files if they are uploaded at the same time.
	 */
	private function createNewId(){
		$last_entry = $this->SQL->fetchRow("SELECT url_id
			FROM files
			ORDER BY id DESC
			LIMIT 1;");

		if($last_entry == false) return "1";

		$last_id = $last_entry["url_id"];

		return $this->incrementID($last_id);
	}

	/**
	 * Internal method to increment a new url_id.
	 *
	 * @param string $input Current url_id to increment.
	 * @param int $curr_index internal index for keeping track of the current character to increnet.
	 * @return string New incremented url_id.
	 */
	private function incrementID($input, $curr_index = null){
		$morpher = array("1" => "2",
			"2" => "3", "3" => "4", "4" => "5", "5" => "6",
			"6" => "7", "7" => "8", "8" => "9", "9" => "0",
			"0" => "a", "a" => "b", "b" => "c", "c" => "d", 
			"d" => "e", "e" => "f", "f" => "g", "g" => "h",
			"h" => "i", "i" => "j", "j" => "k", "k" => "l",
			"l" => "m", "m" => "n", "n" => "o", "o" => "p",
			"p" => "q", "q" => "r", "r" => "s", "s" => "t",
			"t" => "u", "u" => "v", "v" => "w", "w" => "x",
			"x" => "y", "y" => "z", "z" => "A", "A" => "B",
			"B" => "C", "C" => "D", "D" => "E", "E" => "F",
			"F" => "G", "G" => "H", "H" => "I", "I" => "J",
			"J" => "K", "K" => "L", "L" => "M", "M" => "N",
			"N" => "O", "O" => "P", "P" => "Q", "Q" => "R",
			"R" => "S", "S" => "T", "T" => "U", "U" => "V",
			"V" => "W", "W" => "X", "X" => "Y", "Y" => "Z",
			"Z" => "1");

		if($curr_index === null){
			$curr_index = strlen($input) - 1;
		}

		if($curr_index < 0){
			return "1" . $input;
		}

		$input[$curr_index] = $morpher[$input[$curr_index]];
		if($input[$curr_index] == "1"){
			return $this->incrementID($input, $curr_index - 1);
		}

		return $input;
	}

	/**
	 * @var mixed Lots of common mime types.
	 */
	private $mime_types = array(
	"323" => "text/h323",
	"7z" => "application/x-7z-compressed",
	"acx" => "application/internet-property-stream",
	"ai" => "application/postscript",
	"aif" => "audio/x-aiff",
	"aifc" => "audio/x-aiff",
	"aiff" => "audio/x-aiff",
	"asf" => "video/x-ms-asf",
	"asr" => "video/x-ms-asf",
	"asx" => "video/x-ms-asf",
	"au" => "audio/basic",
	"avi" => "video/x-msvideo",
	"axs" => "application/olescript",
	"bas" => "text/plain",
	"bcpio" => "application/x-bcpio",
	"bin" => "application/octet-stream",
	"bmp" => "image/bmp",
	"c" => "text/plain",
	"cat" => "application/vnd.ms-pkiseccat",
	"cdf" => "application/x-cdf",
	"cer" => "application/x-x509-ca-cert",
	"class" => "application/octet-stream",
	"clp" => "application/x-msclip",
	"cmx" => "image/x-cmx",
	"cod" => "image/cis-cod",
	"cpio" => "application/x-cpio",
	"crd" => "application/x-mscardfile",
	"crl" => "application/pkix-crl",
	"crt" => "application/x-x509-ca-cert",
	"csh" => "application/x-csh",
	"css" => "text/css",
	"dcr" => "application/x-director",
	"der" => "application/x-x509-ca-cert",
	"dir" => "application/x-director",
	"dll" => "application/x-msdownload",
	"dms" => "application/octet-stream",
	"doc" => "application/msword",
	"dot" => "application/msword",
	"dvi" => "application/x-dvi",
	"dxr" => "application/x-director",
	"eps" => "application/postscript",
	"etx" => "text/x-setext",
	"evy" => "application/envoy",
	"exe" => "application/octet-stream",
	"fif" => "application/fractals",
	"flr" => "x-world/x-vrml",
	"gif" => "image/gif",
	"gtar" => "application/x-gtar",
	"gz" => "application/x-gzip",
	"h" => "text/plain",
	"hdf" => "application/x-hdf",
	"hlp" => "application/winhlp",
	"hqx" => "application/mac-binhex40",
	"hta" => "application/hta",
	"htc" => "text/x-component",
	"htm" => "text/html",
	"html" => "text/html",
	"htt" => "text/webviewhtml",
	"ico" => "image/x-icon",
	"ief" => "image/ief",
	"iii" => "application/x-iphone",
	"ins" => "application/x-internet-signup",
	"isp" => "application/x-internet-signup",
	"jfif" => "image/pipeg",
	"jpe" => "image/jpeg",
	"jpeg" => "image/jpeg",
	"jpg" => "image/jpeg",
	"js" => "application/x-javascript",
	"latex" => "application/x-latex",
	"lha" => "application/octet-stream",
	"lsf" => "video/x-la-asf",
	"lsx" => "video/x-la-asf",
	"lzh" => "application/octet-stream",
	"m13" => "application/x-msmediaview",
	"m14" => "application/x-msmediaview",
	"m3u" => "audio/x-mpegurl",
	"man" => "application/x-troff-man",
	"mdb" => "application/x-msaccess",
	"me" => "application/x-troff-me",
	"mht" => "message/rfc822",
	"mhtml" => "message/rfc822",
	"mid" => "audio/mid",
	"mny" => "application/x-msmoney",
	"mov" => "video/quicktime",
	"movie" => "video/x-sgi-movie",
	"mp2" => "video/mpeg",
	"mp3" => "audio/mpeg",
	"mpa" => "video/mpeg",
	"mpe" => "video/mpeg",
	"mpeg" => "video/mpeg",
	"mpg" => "video/mpeg",
	"mpp" => "application/vnd.ms-project",
	"mpv2" => "video/mpeg",
	"ms" => "application/x-troff-ms",
	"mvb" => "application/x-msmediaview",
	"nws" => "message/rfc822",
	"oda" => "application/oda",
	"p10" => "application/pkcs10",
	"p12" => "application/x-pkcs12",
	"p7b" => "application/x-pkcs7-certificates",
	"p7c" => "application/x-pkcs7-mime",
	"p7m" => "application/x-pkcs7-mime",
	"p7r" => "application/x-pkcs7-certreqresp",
	"p7s" => "application/x-pkcs7-signature",
	"pbm" => "image/x-portable-bitmap",
	"pdf" => "application/pdf",
	"pfx" => "application/x-pkcs12",
	"pgm" => "image/x-portable-graymap",
	"pko" => "application/ynd.ms-pkipko",
	"pma" => "application/x-perfmon",
	"pmc" => "application/x-perfmon",
	"pml" => "application/x-perfmon",
	"pmr" => "application/x-perfmon",
	"pmw" => "application/x-perfmon",
	"png" => "image/png",
	"pnm" => "image/x-portable-anymap",
	"pot" => "application/vnd.ms-powerpoint",
	"ppm" => "image/x-portable-pixmap",
	"pps" => "application/vnd.ms-powerpoint",
	"ppt" => "application/vnd.ms-powerpoint",
	"prf" => "application/pics-rules",
	"ps" => "application/postscript",
	"pub" => "application/x-mspublisher",
	"qt" => "video/quicktime",
	"ra" => "audio/x-pn-realaudio",
	"ram" => "audio/x-pn-realaudio",
	"ras" => "image/x-cmu-raster",
	"rgb" => "image/x-rgb",
	"rmi" => "audio/mid",
	"roff" => "application/x-troff",
	"rtf" => "application/rtf",
	"rtx" => "text/richtext",
	"scd" => "application/x-msschedule",
	"sct" => "text/scriptlet",
	"setpay" => "application/set-payment-initiation",
	"setreg" => "application/set-registration-initiation",
	"sh" => "application/x-sh",
	"shar" => "application/x-shar",
	"sit" => "application/x-stuffit",
	"snd" => "audio/basic",
	"spc" => "application/x-pkcs7-certificates",
	"spl" => "application/futuresplash",
	"src" => "application/x-wais-source",
	"sst" => "application/vnd.ms-pkicertstore",
	"stl" => "application/vnd.ms-pkistl",
	"stm" => "text/html",
	"svg" => "image/svg+xml",
	"sv4cpio" => "application/x-sv4cpio",
	"sv4crc" => "application/x-sv4crc",
	"t" => "application/x-troff",
	"tar" => "application/x-tar",
	"tcl" => "application/x-tcl",
	"tex" => "application/x-tex",
	"texi" => "application/x-texinfo",
	"texinfo" => "application/x-texinfo",
	"tgz" => "application/x-compressed",
	"tif" => "image/tiff",
	"tiff" => "image/tiff",
	"tr" => "application/x-troff",
	"trm" => "application/x-msterminal",
	"tsv" => "text/tab-separated-values",
	"txt" => "text/plain",
	"uls" => "text/iuls",
	"ustar" => "application/x-ustar",
	"vcf" => "text/x-vcard",
	"vrml" => "x-world/x-vrml",
	"wav" => "audio/x-wav",
	"wcm" => "application/vnd.ms-works",
	"wdb" => "application/vnd.ms-works",
	"wks" => "application/vnd.ms-works",
	"wmf" => "application/x-msmetafile",
	"wps" => "application/vnd.ms-works",
	"wri" => "application/x-mswrite",
	"wrl" => "x-world/x-vrml",
	"wrz" => "x-world/x-vrml",
	"xaf" => "x-world/x-vrml",
	"xbm" => "image/x-xbitmap",
	"xla" => "application/vnd.ms-excel",
	"xlc" => "application/vnd.ms-excel",
	"xlm" => "application/vnd.ms-excel",
	"xls" => "application/vnd.ms-excel",
	"xlt" => "application/vnd.ms-excel",
	"xlw" => "application/vnd.ms-excel",
	"xof" => "x-world/x-vrml",
	"xpm" => "image/x-xpixmap",
	"xwd" => "image/x-xwindowdump",
	"z" => "application/x-compress",
	"zip" => "application/zip");

}

?>
