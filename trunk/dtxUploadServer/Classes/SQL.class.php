<?php
/**
 * Class that manages safe transactions with a PostgreSql or MySQL server.
 */
Class SQL{
	/**
	 * @var integer Total times the server has been queried this session.
	 */
	private $query_count = 0;
	
	/**
	 * WARNING:  When setting this to true, it might be good to have the server
	 * set to maintenance mode to prevent other people from capturing private information.
	 * 
	 * @var bool True for outputting all queries and their associated information.
	 */
	public $verbose = true;
	
	/**
	 * Location to log all the query and information to. Ignored if $verbose is set to false.
	 * 
	 * @var string Location of the log file
	 */
	private $verbose_log = "sql.log";
	
	/**
	 * @var bool True if log has been written to this session.
	 */
	private $log_opened = false;

	/**
	 * @var resource Link ID for the open PostgreSql or MySQL connection.  null if there is no connection.
	 */
	private $sql_connection = null;
	
	/**
	 *
	 * @var string String that contains the data as to what type of SQL server we are connected to. (pgsql, mysql, null)
	 */
	private $sql_type = null;

	/**
	 * Connect to a PostgreSQL server and set the class to handle PgSQL calls.
	 *
	 * @param string $server Server to connect to. (Usually "localhost")
	 * @param int $port POrt of the server to connect to.  Default: 5432
	 * @param string $database Database to connect to.
	 * @param string $user Username for the database.
	 * @param string $password User password for the database.
	 */
	public function pgsqlConnect($server, $port, $database, $user, $password){
		if($this->sql_connection != null)
			die("Server is already connected.");
				
		$this->sql_connection = pg_connect("dbname={$database} user={$user} password={$password}");
		if($this->sql_connection === false)
			die("Could not connect to the SQL server. " . pg_last_error());

		if($this->verbose)
			$this->log("Connected to server. \n");
		
		$this->sql_type = "pgsql";
	}
	
	/**
	 * Connect to a MySQL server and set the class to handle MySQL calls.
	 *
	 * @param string $server Server to connect to. (Usually "localhost")
	 * @param string $database Database to connect to.
	 * @param string $user Username for the database.
	 * @param string $password User password for the database.
	 */
	public function mysqlConnect($server, $database, $user, $password){
		if($this->sql_connection != null)
			die("Server is already connected.");

		$this->sql_connection = mysql_connect($server, $user, $password);
		if($this->sql_connection === false)
			die("Could not connect to MySQL server");

		if(mysql_select_db($database, $this->sql_connection) == false)
			die("Could not select database: \"". $database ."\". " . mysql_error($this->sql_connection));

		if($this->verbose)
			$this->log("Connected to server.");
		
		$this->sql_type = "mysql";
	}

	/**
	 * Used to define when a SQL function is intended to be used in a query.
	 *
	 * Only used for insert & update methods.
	 *
	 * @param string $function SQL function to call such as NOW(). Include the "()"
	 * @return mixed Array to be used in a query value.
	 *
	 * <b>Example:</b><br />
	 * In the following, the NOW() will be placed inside quotes and will not be a valid MySQL function.
	 * <code>
	 * SQL->update("my_table", array(
	 * "curr_time" => "NOW()"
	 * ), "WHERE uid = '1'");
	 * </code>
	 *
	 * In the following, the NOW() is recognized by the parser as being a MySQL function and will not tamper with it.
	 * <code>
	 * SQL->update("my_table", array(
	 * "curr_time" => SQL:func("NOW()")
	 * ), "WHERE uid = '1'");
	 * </code>
	 */
	public static function func($function) {
		return array("sql_function", $function);
	}

	/**
	 * Execute a buffered query on the MySQL server.
	 *
	 * @param string $query String that contains the query to execute.
	 * @param mixed $values Values to be inserted into the query via sprintf.
	 *
	 * @return resource PHP resource for the query.
	 *
	 * <b>Example:</b><br />
	 * <code>
	 * SQL->query("SELECT user_name FROM users WHERE uid = '%s'", array(
	 * $user_id
	 * ));
	 * </code>
	 */
	public function query($query, $values = null){
		if(empty($query))
			return false;
		
		if($this->verbose)
			$this->log("QUERY PARSING: <code>{$query}</code>");
			
		$this->formatSql($query, $values);
		if($this->verbose)
			$this->log("QUERY PARSED: <code>{$query}</code>");

		// Increment the total number of queries.
		$this->query_count++;
		
		if($this->sql_type == "pgsql"){
			$result = pg_query($this->sql_connection, $query);
			$error = pg_last_error($this->sql_connection);
			if($this->verbose && $error != false)
				$this->log("POSTGRESQL ERROR: ". $error);
			
		}elseif($this->sql_type == "mysql"){
			$result = mysql_query($query, $this->sql_connection);
			if($this->verbose && mysql_errno($this->sql_connection) !== 0)
				$this->log("MYSQL ERROR[". mysql_errno($this->sql_connection) ."]: ". mysql_error($this->sql_connection));
		}else{
			die("SQL is not connected to a server.");
		}
		return $result;
	}

	/**
	 * Executes an insert query. <br />
	 * <b>WARNING:</b> Do not pass anything but a string or int as a value on the $name_values array.
	 *
	 * @param string $table table to insert a new row in.
	 * @param mixed $name_values array(Column Name => Value) to be inserted into.
	 *
	 * @return bool True on a successful insert, false on a failure.
	 *
	 * <b>Example:</b><br />
	 * <code>
	 * SQL->insert("users", array(
	 * "username" => $username,
	 * "password" => md5($password)
	 * ));
	 * </code>
	 */
	public function insert($table, $name_values){
		if($name_values == null)
			return false;
		
		$query = $this->buildInsert($table, $name_values);
		return $this->successful($query);
	}
	
	/**
	 * Executes an update query. Inserts $values into $where via sprintf.
	 *
	 * @param string $table table to update a row.
	 * @param mixed $name_values array(Column Name => Value) to be inserted into.
	 * @param string $where String that contains the "WHERE" update command. Include the WHERE
	 * @param mixed $values Values to be inserted into the $where via sprintf.
	 *
	 * @return bool True on successful update, false on failure.
	 *
	 *<b>Example:</b><br />
	 * <code>
	 * SQL->update("users", array(
	 *      "username" => "new user name",
	 *      "last_active" => SQL::func("NOW()")
	 * ), "WHERE user_id = '%s'", array(
	 *      $user_id
	 * ));
	 * </code>
	 */
	public function update($table, $name_values, $where, $values){
		if($name_values == null)
			return false;
		$this->formatSql($where, $values);
		
		$query = $this->buildUpdate($table, $name_values, $where);
		return $this->successful($query);
	}
	
	/**
	 * Execute a buffered query on the MySQL server and return the first result.
	 *
	 * @param string $query String that contains the query to execute.
	 * @param mixed $values Values to be inserted into the query via sprintf.
	 *
	 * @return mixed Assosotive array that contains the result data.  False on no data.
	 */
	public function fetchRow($query, $values = null){
		$result = $this->query($query, $values);

		if($this->sql_type == "pgsql"){
			$return = pg_fetch_assoc($result);
			
		}elseif($this->sql_type == "mysql"){
			$return = mysql_fetch_assoc($result);
		}

		return $return;
	}

	/**
	 * Execute a buffered query on the SQL server.
	 * 
	 * @param string $query String that contains the query to execute.
	 * @param mixed $values Values to be inserted into the query via sprintf.
	 *
	 * @return mixed Assosotive array that contains the result data.  False on no data.
	 */
	public function fetchRows($query, $values = null){
		$result = $this->query($query, $values);

		$return = array();
		if($this->sql_type == "pgsql"){
			while($row = pg_fetch_assoc($result)){
				$return[] = $row;
			}
			
		}elseif($this->sql_type == "mysql"){
			while($row = mysql_fetch_assoc($result)){
				$return[] = $row;
			}
		}

		if(count($return) == 0){
			return false;
		}
		return $return;
	}	

	/**
	 * Execute a buffered query on the MySQL server and return the first result.
	 *
	 * @param string $query String that contains the query to execute.
	 * @param mixed $values Values to be inserted into the query via sprintf.
	 *
	 * @return mixed Enumerated array that contains the result data.  False on no data.
	 */
	public function fetchResult($query, $values = null){
		$result = $this->query($query, $values);
		
		if($this->sql_type == "pgsql"){
			$return = pg_fetch_row($result);
			
		}elseif($this->sql_type == "mysql"){
			$return = mysql_fetch_row($result);
		}

		return $return;
	}

	/**
	 * Execute a buffered query on the MySQL server and return the first result.
	 *
	 * @param string $query String that contains the query to execute.
	 * @param mixed $values Values to be inserted into the query via sprintf.
	 *
	 * @return array Enumerated array that contains the result data.  False on no data.
	 */
	public function fetchRowsEnum($query, $values = null){
		$result = $this->query($query, $values);
		
		$return = array();
		if($this->sql_type == "pgsql"){
			while($row = pg_fetch_array($result, "PGSQL_NUM")){
				$return[] = $row;
			}
			
		}elseif($this->sql_type == "mysql"){
			while($row = mysql_fetch_array($result, "MYSQL_NUM")){
				$return[] = $row;
			}
		}

		if(count($return) == 0){
			return false;
		}
		return $return;
	}

	/**
	 * Execute a buffered query on the MySQL server.
	 *
	 * @param string $query String that contains the query to execute.
	 * @param mixed $values Values to be inserted into the query via sprintf.
	 *
	 * @return integer The number of rows that was affected by the inputted query
	 */
	public function affected($query, $values = null){
		$result = $this->query($query, $values);
		if($this->sql_type == "pgsql"){
			$affected = pg_affected_rows($result);
			
		}elseif($this->sql_type == "mysql"){
			$affected = mysql_affected_rows($this->sql_connection);
		}

		return $affected;
	}

	/**
	 * Execute a buffered query on the MySQL server.
	 *
	 * @param string $query String that contains the query to execute.
	 * @param mixed $values Values to be inserted into the query via sprintf.
	 *
	 * @return bool True if the query affected at least one row, false on no rows being affected.
	 */
	public function successful($query, $values = null){
		$affected = $this->affected($query, $values);
		if($affected > 0){
			return true;
		}
		return false;
	}

	/**
	 * Execute a buffered query on the MySQL server.
	 *
	 * @param string $query String that contains the query to execute.
	 * @param mixed $values Values to be inserted into the query via sprintf.
	 *
	 * @return bool True if the executed query returnd rows, false on no rows being returned;
	 */
	public function hasRows($query, $values = null){
		$count = $this->count($query, $values);
		return ($count > 0)? true : false;
	}

	/**
	 * Execute a buffered query on the MySQL server.
	 *
	 * @param string $query String that contains the query to execute.
	 * @param mixed $values Values to be inserted into the query via sprintf.
	 *
	 * @return bool Counts the number of rows returned.
	 */
	public function count($query, $values = null){
		$result = $this->query($query, $values);
		if($this->sql_type == "pgsql"){
			$count = pg_num_rows($result);
			
		}elseif($this->sql_type == "mysql"){
			$count = mysql_num_rows($result);
		}
		
		//@mysql_free_result($result);
		return $count;
	}

	/**
	 * Escape all injection attempts in the input array.
	 *
	 * @param array $array Array input that contains objects to be escaped.
	 */
	private function escapeAll(&$array){
		// If we are dealing with a string, then encapsulate it in an array.
		if(is_string($array))
			$array = array($array);
		
		if($this->sql_type == "pgsql"){
			foreach($array as &$value){
				$value = pg_escape_string($this->sql_connection, $value);
			}
			
		}elseif($this->sql_type == "mysql"){
			foreach($array as &$value){
				$value = mysql_real_escape_string($value, $this->sql_connection);
			}
		}
	}
	
	/**
	 * Escape all injection attempts in the string.
	 *
	 * @param string $string Input string that is to be escaped.
	 * @return string Escaped string.
	 */
	private function escapeString($string){
		if($this->sql_type == "pgsql"){
			return pg_escape_string($this->sql_connection, $string);

		}elseif($this->sql_type == "mysql"){
			return mysql_real_escape_string($string, $this->sql_connection);

		}else{
			die("Unknown server connection type.");
		}
	}

	/**
	 * Take a query and apply the $values to the query via sprintf.
	 *
	 * @param string $query String that contains the query to execute.
	 * @param mixed $values Values to be inserted into the query via sprintf.
	 */
	private function formatSql(&$query, &$values){
		if($values !== null && !empty($values)){
			$this->escapeAll($values);
			$query = vsprintf($query, $values);
		}
	}
	
	/**
	 * Internal method to build a save INSERT query.
	 *
	 * @param type $table Table that this insert is to be performed on.
	 * @param array $name_values Associative array that has the key of the column, and the new value to insert.
	 * @return string Formetted query string.
	 */
	private function buildInsert($table, $name_values){
		$columns_builder[] = "INSERT INTO {$table} (";
		$values_builder[] = "VALUES (";
		
		foreach($name_values as $col => $value){
			// Add each key to the column.
			$columns_builder[] = $col;
			$columns_builder[] = ", ";
			
			// Add each value to the query.
			$values_builder[] = $this->formatInput($value);
			$values_builder[] = ", ";
		}
		
		// Pop both ", " off the end of both arrays.
		array_pop($columns_builder);
		array_pop($values_builder);
		
		$columns_builder[] = ")\n";
		$values_builder[] = ");";

		return implode("", array_merge($columns_builder, $values_builder));
	}
	
	/**
	 * Internal method to build a save UPDATE query.
	 *
	 * @param type $table Table that this update is to be performed on.
	 * @param array $name_values Associative array that has the key of the column, and the value to set.
	 * @param type $where The WHERE statement that contains "WHERE"
	 * @return string Formetted query string.
	 */
	private function buildUpdate($table, $name_values, $where){
		$query_builder[] = "UPDATE {$table} SET ";
		
		foreach($name_values as $col => $value){
			// Add each key to the column.
			$query_builder[] = "{$col} = ";
			$query_builder[] = $this->formatInput($value);
			$query_builder[] = ", ";
		}
		
		// Pop the ", " off the end.
		array_pop($query_builder);

		$query_builder[] = " ";
		$query_builder[] = $where;
		
		return implode("", $query_builder);
	}

	/**
	 * Internal method to ensure that SQL functions such as NOW() are not overriden
	 * and treated as a string by the parser.
	 *
	 * @param mixed $value Value to escape and place inside quotes.
	 * @return string Output after the value has been escaped..
	 */
	private function formatInput($value){
		// Determine if the passed value is something that we do not want to mess with such as a MySQL function.
		$input = $value;
		if(is_array($value)){
			if($value[0] == "sql_function"){
				$encapsulate = false;
				$input = $value[1];

			}else{
				$encapsulate = true;
			}
		}else{
			$encapsulate = true;
		}
		if($input === null){
			return "NULL";

		}else{
			$input = $this->escapeString($input);
			if($encapsulate){
				return "'". $input ."'";

			}else{
				return $input;
			}
		}
	}
	
	/**
	 * Internal loging methods to handle debugging.
	 * 
	 * @param mixed $data Array or a string containing what data to write.
	 * @return void
	 */
	private function log($data){
		if(!$this->verbose)
			return;
		
		// If the data is an array, then put it in JSON format so that we can see it.
		if(is_array($data))
			$data = json_encode($data);
		
		$data = "TIME[". date("m.d.Y H:i:s") . "] " . $data . "\n\n";
		
		if($this->verbose_log == null){
			echo $data;
			return;
		}
		
		// Determine if we need to wipe the log file.
		$file_open_type = ($this->log_opened)? "a" : "w";
		$handle = fopen($this->verbose_log, $file_open_type) or die("Can not open SQL log file for writing.");
		fwrite($handle, $data);
		fclose($handle);
		
		$this->log_opened = true;
	}
}
?>
