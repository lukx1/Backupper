-- phpMyAdmin SQL Dump
-- version 4.2.9.1
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Feb 11, 2018 at 06:16 PM
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
-- Table structure for table `daemons`
--

CREATE TABLE IF NOT EXISTS `daemons` (
`id` int(11) NOT NULL,
  `uuid` char(36) CHARACTER SET utf8 NOT NULL,
  `idUser` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_czech_ci;

--
-- Triggers `daemons`
--
DELIMITER //
CREATE TRIGGER `beforeInsertDaemonsUUID` BEFORE INSERT ON `daemons`
 FOR EACH ROW SET new.uuid = uuid()
//
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `locations`
--

CREATE TABLE IF NOT EXISTS `locations` (
`id` int(11) NOT NULL,
  `uri` varchar(1024) CHARACTER SET utf8 NOT NULL,
  `idProtocol` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COLLATE=utf8_czech_ci;

-- --------------------------------------------------------

--
-- Table structure for table `protocols`
--

CREATE TABLE IF NOT EXISTS `protocols` (
`id` int(11) NOT NULL,
  `ShortName` char(3) CHARACTER SET utf8 NOT NULL,
  `LongName` varchar(32) CHARACTER SET utf8 NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_czech_ci;

-- --------------------------------------------------------

--
-- Table structure for table `taskLocations`
--

CREATE TABLE IF NOT EXISTS `taskLocations` (
`id` int(11) NOT NULL,
  `idTask` int(11) NOT NULL,
  `idSource` int(11) NOT NULL,
  `idDestination` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_czech_ci;

-- --------------------------------------------------------

--
-- Table structure for table `taskLocationsTimes`
--

CREATE TABLE IF NOT EXISTS `taskLocationsTimes` (
`id` int(11) NOT NULL,
  `idTaskLocation` int(11) NOT NULL,
  `idTime` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_czech_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tasks`
--

CREATE TABLE IF NOT EXISTS `tasks` (
`id` int(11) NOT NULL,
  `idDaemon` int(11) NOT NULL,
  `Name` varchar(40) CHARACTER SET utf8 NOT NULL,
  `Description` varchar(200) CHARACTER SET utf8 NOT NULL DEFAULT ''
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_czech_ci;

-- --------------------------------------------------------

--
-- Table structure for table `times`
--

CREATE TABLE IF NOT EXISTS `times` (
`id` int(11) NOT NULL,
  `interval` int(11) NOT NULL COMMENT 'in seconds',
  `name` varchar(40) CHARACTER SET utf8 NOT NULL,
  `repeat` tinyint(1) NOT NULL DEFAULT '0',
  `startTime` datetime NOT NULL,
  `endTime` datetime DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_czech_ci;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE IF NOT EXISTS `users` (
`id` int(11) NOT NULL,
  `Name` varchar(100) CHARACTER SET utf8 NOT NULL,
  `Surname` varchar(100) CHARACTER SET utf8 NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_czech_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `daemons`
--
ALTER TABLE `daemons`
 ADD PRIMARY KEY (`id`), ADD UNIQUE KEY `uuid` (`uuid`), ADD KEY `idUser` (`idUser`);

--
-- Indexes for table `locations`
--
ALTER TABLE `locations`
 ADD PRIMARY KEY (`id`), ADD KEY `idProtocol` (`idProtocol`);

--
-- Indexes for table `protocols`
--
ALTER TABLE `protocols`
 ADD PRIMARY KEY (`id`), ADD UNIQUE KEY `ShortName` (`ShortName`);

--
-- Indexes for table `taskLocations`
--
ALTER TABLE `taskLocations`
 ADD PRIMARY KEY (`id`), ADD KEY `idTask` (`idTask`,`idSource`,`idDestination`), ADD KEY `idTask_2` (`idTask`), ADD KEY `idSource` (`idSource`), ADD KEY `idDestination` (`idDestination`);

--
-- Indexes for table `taskLocationsTimes`
--
ALTER TABLE `taskLocationsTimes`
 ADD PRIMARY KEY (`id`), ADD KEY `idTaskLocation` (`idTaskLocation`), ADD KEY `idTime` (`idTime`);

--
-- Indexes for table `tasks`
--
ALTER TABLE `tasks`
 ADD PRIMARY KEY (`id`), ADD KEY `idDaemon` (`idDaemon`);

--
-- Indexes for table `times`
--
ALTER TABLE `times`
 ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
 ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `daemons`
--
ALTER TABLE `daemons`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT for table `locations`
--
ALTER TABLE `locations`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=6;
--
-- AUTO_INCREMENT for table `protocols`
--
ALTER TABLE `protocols`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT for table `taskLocations`
--
ALTER TABLE `taskLocations`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT for table `taskLocationsTimes`
--
ALTER TABLE `taskLocationsTimes`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT for table `tasks`
--
ALTER TABLE `tasks`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT for table `times`
--
ALTER TABLE `times`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=2;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `daemons`
--
ALTER TABLE `daemons`
ADD CONSTRAINT `daemons_ibfk_1` FOREIGN KEY (`idUser`) REFERENCES `users` (`id`);

--
-- Constraints for table `locations`
--
ALTER TABLE `locations`
ADD CONSTRAINT `locations_ibfk_1` FOREIGN KEY (`idProtocol`) REFERENCES `protocols` (`id`);

--
-- Constraints for table `taskLocations`
--
ALTER TABLE `taskLocations`
ADD CONSTRAINT `taskLocations_ibfk_1` FOREIGN KEY (`idTask`) REFERENCES `tasks` (`id`),
ADD CONSTRAINT `taskLocations_ibfk_2` FOREIGN KEY (`idSource`) REFERENCES `locations` (`id`),
ADD CONSTRAINT `taskLocations_ibfk_3` FOREIGN KEY (`idDestination`) REFERENCES `locations` (`id`);

--
-- Constraints for table `taskLocationsTimes`
--
ALTER TABLE `taskLocationsTimes`
ADD CONSTRAINT `taskLocationsTimes_ibfk_2` FOREIGN KEY (`idTime`) REFERENCES `times` (`id`),
ADD CONSTRAINT `taskLocationsTimes_ibfk_1` FOREIGN KEY (`idTaskLocation`) REFERENCES `taskLocations` (`id`);

--
-- Constraints for table `tasks`
--
ALTER TABLE `tasks`
ADD CONSTRAINT `tasks_ibfk_1` FOREIGN KEY (`idDaemon`) REFERENCES `daemons` (`id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
