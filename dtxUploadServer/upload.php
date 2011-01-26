<?php

include_once("./functions.php");

function _getNewUploadId(){
	global $_USER, $_CONFIG;
	
	$id = md5(microtime());
	
	$rows_changed = mysqlQuery("INSERT INTO `files` (
		`id`,
		`owner_id`,
		`upload_date`,
		`upload_id`,
		`is_public`,
		`is_visible`,
		`is_disabled`,
		`file_status`,
		`is_encrypted`
	)
	VALUES (
		NULL,
		'%s',
		NOW( ),
		'%s',
		'0',
		'0',
		'1',
		'5',
		'0',
	);", "affected", $_USER["id"], $id);
	
	if($rows_changed = 0){
		
	}




}





/*
	$rows_changed = mysqlQuery("INSERT INTO `files` (
		`id` ,
		`owner_id` ,
		`url_id` ,
		`tags` ,
		`upload_date` ,
		`upload_id` ,
		`last_accessed` ,
		`is_public` ,
		`is_visible` ,
		`is_disabled` ,
		`shared_ids` ,
		`file_status` ,
		`is_encrypted` ,
		`file_name` ,
		`file_size` ,
		`file_mime`
	)
	VALUES (
		NULL,
		'%s',
		'b',
		'N/A',
		NOW( ),
		MD5( '54536564' ),
		NOW( ),
		'1',
		'1',
		'0',
		'|24|',
		'5',
		'0',
		'TestFile.jpg',
		'214312',
		'image/jpeg'
	);", "affected", 
	*/

?>