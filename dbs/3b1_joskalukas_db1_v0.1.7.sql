-- phpMyAdmin SQL Dump
-- version 4.2.9.1
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Mar 24, 2018 at 01:37 PM
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

--
-- Dumping data for table `BackupTypes`
--

INSERT INTO `BackupTypes` (`Id`, `ShortName`, `LongName`) VALUES
(1, 'NORM', 'Normal'),
(2, 'DIFF', 'Differential'),
(3, 'INCR', 'Incremental');

-- --------------------------------------------------------

--
-- Table structure for table `DaemonGroups`
--

CREATE TABLE IF NOT EXISTS `DaemonGroups` (
`Id` int(11) NOT NULL,
  `IdDaemon` int(11) NOT NULL,
  `IdGroup` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `DaemonInfos`
--

CREATE TABLE IF NOT EXISTS `DaemonInfos` (
`Id` int(11) NOT NULL,
  `Os` varchar(64) NOT NULL,
  `Mac` char(12) NOT NULL,
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `DaemonInfos`
--

INSERT INTO `DaemonInfos` (`Id`, `Os`, `Mac`, `DateAdded`) VALUES
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
(11, 'Win10', '000000000000', '0000-00-00 00:00:00'),
(12, 'Win10', '000000000000', '0000-00-00 00:00:00');

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
  `Header` varchar(64) NOT NULL,
  `Content` varchar(512) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `DaemonPreSharedKeys`
--

CREATE TABLE IF NOT EXISTS `DaemonPreSharedKeys` (
`Id` int(11) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `PreSharedKey` char(68) NOT NULL COMMENT 'Pbkdf2',
  `Expires` datetime NOT NULL,
  `Used` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `DaemonPreSharedKeys`
--

INSERT INTO `DaemonPreSharedKeys` (`Id`, `IdUser`, `PreSharedKey`, `Expires`, `Used`) VALUES
(1, -1, 'CB+bu/TW5Hh0THNcfZ1Y0w==vDuyXVPabjAcqhHyMItaIXERt2c+HugYXT3zKcnxU9A=', '2019-01-01 00:00:00', 1),
(2, -1, 'nK/TPSUHsl9P8j6YM5rOQw==nYnQc+NwaqZeLuClGgSAYfWrT8/bD2buzqbDOnv4+fY=', '2019-01-01 00:00:00', 0);

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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `Daemons`
--

INSERT INTO `Daemons` (`Id`, `Uuid`, `IdUser`, `Password`, `IdDaemonInfo`) VALUES
(1, '50a7cd9f-d5f9-4c40-8e0f-bfcbb21a5f0e', -1, '9MFKKS7SUgc2ypCMjVcaRw==hhBA0Us7sUd+7Y/YhU7ahC/jzTNKgCJdLoxV2IvpVvo=', 1),
(2, '2afecca5-cade-4f48-9539-28b65bbbc380', -1, 'RIsxDbcWENpx+HRLV2XTAw==A8p2Z851LXUpz3UsF2xVJsolYi6tBHYiTCr0cbCTotE=', 2),
(3, '07443701-2219-4ee0-adc6-4d94b3744d57', -1, 'gUiQGFz3ty9x43zdUIBczA==+S6vfyDnxk8lhIY2B3P/2MSr+mkhRio4xqLeCfruino=', 3),
(4, 'adcd71b3-ddba-49c8-b502-e83fadbf84c1', -1, 'rdXpzzzYgGtRMIdgNf0FIA==RVskf+TJc/348Z8Lisbs86fFO0879H+ev4Ov0ZlB0Ds=', 4),
(5, '291de540-7da6-48eb-bcae-38f009141c12', -1, '1EmExnDg+ZKlkG0O22RXZA==19KUYLEpzdB2FTYcx9e47Rnql5YUN2/zeiE2K8sl7/M=', 5);

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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `GroupPermissions`
--

INSERT INTO `GroupPermissions` (`Id`, `IdGroup`, `IdPermission`) VALUES
(1, 1, 1),
(2, 2, 2);

-- --------------------------------------------------------

--
-- Table structure for table `Groups`
--

CREATE TABLE IF NOT EXISTS `Groups` (
`Id` int(11) NOT NULL,
  `Name` varchar(64) NOT NULL,
  `Description` varchar(256) NOT NULL,
  `ForDaemons` tinyint(1) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `Groups`
--

INSERT INTO `Groups` (`Id`, `Name`, `Description`, `ForDaemons`) VALUES
(-999, 'Server', 'Server', 0),
(-1, 'DebugGroup', 'For debugging', 0),
(1, 'Admins', 'Admins', 0),
(2, 'Daemons', 'For every daemon', 1);

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
  `Password` char(72) DEFAULT NULL COMMENT 'aes256'
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `LocationCredentials`
--

INSERT INTO `LocationCredentials` (`Id`, `Host`, `Port`, `IdLogonType`, `Username`, `Password`) VALUES
(1, 'test.com', NULL, 1, NULL, NULL),
(2, 'test.com/myName', 21, 2, 'myName', 'abc'),
(3, 'test.com/myName', 21, 2, 'myName', 'abc');

-- --------------------------------------------------------

--
-- Table structure for table `Locations`
--

CREATE TABLE IF NOT EXISTS `Locations` (
`Id` int(11) NOT NULL,
  `Uri` varchar(1024) NOT NULL,
  `IdProtocol` int(11) NOT NULL,
  `IdLocationCredentails` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `Locations`
--

INSERT INTO `Locations` (`Id`, `Uri`, `IdProtocol`, `IdLocationCredentails`) VALUES
(1, 'C:\\Users\\TestUser\\Desktop\\Docs\\*', 1, 1),
(2, 'C:\\Users\\TestUser\\Desktop\\Pictures\\*', 1, 1),
(3, 'E:\\Backups', 1, 1),
(4, 'test.com', 3, 2),
(5, 'test.com', 3, 3);

-- --------------------------------------------------------

--
-- Table structure for table `LogedInDaemons`
--

CREATE TABLE IF NOT EXISTS `LogedInDaemons` (
  `IdDaemon` int(11) NOT NULL,
  `Expires` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `SessionUuid` char(36) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `LogedInDaemons`
--

INSERT INTO `LogedInDaemons` (`IdDaemon`, `Expires`, `SessionUuid`) VALUES
(1, '2018-03-21 21:31:50', '538108e1-422a-4347-a1fd-9d40dd0ebf53');

-- --------------------------------------------------------

--
-- Table structure for table `LogedInUsers`
--

CREATE TABLE IF NOT EXISTS `LogedInUsers` (
  `IdUser` int(11) NOT NULL,
  `Expires` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `SessionUuid` char(36) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `LogedInUsers`
--

INSERT INTO `LogedInUsers` (`IdUser`, `Expires`, `SessionUuid`) VALUES
(-999, '2018-03-21 19:54:28', 'f3ea414b-6c90-4f41-9247-0b587921f739'),
(1, '2018-03-21 12:29:21', 'a8f4eb21-ad66-46ed-954f-f9c5d0c6ef93');

-- --------------------------------------------------------

--
-- Table structure for table `LogonTypes`
--

CREATE TABLE IF NOT EXISTS `LogonTypes` (
`Id` int(11) NOT NULL,
  `Name` varchar(32) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `LogonTypes`
--

INSERT INTO `LogonTypes` (`Id`, `Name`) VALUES
(1, 'Anonymous'),
(2, 'Normal'),
(3, 'Ask for password'),
(4, 'Interactive'),
(5, 'Account');

-- --------------------------------------------------------

--
-- Table structure for table `LogType`
--

CREATE TABLE IF NOT EXISTS `LogType` (
`Id` int(11) NOT NULL,
  `Name` varchar(16) NOT NULL,
  `Description` varchar(32) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `LogType`
--

INSERT INTO `LogType` (`Id`, `Name`, `Description`) VALUES
(1, 'Emergency', 'System unstable'),
(2, 'Alert', 'Immediate action needed'),
(3, 'Critical', 'Critical conditions'),
(4, 'Error', 'Error conditions'),
(5, 'Warning', 'Warning conditions'),
(6, 'Notification', 'Normal but significant condition'),
(7, 'Information', 'Informational messages only'),
(8, 'Debug', 'Debugging messages');

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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `Permissions`
--

INSERT INTO `Permissions` (`Id`, `Name`, `Description`) VALUES
(1, 'Skip', 'Přeskakuje kontrolu permisí - UNIVERZÁLNÍ PŘÍSTUP'),
(2, 'Login', 'Dovoluje daemonovi nebo userovy se přihlásit'),
(3, 'ManagePreShared', 'Dovoluje vytvářet a mazet předzdílené klíče'),
(4, 'ManageSelfUser', 'Dovoluje uživateli spravovat sám sebe'),
(5, 'ManageOtherUsers', 'Dovoluje  uživateli spravovat ostatní uživatele'),
(6, 'ManageSelfDaemons', 'Dovoluje daemonovi spravovat sám sebe'),
(7, 'ManageOtherDaemons', 'Dovoluje  daemonovi spravovat ostatní daemony'),
(8, 'ManagePermission', 'Přidělování a odebírání práv skupinám'),
(9, 'ManageGroups', 'Vytváření skupin a přidávání do nich uživatelů a daemonů');

-- --------------------------------------------------------

--
-- Table structure for table `Protocols`
--

CREATE TABLE IF NOT EXISTS `Protocols` (
`Id` int(11) NOT NULL,
  `ShortName` char(4) NOT NULL,
  `LongName` varchar(32) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `Protocols`
--

INSERT INTO `Protocols` (`Id`, `ShortName`, `LongName`) VALUES
(1, 'WND', 'Windows Standard URI'),
(2, 'WRD', 'Windows Remote URI'),
(3, 'FTP', 'File Transfer Protocol'),
(4, 'SFTP', 'Secure File Transfer Protocol');

-- --------------------------------------------------------

--
-- Table structure for table `TaskLocationDetails`
--

CREATE TABLE IF NOT EXISTS `TaskLocationDetails` (
`Id` int(11) NOT NULL,
  `ZipAlgorithm` varchar(32) DEFAULT NULL,
  `CompressionLevel` int(11) DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `TaskLocationDetails`
--

INSERT INTO `TaskLocationDetails` (`Id`, `ZipAlgorithm`, `CompressionLevel`) VALUES
(1, NULL, NULL),
(2, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `TaskLocationLogs`
--

CREATE TABLE IF NOT EXISTS `TaskLocationLogs` (
`Id` int(11) NOT NULL,
  `IdTaskLocation` int(11) NOT NULL,
  `IdLogType` int(11) NOT NULL,
  `Code` int(11) NOT NULL,
  `DateCreated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Header` varchar(64) NOT NULL,
  `Content` varchar(512) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `TaskLocations`
--

CREATE TABLE IF NOT EXISTS `TaskLocations` (
`Id` int(11) NOT NULL,
  `IdTask` int(11) NOT NULL,
  `IdSource` int(11) NOT NULL,
  `IdDestination` int(11) NOT NULL,
  `IdBackupTypes` int(11) NOT NULL,
  `IdTaskLocationDetails` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `TaskLocations`
--

INSERT INTO `TaskLocations` (`Id`, `IdTask`, `IdSource`, `IdDestination`, `IdBackupTypes`, `IdTaskLocationDetails`) VALUES
(1, 1, 1, 2, 1, 1);

-- --------------------------------------------------------

--
-- Table structure for table `Tasks`
--

CREATE TABLE IF NOT EXISTS `Tasks` (
`Id` int(11) NOT NULL,
  `IdDaemon` int(11) NOT NULL,
  `Name` varchar(40) NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  `LastChanged` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Při jakékoliv změně',
  `Time` int(11) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `Tasks`
--

INSERT INTO `Tasks` (`Id`, `IdDaemon`, `Name`, `Description`, `LastChanged`, `Time`) VALUES
(1, 1, 'DebugTask', 'For debugging', '2018-03-04 23:00:00', 1),
(13, 1, 'wafwaf', 'wafafwaf', '2018-03-04 23:00:00', 1),
(14, 1, 'awfwaf', NULL, '2018-03-11 23:00:00', 1);

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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `Times`
--

INSERT INTO `Times` (`Id`, `Interval`, `Name`, `Repeat`, `StartTime`, `EndTime`) VALUES
(1, 86400, 'TestBackupEveryDay', 1, '2018-02-11 01:00:00', NULL),
(2, 604800, 'TestBackupNoonSunday', 1, '2018-02-11 12:00:00', NULL),
(3, 0, 'Dneska', 0, '2018-02-26 20:49:59', '0001-01-01 00:00:00'),
(4, 604800, 'Kazdy Patek', 1, '2018-02-23 00:00:00', '0001-01-01 00:00:00');

-- --------------------------------------------------------

--
-- Table structure for table `UniversalLogs`
--

CREATE TABLE IF NOT EXISTS `UniversalLogs` (
  `Id` int(11) NOT NULL,
  `IdLogType` int(11) NOT NULL,
  `Code` int(11) NOT NULL,
  `DateCreated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Header` varchar(64) NOT NULL,
  `Content` varchar(512) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `UserGroups`
--

CREATE TABLE IF NOT EXISTS `UserGroups` (
`Id` int(11) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `IdGroup` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `UserLogs`
--

CREATE TABLE IF NOT EXISTS `UserLogs` (
`Id` int(11) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `IdLogType` int(11) NOT NULL,
  `Code` int(11) NOT NULL,
  `DateCreated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Header` varchar(64) NOT NULL,
  `Content` varchar(512) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `Users`
--

CREATE TABLE IF NOT EXISTS `Users` (
`Id` int(11) NOT NULL,
  `Nickname` varchar(100) NOT NULL,
  `FullName` varchar(100) NOT NULL,
  `Password` char(68) NOT NULL COMMENT 'Pbkdf2'
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `Users`
--

INSERT INTO `Users` (`Id`, `Nickname`, `FullName`, `Password`) VALUES
(-999, 'Server', 'Ser Ver', 'z10k0cIA04y4YIjQK9pvDw==BGHo8AN584QCtc6XbI8NGY4970EGArUNOf6xdD+iIMo='),
(-1, 'Debug', '', '543453'),
(1, 'Admin', 'ADMINISTRATOR', 'a8hL7q9GaQQDp60J5Ffvxw==pXCTaEl2U25SBBrhbE83n29WXNXZCh9W56Ug2aS7xSc='),
(2, 'Foo', 'For testing', '61234'),
(3, 'TESTJMENO', 'TESTPRIJMENI', 'TESTHESLO');

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
-- Indexes for table `DaemonLogs`
--
ALTER TABLE `DaemonLogs`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdDaemon` (`IdDaemon`);

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
-- Indexes for table `TaskLocationDetails`
--
ALTER TABLE `TaskLocationDetails`
 ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `TaskLocationLogs`
--
ALTER TABLE `TaskLocationLogs`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdTaskLocation` (`IdTaskLocation`), ADD KEY `IX_IdLogType` (`IdLogType`);

--
-- Indexes for table `TaskLocations`
--
ALTER TABLE `TaskLocations`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdTask` (`IdTask`), ADD KEY `IX_IdSource` (`IdSource`), ADD KEY `IX_IdDestination` (`IdDestination`), ADD KEY `IX_IdBackupTypes` (`IdBackupTypes`), ADD KEY `IdTaskLocationDetails` (`IdTaskLocationDetails`);

--
-- Indexes for table `Tasks`
--
ALTER TABLE `Tasks`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdDaemon` (`IdDaemon`), ADD KEY `Time` (`Time`);

--
-- Indexes for table `Times`
--
ALTER TABLE `Times`
 ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `UniversalLogs`
--
ALTER TABLE `UniversalLogs`
 ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `UserGroups`
--
ALTER TABLE `UserGroups`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdUser` (`IdUser`), ADD KEY `IX_IdGroup` (`IdGroup`);

--
-- Indexes for table `UserLogs`
--
ALTER TABLE `UserLogs`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdUser` (`IdUser`), ADD KEY `IX_IdLogType` (`IdLogType`);

--
-- Indexes for table `Users`
--
ALTER TABLE `Users`
 ADD PRIMARY KEY (`Id`), ADD UNIQUE KEY `Name` (`Nickname`);

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
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `DaemonInfos`
--
ALTER TABLE `DaemonInfos`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=13;
--
-- AUTO_INCREMENT for table `DaemonLogs`
--
ALTER TABLE `DaemonLogs`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `DaemonPreSharedKeys`
--
ALTER TABLE `DaemonPreSharedKeys`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT for table `Daemons`
--
ALTER TABLE `Daemons`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=6;
--
-- AUTO_INCREMENT for table `GroupPermissions`
--
ALTER TABLE `GroupPermissions`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT for table `Groups`
--
ALTER TABLE `Groups`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT for table `LocationCredentials`
--
ALTER TABLE `LocationCredentials`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT for table `Locations`
--
ALTER TABLE `Locations`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=6;
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
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=10;
--
-- AUTO_INCREMENT for table `Protocols`
--
ALTER TABLE `Protocols`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT for table `TaskLocationDetails`
--
ALTER TABLE `TaskLocationDetails`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT for table `TaskLocationLogs`
--
ALTER TABLE `TaskLocationLogs`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `TaskLocations`
--
ALTER TABLE `TaskLocations`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT for table `Tasks`
--
ALTER TABLE `Tasks`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=15;
--
-- AUTO_INCREMENT for table `Times`
--
ALTER TABLE `Times`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT for table `UserGroups`
--
ALTER TABLE `UserGroups`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `UserLogs`
--
ALTER TABLE `UserLogs`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `Users`
--
ALTER TABLE `Users`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=4;
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
-- Constraints for table `DaemonLogs`
--
ALTER TABLE `DaemonLogs`
ADD CONSTRAINT `DaemonLogs_FK_IdDaemon_Daemons$Id` FOREIGN KEY (`IdDaemon`) REFERENCES `Daemons` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

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
-- Constraints for table `TaskLocationLogs`
--
ALTER TABLE `TaskLocationLogs`
ADD CONSTRAINT `TaskLocationLogs_FK_IdLogType_LogType$Id` FOREIGN KEY (`IdLogType`) REFERENCES `LogType` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `TaskLocationLogs_FK_IdTaskLocation_TaskLocations$Id` FOREIGN KEY (`IdTaskLocation`) REFERENCES `TaskLocations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `TaskLocations`
--
ALTER TABLE `TaskLocations`
ADD CONSTRAINT `TaskLocations_ibfk_1` FOREIGN KEY (`IdTaskLocationDetails`) REFERENCES `TaskLocationDetails` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `TaskLocations_FK_IdBackupType_BackupType$Id` FOREIGN KEY (`IdBackupTypes`) REFERENCES `BackupTypes` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `TaskLocations_FK_IdDestination_Locations$Id` FOREIGN KEY (`IdDestination`) REFERENCES `Locations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `TaskLocations_FK_IdSource_Locations$Id` FOREIGN KEY (`IdSource`) REFERENCES `Locations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `TaskLocations_FK_IdTask_Tasks$Id` FOREIGN KEY (`IdTask`) REFERENCES `Tasks` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `Tasks`
--
ALTER TABLE `Tasks`
ADD CONSTRAINT `Tasks_ibfk_1` FOREIGN KEY (`Time`) REFERENCES `Times` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `Tasks_FK_IdDaemon_Daemons$Id` FOREIGN KEY (`IdDaemon`) REFERENCES `Daemons` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `UserGroups`
--
ALTER TABLE `UserGroups`
ADD CONSTRAINT `UserGroups_FK_IdGroup_Groups$Id` FOREIGN KEY (`IdGroup`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `UserGroups_FK_IdUser_Users$Id` FOREIGN KEY (`IdUser`) REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `UserLogs`
--
ALTER TABLE `UserLogs`
ADD CONSTRAINT `UserLogs_FK_IdLogType_LogType$Id` FOREIGN KEY (`IdLogType`) REFERENCES `LogType` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `UserLogs_FK_IdUser_Users$Id` FOREIGN KEY (`IdUser`) REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
