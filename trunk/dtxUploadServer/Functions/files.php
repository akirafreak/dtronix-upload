<?php

class Files{
	private $_USER, $_CONFIG, $_QUERY;
	
	public function Files(&$user, &$config, $query) {
		$this->_USER = &$user;
		$this->_CONFIG = &$config;
		$this->_QUERY = $query;
	}
	public function  exist(){
		validateUser();

		$upload_id = $this->_QUERY[0];

		// TODO: Add any checks to see if the file is visible?
		return file_exists($this->_CONFIG["upload_dir"] . $upload_id);
	}

	public function listDirectory(){
		validateUser();

		$directory = $this->_QUERY[0];

		$files = sqlQuery("SELECT `file_name`, `upload_date`, `file_size`, `last_accessed`, `url_id`, `directory`
			FROM `files`
			WHERE `owner_id` = '%s'
			AND `directory` = '%s'
			LIMIT 0 , 30", array(
				$this->_USER["id"],
				$directory
			));

		if(!empty($files)){
			callClientMethod("directory_contents", $files);
		}else{
			callClientMethod("directory_contents", array(array()));
		}
	}

	public function info(){
		validateUser();

		$url_id = $this->_QUERY[0];

		$file = sqlQuery("SELECT `file_name`, `upload_date`, `file_size`, `last_accessed`, `url_id`
			FROM `files`
			WHERE `owner_id` = '%s'
			AND `url_id` LIKE BINARY '%s'
			LIMIT 1", array(
				$this->_USER["id"],
				$url_id
			));

		if(!empty($files)){
			callClientMethod("file_info", $file);
		}else{
			callClientMethod("file_info", array());
		}
	}

	public function listFiles(){
		validateUser();

		// The total pages into the list to go.
		$start = $this->_QUERY[0];
		$files_per_page = 60;

		$files = sqlQuery("SELECT `url_id`, `upload_date`, `file_name`
			FROM `files`
			WHERE `owner_id` = '%s' AND `is_visible` = '1'
			ORDER BY `upload_date` DESC
			LIMIT %s , %s", array(
				$this->_USER["id"],
				$start * $files_per_page,
				($start * $files_per_page) + $files_per_page
			));

		if(count($files) > 0){
			callClientMethod("files_uploaded_quick", $files);

		}else{
			callClientMethod("files_uploaded_no_files");
		}
	}

	public function delete(){
		validateUser();
		
		$delete_id = $this->_QUERY[0];

		// Check for file ownership
		if(!$this->isOwner($delete_id)){
			callClientMethod("file_delete_failure_owner");
		}

		$file = sqlQuery("SELECT `file_size` FROM `files`
			WHERE `url_id` LIKE BINARY '%s'
			LIMIT 1;",
		array(
			$delete_id
		));

		$deleted = sqlQuery("DELETE FROM `files`
			WHERE `url_id` LIKE BINARY '%s'
			LIMIT 1 ",
		array(
			$delete_id
		), "successful");

		if(file_exists($this->_CONFIG["upload_dir"] . $delete_id)){
			if(unlink($this->_CONFIG["upload_dir"] . $delete_id)){
				if($deleted){
					$toal_file_sizes = $this->_USER["total_uploaded_filesizes"] - $file[0]["file_size"];
					sqlUpdate("users", array(
						"total_uploaded_filesizes" => ($toal_file_sizes < 0)? 0 : $toal_file_sizes,
						"total_files_uploaded" => ($this->_USER["total_files_uploaded"] < 1)? 0 : $this->_USER["total_files_uploaded"] - 1
					), "`id` = '". $this->_USER["id"] ."'");

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
	
	public function upload(){
		validateUser();
		
		//callClientMethod("error", $_FILES);
		// Check to see if this file exceeds the maximum size alotted.
		$total_used_space = $this->_USER["total_uploaded_filesizes"] + $_FILES["file"]["size"];
		if($total_used_space > getPermission("max_upload_space")){
			callClientMethod("upload_failed_exceeded_toal_used_space", array( $total_used_space, getPermission("max_upload_space")));
		}

		if($_FILES["file"]["size"] > getPermission("max_upload_size")){
			callClientMethod("upload_failed_exceeded_file_size");
		}

		$url_id = $this->createNewId();



		if(move_uploaded_file($_FILES["file"]["tmp_name"], $this->_CONFIG["upload_dir"] . $url_id)) {

			$file_parts = explode(".", $_FILES["file"]["name"]);
			$file_extention = strtolower($file_parts[count($file_parts) - 1]);
			$file_mime = (array_key_exists($file_extention, $this->_MIMES))? $this->_MIMES[$file_extention] : $_FILES["file"]["type"];

			$inserted = sqlInsert("files", array(
				"owner_id" => $this->_USER["id"],
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
				sqlUpdate("users", array(
					"total_uploaded_filesizes" => $total_used_space,
					"total_files_uploaded" => $this->_USER["total_files_uploaded"] + 1
				), "`id` = '". $this->_USER["id"] ."'");

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

	public function view(){

		$file_id = $this->_QUERY[0];

		$file_exist = sqlQuery("SELECT * FROM `files`
			WHERE `url_id` LIKE BINARY '%s'
			LIMIT 1", array($file_id));

		if(empty($file_exist)){
			echo "File does not exist";

		}else{
			$file = $file_exist[0];
			sqlUpdate("files", array(
				"last_accessed" => "NOW()",
				"total_views" => $file["total_views"] + 1
			), "`id` LIKE BINARY '" . $file["id"]. "'");

			header("Cache-Control: no-cache, must-revalidate"); // HTTP/1.1
			header("Expires: Sat, 26 Jul 1997 05:00:00 GMT"); // Date in the past

			if (file_exists($this->_CONFIG["upload_dir"] . $file["url_id"])) {
				header("Content-Type: ". $file["file_mime"]);

				// Make sure that the file is not one that should be viewed in the browser.
				if(strpos($file["file_mime"], "image") === false && strpos($file["file_mime"], "text/plain") === false){
					header("Content-Transfer-Encoding: binary");

					$this->outputFile($this->_CONFIG["upload_dir"] . $file_id, basename($file["file_name"]), $file["file_mime"], true);
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

				$this->outputFile($this->_CONFIG["upload_dir"] . $file_id, basename($file["file_name"]), $file["file_mime"], false);
				die();
			}
		}

	}

	// Thanks to Edward Jaramilla (http://www.php.net/manual/en/function.fread.php#84115) for code base.
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

	private function isOwner($file_id){
		validateUser();

		$can_delete = sqlQuery("SELECT `id`
			FROM `files`
			WHERE `owner_id` = '%s'
			AND `url_id` LIKE BINARY '%s'
			LIMIT 1 ;",
		array(
			$this->_USER["id"],
			$file_id
		), "has_rows");

		if($can_delete){
			return true;
		}else{
			return false;
		}
	}

	private function createNewId(){
		$last_entry = sqlQuery("SELECT `url_id`
			FROM `files`
			ORDER BY `id` DESC
			LIMIT 1");

		if(empty($last_entry[0])) return "1";

		$last_id = $last_entry[0]["url_id"];

		return $this->incrementID($last_id);
	}

	private function incrementID($input, $curr_index = null){
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

	private $_MIMES = array(
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
