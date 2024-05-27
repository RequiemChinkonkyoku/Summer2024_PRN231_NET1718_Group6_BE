	USE [master]
	GO

	drop database if exists DentalClinicDB

	/****** Object:  Database [DentalClinicDB]    Script Date: 22/05/2024 16:46:30 ******/
	CREATE DATABASE [DentalClinicDB]
	 CONTAINMENT = NONE
	 ON  PRIMARY 
	( NAME = N'DentalClinicDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\DentalClinicDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
	 LOG ON 
	( NAME = N'DentalClinicDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\DentalClinicDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
	 WITH CATALOG_COLLATION = DATABASE_DEFAULT
	GO
	ALTER DATABASE [DentalClinicDB] SET COMPATIBILITY_LEVEL = 150
	GO
	IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
	begin
	EXEC [DentalClinicDB].[dbo].[sp_fulltext_database] @action = 'enable'
	end
	GO
	ALTER DATABASE [DentalClinicDB] SET ANSI_NULL_DEFAULT OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET ANSI_NULLS OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET ANSI_PADDING OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET ANSI_WARNINGS OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET ARITHABORT OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET AUTO_CLOSE ON 
	GO
	ALTER DATABASE [DentalClinicDB] SET AUTO_SHRINK OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET AUTO_UPDATE_STATISTICS ON 
	GO
	ALTER DATABASE [DentalClinicDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET CURSOR_DEFAULT  GLOBAL 
	GO
	ALTER DATABASE [DentalClinicDB] SET CONCAT_NULL_YIELDS_NULL OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET NUMERIC_ROUNDABORT OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET QUOTED_IDENTIFIER OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET RECURSIVE_TRIGGERS OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET  ENABLE_BROKER 
	GO
	ALTER DATABASE [DentalClinicDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET TRUSTWORTHY OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET PARAMETERIZATION SIMPLE 
	GO
	ALTER DATABASE [DentalClinicDB] SET READ_COMMITTED_SNAPSHOT OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET HONOR_BROKER_PRIORITY OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET RECOVERY SIMPLE 
	GO
	ALTER DATABASE [DentalClinicDB] SET  MULTI_USER 
	GO
	ALTER DATABASE [DentalClinicDB] SET PAGE_VERIFY CHECKSUM  
	GO
	ALTER DATABASE [DentalClinicDB] SET DB_CHAINING OFF 
	GO
	ALTER DATABASE [DentalClinicDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
	GO
	ALTER DATABASE [DentalClinicDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
	GO
	ALTER DATABASE [DentalClinicDB] SET DELAYED_DURABILITY = DISABLED 
	GO
	ALTER DATABASE [DentalClinicDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
	GO
	ALTER DATABASE [DentalClinicDB] SET QUERY_STORE = OFF
	GO
	USE [DentalClinicDB]
	GO

	DROP TABLE IF EXISTS appointmentDetails;
	DROP TABLE IF EXISTS dentistMedicalService;
	DROP TABLE IF EXISTS schedule;
	DROP TABLE IF EXISTS medicalService;
	DROP TABLE IF EXISTS dentist;
	DROP TABLE IF EXISTS "transaction";
	DROP TABLE IF EXISTS medicalRecord;
	DROP TABLE IF EXISTS appointment;
	DROP TABLE IF EXISTS patient;

	/****** Object:  Table [dbo].[appointment]    Script Date: 22/05/2024 16:46:30 ******/
	SET ANSI_NULLS ON
	GO
	SET QUOTED_IDENTIFIER ON
	GO
	CREATE TABLE [dbo].[appointment](
		[appointmentID] [int] Identity(1,1) NOT NULL,
		[arrivalDate] [date] NULL,
		[type] [varchar](100) NULL,
		[status] [int] NULL,
		[timeSlot] [int] NULL,
		[patientID] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[appointmentID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO
	/****** Object:  Table [dbo].[appointmentDetails]    Script Date: 22/05/2024 16:46:30 ******/
	SET ANSI_NULLS ON
	GO
	SET QUOTED_IDENTIFIER ON
	GO
	CREATE TABLE [dbo].[appointmentDetails](
		[appointmentDetailID]  [int]  Identity(1,1) NOT NULL,
		[appointmentID] [int] NULL,
		[medicalServiceID] [int] NULL,
		[dentistID] [int] NULL,
		[scheduleID] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[appointmentDetailID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO
	/****** Object:  Table [dbo].[dentist]    Script Date: 22/05/2024 16:46:30 ******/
	SET ANSI_NULLS ON
	GO
	SET QUOTED_IDENTIFIER ON
	GO
	CREATE TABLE [dbo].[dentist](
		[dentistID] [int] Identity(1,1) NOT NULL  ,
		[name] [varchar](100) NULL,
		[email] [varchar](100) NULL,
		[password] [varchar](100) NULL,
		[type] [int] NULL,
		[contractType] [varchar](100) NULL,
		[status] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[dentistID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO

	/****** Object:  Table [dbo].[dentistMedicalService]    Script Date: 22/05/2024 16:46:30 ******/
	SET ANSI_NULLS ON
	GO
	SET QUOTED_IDENTIFIER ON
	GO
	CREATE TABLE [dbo].[dentistMedicalService](
		[dentistMedicalServiceID] [int] Identity(1,1) NOT NULL,
		[dentistID] [int] NULL,
		[medicalServiceID] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[dentistMedicalServiceID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO
	/****** Object:  Table [dbo].[medicalRecord]    Script Date: 22/05/2024 16:46:30 ******/
	SET ANSI_NULLS ON
	GO
	SET QUOTED_IDENTIFIER ON
	GO
	CREATE TABLE [dbo].[medicalRecord](
		[recordID] [int] Identity(1,1) NOT NULL,
		[treatment] [varchar](100) NULL,
		[diagnosis] [varchar](100) NULL,
		[note] [varchar](200) NULL,
		[followUpDate] [date] NULL,
		[patientID] [int] NULL,
		[appointmentID] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[recordID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO
	/****** Object:  Table [dbo].[patient]    Script Date: 22/05/2024 16:46:30 ******/
	SET ANSI_NULLS ON
	GO
	SET QUOTED_IDENTIFIER ON
	GO
	CREATE TABLE [dbo].[patient](
		[patientID] [int] Identity(1,1) NOT NULL,
		[name] [varchar](100) NULL,
		[age] [int] NULL,
		[address] [varchar](100) NULL,
		[gender] [int] NULL,
		[email] [varchar](100) NULL,
		[password] [varchar](100) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[patientID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO
	/****** Object:  Table [dbo].[schedule]    Script Date: 22/05/2024 16:46:30 ******/
	SET ANSI_NULLS ON
	GO
	SET QUOTED_IDENTIFIER ON
	GO
	CREATE TABLE [dbo].[schedule](
		[scheduleID] [int] Identity(1,1) NOT NULL,
		[workDate] [date] NULL,
		[timeSlot] [int] NULL,
		[status] [int] NULL,
		[dentistID] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[scheduleID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO
	/****** Object:  Table [dbo].[medicalService]    Script Date: 22/05/2024 16:46:30 ******/
	SET ANSI_NULLS ON
	GO
	SET QUOTED_IDENTIFIER ON
	GO
	CREATE TABLE [dbo].[medicalService](
		[medicalServiceID] [int] Identity(1,1) NOT NULL,
		[name] [varchar](100) NULL,
		[price] [int] NULL,
		[description] [varchar](100) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[medicalServiceID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO
	/****** Object:  Table [dbo].[transaction]    Script Date: 22/05/2024 16:46:30 ******/
	SET ANSI_NULLS ON
	GO
	SET QUOTED_IDENTIFIER ON
	GO
	CREATE TABLE [dbo].[transaction](
		[transactionID] [int] Identity(1,1) NOT NULL,
		[price] [int] NULL,
		[transactionTime] [time] NULL,
		[appointmentID] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[transactionID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO
	ALTER TABLE [dbo].[appointment]  WITH CHECK ADD FOREIGN KEY([patientID])
	REFERENCES [dbo].[patient] ([patientID])
	GO
	ALTER TABLE [dbo].[appointmentDetails]  WITH CHECK ADD FOREIGN KEY([appointmentID])
	REFERENCES [dbo].[appointment] ([appointmentID])
	GO
	ALTER TABLE [dbo].[appointmentDetails]  WITH CHECK ADD FOREIGN KEY([dentistID])
	REFERENCES [dbo].[dentist] ([dentistID])
	GO
	ALTER TABLE [dbo].[appointmentDetails]  WITH CHECK ADD FOREIGN KEY([scheduleID])
	REFERENCES [dbo].[schedule] ([scheduleID])
	GO
	ALTER TABLE [dbo].[appointmentDetails]  WITH CHECK ADD FOREIGN KEY([medicalServiceID])
	REFERENCES [dbo].[medicalService] ([medicalServiceID])
	GO
	ALTER TABLE [dbo].[dentistMedicalService]  WITH CHECK ADD FOREIGN KEY([dentistID])
	REFERENCES [dbo].[dentist] ([dentistID])
	GO
	ALTER TABLE [dbo].[dentistMedicalService]  WITH CHECK ADD FOREIGN KEY([medicalServiceID])
	REFERENCES [dbo].[medicalService] ([medicalServiceID])
	GO
	ALTER TABLE [dbo].[medicalRecord]  WITH CHECK ADD FOREIGN KEY([appointmentID])
	REFERENCES [dbo].[appointment] ([appointmentID])
	GO
	ALTER TABLE [dbo].[medicalRecord]  WITH CHECK ADD FOREIGN KEY([patientID])
	REFERENCES [dbo].[patient] ([patientID])
	GO
	ALTER TABLE [dbo].[schedule]  WITH CHECK ADD FOREIGN KEY([dentistID])
	REFERENCES [dbo].[dentist] ([dentistID])
	GO
	ALTER TABLE [dbo].[transaction]  WITH CHECK ADD FOREIGN KEY([appointmentID])
	REFERENCES [dbo].[appointment] ([appointmentID])
	GO

	--Patient Table
	INSERT INTO [dbo].[patient] ( name, age, address, gender, email, password)
	VALUES 
	('Default', 20, '123 Street', 1, 'default@gmail.com', '123'),
	('Nguyen', 30, '123 Duong Chinh', 1, 'nguyen@gmail.com', '123'),
	('Tran', 25, '456 Duong Huynh Van Banh', 2, 'tran@gmail.com', '456'),
	('Le', 35, '789 Duong Le Van Sy', 2, 'le@gmail.com', '789'),
	('Pham', 28, '321 Duong Nguyen Thi Minh Khai', 1, 'pham@gmail.com', '1011'),
	('Hoang', 40, '654 Duong Thu Duc', 2, 'hoang@gmail.com', '1213');


	-- Dentist table
	INSERT INTO [dbo].[dentist] (name, email, password, type, contractType, status)
	VALUES 
	( 'Bac si Nguyen','abc@gmail.com','123', 1, 'Full-time', 1),
	( 'Bac si Tran', '123@gmail.com', '123', 2, 'Part-time', 1),
	( 'Bac si Le', '234@gmail.com', '123', 1, 'Full-time', 1),
	( 'Bac si Pham', '321@gmail.com', '123',  2, 'Full-time', 0),
	('Bac si Hoang', '213@gmail.com', '123',  1, 'Full-time', 0);


	--  Service table
	INSERT INTO [dbo].[medicalService] ( name, price, description)
	VALUES 
	( 'Ve sinh rang', 100000, 'Ve sinh rang co ban'),
	( 'Tram rang', 200000, 'Tram rang'),
	( 'Nho rang', 300000, 'Nho rang'),
	( 'Tay trang rang', 400000, 'Tay trang rang'),
	( 'Nieng rang', 5000000, 'Nieng rang chinh nha');

	-- Schedule table
	INSERT INTO [dbo].[schedule] ( workDate, timeSlot, status, dentistID)
	VALUES 
	( '2024-05-24', 1, 1, 1),
	( '2024-05-24', 2, 1, 2),
	( '2024-05-25', 1, 1, 3),
	( '2024-05-25', 2, 1, 4),
	( '2024-05-26', 1, 1, 5);

	-- Appointment table
	INSERT INTO [dbo].[appointment] ( arrivalDate, type, status, timeSlot, patientID)
	VALUES 
	( '2024-05-24', 'New Patient', 1, 1, 1),
	( '2024-05-24', 'Routine Check-up', 1, 2, 2),
	( '2024-05-25', 'Emergency', 1, 1, 3),
	( '2024-05-25', 'New Patient', 1, 2, 4),
	( '2024-05-26', 'Emergency', 1, 1, 5);

	-- AppointmentDetails table
	INSERT INTO [dbo].[appointmentDetails] ( appointmentID, medicalServiceID, dentistID, scheduleID)
	VALUES 
	( 1, 1, 1, 1),
	( 2, 2, 2, 2),
	( 3, 3, 3, 3),
	( 4, 4, 4, 4),
	( 5, 5, 5, 5);

	-- DentistService table
	INSERT INTO [dbo].[dentistMedicalService] ( dentistID, medicalServiceID)
	VALUES 
	( 1, 1),
	( 2, 2),
	( 3, 3),
	( 4, 4),
	( 5, 5);

	-- MedicalRecord table
	INSERT INTO [dbo].[medicalRecord] ( treatment, diagnosis, note, followUpDate, patientID, appointmentID)
	VALUES 
	( 'Ve sinh rang', 'Khong van de', 'Tinh trang tot', '2024-06-24', 1, 1),
	( 'Tram rang', 'Sau rang', 'Da tram rang', '2024-06-24', 2, 2),
	( 'Nho rang', 'Rang khon', 'Da nho rang', '2024-06-25', 3, 3),
	( 'Tay trang rang', 'Rang o mau', 'Da tay trang', '2024-06-25', 4, 4),
	( 'Nieng rang', 'Rang lech', 'Da nieng rang', '2024-06-26', 5, 5);

	-- Transaction table
	INSERT INTO [dbo].[transaction] (price, transactionTime, appointmentID)
	VALUES 
	( 100000, '09:00:00', 1),
	( 200000, '10:00:00', 2),
	( 300000, '11:00:00', 3),
	( 400000, '12:00:00', 4),
	( 500000, '13:00:00', 5);

	USE [master]
	GO
	

