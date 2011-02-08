<?php

include_once("./functions.php");

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

	// MAJOR TODO!  Make sure the requested file is OWNED by the deleter.
	$deleted = mysqlQuery("DELETE FROM `files` WHERE `url_id` = '%s' LIMIT 1 ", array(
		$delete_id
	), "successful");
	if(file_exists($_CONFIG["upload_dir"] . $delete_id)){
		if(unlink($_CONFIG["upload_dir"] . $delete_id)){
			if($deleted){
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
	$file_exist = mysqlQuery("SELECT * FROM `files`
		WHERE `url_id` = '%s'
		LIMIT 1", array($_GET["file"]));

	if(empty($file_exist)){
		echo "File does not exist";
		
	}else{
		$file = $file_exist[0];
		myUpdate("files", array(
			"last_accessed" => "NOW()",
			"total_views" => $file["total_views"] + 1
		), "`id` = '" . $file["id"]. "'");

		if (file_exists($_CONFIG["upload_dir"] . $file["url_id"])) {
			header("Content-Type: ". $file["file_mime"]);

			if(strpos($file["file_mime"], "image") === false && strpos($file["file_mime"], "text/plain") === false){
				header("Content-Description: File Transfer");
				header("Content-Disposition: attachment; filename=\"". basename($file["file_name"]) ."\"");
				header("Content-Transfer-Encoding: binary");
			}

			header("Pragma: public");
			header("Content-Length: " . filesize($_CONFIG["upload_dir"] . $file["url_id"]));
			readfile($_CONFIG["upload_dir"] . $file["url_id"]);
			die();
		}
	}

}


?>