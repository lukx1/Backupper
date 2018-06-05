-- --------------------------------------------------------
-- Host:                         mysqlstudenti.litv.sssvt.cz
-- Server version:               5.5.55-0+deb7u1 - (Debian)
-- Server OS:                    debian-linux-gnu
-- HeidiSQL Version:             9.4.0.5125
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for table 3b1_joskalukas_db1.BackupTypes
CREATE TABLE IF NOT EXISTS `BackupTypes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ShortName` char(4) NOT NULL,
  `LongName` varchar(32) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.DaemonGroups
CREATE TABLE IF NOT EXISTS `DaemonGroups` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdDaemon` int(11) NOT NULL,
  `IdGroup` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_IdDaemon` (`IdDaemon`),
  KEY `IX_IdGroup` (`IdGroup`),
  CONSTRAINT `DaemonGroups_FK_IdDaemon_Daemons$Id` FOREIGN KEY (`IdDaemon`) REFERENCES `Daemons` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `DaemonGroups_FK_IdGroup_Groups$Id` FOREIGN KEY (`IdGroup`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.DaemonInfos
CREATE TABLE IF NOT EXISTS `DaemonInfos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Os` varchar(64) NOT NULL,
  `PCUuid` char(24) NOT NULL,
  `Mac` char(12) NOT NULL,
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Name` varchar(64) DEFAULT NULL,
  `PcName` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for view 3b1_joskalukas_db1.DaemonPerms
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `DaemonPerms` (
	`IdPermission` INT(11) NOT NULL,
	`Uuid` CHAR(36) NOT NULL COLLATE 'utf8mb4_general_ci'
) ENGINE=MyISAM;

-- Dumping structure for table 3b1_joskalukas_db1.DaemonPreSharedKeys
CREATE TABLE IF NOT EXISTS `DaemonPreSharedKeys` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdUser` int(11) NOT NULL,
  `PreSharedKey` char(68) NOT NULL COMMENT 'Pbkdf2',
  `Expires` datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
  `Used` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_IdUser` (`IdUser`),
  CONSTRAINT `DaemonPreSharedKeys_FK_IdUser_Users$Id` FOREIGN KEY (`IdUser`) REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.Daemons
CREATE TABLE IF NOT EXISTS `Daemons` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Uuid` char(36) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `Password` char(68) NOT NULL COMMENT 'Pbkdf2',
  `IdDaemonInfo` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Uuid` (`Uuid`),
  KEY `IX_IdUser` (`IdUser`),
  KEY `IX_IdDaemonInfo` (`IdDaemonInfo`),
  CONSTRAINT `Daemons_FK_IdDaemonInfo_DaemonInfos$Id` FOREIGN KEY (`IdDaemonInfo`) REFERENCES `DaemonInfos` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Daemons_FK_IdUser_Users$Id` FOREIGN KEY (`IdUser`) REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for view 3b1_joskalukas_db1.GroupEnum
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `GroupEnum` (
	`Group` VARCHAR(79) NOT NULL COLLATE 'utf8mb4_general_ci'
) ENGINE=MyISAM;

-- Dumping structure for table 3b1_joskalukas_db1.GroupPermissions
CREATE TABLE IF NOT EXISTS `GroupPermissions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdGroup` int(11) NOT NULL,
  `IdPermission` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_IdGroup$IdPermission` (`IdGroup`,`IdPermission`),
  KEY `IX_IdGroup` (`IdGroup`),
  KEY `IX_IdPermission` (`IdPermission`),
  CONSTRAINT `GroupPermissions_FK_IdGroup_Groups$Id` FOREIGN KEY (`IdGroup`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `GroupPermissions_FK_IdPermission_Permissions$Id` FOREIGN KEY (`IdPermission`) REFERENCES `Permissions` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.Groups
CREATE TABLE IF NOT EXISTS `Groups` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(64) NOT NULL,
  `Description` varchar(256) NOT NULL,
  `ForDaemons` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ForDaemons` (`ForDaemons`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.LocationCredentials
CREATE TABLE IF NOT EXISTS `LocationCredentials` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Host` varchar(256) DEFAULT NULL,
  `Port` int(11) DEFAULT '-1',
  `IdLogonType` int(11) DEFAULT NULL,
  `Username` varchar(128) DEFAULT NULL,
  `Password` varchar(344) DEFAULT NULL COMMENT 'rsa -length 344',
  PRIMARY KEY (`Id`),
  KEY `IX_IdLogonType` (`IdLogonType`),
  CONSTRAINT `LocationCredentials_FK_IdLogonType_LogonTypes$Id` FOREIGN KEY (`IdLogonType`) REFERENCES `LogonTypes` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.Locations
CREATE TABLE IF NOT EXISTS `Locations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Uri` varchar(1024) NOT NULL,
  `IdProtocol` int(11) NOT NULL,
  `IdLocationCredentails` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_IdLocationCredentails` (`IdLocationCredentails`),
  KEY `IX_IdProtocol` (`IdProtocol`),
  CONSTRAINT `Locations_FK_IdLocationCredentails_LocationCredentials$Id` FOREIGN KEY (`IdLocationCredentails`) REFERENCES `LocationCredentials` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Locations_FK_IdProtocol_Protocols$Id` FOREIGN KEY (`IdProtocol`) REFERENCES `Protocols` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.LogedInDaemons
CREATE TABLE IF NOT EXISTS `LogedInDaemons` (
  `IdDaemon` int(11) NOT NULL,
  `Expires` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `SessionUuid` char(36) NOT NULL,
  PRIMARY KEY (`IdDaemon`),
  UNIQUE KEY `IX_SessionUuid` (`SessionUuid`),
  CONSTRAINT `LogedInDaemons_FK_IdDaemon_Daemons$Id` FOREIGN KEY (`IdDaemon`) REFERENCES `Daemons` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.LogedInUsers
CREATE TABLE IF NOT EXISTS `LogedInUsers` (
  `IdUser` int(11) NOT NULL,
  `Expires` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `SessionUuid` char(36) NOT NULL,
  PRIMARY KEY (`IdUser`),
  UNIQUE KEY `IX_SessionUuid` (`SessionUuid`),
  CONSTRAINT `LogedInUsers_FK_IdUser_Users$Id` FOREIGN KEY (`IdUser`) REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.LogonTypes
CREATE TABLE IF NOT EXISTS `LogonTypes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(32) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.LogType
CREATE TABLE IF NOT EXISTS `LogType` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(16) NOT NULL,
  `Description` varchar(32) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for view 3b1_joskalukas_db1.PermissionEnum
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `PermissionEnum` (
	`Permission` VARCHAR(47) NOT NULL COLLATE 'utf8mb4_general_ci'
) ENGINE=MyISAM;

-- Dumping structure for table 3b1_joskalukas_db1.Permissions
CREATE TABLE IF NOT EXISTS `Permissions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(32) NOT NULL,
  `Description` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.Protocols
CREATE TABLE IF NOT EXISTS `Protocols` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ShortName` char(4) NOT NULL,
  `LongName` varchar(32) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `ShortName` (`ShortName`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.TaskDetails
CREATE TABLE IF NOT EXISTS `TaskDetails` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ZipAlgorithm` varchar(32) DEFAULT NULL,
  `CompressionLevel` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.TaskLocations
CREATE TABLE IF NOT EXISTS `TaskLocations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdTask` int(11) NOT NULL,
  `IdSource` int(11) NOT NULL,
  `IdDestination` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_IdTask` (`IdTask`),
  KEY `IX_IdSource` (`IdSource`),
  KEY `IX_IdDestination` (`IdDestination`),
  CONSTRAINT `TaskLocations_FK_IdDestination_Locations$Id` FOREIGN KEY (`IdDestination`) REFERENCES `Locations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `TaskLocations_FK_IdSource_Locations$Id` FOREIGN KEY (`IdSource`) REFERENCES `Locations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `TaskLocations_FK_IdTask_Tasks$Id` FOREIGN KEY (`IdTask`) REFERENCES `Tasks` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.Tasks
CREATE TABLE IF NOT EXISTS `Tasks` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdDaemon` int(11) NOT NULL,
  `Name` varchar(40) NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  `IdTaskDetails` int(11) NOT NULL,
  `IdBackupTypes` int(11) NOT NULL,
  `LastChanged` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Při jakékoliv změně',
  `ActionBefore` varchar(4096) DEFAULT NULL,
  `ActionAfter` varchar(4096) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_IdDaemon` (`IdDaemon`),
  KEY `IdTaskDetails` (`IdTaskDetails`,`IdBackupTypes`),
  KEY `IdTaskDetails_2` (`IdTaskDetails`),
  KEY `IdBackupTypes` (`IdBackupTypes`),
  CONSTRAINT `Tasks_FK_IdDaemon_Daemons$Id` FOREIGN KEY (`IdDaemon`) REFERENCES `Daemons` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Tasks_ibfk_1` FOREIGN KEY (`IdTaskDetails`) REFERENCES `TaskDetails` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Tasks_ibfk_2` FOREIGN KEY (`IdBackupTypes`) REFERENCES `BackupTypes` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.TaskTimes
CREATE TABLE IF NOT EXISTS `TaskTimes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdTask` int(11) NOT NULL,
  `IdTime` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IdTask` (`IdTask`,`IdTime`),
  KEY `IdTime` (`IdTime`),
  CONSTRAINT `TaskTimes_ibfk_1` FOREIGN KEY (`IdTask`) REFERENCES `Tasks` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `TaskTimes_ibfk_2` FOREIGN KEY (`IdTime`) REFERENCES `Times` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.Times
CREATE TABLE IF NOT EXISTS `Times` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Interval` int(11) NOT NULL DEFAULT '0' COMMENT 'seconds',
  `Name` varchar(40) NOT NULL,
  `Repeat` tinyint(1) NOT NULL DEFAULT '0',
  `StartTime` datetime NOT NULL,
  `EndTime` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.UniversalLogs
CREATE TABLE IF NOT EXISTS `UniversalLogs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdLogType` int(11) NOT NULL,
  `Code` char(36) NOT NULL,
  `DateCreated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Content` text NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `DateCreated` (`DateCreated`),
  KEY `Code` (`Code`)
) ENGINE=InnoDB AUTO_INCREMENT=777 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.UpTimes
CREATE TABLE IF NOT EXISTS `UpTimes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdSource` int(11) NOT NULL,
  `IsDaemon` tinyint(1) NOT NULL COMMENT 'True Daemon,False server',
  `DateStart` datetime NOT NULL,
  `DateEnd` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.UserGroups
CREATE TABLE IF NOT EXISTS `UserGroups` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdUser` int(11) NOT NULL,
  `IdGroup` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_IdUser` (`IdUser`),
  KEY `IX_IdGroup` (`IdGroup`),
  CONSTRAINT `UserGroups_FK_IdGroup_Groups$Id` FOREIGN KEY (`IdGroup`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `UserGroups_FK_IdUser_Users$Id` FOREIGN KEY (`IdUser`) REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.Users
CREATE TABLE IF NOT EXISTS `Users` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nickname` varchar(100) NOT NULL,
  `FullName` varchar(100) NOT NULL,
  `Password` char(68) NOT NULL COMMENT 'Pbkdf2(SHA)',
  `PublicKey` varchar(415) NOT NULL COMMENT 'RSA',
  `PrivateKey` varchar(2288) NOT NULL COMMENT 'AES+RSA',
  `Email` varchar(256) DEFAULT NULL,
  `WantsReport` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Name` (`Nickname`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for table 3b1_joskalukas_db1.WaitingForOneClick
CREATE TABLE IF NOT EXISTS `WaitingForOneClick` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdDaemonInfo` int(11) NOT NULL DEFAULT '0',
  `User` varchar(100) NOT NULL DEFAULT '0',
  `DateReceived` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Confirmed` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `FK_WaitingForOneClick_DaemonInfos` (`IdDaemonInfo`),
  CONSTRAINT `FK_WaitingForOneClick_DaemonInfos` FOREIGN KEY (`IdDaemonInfo`) REFERENCES `DaemonInfos` (`Id`) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.
-- Dumping structure for view 3b1_joskalukas_db1.DaemonPerms
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `DaemonPerms`;
CREATE ALGORITHM=UNDEFINED DEFINER=`joskalukas`@`%` SQL SECURITY DEFINER VIEW `DaemonPerms` AS select `GroupPermissions`.`IdPermission` AS `IdPermission`,`Daemons`.`Uuid` AS `Uuid` from (((`Daemons` join `DaemonGroups` on((`DaemonGroups`.`IdDaemon` = `Daemons`.`Id`))) join `Groups` on((`DaemonGroups`.`Id` = `Groups`.`Id`))) join `GroupPermissions` on((`Groups`.`Id` = `GroupPermissions`.`IdGroup`)));

-- Dumping structure for view 3b1_joskalukas_db1.GroupEnum
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `GroupEnum`;
CREATE ALGORITHM=UNDEFINED DEFINER=`joskalukas`@`%` SQL SECURITY DEFINER VIEW `GroupEnum` AS select concat(ucase(`Groups`.`Name`),' = ',`Groups`.`Id`,',') AS `Group` from `Groups`;

-- Dumping structure for view 3b1_joskalukas_db1.PermissionEnum
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `PermissionEnum`;
CREATE ALGORITHM=UNDEFINED DEFINER=`joskalukas`@`%` SQL SECURITY DEFINER VIEW `PermissionEnum` AS select concat(ucase(`Permissions`.`Name`),' = ',`Permissions`.`Id`,',') AS `Permission` from `Permissions`;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

INSERT INTO `BackupTypes` (`Id`, `ShortName`, `LongName`) VALUES
	(1, 'NORM', 'Normal'),
	(2, 'DIFF', 'Differential'),
	(3, 'INCR', 'Incremental');

INSERT INTO `Groups` (`Id`, `Name`, `Description`, `ForDaemons`) VALUES
	(-999, 'Server', 'Server', 0),
	(-998, 'DaemonAdmins', '', 1),
	(-1, 'DebugGroup', 'For debugging', 0),
	(1, 'Admins', 'Admins', 0),
	(2, 'Daemons', 'Every daemon should be in this group', 1),
	(3, 'Users', 'Every user should be in this group', 0),

	INSERT INTO `LogonTypes` (`Id`, `Name`) VALUES
	(1, 'Anonymous'),
	(2, 'Normal'),
	(3, 'Ask for password'),
	(4, 'Interactive'),
	(5, 'Account');
	
	INSERT INTO `LogType` (`Id`, `Name`, `Description`) VALUES
	(1, 'Emergency', 'System unstable'),
	(2, 'Alert', 'Immediate action needed'),
	(3, 'Critical', 'Critical conditions'),
	(4, 'Error', 'Error conditions'),
	(5, 'Warning', 'Warning conditions'),
	(6, 'Notification', 'Normal but significant condition'),
	(7, 'Information', 'Informational messages only'),
	(8, 'Debug', 'Debugging messages');
	
	INSERT INTO `Permissions` (`Id`, `Name`, `Description`) VALUES
	(1, 'Skip', 'Přeskakuje kontrolu permisí - UNIVERZÁLNÍ PŘÍSTUP'),
	(2, 'Login', 'Dovoluje daemonovi nebo userovy se přihlásit'),
	(3, 'ManagePreShared', 'Dovoluje vytvářet a mazet předzdílené klíče'),
	(4, 'ManageSelfUser', 'Dovoluje uživateli spravovat sám sebe'),
	(5, 'ManageOtherUsers', 'Dovoluje  uživateli spravovat ostatní uživatele'),
	(6, 'ManageSelfDaemons', 'Dovoluje daemonovi spravovat sám sebe'),
	(7, 'ManageOtherDaemons', 'Dovoluje  daemonovi spravovat ostatní daemony'),
	(8, 'ManagePermission', 'Přidělování a odebírání práv skupinám'),
	(9, 'ManageGroups', 'Vytváření skupin a přidávání do nich uživatelů a daemonů'),
	(10, 'DaemonFetchTasks', 'Dovoluje daemonovi požádat o svoje tasky'),
	(11, 'ManageTimes', 'Dovoluje uživateli spravovat časy'),
	(12, 'ManageLocations', 'Dovoluje uživateli spravovat lokace'),
	(13, 'ManageCredentials', 'Dovoluje uživateli spravovat pověření'),
	(14, 'ManageServerStatus', 'Dovoluje uživateli spravovat stav serveru'),
	(15, 'ManageLogs', 'Dovoluje uživateli spravovat logy'),
	(16, 'ManagePower', 'Dovoluje uživateli restartovat server apod.'),
	(17, 'ManageEmail', 'Dovoluje uživateli si nastavit mail');
	
	INSERT INTO `Protocols` (`Id`, `ShortName`, `LongName`) VALUES
	(1, 'WND', 'Windows Standard URI'),
	(2, 'WRD', 'Windows Remote URI'),
	(3, 'FTP', 'File Transfer Protocol'),
	(4, 'SFTP', 'Secure File Transfer Protocol'),
	(5, 'MYSQ', 'MySQL');
	
INSERT INTO `GroupPermissions` (`Id`, `IdGroup`, `IdPermission`) VALUES
	(12, -999, 1),
	(6, -998, 1),
	(7, 1, 1),
	(2, 2, 2),
	(3, 2, 10),
	(4, 3, 2);
	
	INSERT INTO `UserGroups` (`Id`, `IdUser`, `IdGroup`) VALUES
	(2, -999, 3),
	(11, 1, 1),
	(16, -999, 1),
	(17, -999, -999)
	
	INSERT INTO `Users` (`Id`, `Nickname`, `FullName`, `Password`, `PublicKey`, `PrivateKey`, `Email`, `WantsReport`) VALUES
	(-999, 'Server', 'SerVer', 'dGBA9pqGj4JVRojAm1CsjA==gZkU/WCPdImJOXijXQhcTXqyh83IGbPs7VWTKjImZGU=', '<RSAKeyValue><Modulus>lin8uskh9oscMrG2KR9YEOcMfatVPGDKonRjNRc5CVnzlWWJvsFMMXAqXQ3bNanAeHX8K7n3//d9CN+I38luTytX4Mk7CniK4ripFU3v/fEDLnf5TD/HIbyeoaChGhh1wjoWdfw58mmCILoRpFPXxKeP9mI4CbBQRkpYWCvDe5QonDfMuuPTTu9BU+hLC9xUl3/9svhu067m/Ial5mqeZg7yA2MsPFJV5MAeNADYvH4CAi8S7KqKQ+CjnYexY1gZ8VBpstJaaQNs08LUzNOno+ApOkOlmt77kEfNSU3VNNryxqnI8ffYpcJ2O292GXNVAY8p3XoARu6ES7fPiN7cLQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>', 'QVllLtAEBTZuQTMyZvit5A==It0/rUGq5qMOLaWhVLkl3w==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', NULL, 0),
	(-1, 'Debug', '', 'wogetkZGO/zNHG0kN/F9/A==Q8Ut93bMzhWRhNulMIsNbtV7HAor2mKN86p4j1nL3AU=', '<RSAKeyValue><Modulus>ufEenaLE4+bcF9FAT/iiqtYGYYRI6ZNeEwNYWw4Hs7l1XJm21C/QR1FckxUXkMxpoTMhxx+hMuulNBpBhPWTVACRO9m/mgo+BJE0DmeriXRhUdMCqTXoObOCZYUr+Q3XRru+f5X5Dd49G/twaIX89EILjriGW2I6khu8rtxIUnq4WCcoAY2MWvve9zsiWz5xNJrNg1YTFQz7DfI8gmkkRFc7VLHnE7ssDZ8UWl7S3GE4Du+nGK7YlATwj5gtoMBpZxY57RddqXymwe1j04yNy37GFEker6V0aRXv35650KZk5vMmXMg0XKJc1kbnqocT93CNqPP+x1nRJ+UVIM3+aQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>', '291Z17ZP8O/+gEj8eR1Hvw==DlUQ6tUa/umsBgvzVPEgxA==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', NULL, 0),
	