<?php

include_once("./functions.php");

function _fileExist(){
	global $_USER, $_CONFIG;
	
	list($upload_id) = $_GET["args"];

	// TODO: Add any checks to see if the file is visible?
	return file_exists($_CONFIG["upload_dir"] . $upload_id);
}

function _filesInDirectory(){
	global $_USER, $_CONFIG;

	list($directory) = $_GET["args"];

	$files = mysqlQuery("SELECT `file_name`, `upload_date`, `file_size`, `last_accessed`, `url_id`
		FROM `files`
		WHERE `owner_id` = '%s'
		AND `directory` = '%s'
		LIMIT 0 , 30", array(
			$_USER["id"],
			$directory
		));

	if(!empty($files)){
		callClientMethod("directory_contents", $files);
	}else{
		callClientMethod("directory_contents", array(array()));
	}
}

function _fileInfo(){
	global $_USER, $_CONFIG;

	list($url_id) = $_GET["args"];

	$file = mysqlQuery("SELECT `file_name`, `upload_date`, `file_size`, `last_accessed`, `url_id`
		FROM `files`
		WHERE `owner_id` = '%s'
		AND `url_id` LIKE BINARY '%s'
		LIMIT 1", array(
			$_USER["id"],
			$url_id
		));

	if(!empty($files)){
		callClientMethod("file_info", $file);
	}else{
		callClientMethod("file_info", array());
	}
}



function _filesUploadedQuick(){
	global $_USER, $_CONFIG;

	// The total pages into the list to go.
	$start = $_GET["page"];
	$files_per_page = 60;

	$files = mysqlQuery("SELECT `url_id`, `upload_date`, `file_name`
		FROM `files`
		WHERE `owner_id` = '%s' AND `is_visible` = '1'
		ORDER BY `upload_date` DESC
		LIMIT %s , %s", array(
			$_USER["id"],
			$start * $files_per_page,
			($start * $files_per_page) + $files_per_page
		));

	if(count($files) > 0){
		callClientMethod("files_uploaded_quick", $files);

	}else{
		callClientMethod("files_uploaded_no_files");
	}
}

function _fileDelete(){
	global $_USER, $_CONFIG;

	$delete_id = $_GET["file_id"];

	// Check for file ownership
	if(!isOwner($delete_id)){
		callClientMethod("file_delete_failure_owner");
	}

	$file = mysqlQuery("SELECT `file_size` FROM `files`
		WHERE `url_id` LIKE BINARY '%s'
		LIMIT 1;",
	array(
		$delete_id
	));

	$deleted = mysqlQuery("DELETE FROM `files` 
		WHERE `url_id` LIKE BINARY '%s'
		LIMIT 1 ",
	array(
		$delete_id
	), "successful");

	if(file_exists($_CONFIG["upload_dir"] . $delete_id)){
		if(unlink($_CONFIG["upload_dir"] . $delete_id)){
			if($deleted){
				$toal_file_sizes = $_USER["total_uploaded_filesizes"] - $file[0]["file_size"];
				myUpdate("users", array(
					"total_uploaded_filesizes" => ($toal_file_sizes < 0)? 0 : $toal_file_sizes,
					"total_files_uploaded" => ($_USER["total_files_uploaded"] < 1)? 0 : $_USER["total_files_uploaded"] - 1
				), "`id` = '". $_USER["id"] ."'");

				callClientMethod("file_delete_confirmation");

			}else{
				callClientMethod("file_delete_failure");
			}

		}else{
			callClientMethod("file_delete_failure");
		}

	}else{
		callClientMethod("file_delete_failure");
	}
}

function isOwner($file_id){
	global $_USER;

	$can_delete = mysqlQuery("SELECT `id`
		FROM `files`
		WHERE `owner_id` = '%s'
		AND `url_id` LIKE BINARY '%s'
		LIMIT 1 ;",
	array(
		$_USER["id"],
		$file_id
	), "has_rows");
	
	if($can_delete){
		return true;
	}else{
		return false;
	}
}


