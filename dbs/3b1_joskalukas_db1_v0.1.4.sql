-- phpMyAdmin SQL Dump
-- version 4.2.9.1
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Feb 23, 2018 at 10:58 PM
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
-- Stand-in structure for view `LogedInDaemonsView`
--
CREATE TABLE IF NOT EXISTS `LogedInDaemonsView` (
`idDaemon` int(11)
,`expires` timestamp
,`id` int(11)
,`uuid` char(36)
,`idUser` int(11)
,`password` char(68)
,`idDaemonInfo` int(11)
);
-- --------------------------------------------------------

--
-- Table structure for table `backupTypes`
--

CREATE TABLE IF NOT EXISTS `backupTypes` (
`id` int(11) NOT NULL,
  `ShortName` char(4) NOT NULL,
  `LongName` varchar(32) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `backupTypes`
--

INSERT INTO `backupTypes` (`id`, `ShortName`, `LongName`) VALUES
(1, 'NORM', 'Normal'),
(2, 'DIFF', 'Differential'),
(3, 'INCR', 'Incremental');

-- --------------------------------------------------------

--
-- Table structure for table `daemonGroups`
--

CREATE TABLE IF NOT EXISTS `daemonGroups` (
`id` int(11) NOT NULL,
  `idDaemon` int(11) NOT NULL,
  `idGroup` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `daemonInfos`
--

CREATE TABLE IF NOT EXISTS `daemonInfos` (
`id` int(11) NOT NULL,
  `os` varchar(64) NOT NULL,
  `mac` char(12) NOT NULL,
  `dateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `daemonInfos`
--

INSERT INTO `daemonInfos` (`id`, `os`, `mac`, `dateAdded`) VALUES
(1, 'Win10', '000000000000', '2017-12-31 23:00:00'),
(2, 'Win10', '000000000000', '2017-12-31 23:00:00'),
(3, 'Win10', '000000000000', '2017-12-31 23:00:00'),
(4, 'Win10', '000000000000', '2017-12-31 23:00:00'),
(5, 'Win10', '000000000000', '2018-02-17 15:34:54'),
(6, 'Win10', '000000000000', '2018-02-17 15:37:52'),
(7, 'Win10', '000000000000', '2018-02-17 15:52:11'),
(8, 'Win10', '000000000000', '2018-02-10 23:00:00'),
(9, 'Win10', '000000000000', '0000-00-00 00:00:00'),
(10, 'Win10', '000000000000', '0000-00-00 00:00:00'),
(11, 'Win10', '000000000000', '0000-00-00 00:00:00');

-- --------------------------------------------------------

--
-- Table structure for table `daemonPreSharedKeys`
--

CREATE TABLE IF NOT EXISTS `daemonPreSharedKeys` (
`id` int(11) NOT NULL,
  `idUser` int(11) NOT NULL,
  `preSharedKey` char(68) NOT NULL COMMENT 'Pbkdf2',
  `expires` datetime NOT NULL,
  `used` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `daemonPreSharedKeys`
--

INSERT INTO `daemonPreSharedKeys` (`id`, `idUser`, `preSharedKey`, `expires`, `used`) VALUES
(1, -1, 'CB+bu/TW5Hh0THNcfZ1Y0w==vDuyXVPabjAcqhHyMItaIXERt2c+HugYXT3zKcnxU9A=', '2019-01-01 00:00:00', 0),
(2, -1, 'nK/TPSUHsl9P8j6YM5rOQw==nYnQc+NwaqZeLuClGgSAYfWrT8/bD2buzqbDOnv4+fY=', '2019-01-01 00:00:00', 0);

-- --------------------------------------------------------

--
-- Table structure for table `daemons`
--

CREATE TABLE IF NOT EXISTS `daemons` (
`id` int(11) NOT NULL,
  `uuid` char(36) CHARACTER SET utf8 NOT NULL,
  `idUser` int(11) NOT NULL,
  `password` char(68) CHARACTER SET utf8 NOT NULL COMMENT 'Pbkdf2',
  `idDaemonInfo` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8 COLLATE=utf8_czech_ci;

--
-- Dumping data for table `daemons`
--

INSERT INTO `daemons` (`id`, `uuid`, `idUser`, `password`, `idDaemonInfo`) VALUES
(8, '50a7cd9f-d5f9-4c40-8e0f-bfcbb21a5f0e', -1, '9MFKKS7SUgc2ypCMjVcaRw==hhBA0Us7sUd+7Y/YhU7ahC/jzTNKgCJdLoxV2IvpVvo=', 8),
(9, '2afecca5-cade-4f48-9539-28b65bbbc380', -1, 'RIsxDbcWENpx+HRLV2XTAw==A8p2Z851LXUpz3UsF2xVJsolYi6tBHYiTCr0cbCTotE=', 9),
(10, '07443701-2219-4ee0-adc6-4d94b3744d57', -1, 'gUiQGFz3ty9x43zdUIBczA==+S6vfyDnxk8lhIY2B3P/2MSr+mkhRio4xqLeCfruino=', 10),
(11, 'adcd71b3-ddba-49c8-b502-e83fadbf84c1', -1, 'rdXpzzzYgGtRMIdgNf0FIA==RVskf+TJc/348Z8Lisbs86fFO0879H+ev4Ov0ZlB0Ds=', 11);

-- --------------------------------------------------------

--
-- Table structure for table `groupPermissions`
--

CREATE TABLE IF NOT EXISTS `groupPermissions` (
`id` int(11) NOT NULL,
  `idGroup` int(11) NOT NULL,
  `idPermission` int(11) NOT NULL,
  `allow` bit(2) NOT NULL,
  `deny` bit(2) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `groupPermissions`
--

INSERT INTO `groupPermissions` (`id`, `idGroup`, `idPermission`, `allow`, `deny`) VALUES
(4, -1, -1, b'10', b'00');

-- --------------------------------------------------------

--
-- Table structure for table `groups`
--

CREATE TABLE IF NOT EXISTS `groups` (
`id` int(11) NOT NULL,
  `name` varchar(64) NOT NULL,
  `description` varchar(256) NOT NULL,
  `forDaemons` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `groups`
--

INSERT INTO `groups` (`id`, `name`, `description`, `forDaemons`) VALUES
(-1, 'DebugGroup', 'For debugging', 0);

-- --------------------------------------------------------

--
-- Table structure for table `locationCredentials`
--

CREATE TABLE IF NOT EXISTS `locationCredentials` (
`id` int(11) NOT NULL,
  `host` varchar(256) DEFAULT NULL,
  `port` int(11) DEFAULT NULL,
  `idLogonType` int(11) NOT NULL,
  `username` varchar(128) DEFAULT NULL,
  `password` char(72) DEFAULT NULL COMMENT 'aes256'
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `locationCredentials`
--

INSERT INTO `locationCredentials` (`id`, `host`, `port`, `idLogonType`, `username`, `password`) VALUES
(3, 'test.com', NULL, 1, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `locations`
--

CREATE TABLE IF NOT EXISTS `locations` (
`id` int(11) NOT NULL,
  `uri` varchar(1024) CHARACTER SET utf8 NOT NULL,
  `idProtocol` int(11) NOT NULL,
  `idLocationCredentails` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COLLATE=utf8_czech_ci;

--
-- Dumping data for table `locations`
--

INSERT INTO `locations` (`id`, `uri`, `idProtocol`, `idLocationCredentails`) VALUES
(3, 'C:\\Users\\TestUser\\Desktop\\Docs\\*', 1, 3),
(4, 'C:\\Users\\TestUser\\Desktop\\Pictures\\*', 1, 3),
(5, 'E:\\Backups', 1, 3);

-- --------------------------------------------------------

--
-- Table structure for table `logedInDaemons`
--

CREATE TABLE IF NOT EXISTS `logedInDaemons` (
  `idDaemon` int(11) NOT NULL,
  `expires` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `sessionUuid` char(38) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `logedInDaemons`
--

INSERT INTO `logedInDaemons` (`idDaemon`, `expires`, `sessionUuid`) VALUES
(8, '2018-02-21 12:07:54', '437f4a2f-4f7c-4ff7-bbac-f148580990b3');

-- --------------------------------------------------------

--
-- Table structure for table `logonTypes`
--

CREATE TABLE IF NOT EXISTS `logonTypes` (
`id` int(11) NOT NULL,
  `name` varchar(32) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `logonTypes`
--

INSERT INTO `logonTypes` (`id`, `name`) VALUES
(1, 'Anonymous');

-- --------------------------------------------------------

--
-- Table structure for table `permissions`
--

CREATE TABLE IF NOT EXISTS `permissions` (
`id` int(11) NOT NULL,
  `Name` varchar(32) NOT NULL,
  `Description` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `permissions`
--

INSERT INTO `permissions` (`id`, `Name`, `Description`) VALUES
(-1, 'Debug', 'Debugging');

-- --------------------------------------------------------

--
-- Table structure for table `protocols`
--

CREATE TABLE IF NOT EXISTS `protocols` (
`id` int(11) NOT NULL,
  `ShortName` char(3) CHARACTER SET utf8 NOT NULL,
  `LongName` varchar(32) CHARACTER SET utf8 NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_czech_ci;

--
-- Dumping data for table `protocols`
--

INSERT INTO `protocols` (`id`, `ShortName`, `LongName`) VALUES
(1, 'WND', 'Windows standard URI');

-- --------------------------------------------------------

--
-- Table structure for table `taskLocations`
--

CREATE TABLE IF NOT EXISTS `taskLocations` (
`id` int(11) NOT NULL,
  `idTask` int(11) NOT NULL,
  `idSource` int(11) NOT NULL,
  `idDestination` int(11) NOT NULL,
  `idBackupTypes` int(11) NOT NULL
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
  `interval` int(11) DEFAULT NULL COMMENT 'in seconds',
  `name` varchar(40) CHARACTER SET utf8 NOT NULL,
  `repeat` tinyint(1) NOT NULL DEFAULT '0',
  `startTime` datetime NOT NULL,
  `endTime` datetime DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_czech_ci;

--
-- Dumping data for table `times`
--

INSERT INTO `times` (`id`, `interval`, `name`, `repeat`, `startTime`, `endTime`) VALUES
(1, 86400, 'TestBackupEveryDay', 1, '2018-02-11 01:00:00', NULL),
(2, 604800, 'TestBackupNoonSunday', 1, '2018-02-11 12:00:00', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `userGroups`
--

CREATE TABLE IF NOT EXISTS `userGroups` (
`id` int(11) NOT NULL,
  `idUser` int(11) NOT NULL,
  `idGroup` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `userGroups`
--

INSERT INTO `userGroups` (`id`, `idUser`, `idGroup`) VALUES
(1, -1, -1);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE IF NOT EXISTS `users` (
`id` int(11) NOT NULL,
  `Name` varchar(100) CHARACTER SET utf8 NOT NULL,
  `Surname` varchar(100) CHARACTER SET utf8 NOT NULL,
  `password` char(68) CHARACTER SET utf8 NOT NULL COMMENT 'Pbkdf2'
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_czech_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `Name`, `Surname`, `password`) VALUES
(-1, 'Debug', '', ''),
(1, 'Foo', 'For testing', '');

-- --------------------------------------------------------

--
-- Structure for view `LogedInDaemonsView`
--
DROP TABLE IF EXISTS `LogedInDaemonsView`;

CREATE ALGORITHM=UNDEFINED DEFINER=`joskalukas`@`%` SQL SECURITY DEFINER VIEW `LogedInDaemonsView` AS select `logedInDaemons`.`idDaemon` AS `idDaemon`,`logedInDaemons`.`expires` AS `expires`,`daemons`.`id` AS `id`,`daemons`.`uuid` AS `uuid`,`daemons`.`idUser` AS `idUser`,`daemons`.`password` AS `password`,`daemons`.`idDaemonInfo` AS `idDaemonInfo` from (`logedInDaemons` join `daemons` on((`daemons`.`id` = `logedInDaemons`.`idDaemon`)));

--
-- Indexes for dumped tables
--

--
-- Indexes for table `backupTypes`
--
ALTER TABLE `backupTypes`
 ADD PRIMARY KEY (`id`);

--
-- Indexes for table `daemonGroups`
--
ALTER TABLE `daemonGroups`
 ADD PRIMARY KEY (`id`), ADD KEY `idDaemon` (`idDaemon`), ADD KEY `idGroup` (`idGroup`), ADD KEY `idDaemon_2` (`idDaemon`), ADD KEY `idGroup_2` (`idGroup`);

--
-- Indexes for table `daemonInfos`
--
ALTER TABLE `daemonInfos`
 ADD PRIMARY KEY (`id`);

--
-- Indexes for table `daemonPreSharedKeys`
--
ALTER TABLE `daemonPreSharedKeys`
 ADD PRIMARY KEY (`id`), ADD KEY `idUser` (`idUser`), ADD KEY `idUser_2` (`idUser`);

--
-- Indexes for table `daemons`
--
ALTER TABLE `daemons`
 ADD PRIMARY KEY (`id`), ADD UNIQUE KEY `uuid` (`uuid`), ADD KEY `idUser` (`idUser`), ADD KEY `idDaemonInfo` (`idDaemonInfo`), ADD KEY `idUser_2` (`idUser`), ADD KEY `idDaemonInfo_2` (`idDaemonInfo`), ADD KEY `uuid_2` (`uuid`), ADD KEY `idUser_3` (`idUser`);

--
-- Indexes for table `groupPermissions`
--
ALTER TABLE `groupPermissions`
 ADD PRIMARY KEY (`id`), ADD UNIQUE KEY `idGroup` (`idGroup`,`idPermission`), ADD KEY `idGroup_2` (`idGroup`), ADD KEY `idPermission` (`idPermission`);

--
-- Indexes for table `groups`
--
ALTER TABLE `groups`
 ADD PRIMARY KEY (`id`), ADD KEY `forDaemons` (`forDaemons`);

--
-- Indexes for table `locationCredentials`
--
ALTER TABLE `locationCredentials`
 ADD PRIMARY KEY (`id`), ADD KEY `idLogonType` (`idLogonType`);

--
-- Indexes for table `locations`
--
ALTER TABLE `locations`
 ADD PRIMARY KEY (`id`), ADD KEY `idLocationCredentails` (`idLocationCredentails`), ADD KEY `idProtocol` (`idProtocol`), ADD KEY `idProtocol_2` (`idProtocol`), ADD KEY `idLocationCredentails_2` (`idLocationCredentails`);

--
-- Indexes for table `logedInDaemons`
--
ALTER TABLE `logedInDaemons`
 ADD PRIMARY KEY (`idDaemon`), ADD KEY `sessionUuid` (`sessionUuid`);

--
-- Indexes for table `logonTypes`
--
ALTER TABLE `logonTypes`
 ADD PRIMARY KEY (`id`);

--
-- Indexes for table `permissions`
--
ALTER TABLE `permissions`
 ADD PRIMARY KEY (`id`);

--
-- Indexes for table `protocols`
--
ALTER TABLE `protocols`
 ADD PRIMARY KEY (`id`), ADD UNIQUE KEY `ShortName` (`ShortName`);

--
-- Indexes for table `taskLocations`
--
ALTER TABLE `taskLocations`
 ADD PRIMARY KEY (`id`), ADD KEY `idTask` (`idTask`,`idSource`,`idDestination`), ADD KEY `idTask_2` (`idTask`), ADD KEY `idSource` (`idSource`), ADD KEY `idDestination` (`idDestination`), ADD KEY `idBackupTypes` (`idBackupTypes`), ADD KEY `idTask_3` (`idTask`);

--
-- Indexes for table `taskLocationsTimes`
--
ALTER TABLE `taskLocationsTimes`
 ADD PRIMARY KEY (`id`), ADD KEY `idTaskLocation` (`idTaskLocation`), ADD KEY `idTime` (`idTime`), ADD KEY `idTaskLocation_2` (`idTaskLocation`);

--
-- Indexes for table `tasks`
--
ALTER TABLE `tasks`
 ADD PRIMARY KEY (`id`), ADD KEY `idDaemon` (`idDaemon`), ADD KEY `idDaemon_2` (`idDaemon`);

--
-- Indexes for table `times`
--
ALTER TABLE `times`
 ADD PRIMARY KEY (`id`);

--
-- Indexes for table `userGroups`
--
ALTER TABLE `userGroups`
 ADD PRIMARY KEY (`id`), ADD KEY `idUser` (`idUser`), ADD KEY `idUser_2` (`idUser`), ADD KEY `idUserGroup` (`idGroup`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
 ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `backupTypes`
--
ALTER TABLE `backupTypes`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT for table `daemonGroups`
--
ALTER TABLE `daemonGroups`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `daemonInfos`
--
ALTER TABLE `daemonInfos`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=12;
--
-- AUTO_INCREMENT for table `daemonPreSharedKeys`
--
ALTER TABLE `daemonPreSharedKeys`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT for table `daemons`
--
ALTER TABLE `daemons`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=12;
--
-- AUTO_INCREMENT for table `groupPermissions`
--
ALTER TABLE `groupPermissions`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=6;
--
-- AUTO_INCREMENT for table `groups`
--
ALTER TABLE `groups`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `locationCredentials`
--
ALTER TABLE `locationCredentials`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT for table `locations`
--
ALTER TABLE `locations`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=6;
--
-- AUTO_INCREMENT for table `logonTypes`
--
ALTER TABLE `logonTypes`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT for table `permissions`
--
ALTER TABLE `permissions`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
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
-- AUTO_INCREMENT for table `userGroups`
--
ALTER TABLE `userGroups`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=2;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `daemonGroups`
--
ALTER TABLE `daemonGroups`
ADD CONSTRAINT `daemonGroups_ibfk_2` FOREIGN KEY (`idGroup`) REFERENCES `groups` (`id`),
ADD CONSTRAINT `daemonGroups_ibfk_1` FOREIGN KEY (`idDaemon`) REFERENCES `daemons` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `daemonPreSharedKeys`
--
ALTER TABLE `daemonPreSharedKeys`
ADD CONSTRAINT `daemonPreSharedKeys_ibfk_1` FOREIGN KEY (`idUser`) REFERENCES `users` (`id`);

--
-- Constraints for table `daemons`
--
ALTER TABLE `daemons`
ADD CONSTRAINT `daemons_ibfk_2` FOREIGN KEY (`idDaemonInfo`) REFERENCES `daemonInfos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `daemons_ibfk_1` FOREIGN KEY (`idUser`) REFERENCES `users` (`id`);

--
-- Constraints for table `groupPermissions`
--
ALTER TABLE `groupPermissions`
ADD CONSTRAINT `groupPermissions_ibfk_1` FOREIGN KEY (`idGroup`) REFERENCES `groups` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `groupPermissions_ibfk_2` FOREIGN KEY (`idPermission`) REFERENCES `permissions` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `locationCredentials`
--
ALTER TABLE `locationCredentials`
ADD CONSTRAINT `locationCredentials_ibfk_1` FOREIGN KEY (`idLogonType`) REFERENCES `logonTypes` (`id`);

--
-- Constraints for table `locations`
--
ALTER TABLE `locations`
ADD CONSTRAINT `locations_ibfk_2` FOREIGN KEY (`idLocationCredentails`) REFERENCES `locationCredentials` (`id`),
ADD CONSTRAINT `locations_ibfk_1` FOREIGN KEY (`idProtocol`) REFERENCES `protocols` (`id`);

--
-- Constraints for table `logedInDaemons`
--
ALTER TABLE `logedInDaemons`
ADD CONSTRAINT `logedInDaemons_ibfk_1` FOREIGN KEY (`idDaemon`) REFERENCES `daemons` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `taskLocations`
--
ALTER TABLE `taskLocations`
ADD CONSTRAINT `taskLocations_ibfk_4` FOREIGN KEY (`idBackupTypes`) REFERENCES `backupTypes` (`id`) ON UPDATE CASCADE,
ADD CONSTRAINT `taskLocations_ibfk_1` FOREIGN KEY (`idTask`) REFERENCES `tasks` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `taskLocations_ibfk_2` FOREIGN KEY (`idSource`) REFERENCES `locations` (`id`) ON UPDATE CASCADE,
ADD CONSTRAINT `taskLocations_ibfk_3` FOREIGN KEY (`idDestination`) REFERENCES `locations` (`id`) ON UPDATE CASCADE;

--
-- Constraints for table `taskLocationsTimes`
--
ALTER TABLE `taskLocationsTimes`
ADD CONSTRAINT `taskLocationsTimes_ibfk_2` FOREIGN KEY (`idTime`) REFERENCES `times` (`id`) ON UPDATE CASCADE,
ADD CONSTRAINT `taskLocationsTimes_ibfk_1` FOREIGN KEY (`idTaskLocation`) REFERENCES `taskLocations` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `tasks`
--
ALTER TABLE `tasks`
ADD CONSTRAINT `tasks_ibfk_1` FOREIGN KEY (`idDaemon`) REFERENCES `daemons` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `userGroups`
--
ALTER TABLE `userGroups`
ADD CONSTRAINT `userGroups_ibfk_2` FOREIGN KEY (`idGroup`) REFERENCES `groups` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `userGroups_ibfk_1` FOREIGN KEY (`idUser`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
--
-- Database: `3b1_joskalukas_db2`
--

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
