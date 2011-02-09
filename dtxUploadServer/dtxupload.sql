-- phpMyAdmin SQL Dump
-- version 3.2.4
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Feb 09, 2011 at 01:54 PM
-- Server version: 5.1.41
-- PHP Version: 5.3.1

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `dtxupload`
--

-- --------------------------------------------------------

--
-- Table structure for table `files`
--

CREATE TABLE IF NOT EXISTS `files` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `owner_id` int(11) NOT NULL,
  `url_id` varchar(32) NOT NULL,
  `tags` text NOT NULL,
  `upload_date` date NOT NULL,
  `upload_id` varchar(32) NOT NULL,
  `last_accessed` date NOT NULL,
  `total_views` int(11) NOT NULL DEFAULT '0',
  `is_public` tinyint(1) NOT NULL,
  `is_visible` tinyint(1) NOT NULL,
  `is_disabled` tinyint(1) NOT NULL,
  `shared_ids` text NOT NULL,
  `file_status` int(11) NOT NULL,
  `is_encrypted` tinyint(1) NOT NULL,
  `file_name` varchar(128) NOT NULL,
  `file_size` int(11) NOT NULL,
  `file_mime` varchar(64) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `url_id` (`url_id`),
  KEY `owner_id` (`owner_id`),
  FULLTEXT KEY `tags` (`tags`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=92 ;

--
-- Dumping data for table `files`
--


-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE IF NOT EXISTS `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(32) NOT NULL,
  `password` varchar(32) NOT NULL,
  `session_key` varchar(32) NOT NULL,
  `session_last_active` datetime NOT NULL,
  `registration_date` date NOT NULL,
  `email` varchar(128) NOT NULL,
  `is_email_validated` tinyint(1) NOT NULL,
  `account_verification_code` varchar(32) NOT NULL,
  `permissions` int(11) NOT NULL,
  `total_files_uploaded` int(11) NOT NULL DEFAULT '0',
  `total_uploaded_filesizes` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `username` (`username`),
  KEY `session_id` (`session_key`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=63 ;

--
-- Dumping data for table `users`
--


-- --------------------------------------------------------

--
-- Table structure for table `users_permissions`
--

CREATE TABLE IF NOT EXISTS `users_permissions` (
  `id` int(11) NOT NULL,
  `name` varchar(64) NOT NULL,
  `is_disabled` tinyint(1) NOT NULL,
  `can_connect` tinyint(1) NOT NULL,
  `can_upload` tinyint(1) NOT NULL,
  `manage_users` tinyint(1) NOT NULL,
  `manage_uploads` tinyint(1) NOT NULL,
  `full_access` tinyint(1) NOT NULL,
  `max_upload_space` int(11) NOT NULL,
  `max_upload_size` int(11) NOT NULL,
  `default_permission_set` tinyint(1) NOT NULL COMMENT 'True if the current record is part of the default permission system.',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `users_permissions`
--

INSERT INTO `users_permissions` (`id`, `name`, `is_disabled`, `can_connect`, `can_upload`, `manage_users`, `manage_uploads`, `full_access`, `max_upload_space`, `max_upload_size`, `default_permission_set`) VALUES
(0, 'Admin', 0, 1, 1, 1, 1, 1, 0, 0, 1),
(1, 'User', 0, 1, 1, 0, 0, 0, 100000, 20000, 1),
(2, 'Banned', 1, 0, 0, 0, 0, 0, -1, -1, 1);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
