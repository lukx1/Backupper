SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";
SET default_storage_engine=InnoDB;

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
DROP DATABASE IF EXISTS `3b1_joskalukas_db1`;
CREATE DATABASE IF NOT EXISTS `3b1_joskalukas_db1`
	DEFAULT CHARACTER SET utf8mb4
	COLLATE utf8mb4_general_ci;
USE `3b1_joskalukas_db1`;

-- TABLE DEFINITIONS
DROP TABLE IF EXISTS `BackupTypes`;
CREATE TABLE IF NOT EXISTS `BackupTypes`
(
	`Id`		INT(11) NOT NULL,
	`ShortName`	CHAR(4) NOT NULL,
	`LongName`	VARCHAR(32) NOT NULL
);

DROP TABLE IF EXISTS `DaemonGroups`;
CREATE TABLE IF NOT EXISTS `DaemonGroups` (
	`Id`        INT(11) NOT NULL,
	`IdDaemon`  INT(11) DEFAULT NULL,
	`IdGroup`   INT(11) DEFAULT NULL
);

DROP TABLE IF EXISTS `DaemonInfos`;
CREATE TABLE IF NOT EXISTS `DaemonInfos` (
	`Id`		INT(11) NOT NULL,
	`Os`		VARCHAR(64) NOT NULL,
	`Mac`		CHAR(12) NOT NULL,
	`DateAdded`	TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

DROP TABLE IF EXISTS `DaemonLogs`;
CREATE TABLE IF NOT EXISTS `DaemonLogs` (
	`Id`			INT(11) NOT NULL,
	`IdDaemon`		INT(11) NOT NULL,
	`IdLogType`		INT(11) NOT NULL,
	`Code`			INT(11) NOT NULL,
	`DateCreated`	TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	`ShortText`		VARCHAR(64) NOT NULL,
	`LongText`		VARCHAR(512) NOT NULL
);

DROP TABLE IF EXISTS `DaemonPreSharedKeys`;
CREATE TABLE IF NOT EXISTS `DaemonPreSharedKeys` (
	`Id`			INT(11) NOT NULL,
	`IdUser`		INT(11) NOT NULL,
	`PreSharedKey`	CHAR(68) NOT NULL COMMENT 'Pbkdf2',
	`Expires`		DATETIME NOT NULL,
	`Used`			TINYINT(1) NOT NULL DEFAULT '0'
);

DROP TABLE IF EXISTS `Daemons`;
CREATE TABLE IF NOT EXISTS `Daemons` (
	`Id`			INT(11) NOT NULL,
	`Uuid`			CHAR(36) NOT NULL,
	`IdUser`		INT(11) NOT NULL,
	`Password`		CHAR(68) NOT NULL COMMENT 'Pbkdf2',
	`IdDaemonInfo`	INT(11) NOT NULL
);

DROP TABLE IF EXISTS `GroupPermissions`;
CREATE TABLE IF NOT EXISTS `GroupPermissions` (
	`Id`			INT(11) NOT NULL,
	`IdGroup`		INT(11) NOT NULL,
	`IdPermission`	INT(11) NOT NULL
);

DROP TABLE IF EXISTS `Groups`;
CREATE TABLE IF NOT EXISTS `Groups` (
	`Id`			INT(11) NOT NULL,
	`Name`			VARCHAR(64) NOT NULL,
	`Description`	VARCHAR(256) NOT NULL,
	`ForDaemons`	TINYINT(1) NOT NULL
);

DROP TABLE IF EXISTS `LocationCredentials`;
CREATE TABLE IF NOT EXISTS `LocationCredentials` (
	`Id`			INT(11) NOT NULL,
	`Host`			VARCHAR(256) DEFAULT NULL,
	`Port`			INT(11) DEFAULT NULL,
	`IdLogonType`	INT(11) DEFAULT NULL,
	`Username`		VARCHAR(128) DEFAULT NULL,
	`Password`		CHAR(72) DEFAULT NULL COMMENT 'aes256'
);

DROP TABLE IF EXISTS `Locations`;
CREATE TABLE IF NOT EXISTS `Locations` (
	`Id`					INT(11) NOT NULL,
	`Uri`					VARCHAR(1024) NOT NULL,
	`IdProtocol`			INT(11) DEFAULT NULL,
	`IdLocationCredentails`	INT(11) DEFAULT NULL
);

DROP TABLE IF EXISTS `LogedInDaemons`;
CREATE TABLE IF NOT EXISTS `LogedInDaemons` (
	`IdDaemon`		INT(11) NOT NULL,
	`Expires`		TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	`SessionUuid`	CHAR(36) NOT NULL
);

DROP TABLE IF EXISTS `LogedInUsers`;
CREATE TABLE IF NOT EXISTS `LogedInUsers` (
	`IdUser`		INT(11) NOT NULL,
	`Expires`		TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	`SessionUuid`	CHAR(36) NOT NULL
);

DROP TABLE IF EXISTS `LogonTypes`;
CREATE TABLE IF NOT EXISTS `LogonTypes` (
	`Id`	INT(11) NOT NULL,
	`Name`	VARCHAR(32) NOT NULL
);

DROP TABLE IF EXISTS `LogType`;
CREATE TABLE IF NOT EXISTS `LogType` (
	`Id` INT(11) NOT NULL,
	`Name` VARCHAR(16) NOT NULL,
	`Description` VARCHAR(32) NOT NULL
);

DROP TABLE IF EXISTS `Permissions`;
CREATE TABLE IF NOT EXISTS `Permissions` (
	`Id`			INT(11) NOT NULL,
	`Name`			VARCHAR(32) NOT NULL,
	`Description`	VARCHAR(256) DEFAULT NULL
);

DROP TABLE IF EXISTS `Protocols`;
CREATE TABLE IF NOT EXISTS `Protocols` (
	`Id`		INT(11) NOT NULL,
	`ShortName`	CHAR(4) NOT NULL,
	`LongName`	VARCHAR(32) NOT NULL
);

DROP TABLE IF EXISTS `TaskLocationLogs`;
CREATE TABLE IF NOT EXISTS `TaskLocationLogs` (
	`Id`				INT(11) NOT NULL,
	`IdTaskLocation`	INT(11) DEFAULT NULL,
	`IdLogType`			INT(11) DEFAULT NULL,
	`Code`				INT(11) NOT NULL,
	`DateCreated` 		TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	`ShortText`			VARCHAR(64) NOT NULL,
	`LongText`			VARCHAR(512) NOT NULL
);

DROP TABLE IF EXISTS `TaskLocations`;
CREATE TABLE IF NOT EXISTS `TaskLocations` (
	`Id`			INT(11) NOT NULL,
	`IdTask`		INT(11) DEFAULT NULL,
	`IdSource`		INT(11) DEFAULT NULL,
	`IdDestination` INT(11) DEFAULT NULL,
	`IdBackupTypes` INT(11) DEFAULT NULL
);

DROP TABLE IF EXISTS `TaskLocationsTimes`;
CREATE TABLE IF NOT EXISTS `TaskLocationsTimes` (
	`Id`				INT(11) NOT NULL,
	`IdTaskLocation`	INT(11) DEFAULT NULL,
	`IdTime`			INT(11) DEFAULT NULL
);

DROP TABLE IF EXISTS `Tasks`;
CREATE TABLE IF NOT EXISTS `Tasks` (
	`Id`			INT(11) NOT NULL,
	`IdDaemon`		INT(11) DEFAULT NULL,
	`Name`			VARCHAR(40) NOT NULL,
	`Description`	VARCHAR(200) DEFAULT NULL
);

DROP TABLE IF EXISTS `Times`;
CREATE TABLE IF NOT EXISTS `Times` (
	`Id`		INT(11) NOT NULL,
	`Interval`	INT(11) DEFAULT NULL COMMENT 'seconds',
	`Name`		VARCHAR(40) NOT NULL,
	`Repeat`	TINYINT(1) NOT NULL DEFAULT '0',
	`StartTime`	DATETIME NOT NULL,
	`EndTime`	DATETIME DEFAULT NULL
);

DROP TABLE IF EXISTS `UserGroups`;
CREATE TABLE IF NOT EXISTS `UserGroups` (
	`Id`		INT(11) NOT NULL,
	`IdUser`	INT(11) NOT NULL,
	`IdGroup`	INT(11) NOT NULL
);

DROP TABLE IF EXISTS `UserLogs`;
CREATE TABLE IF NOT EXISTS `UserLogs` (
	`Id`			INT(11) NOT NULL,
	`IdUser`		INT(11) NOT NULL,
	`IdLogType`		INT(11) NOT NULL,
	`Code`			INT(11) NOT NULL,
	`DateCreated`	TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	`ShortText`		VARCHAR(64) NOT NULL,
	`LongText`		VARCHAR(512) NOT NULL
);

DROP TABLE IF EXISTS `Users`;
CREATE TABLE IF NOT EXISTS `Users` (
	`Id`		INT(11) NOT NULL,
	`Nickname`	VARCHAR(100) NOT NULL,
	`FullName`	VARCHAR(100) NOT NULL,
	`Password`	CHAR(68) NOT NULL COMMENT 'Pbkdf2'
);

/*
-- ENUM TABLE DEFINITIONS
DROP VIEW IF EXISTS `GroupEnum`;
CREATE TABLE IF NOT EXISTS `GroupEnum` (
`Group` VARCHAR(79)
);

DROP VIEW IF EXISTS `PermissionEnum`;
CREATE TABLE IF NOT EXISTS `PermissionEnum` (
`Permission` VARCHAR(47)
);
*/

-- ENUMS
DROP VIEW IF EXISTS `GroupEnum`;
CREATE VIEW `GroupEnum` AS
	SELECT CONCAT(UCASE(`Groups`.`Name`),' = ',`Groups`.`Id`,',') AS `Group` FROM `Groups`;

DROP VIEW IF EXISTS `PermissionEnum`;
CREATE VIEW `PermissionEnum` AS
	SELECT CONCAT(UCASE(`Permissions`.`Name`),' = ',`Permissions`.`Id`,',') AS `Permission` FROM `Permissions`;

-- KEYS
ALTER TABLE `BackupTypes`
	ADD PRIMARY KEY (`Id`);
ALTER TABLE `DaemonGroups`
	ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdDaemon` (`IdDaemon`), ADD KEY `IX_IdGroup` (`IdGroup`);
ALTER TABLE `DaemonInfos`
	ADD PRIMARY KEY (`Id`);
ALTER TABLE `DaemonLogs`
	ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdDaemon` (`IdDaemon`);
ALTER TABLE `DaemonPreSharedKeys`
	ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdUser` (`IdUser`);
ALTER TABLE `Daemons`
	ADD PRIMARY KEY (`Id`), ADD UNIQUE KEY `IX_Uuid` (`Uuid`), ADD KEY `IX_IdUser` (`IdUser`), ADD KEY `IX_IdDaemonInfo` (`IdDaemonInfo`);
ALTER TABLE `GroupPermissions`
	ADD PRIMARY KEY (`Id`), ADD UNIQUE KEY `IX_IdGroup$IdPermission` (`IdGroup`,`IdPermission`), ADD KEY `IX_IdGroup` (`IdGroup`), ADD KEY `IX_IdPermission` (`IdPermission`);
ALTER TABLE `Groups`
	ADD PRIMARY KEY (`Id`), ADD KEY `IX_ForDaemons` (`ForDaemons`);
ALTER TABLE `LocationCredentials`
	ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdLogonType` (`IdLogonType`);
ALTER TABLE `Locations`
	ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdLocationCredentails` (`IdLocationCredentails`), ADD KEY `IX_IdProtocol` (`IdProtocol`);
ALTER TABLE `LogedInDaemons`
	ADD PRIMARY KEY (`IdDaemon`), ADD UNIQUE KEY `IX_SessionUuid` (`SessionUuid`);
ALTER TABLE `LogedInUsers`
	ADD PRIMARY KEY (`IdUser`), ADD UNIQUE KEY `IX_SessionUuid` (`SessionUuid`);
ALTER TABLE `LogonTypes`
	ADD PRIMARY KEY (`Id`);
ALTER TABLE `LogType`
	ADD PRIMARY KEY (`Id`);
ALTER TABLE `Permissions`
	ADD PRIMARY KEY (`Id`);
ALTER TABLE `Protocols`
	ADD PRIMARY KEY (`Id`), ADD UNIQUE KEY `ShortName` (`ShortName`);
ALTER TABLE `TaskLocationLogs`
	ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdTaskLocation` (`IdTaskLocation`), ADD KEY `IX_IdLogType` (`IdLogType`);
ALTER TABLE `TaskLocations`
	ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdTask` (`IdTask`), ADD KEY `IX_IdSource` (`IdSource`), ADD KEY `IX_IdDestination` (`IdDestination`), ADD KEY `IX_IdBackupTypes` (`IdBackupTypes`);
ALTER TABLE `TaskLocationsTimes`
	ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdTaskLocation` (`IdTaskLocation`), ADD KEY `IX_IdTime` (`IdTime`);
ALTER TABLE `Tasks`
	ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdDaemon` (`IdDaemon`);
ALTER TABLE `Times`
	ADD PRIMARY KEY (`Id`);
ALTER TABLE `UserGroups`
	ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdUser` (`IdUser`), ADD KEY `IX_IdGroup` (`IdGroup`);
ALTER TABLE `UserLogs`
	ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdUser` (`IdUser`), ADD KEY `IX_IdLogType` (`IdLogType`);
ALTER TABLE `Users`
	ADD PRIMARY KEY (`Id`), ADD UNIQUE KEY `Name` (`Nickname`);

-- AUTO_INCREMENT
ALTER TABLE `BackupTypes`			MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `DaemonGroups`			MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `DaemonInfos`			MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `DaemonLogs`			MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `DaemonPreSharedKeys`	MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `Daemons`				MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `GroupPermissions`		MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `Groups`				MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `LocationCredentials`	MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `Locations`				MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `LogonTypes`			MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `LogType`				MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `Permissions`			MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `Protocols`				MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `TaskLocationLogs`		MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `TaskLocations`			MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `TaskLocationsTimes`	MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `Tasks`					MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `Times`					MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `UserGroups`			MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `UserLogs`				MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `Users`					MODIFY `Id` INT(11) NOT NULL AUTO_INCREMENT;

-- FOREIGN KEYS
ALTER TABLE `DaemonGroups`
	ADD CONSTRAINT `DaemonGroups_FK_IdGroup_Groups$Id`							FOREIGN KEY (`IdGroup`)					REFERENCES `Groups` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
	ADD CONSTRAINT `DaemonGroups_FK_IdDaemon_Daemons$Id`						FOREIGN KEY (`IdDaemon`)				REFERENCES `Daemons` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `DaemonLogs`
	ADD CONSTRAINT `DaemonLogs_FK_IdDaemon_Daemons$Id`							FOREIGN KEY (`IdDaemon`)				REFERENCES `Daemons` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `DaemonPreSharedKeys`
	ADD CONSTRAINT `DaemonPreSharedKeys_FK_IdUser_Users$Id`						FOREIGN KEY (`IdUser`)					REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `Daemons`
	ADD CONSTRAINT `Daemons_FK_IdDaemonInfo_DaemonInfos$Id`						FOREIGN KEY (`IdDaemonInfo`)			REFERENCES `DaemonInfos` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
	ADD CONSTRAINT `Daemons_FK_IdUser_Users$Id`									FOREIGN KEY (`IdUser`)					REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `GroupPermissions`
	ADD CONSTRAINT `GroupPermissions_FK_IdGroup_Groups$Id`						FOREIGN KEY (`IdGroup`)					REFERENCES `Groups` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
	ADD CONSTRAINT `GroupPermissions_FK_IdPermission_Permissions$Id`			FOREIGN KEY (`IdPermission`)			REFERENCES `Permissions` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `LocationCredentials`
	ADD CONSTRAINT `LocationCredentials_FK_IdLogonType_LogonTypes$Id`			FOREIGN KEY (`IdLogonType`)				REFERENCES `LogonTypes` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `Locations`
	ADD CONSTRAINT `Locations_FK_IdLocationCredentails_LocationCredentials$Id`	FOREIGN KEY (`IdLocationCredentails`)	REFERENCES `LocationCredentials` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
	ADD CONSTRAINT `Locations_FK_IdProtocol_Protocols$Id`						FOREIGN KEY (`IdProtocol`)				REFERENCES `Protocols` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `LogedInDaemons`
	ADD CONSTRAINT `LogedInDaemons_FK_IdDaemon_Daemons$Id`						FOREIGN KEY (`IdDaemon`)				REFERENCES `Daemons` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `LogedInUsers`
	ADD CONSTRAINT `LogedInUsers_FK_IdUser_Users$Id`							FOREIGN KEY (`IdUser`)					REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `TaskLocationLogs`
	ADD CONSTRAINT `TaskLocationLogs_FK_IdTaskLocation_TaskLocations$Id`		FOREIGN KEY (`IdTaskLocation`)			REFERENCES `TaskLocations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
	ADD CONSTRAINT `TaskLocationLogs_FK_IdLogType_LogType$Id`					FOREIGN KEY (`IdLogType`)				REFERENCES `LogType` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `TaskLocations`
	ADD CONSTRAINT `TaskLocations_FK_IdBackupType_BackupType$Id`				FOREIGN KEY (`IdBackupTypes`)			REFERENCES `BackupTypes` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
	ADD CONSTRAINT `TaskLocations_FK_IdTask_Tasks$Id`							FOREIGN KEY (`IdTask`)					REFERENCES `Tasks` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
	ADD CONSTRAINT `TaskLocations_FK_IdSource_Locations$Id`						FOREIGN KEY (`IdSource`)				REFERENCES `Locations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
	ADD CONSTRAINT `TaskLocations_FK_IdDestination_Locations$Id`				FOREIGN KEY (`IdDestination`)			REFERENCES `Locations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `TaskLocationsTimes`
	ADD CONSTRAINT `TaskLocationsTimes_FK_IdTime_Times$Id`						FOREIGN KEY (`IdTime`)					REFERENCES `Times` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
	ADD CONSTRAINT `TaskLocationsTimes_FK_IdTaskLocation_TaskLocations$Id`		FOREIGN KEY (`IdTaskLocation`)			REFERENCES `TaskLocations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `Tasks`
	ADD CONSTRAINT `Tasks_FK_IdDaemon_Daemons$Id`								FOREIGN KEY (`IdDaemon`)				REFERENCES `Daemons` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `UserGroups`
	ADD CONSTRAINT `UserGroups_FK_IdGroup_Groups$Id`							FOREIGN KEY (`IdGroup`)					REFERENCES `Groups` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
	ADD CONSTRAINT `UserGroups_FK_IdUser_Users$Id`								FOREIGN KEY (`IdUser`)					REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `UserLogs`
	ADD CONSTRAINT `UserLogs_FK_IdLogType_LogType$Id`							FOREIGN KEY (`IdLogType`)				REFERENCES `LogType` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
	ADD CONSTRAINT `UserLogs_FK_IdUser_Users$Id`								FOREIGN KEY (`IdUser`)					REFERENCES `Users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

-- INIT DATA
INSERT INTO `BackupTypes` (`Id`, `ShortName`, `LongName`) VALUES
	(1, 'NORM', 'Normal'),
	(2, 'DIFF', 'Differential'),
	(3, 'INCR', 'Incremental');
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
INSERT INTO `Users` (`Id`, `Nickname`, `FullName`, `Password`) VALUES
	(-999, 'Server', 'Ser Ver', 'z10k0cIA04y4YIjQK9pvDw==BGHo8AN584QCtc6XbI8NGY4970EGArUNOf6xdD+iIMo='),
	(-1, 'Debug', '', '543453'),
	(1, 'Admin', 'ADMINISTRATOR', 'a8hL7q9GaQQDp60J5Ffvxw==pXCTaEl2U25SBBrhbE83n29WXNXZCh9W56Ug2aS7xSc='),
	(2, 'Foo', 'For testing', '61234');
INSERT INTO `DaemonPreSharedKeys` (`Id`, `IdUser`, `PreSharedKey`, `Expires`, `Used`) VALUES
	(1, -1, 'CB+bu/TW5Hh0THNcfZ1Y0w==vDuyXVPabjAcqhHyMItaIXERt2c+HugYXT3zKcnxU9A=', '2019-01-01 00:00:00', 1),
	(2, -1, 'nK/TPSUHsl9P8j6YM5rOQw==nYnQc+NwaqZeLuClGgSAYfWrT8/bD2buzqbDOnv4+fY=', '2019-01-01 00:00:00', 0);
INSERT INTO `Daemons` (`Id`, `Uuid`, `IdUser`, `Password`, `IdDaemonInfo`) VALUES
	(1, '50a7cd9f-d5f9-4c40-8e0f-bfcbb21a5f0e', -1, '9MFKKS7SUgc2ypCMjVcaRw==hhBA0Us7sUd+7Y/YhU7ahC/jzTNKgCJdLoxV2IvpVvo=', 1),
	(2, '2afecca5-cade-4f48-9539-28b65bbbc380', -1, 'RIsxDbcWENpx+HRLV2XTAw==A8p2Z851LXUpz3UsF2xVJsolYi6tBHYiTCr0cbCTotE=', 2),
	(3, '07443701-2219-4ee0-adc6-4d94b3744d57', -1, 'gUiQGFz3ty9x43zdUIBczA==+S6vfyDnxk8lhIY2B3P/2MSr+mkhRio4xqLeCfruino=', 3),
	(4, 'adcd71b3-ddba-49c8-b502-e83fadbf84c1', -1, 'rdXpzzzYgGtRMIdgNf0FIA==RVskf+TJc/348Z8Lisbs86fFO0879H+ev4Ov0ZlB0Ds=', 4),
	(5, '291de540-7da6-48eb-bcae-38f009141c12', -1, '1EmExnDg+ZKlkG0O22RXZA==19KUYLEpzdB2FTYcx9e47Rnql5YUN2/zeiE2K8sl7/M=', 5);
INSERT INTO `Groups` (`Id`, `Name`, `Description`, `ForDaemons`) VALUES
	(-999, 'Server', 'Server', 0),
	(-1, 'DebugGroup', 'For debugging', 0),
	(1, 'Admins', 'Admins', 0),
	(2, 'Daemons', 'For every daemon', 1);
INSERT INTO `Permissions` (`Id`, `Name`, `Description`) VALUES
	(1, 'Skip', 'Přeskakuje kontrolu permisí - UNIVERZÁLNÍ PŘÍSTUP'),
	(2, 'Login', NULL),
	(3, 'ManagePreShared', NULL),
	(4, 'ManageSelfUser', NULL),
	(5, 'ManageOtherUsers', NULL),
	(6, 'ManageSelfDaemons', NULL),
	(7, 'ManageOtherDaemons', NULL),
	(8, 'ManagePermission', 'Přidělování a odebírání práv skupinám'),
	(9, 'ManageGroups', 'Vytváření skupin a přidávání do nich uživatelů a daemonů');
INSERT INTO `GroupPermissions` (`Id`, `IdGroup`, `IdPermission`) VALUES
	(1, 1, 1),
	(2, 2, 2);
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
INSERT INTO `Protocols` (`Id`, `ShortName`, `LongName`) VALUES
	(1, 'WND', 'Windows Standard URI'),
	(2, 'WRD', 'Windows Remote URI'),
	(3, 'FTP', 'File Transfer Protocol'),
	(4, 'SFTP', 'Secure File Transfer Protocol');
INSERT INTO `LocationCredentials` (`Id`, `Host`, `Port`, `IdLogonType`, `Username`, `Password`) VALUES
	(1, 'test.com', NULL, 1, NULL, NULL),
	(2, 'test.com/myName', 21, 2, 'myName', 'abc'),
	(3, 'test.com/myName', 21, 2, 'myName', 'abc');
INSERT INTO `Locations` (`Id`, `Uri`, `IdProtocol`, `IdLocationCredentails`) VALUES
	(1, 'C:\\Users\\TestUser\\Desktop\\Docs\\*', 1, 1),
	(2, 'C:\\Users\\TestUser\\Desktop\\Pictures\\*', 1, 1),
	(3, 'E:\\Backups', 1, 1),
	(4, 'test.com', 3, 2),
	(5, 'test.com', 3, 3);
INSERT INTO `Tasks` (`Id`, `IdDaemon`, `Name`, `Description`) VALUES
	(1, 1, 'DebugTask', 'For debugging');
INSERT INTO `Times` (`Id`, `Interval`, `Name`, `Repeat`, `StartTime`, `EndTime`) VALUES
	(1, 86400, 'TestBackupEveryDay', 1, '2018-02-11 01:00:00', NULL),
	(2, 604800, 'TestBackupNoonSunday', 1, '2018-02-11 12:00:00', NULL),
	(3, 0, 'Dneska', 0, '2018-02-26 20:49:59', '0001-01-01 00:00:00'),
	(4, 604800, 'Kazdy Patek', 1, '2018-02-23 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `TaskLocations` (`Id`, `IdTask`, `IdSource`, `IdDestination`, `IdBackupTypes`) VALUES
	(1, 1, 1, 2, 1);
INSERT INTO `TaskLocationsTimes` (`Id`, `IdTaskLocation`, `IdTime`) VALUES
	(1, 1, 3),
	(2, 1, 4);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
