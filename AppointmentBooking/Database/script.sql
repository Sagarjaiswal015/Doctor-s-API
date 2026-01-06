CREATE DATABASE IF NOT EXISTS appointmentbooking;
USE appointmentbooking;

CREATE TABLE `Users` (
  `Id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `Email` varchar(255) NOT NULL UNIQUE,
  `PasswordHash` longtext NOT NULL,
  `Role` varchar(50) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL
);

CREATE TABLE `Doctors` (
  `Id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `Name` longtext NOT NULL,
  `Specialization` longtext NOT NULL,
  `Phone` longtext NOT NULL,
  `Email` longtext NOT NULL,
  `UserId` int NOT NULL,
  FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`)
);

CREATE TABLE `AppointmentSlots` (
  `Id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `DoctorId` int NOT NULL,
  `StartTime` datetime(6) NOT NULL,
  `EndTime` datetime(6) NOT NULL,
  `MaxCapacity` int NOT NULL,
  `CurrentBookings` int NOT NULL,
  FOREIGN KEY (`DoctorId`) REFERENCES `Doctors` (`Id`)
);

CREATE TABLE `Appointments` (
  `Id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `AppointmentSlotId` int NOT NULL,
  `DoctorId` int NOT NULL,
  `UserId` int NOT NULL,
  `PatientName` longtext NOT NULL,
  `PatientPhone` longtext NOT NULL,
  `PatientEmail` longtext NOT NULL,
  `Reason` longtext NOT NULL,
  `Status` longtext NOT NULL,
  `BookedAt` datetime(6) NOT NULL,
  FOREIGN KEY (`AppointmentSlotId`) REFERENCES `AppointmentSlots` (`Id`),
  FOREIGN KEY (`DoctorId`) REFERENCES `Doctors` (`Id`),
  FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`)
);

CREATE INDEX `IX_Doctors_UserId` ON `Doctors` (`UserId`);
CREATE INDEX `IX_AppointmentSlots_DoctorId` ON `AppointmentSlots` (`DoctorId`);
CREATE INDEX `IX_Appointments_AppointmentSlotId` ON `Appointments` (`AppointmentSlotId`);
CREATE INDEX `IX_Appointments_DoctorId` ON `Appointments` (`DoctorId`);
CREATE INDEX `IX_Appointments_UserId` ON `Appointments` (`UserId`);
