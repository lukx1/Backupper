-- phpMyAdmin SQL Dump
-- version 4.2.9.1
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: May 29, 2018 at 06:25 PM
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
-- Table structure for table `BackupTypes`
--

CREATE TABLE IF NOT EXISTS `BackupTypes` (
`Id` int(11) NOT NULL,
  `ShortName` char(4) NOT NULL,
  `LongName` varchar(32) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `DaemonGroups`
--

CREATE TABLE IF NOT EXISTS `DaemonGroups` (
`Id` int(11) NOT NULL,
  `IdDaemon` int(11) NOT NULL,
  `IdGroup` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `DaemonInfos`
--

CREATE TABLE IF NOT EXISTS `DaemonInfos` (
`Id` int(11) NOT NULL,
  `Os` varchar(64) NOT NULL,
  `PCUuid` char(24) NOT NULL,
  `Mac` char(12) NOT NULL,
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Stand-in structure for view `DaemonPerms`
--
CREATE TABLE IF NOT EXISTS `DaemonPerms` (
`IdPermission` int(11)
,`Uuid` char(36)
);
-- --------------------------------------------------------

--
-- Table structure for table `DaemonPreSharedKeys`
--

CREATE TABLE IF NOT EXISTS `DaemonPreSharedKeys` (
`Id` int(11) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `PreSharedKey` char(68) NOT NULL COMMENT 'Pbkdf2',
  `Expires` datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
  `Used` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `Daemons`
--

CREATE TABLE IF NOT EXISTS `Daemons` (
`Id` int(11) NOT NULL,
  `Uuid` char(36) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `Password` char(68) NOT NULL COMMENT 'Pbkdf2',
  `IdDaemonInfo` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Stand-in structure for view `GroupEnum`
--
CREATE TABLE IF NOT EXISTS `GroupEnum` (
`Group` varchar(79)
);
-- --------------------------------------------------------

--
-- Table structure for table `GroupPermissions`
--

CREATE TABLE IF NOT EXISTS `GroupPermissions` (
`Id` int(11) NOT NULL,
  `IdGroup` int(11) NOT NULL,
  `IdPermission` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `Groups`
--

CREATE TABLE IF NOT EXISTS `Groups` (
`Id` int(11) NOT NULL,
  `Name` varchar(64) NOT NULL,
  `Description` varchar(256) NOT NULL,
  `ForDaemons` tinyint(1) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `LocationCredentials`
--

CREATE TABLE IF NOT EXISTS `LocationCredentials` (
`Id` int(11) NOT NULL,
  `Host` varchar(256) DEFAULT NULL,
  `Port` int(11) DEFAULT '-1',
  `IdLogonType` int(11) DEFAULT NULL,
  `Username` varchar(128) DEFAULT NULL,
  `Password` varchar(344) DEFAULT NULL COMMENT 'rsa -length 344'
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `Locations`
--

CREATE TABLE IF NOT EXISTS `Locations` (
`Id` int(11) NOT NULL,
  `Uri` varchar(1024) NOT NULL,
  `IdProtocol` int(11) NOT NULL,
  `IdLocationCredentails` int(11) DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `LogedInDaemons`
--

CREATE TABLE IF NOT EXISTS `LogedInDaemons` (
  `IdDaemon` int(11) NOT NULL,
  `Expires` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `SessionUuid` char(36) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `LogedInUsers`
--

CREATE TABLE IF NOT EXISTS `LogedInUsers` (
  `IdUser` int(11) NOT NULL,
  `Expires` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `SessionUuid` char(36) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `LogonTypes`
--

CREATE TABLE IF NOT EXISTS `LogonTypes` (
`Id` int(11) NOT NULL,
  `Name` varchar(32) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `LogType`
--

CREATE TABLE IF NOT EXISTS `LogType` (
`Id` int(11) NOT NULL,
  `Name` varchar(16) NOT NULL,
  `Description` varchar(32) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Stand-in structure for view `PermissionEnum`
--
CREATE TABLE IF NOT EXISTS `PermissionEnum` (
`Permission` varchar(47)
);
-- --------------------------------------------------------

--
-- Table structure for table `Permissions`
--

CREATE TABLE IF NOT EXISTS `Permissions` (
`Id` int(11) NOT NULL,
  `Name` varchar(32) NOT NULL,
  `Description` varchar(256) DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `Protocols`
--

CREATE TABLE IF NOT EXISTS `Protocols` (
`Id` int(11) NOT NULL,
  `ShortName` char(4) NOT NULL,
  `LongName` varchar(32) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `TaskDetails`
--

CREATE TABLE IF NOT EXISTS `TaskDetails` (
`Id` int(11) NOT NULL,
  `ZipAlgorithm` varchar(32) DEFAULT NULL,
  `CompressionLevel` int(11) DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `TaskLocations`
--

CREATE TABLE IF NOT EXISTS `TaskLocations` (
`Id` int(11) NOT NULL,
  `IdTask` int(11) NOT NULL,
  `IdSource` int(11) NOT NULL,
  `IdDestination` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `Tasks`
--

CREATE TABLE IF NOT EXISTS `Tasks` (
`Id` int(11) NOT NULL,
  `IdDaemon` int(11) NOT NULL,
  `Name` varchar(40) NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  `IdTaskDetails` int(11) NOT NULL,
  `IdBackupTypes` int(11) NOT NULL,
  `LastChanged` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Při jakékoliv změně',
  `ActionBefore` varchar(4096) DEFAULT NULL,
  `ActionAfter` varchar(4096) DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `TaskTimes`
--

CREATE TABLE IF NOT EXISTS `TaskTimes` (
`Id` int(11) NOT NULL,
  `IdTask` int(11) NOT NULL,
  `IdTime` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `Times`
--

CREATE TABLE IF NOT EXISTS `Times` (
`Id` int(11) NOT NULL,
  `Interval` int(11) NOT NULL DEFAULT '0' COMMENT 'seconds',
  `Name` varchar(40) NOT NULL,
  `Repeat` tinyint(1) NOT NULL DEFAULT '0',
  `StartTime` datetime NOT NULL,
  `EndTime` datetime DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `UniversalLogs`
--

CREATE TABLE IF NOT EXISTS `UniversalLogs` (
`Id` int(11) NOT NULL,
  `IdLogType` int(11) NOT NULL,
  `Code` char(36) NOT NULL,
  `DateCreated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Content` text NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=646 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `UpTimes`
--

CREATE TABLE IF NOT EXISTS `UpTimes` (
`Id` int(11) NOT NULL,
  `IdSource` int(11) NOT NULL,
  `IsDaemon` tinyint(1) NOT NULL COMMENT 'True Daemon,False server',
  `DateStart` datetime NOT NULL,
  `DateEnd` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `UserGroups`
--

CREATE TABLE IF NOT EXISTS `UserGroups` (
`Id` int(11) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `IdGroup` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `Users`
--

CREATE TABLE IF NOT EXISTS `Users` (
`Id` int(11) NOT NULL,
  `Nickname` varchar(100) NOT NULL,
  `FullName` varchar(100) NOT NULL,
  `Password` char(68) NOT NULL COMMENT 'Pbkdf2(SHA)',
  `PublicKey` varchar(415) NOT NULL COMMENT 'RSA',
  `PrivateKey` varchar(2288) NOT NULL COMMENT 'AES+RSA'
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `WaitingForOneClick`
--

CREATE TABLE IF NOT EXISTS `WaitingForOneClick` (
`Id` int(11) NOT NULL,
  `IdDaemonInfo` int(11) NOT NULL DEFAULT '0',
  `User` varchar(100) NOT NULL DEFAULT '0',
  `DateReceived` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Confirmed` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Structure for view `DaemonPerms`
--
DROP TABLE IF EXISTS `DaemonPerms`;

CREATE ALGORITHM=UNDEFINED DEFINER=`joskalukas`@`%` SQL SECURITY DEFINER VIEW `DaemonPerms` AS select `GroupPermissions`.`IdPermission` AS `IdPermission`,`Daemons`.`Uuid` AS `Uuid` from (((`Daemons` join `DaemonGroups` on((`DaemonGroups`.`IdDaemon` = `Daemons`.`Id`))) join `Groups` on((`DaemonGroups`.`Id` = `Groups`.`Id`))) join `GroupPermissions` on((`Groups`.`Id` = `GroupPermissions`.`IdGroup`)));

-- --------------------------------------------------------

--
-- Structure for view `GroupEnum`
--
DROP TABLE IF EXISTS `GroupEnum`;

CREATE ALGORITHM=UNDEFINED DEFINER=`joskalukas`@`%` SQL SECURITY DEFINER VIEW `GroupEnum` AS select concat(ucase(`Groups`.`Name`),' = ',`Groups`.`Id`,',') AS `Group` from `Groups`;

-- --------------------------------------------------------

--
-- Structure for view `PermissionEnum`
--
DROP TABLE IF EXISTS `PermissionEnum`;

CREATE ALGORITHM=UNDEFINED DEFINER=`joskalukas`@`%` SQL SECURITY DEFINER VIEW `PermissionEnum` AS select concat(ucase(`Permissions`.`Name`),' = ',`Permissions`.`Id`,',') AS `Permission` from `Permissions`;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `BackupTypes`
--
ALTER TABLE `BackupTypes`
 ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `DaemonGroups`
--
ALTER TABLE `DaemonGroups`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdDaemon` (`IdDaemon`), ADD KEY `IX_IdGroup` (`IdGroup`);

--
-- Indexes for table `DaemonInfos`
--
ALTER TABLE `DaemonInfos`
 ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `DaemonPreSharedKeys`
--
ALTER TABLE `DaemonPreSharedKeys`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdUser` (`IdUser`);

--
-- Indexes for table `Daemons`
--
ALTER TABLE `Daemons`
 ADD PRIMARY KEY (`Id`), ADD UNIQUE KEY `IX_Uuid` (`Uuid`), ADD KEY `IX_IdUser` (`IdUser`), ADD KEY `IX_IdDaemonInfo` (`IdDaemonInfo`);

--
-- Indexes for table `GroupPermissions`
--
ALTER TABLE `GroupPermissions`
 ADD PRIMARY KEY (`Id`), ADD UNIQUE KEY `IX_IdGroup$IdPermission` (`IdGroup`,`IdPermission`), ADD KEY `IX_IdGroup` (`IdGroup`), ADD KEY `IX_IdPermission` (`IdPermission`);

--
-- Indexes for table `Groups`
--
ALTER TABLE `Groups`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_ForDaemons` (`ForDaemons`);

--
-- Indexes for table `LocationCredentials`
--
ALTER TABLE `LocationCredentials`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdLogonType` (`IdLogonType`);

--
-- Indexes for table `Locations`
--
ALTER TABLE `Locations`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdLocationCredentails` (`IdLocationCredentails`), ADD KEY `IX_IdProtocol` (`IdProtocol`);

--
-- Indexes for table `LogedInDaemons`
--
ALTER TABLE `LogedInDaemons`
 ADD PRIMARY KEY (`IdDaemon`), ADD UNIQUE KEY `IX_SessionUuid` (`SessionUuid`);

--
-- Indexes for table `LogedInUsers`
--
ALTER TABLE `LogedInUsers`
 ADD PRIMARY KEY (`IdUser`), ADD UNIQUE KEY `IX_SessionUuid` (`SessionUuid`);

--
-- Indexes for table `LogonTypes`
--
ALTER TABLE `LogonTypes`
 ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `LogType`
--
ALTER TABLE `LogType`
 ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `Permissions`
--
ALTER TABLE `Permissions`
 ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `Protocols`
--
ALTER TABLE `Protocols`
 ADD PRIMARY KEY (`Id`), ADD UNIQUE KEY `ShortName` (`ShortName`);

--
-- Indexes for table `TaskDetails`
--
ALTER TABLE `TaskDetails`
 ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `TaskLocations`
--
ALTER TABLE `TaskLocations`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdTask` (`IdTask`), ADD KEY `IX_IdSource` (`IdSource`), ADD KEY `IX_IdDestination` (`IdDestination`);

--
-- Indexes for table `Tasks`
--
ALTER TABLE `Tasks`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdDaemon` (`IdDaemon`), ADD KEY `IdTaskDetails` (`IdTaskDetails`,`IdBackupTypes`), ADD KEY `IdTaskDetails_2` (`IdTaskDetails`), ADD KEY `IdBackupTypes` (`IdBackupTypes`);

--
-- Indexes for table `TaskTimes`
--
ALTER TABLE `TaskTimes`
 ADD PRIMARY KEY (`Id`), ADD KEY `IdTask` (`IdTask`,`IdTime`), ADD KEY `IdTime` (`IdTime`);

--
-- Indexes for table `Times`
--
ALTER TABLE `Times`
 ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `UniversalLogs`
--
ALTER TABLE `UniversalLogs`
 ADD PRIMARY KEY (`Id`), ADD KEY `DateCreated` (`DateCreated`), ADD KEY `Code` (`Code`);

--
-- Indexes for table `UpTimes`
--
ALTER TABLE `UpTimes`
 ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `UserGroups`
--
ALTER TABLE `UserGroups`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdUser` (`IdUser`), ADD KEY `IX_IdGroup` (`IdGroup`);

--
-- Indexes for table `Users`
--
ALTER TABLE `Users`
 ADD PRIMARY KEY (`Id`), ADD UNIQUE KEY `Name` (`Nickname`);

--
-- Indexes for table `WaitingForOneClick`
--
ALTER TABLE `WaitingForOneClick`
 ADD PRIMARY KEY (`Id`), ADD KEY `FK_WaitingForOneClick_DaemonInfos` (`IdDaemonInfo`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `BackupTypes`
--
ALTER TABLE `BackupTypes`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT for table `DaemonGroups`
--
ALTER TABLE `DaemonGroups`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=25;
--
-- AUTO_INCREMENT for table `DaemonInfos`
--
ALTER TABLE `DaemonInfos`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=34;
--
-- AUTO_INCREMENT for table `DaemonPreSharedKeys`
--
ALTER TABLE `DaemonPreSharedKeys`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT for table `Daemons`
--
ALTER TABLE `Daemons`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=27;
--
-- AUTO_INCREMENT for table `GroupPermissions`
--
ALTER TABLE `GroupPermissions`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=13;
--
-- AUTO_INCREMENT for table `Groups`
--
ALTER TABLE `Groups`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=6;
--
-- AUTO_INCREMENT for table `LocationCredentials`
--
ALTER TABLE `LocationCredentials`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT for table `Locations`
--
ALTER TABLE `Locations`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=11;
--
-- AUTO_INCREMENT for table `LogonTypes`
--
ALTER TABLE `LogonTypes`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=6;
--
-- AUTO_INCREMENT for table `LogType`
--
ALTER TABLE `LogType`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=9;
--
-- AUTO_INCREMENT for table `Permissions`
--
ALTER TABLE `Permissions`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=18;
--
-- AUTO_INCREMENT for table `Protocols`
--
ALTER TABLE `Protocols`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT for table `TaskDetails`
--
ALTER TABLE `TaskDetails`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=10;
--
-- AUTO_INCREMENT for table `TaskLocations`
--
ALTER TABLE `TaskLocations`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=13;
--
-- AUTO_INCREMENT for table `Tasks`
--
ALTER TABLE `Tasks`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=22;
--
-- AUTO_INCREMENT for table `TaskTimes`
--
ALTER TABLE `TaskTimes`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=15;
--
-- AUTO_INCREMENT for table `Times`
--
ALTER TABLE `Times`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=8;
--
-- AUTO_INCREMENT for table `UniversalLogs`
--
ALTER TABLE `UniversalLogs`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=646;
--
-- AUTO_INCREMENT for table `UpTimes`
--
ALTER TABLE `UpTimes`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `UserGroups`
--
ALTER TABLE `UserGroups`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=18;
--
-- AUTO_INCREMENT for table `Users`
--
ALTER TABLE `Users`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=9;
--
-- AUTO_INCREMENT for table `WaitingForOneClick`
--
ALTER TABLE `WaitingForOneClick`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=22;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `DaemonGroups`
--
ALTER TABLE `DaemonGroups`
ADD CONSTRAINT `DaemonGroups_FK_IdDaemon_Daemons$Id` FOREIGN KEY (`IdDaemon`) REFERENCES `Daemons` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `DaemonGroups_FK_IdGroup_Groups$Id` FOREIGN KEY (`IdGroup`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `DaemonPreSharedKeys`
--
ALTER TABLE `DaemonPreSharedKeys`
ADD CONSTRAINT `DaemonPreSharedKeys_FK_IdUser_Users$Id` FOREIGN KEY (`IdUser`) REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `Daemons`
--
ALTER TABLE `Daemons`
ADD CONSTRAINT `Daemons_FK_IdDaemonInfo_DaemonInfos$Id` FOREIGN KEY (`IdDaemonInfo`) REFERENCES `DaemonInfos` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `Daemons_FK_IdUser_Users$Id` FOREIGN KEY (`IdUser`) REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `GroupPermissions`
--
ALTER TABLE `GroupPermissions`
ADD CONSTRAINT `GroupPermissions_FK_IdGroup_Groups$Id` FOREIGN KEY (`IdGroup`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `GroupPermissions_FK_IdPermission_Permissions$Id` FOREIGN KEY (`IdPermission`) REFERENCES `Permissions` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `LocationCredentials`
--
ALTER TABLE `LocationCredentials`
ADD CONSTRAINT `LocationCredentials_FK_IdLogonType_LogonTypes$Id` FOREIGN KEY (`IdLogonType`) REFERENCES `LogonTypes` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `Locations`
--
ALTER TABLE `Locations`
ADD CONSTRAINT `Locations_FK_IdLocationCredentails_LocationCredentials$Id` FOREIGN KEY (`IdLocationCredentails`) REFERENCES `LocationCredentials` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `Locations_FK_IdProtocol_Protocols$Id` FOREIGN KEY (`IdProtocol`) REFERENCES `Protocols` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `LogedInDaemons`
--
ALTER TABLE `LogedInDaemons`
ADD CONSTRAINT `LogedInDaemons_FK_IdDaemon_Daemons$Id` FOREIGN KEY (`IdDaemon`) REFERENCES `Daemons` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `LogedInUsers`
--
ALTER TABLE `LogedInUsers`
ADD CONSTRAINT `LogedInUsers_FK_IdUser_Users$Id` FOREIGN KEY (`IdUser`) REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `TaskLocations`
--
ALTER TABLE `TaskLocations`
ADD CONSTRAINT `TaskLocations_FK_IdDestination_Locations$Id` FOREIGN KEY (`IdDestination`) REFERENCES `Locations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `TaskLocations_FK_IdSource_Locations$Id` FOREIGN KEY (`IdSource`) REFERENCES `Locations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `TaskLocations_FK_IdTask_Tasks$Id` FOREIGN KEY (`IdTask`) REFERENCES `Tasks` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `Tasks`
--
ALTER TABLE `Tasks`
ADD CONSTRAINT `Tasks_FK_IdDaemon_Daemons$Id` FOREIGN KEY (`IdDaemon`) REFERENCES `Daemons` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `Tasks_ibfk_1` FOREIGN KEY (`IdTaskDetails`) REFERENCES `TaskDetails` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `Tasks_ibfk_2` FOREIGN KEY (`IdBackupTypes`) REFERENCES `BackupTypes` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `TaskTimes`
--
ALTER TABLE `TaskTimes`
ADD CONSTRAINT `TaskTimes_ibfk_1` FOREIGN KEY (`IdTask`) REFERENCES `Tasks` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `TaskTimes_ibfk_2` FOREIGN KEY (`IdTime`) REFERENCES `Times` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `UserGroups`
--
ALTER TABLE `UserGroups`
ADD CONSTRAINT `UserGroups_FK_IdGroup_Groups$Id` FOREIGN KEY (`IdGroup`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `UserGroups_FK_IdUser_Users$Id` FOREIGN KEY (`IdUser`) REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `WaitingForOneClick`
--
ALTER TABLE `WaitingForOneClick`
ADD CONSTRAINT `FK_WaitingForOneClick_DaemonInfos` FOREIGN KEY (`IdDaemonInfo`) REFERENCES `DaemonInfos` (`Id`) ON DELETE NO ACTION ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