function createNewId(){
	$last_entry = mysqlQuery("SELECT `url_id`
		FROM `files`
		ORDER BY `id` DESC
		LIMIT 1");

	if(empty($last_entry[0])) return "1";

	$last_id = $last_entry[0]["url_id"];

	return incrementID($last_id);
}


function incrementID($input, $curr_index = null){
	$morpher = array("1" => "2",
		"2" => "3",
		"3" => "4",
		"4" => "5",
		"5" => "6",
		"6" => "7",
		"7" => "8",
		"8" => "9",
		"9" => "0",
		"0" => "a",
		"a" => "b",
		"b" => "c",
		"c" => "d",
		"d" => "e",
		"e" => "f",
		"f" => "g",
		"g" => "h",
		"h" => "i",
		"i" => "j",
		"j" => "k",
		"k" => "l",
		"l" => "m",
		"m" => "n",
		"n" => "o",
		"o" => "p",
		"p" => "q",
		"q" => "r",
		"r" => "s",
		"s" => "t",
		"t" => "u",
		"u" => "v",
		"v" => "w",
		"w" => "x",
		"x" => "y",
		"y" => "z",
		"z" => "A",
		"A" => "B",
		"B" => "C",
		"C" => "D",
		"D" => "E",
		"E" => "F",
		"F" => "G",
		"G" => "H",
		"H" => "I",
		"I" => "J",
		"J" => "K",
		"K" => "L",
		"L" => "M",
		"M" => "N",
		"N" => "O",
		"O" => "P",
		"P" => "Q",
		"Q" => "R",
		"R" => "S",
		"S" => "T",
		"T" => "U",
		"U" => "V",
		"V" => "W",
		"W" => "X",
		"X" => "Y",
		"Y" => "Z",
		"Z" => "1");

	if($curr_index === null){
		$curr_index = strlen($input) - 1;
	}

	if($curr_index < 0){
		return "1" . $input;
	}
	
	$input[$curr_index] = $morpher[$input[$curr_index]];
	if($input[$curr_index] == "1"){
		return incrementID($input, $curr_index - 1);
	}
	
	return $input;
}


function _uploadNewFile(){
	global $_USER, $_CONFIG, $morpher, $mime_types;
	
	// Check to see if this file exceeds the maximum size alotted.
	$total_used_space = $_USER["total_uploaded_filesizes"] + $_FILES["file"]["size"];
	if($total_used_space > getPermission("max_upload_space")){
		callClientMethod("upload_failed_exceeded_toal_used_space", array( $total_used_space, getPermission("max_upload_space")));
	}

	if($_FILES["file"]["size"] > getPermission("max_upload_size")){
		callClientMethod("upload_failed_exceeded_file_size");
	}

	$url_id = createNewId();

	if(move_uploaded_file($_FILES["file"]["tmp_name"], $_CONFIG["upload_dir"] . $url_id)) {

		$file_parts = explode(".", $_FILES["file"]["name"]);
		$file_extention = strtolower($file_parts[count($file_parts) - 1]);
		$file_mime = (array_key_exists($file_extention, $mime_types))? $mime_types[$file_extention] : $_FILES["file"]["type"];

		$inserted = myInsert("files", array(
			"owner_id" => $_USER["id"],
			"url_id" => $url_id,
			"upload_date" => "NOW()",
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

		if($inserted){
			myUpdate("users", array(
				"total_uploaded_filesizes" => $total_used_space,
				"total_files_uploaded" => $_USER["total_files_uploaded"] + 1
			), "`id` = '". $_USER["id"] ."'");
			
			callClientMethod("upload_successful", array(
				"url_id" => $url_id,
				"is_visible" => true,
				"file_status" => 2
			));
		}else{
			callClientMethod("upload_failed_db_error");
		}
	} else{
		callClientMethod("upload_failed_could_not_handle_file");
	}


}

function _viewFile(){
	global $_CONFIG, $mime_types;
	
	if(!isset($_GET["file"]) && isset($_GET["args"])){
		$_GET["file"] = $_GET["args"][0];
	}

	$file_exist = mysqlQuery("SELECT * FROM `files`
		WHERE `url_id` LIKE BINARY '%s'
		LIMIT 1", array($_GET["file"]));

	if(empty($file_exist)){
		echo "File does not exist";
		
	}else{
		$file = $file_exist[0];
		myUpdate("files", array(
			"last_accessed" => "NOW()",
			"total_views" => $file["total_views"] + 1
		), "`id` LIKE BINARY '" . $file["id"]. "'");

		header("Cache-Control: no-cache, must-revalidate"); // HTTP/1.1
		header("Expires: Sat, 26 Jul 1997 05:00:00 GMT"); // Date in the past

		if (file_exists($_CONFIG["upload_dir"] . $file["url_id"])) {
			header("Content-Type: ". $file["file_mime"]);

			// Make sure that the file is not one that should be viewed in the browser.
			if(strpos($file["file_mime"], "image") === false && strpos($file["file_mime"], "text/plain") === false){
				header("Content-Transfer-Encoding: binary");

				outputFile($_CONFIG["upload_dir"] . $_GET["file"],basename($file["file_name"]), $file["file_mime"], true);
				die();
			}
			
			$etag = md5($file["upload_date"] + $file["file_size"] + $file["file_name"]);

			$expires = 60 * 60 * 24 * 360;
			$exp_gmt = gmdate("D, d M Y H:i:s", time() + $expires )." GMT";
			$mod_gmt = gmdate("D, d M Y H:i:s", strtotime($file["upload_date"])) ." GMT";
			header("Expires: ". $exp_gmt);
			header("Last-Modified: ". $mod_gmt);
			header("Cache-Control: public, max-age=". $expires);
			header("ETag: " . $etag );
			
			if(isset($_SERVER['HTTP_IF_NONE_MATCH']) && $etag == $_SERVER['HTTP_IF_NONE_MATCH']){
				header("HTTP/1.1 304 Not Modified");
				die();
			}

			outputFile($_CONFIG["upload_dir"] . $_GET["file"], basename($file["file_name"]), $file["file_mime"], false);
			die();
		}
	}

}

// Thanks to Edward Jaramilla (http://www.php.net/manual/en/function.fread.php#84115) for code base.
function outputFile($file, $uploaded_file_name, $mime, $force_download = false){

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

	//callClientMethod("call", $_SERVER);
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

	header('Content-Length: '.($seek_end - $seek_start + 1));

	set_time_limit(0);
	$fp = fopen($file, 'rb');
	fseek($fp, $seek_start);

	while(!feof($fp)){
		echo fread($fp, 1024*8);
		//flush();
		ob_flush();
	}

	fclose($fp);
}


?>
