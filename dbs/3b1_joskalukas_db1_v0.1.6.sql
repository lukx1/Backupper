-- phpMyAdmin SQL Dump
-- version 4.2.9.1
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Mar 21, 2018 at 07:51 PM
-- Server version: 5.5.55-0+deb7u1
-- PHP Version: 5.4.45-0+deb7u8

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `3b1_joskalukas_db1`
--

-- --------------------------------------------------------

--
-- Table structure for table `DaemonLogs`
--

CREATE TABLE IF NOT EXISTS `DaemonLogs` (
`Id` int(11) NOT NULL,
  `IdDaemon` int(11) NOT NULL,
  `IdLogType` int(11) NOT NULL,
  `Code` int(11) NOT NULL,
  `DateCreated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ShortText` varchar(64) NOT NULL,
  `LongText` varchar(512) DEFAULT NULL,
  `Origin` varchar(32) NOT NULL COMMENT 'Kde byl log vytvořen'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `DaemonLogs`
--
ALTER TABLE `DaemonLogs`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdDaemon` (`IdDaemon`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `DaemonLogs`
--
ALTER TABLE `DaemonLogs`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `DaemonLogs`
--
ALTER TABLE `DaemonLogs`
ADD CONSTRAINT `DaemonLogs_FK_IdDaemon_Daemons$Id` FOREIGN KEY (`IdDaemon`) REFERENCES `Daemons` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
