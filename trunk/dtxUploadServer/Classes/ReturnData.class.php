<?php

/**
 * Class to encapsulate the return data of all the methods.
 */
class ReturnData {
	
	/**
	 * @var bool True if the function returned success, false othereise.
	 */
	public $successful = false;
	
	/**
	 * @var mixed Data that the method is returning.
	 */
	public $data;
	
	/**
	 * @var string Method that is returned by the method.  Usually the method that is returned to the client.
	 */
	public $method;
	
	/**
	 * Data that is to be passed to the calling method is to be placed inside this class.
	 * 
	 * @param bool $successful True if the method ran successfully, false otherwise.
	 * @param string $method Method that is to be returned to the client.
	 * @param mixed $data Data that is to be returned to the client.
	 */
	public function __construct($successful, $method = null, $data = false){
		$this->successful = $successful;
		$this->method = $method;
		$this->data = $data;
	}
	
	/**
	 * Returns to the client the data that is contained inside this class.
	 */
	public function sendClientInfo(){
		returnClientData($this->method, $this->data);
	}
	
	/**
	 * Sends the data to the client if the constructor reported that the method failed.
	 * 
	 * @return ReturnData The current instance of this class to allow for quick sending on failure.
	 */
	public function sendOnFail(){
		if($this->successful == false){
			$this->sendClientInfo();
		}
		return $this;
	}
}

?>
