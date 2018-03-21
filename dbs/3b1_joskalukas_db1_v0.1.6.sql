-- phpMyAdmin SQL Dump
-- version 4.2.9.1
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Mar 21, 2018 at 09:35 PM
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

--
-- Indexes for dumped tables
--

--
-- Indexes for table `TaskLocations`
--
ALTER TABLE `TaskLocations`
 ADD PRIMARY KEY (`Id`), ADD KEY `IX_IdTask` (`IdTask`), ADD KEY `IX_IdSource` (`IdSource`), ADD KEY `IX_IdDestination` (`IdDestination`), ADD KEY `IX_IdBackupTypes` (`IdBackupTypes`), ADD KEY `IdTaskLocationDetails` (`IdTaskLocationDetails`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `TaskLocations`
--
ALTER TABLE `TaskLocations`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=2;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `TaskLocations`
--
ALTER TABLE `TaskLocations`
ADD CONSTRAINT `TaskLocations_ibfk_1` FOREIGN KEY (`IdTaskLocationDetails`) REFERENCES `TaskLocationDetails` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `TaskLocations_FK_IdBackupType_BackupType$Id` FOREIGN KEY (`IdBackupTypes`) REFERENCES `BackupTypes` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `TaskLocations_FK_IdDestination_Locations$Id` FOREIGN KEY (`IdDestination`) REFERENCES `Locations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `TaskLocations_FK_IdSource_Locations$Id` FOREIGN KEY (`IdSource`) REFERENCES `Locations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `TaskLocations_FK_IdTask_Tasks$Id` FOREIGN KEY (`IdTask`) REFERENCES `Tasks` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
