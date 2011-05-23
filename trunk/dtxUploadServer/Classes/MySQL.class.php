<?php
/**
 * Class that manages safe transactions with a MySQL server.
 */
Class MySQL{

	/**
	 * @var integer Total times the server has been queried this session.
	 */
	private $query_count = 0;

	/**
	 * @var resource Link ID for the open mysql connection.  null if there is no connection.
	 */
	private $mysql_connection = null;

	public function MySQL($server, $user, $password){
		$this->mysql_connection = mysql_connect($server, $user, $password);
		if($this->mysql_connection === false){
			die("Could not connect to MySQL server");
		}
	}

	/**
	 *
	 * @param string $database Database that is desired to be connected to.
	 */
	public function selectDb($database){
		if(mysql_select_db($database, $this->mysql_connection) == false){
			die("Could not select database: \"". $database ."\".");
		}
	}

	/**
	 * Used to define when a MySQL function is intended to be used in a query.
	 *
	 * Only used for insert & update methods.
	 *
	 * @param string $function MySQL function to call such as NOW(). Include the "()"
	 * @return mixed Array to be used in a query value.
	 *
	 * <b>Example:</b><br />
	 * In the following, the NOW() will be placed inside quotes and will not be a valid MySQL function.
	 * <code>
	 * MySQL->update("my_table", array(
	 * "curr_time" => "NOW()"
	 * ), "WHERE `uid` = 1");
	 * </code>
	 *
	 * In the following, the NOW() is recognized by the parser as being a MySQL function and will not tamper with it.
	 * <code>
	 * MySQL->update("my_table", array(
	 * "curr_time" => MySQL:func("NOW()")
	 * ), "WHERE `uid` = 1");
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
	 * MySQL->query("SELECT `user_name` FROM `users` WHERE `uid` = '%s'", array(
	 * $user_id
	 * ));
	 * </code>
	 */
	public function query($query, $values = null){
		if(empty($query)) return false;
		$this->formatSql($query, $values);

		// Increment the total number of queries.
		$this->query_count++;
		return mysql_query($query, $this->mysql_connection);
	}

	/**
	 * Executes an insert query.
	 *
	 * @param string $table table to insert a new row in.
	 * @param mixed $name_values array(Column Name => Value) to be inserted into.
	 *
	 * @return bool True on successful insert, false on failure.
	 *
	 * <b>Example:</b><br />
	 * <code>
	 * MySQL->insert("users", array(
	 * "username" => $username,
	 * "password" => md5($password)
	 * ));
	 * </code>
	 */
	public function insert($table, $name_values){
		if($name_values == null)
			return false;
		$query = $this->buildInsertUpdate("INSERT", $table, $name_values);
		return $this->querySuccessful($query);
	}

	/**
	 * Executes an update query.
	 *
	 * @param string $table table to update a row.
	 * @param mixed $name_values array(Column Name => Value) to be inserted into.
	 * @param string $where String that contains the "WHERE" update command.
	 *
	 * @return bool True on successful update, false on failure.
	 *
	 * <b>Example:</b><br />
	 * <code>
	 * MySQL->updateSafe("users", array(
	 *      "username" => "new user name",
	 *      "last_active" => MySQL::func("NOW()")
	 * ), "WHERE `user_id` = '1'"));
	 * </code>
	 */
	public function update($table, $name_values, $where){
		if($name_values == null)
			return false;
		$query = $this->buildInsertUpdate("UPDATE", $table, $name_values, $where);
		return $this->querySuccessful($query);
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
	 * MySQL->updateSafe("users", array(
	 *      "username" => "new user name",
	 *      "last_active" => MySQL::func("NOW()")
	 * ), "WHERE `user_id` = '%s'", array(
	 *      $user_id
	 * ));
	 * </code>
	 */
	public function updateSafe($table, $name_values, $where, $values){
		$this->formatSql($where, $values);
		return $this->update($table, $name_values, $where);
	}

	/**
	 * Execute a buffered query on the MySQL server.
	 *
	 * @param string $query String that contains the query to execute.
	 * @param mixed $values Values to be inserted into the query via sprintf.
	 *
	 * @return mixed Assosotive array that contains the result data.  False on no data.
	 */
	public function queryFetchRows($query, $values = null){
		$result = $this->query($query, $values);

		$return = array();
		while($row = mysql_fetch_assoc($result)){
			$return[] = $row;
		}
		//@mysql_free_result($result);

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
	 * @return mixed Assosotive array that contains the result data.  False on no data.
	 */
	public function queryFetchRow($query, $values = null){
		$result = $this->query($query, $values);

		$return = mysql_fetch_assoc($result);
		//@mysql_free_result($result);

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
	public function queryFetchResult($query, $values = null){
		$result = $this->query($query, $values);
		$return = mysql_fetch_row($result);

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
	public function queryAffected($query, $values = null){
		$result = $this->query($query, $values);
		$affected = mysql_affected_rows($this->mysql_connection);
		//@mysql_free_result($result);
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
	public function querySuccessful($query, $values = null){
		$affected = $this->queryAffected($query, $values);
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
	public function queryHasRows($query, $values = null){
		$count = $this->queryCount($query, $values);
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
	public function queryCount($query, $values = null){
		$result = $this->query($query, $values);
		$count = mysql_num_rows($result);
		//@mysql_free_result($result);
		return $count;
	}

	/**
	 * Escape all injection attempts in the input array.
	 *
	 * More information can be found here: http://php.net/manual/en/function.sprintf.php
	 *
	 * @param mixed $array Input array that contains objects to be escaped.
	 */
	private function escapeAll(&$array){
		foreach($array as $value){
			$value = mysql_real_escape_string($value);
		}
		return $array;
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


	private function buildInsertUpdate($query_type, $table, $name_values, $where = null){
		$query_builder = array($query_type ," `", $table, "`", " SET ");

		foreach($name_values as $key => $value){
			$query_builder[] = "`";
			$query_builder[] = $key;
			$query_builder[] = "` = ";
			$query_builder[] = $this->formatInput($value);

			$query_builder[] = ",";
		}
		array_pop($query_builder);
		if($where != null){
			$query_builder[] = " ";
			$query_builder[] = $where;
		}

		$query_builder[] = ";";

		return implode("", $query_builder);
	}

	/**
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
			$input = mysql_real_escape_string($input);
			if($encapsulate){
				return "'". $input ."'";

			}else{
				return $input;
			}
		}
	}
}
?>
