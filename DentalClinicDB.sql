USE [master]
GO
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
/****** Object:  Table [dbo].[appointment]    Script Date: 22/05/2024 16:46:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[appointment](
	[appointmentID] [int] NOT NULL,
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
	[appointmentDetailID] [int] NOT NULL,
	[appointmentID] [int] NULL,
	[serviceID] [int] NULL,
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
	[dentistID] [int] NOT NULL,
	[name] [varchar](100) NULL,
	[type] [int] NULL,
	[contractType] [varchar](100) NULL,
	[status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[dentistID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[dentistService]    Script Date: 22/05/2024 16:46:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dentistService](
	[dentistServiceID] [int] NOT NULL,
	[dentistID] [int] NULL,
	[serviceID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[dentistServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[medicalRecord]    Script Date: 22/05/2024 16:46:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[medicalRecord](
	[recordID] [int] NOT NULL,
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
	[patientID] [int] NOT NULL,
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
	[scheduleID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[service]    Script Date: 22/05/2024 16:46:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[service](
	[serviceID] [int] NOT NULL,
	[name] [varchar](100) NULL,
	[price] [int] NULL,
	[description] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[serviceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[transaction]    Script Date: 22/05/2024 16:46:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[transaction](
	[transactionID] [int] NOT NULL,
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
ALTER TABLE [dbo].[appointmentDetails]  WITH CHECK ADD FOREIGN KEY([serviceID])
REFERENCES [dbo].[service] ([serviceID])
GO
ALTER TABLE [dbo].[dentistService]  WITH CHECK ADD FOREIGN KEY([dentistID])
REFERENCES [dbo].[dentist] ([dentistID])
GO
ALTER TABLE [dbo].[dentistService]  WITH CHECK ADD FOREIGN KEY([serviceID])
REFERENCES [dbo].[service] ([serviceID])
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
USE [master]
GO
ALTER DATABASE [DentalClinicDB] SET  READ_WRITE 
GO
