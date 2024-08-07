USE master;

-- Drop the database if it exists
DROP DATABASE IF EXISTS DentalClinicDB;
GO

-- Create the database
CREATE DATABASE DentalClinicDB;
GO

-- Use the created database
USE DentalClinicDB;
GO

-- Drop existing tables if they exist
DROP TABLE IF EXISTS appointmentDetails;
DROP TABLE IF EXISTS dentistService;
DROP TABLE IF EXISTS schedule;
DROP TABLE IF EXISTS service;
DROP TABLE IF EXISTS dentist;
DROP TABLE IF EXISTS "transaction";
DROP TABLE IF EXISTS medicalRecord;
DROP TABLE IF EXISTS appointment;
DROP TABLE IF EXISTS patient;
DROP TABLE IF EXISTS customer;
DROP TABLE IF EXISTS profession;
DROP TABLE IF EXISTS treatment;
DROP TABLE IF EXISTS blacklistedTokens;

-- Create customer table
CREATE TABLE customer
(
    customerID INT IDENTITY(1,1) PRIMARY KEY,
    email VARCHAR(255),
    password VARCHAR(255),
	passwordHash VARCHAR(MAX),
    status INT -- 1 for active, 0 for inactive
);

-- Create patient table
CREATE TABLE patient
(
    patientID INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(255),
    yearOfBirth INT,
    address VARCHAR(255),
    gender INT,
    status INT, -- 1 for active, 0 for inactive
    customerID INT FOREIGN KEY REFERENCES customer(customerID)
);

-- Create appointment table
CREATE TABLE appointment
(
    appointmentID INT IDENTITY(1,1) PRIMARY KEY,
    createDate DATE,
    arrivalDate DATE,
    timeSlot INT,
    status INT, -- 1 for scheduled, 0 for canceled
    bookingPrice INT,
    servicePrice INT,
    totalPrice INT,
    customerID INT FOREIGN KEY REFERENCES customer(customerID),
    patientID INT FOREIGN KEY REFERENCES patient(patientID)
);

-- Create medicalRecord table
CREATE TABLE medicalRecord
(
    recordID INT IDENTITY(1,1) PRIMARY KEY,
    diagnosis VARCHAR(255),
    note VARCHAR(255),
    status INT, -- 1 for active, 0 for archived
    patientID INT FOREIGN KEY REFERENCES patient(patientID),
    appointmentID INT FOREIGN KEY REFERENCES appointment(appointmentID)
);

-- Create transaction table
CREATE TABLE "transaction"
(
    transactionID INT IDENTITY(1,1) PRIMARY KEY,
    price INT,
    transactionTime DATETIME,
    status INT, -- 1 for completed, 0 for pending or failed
    customerID INT FOREIGN KEY REFERENCES customer(customerID),
    appointmentID INT FOREIGN KEY REFERENCES appointment(appointmentID)
);

-- Create dentist table
CREATE TABLE dentist
(
    dentistID INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(255),
    email VARCHAR(255),
    password VARCHAR(255),
	passwordHash VARCHAR(MAX),
    type INT,
    contractType VARCHAR(255),
    status INT -- 1 for active, 0 for inactive
);

-- Create treatment table
CREATE TABLE treatment
(
    treatmentID INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(255),
    price INT,
    description VARCHAR(255),
    status INT -- 1 for available, 0 for discontinued
);

-- Create profession table
CREATE TABLE profession
(
    professionID INT IDENTITY(1,1) PRIMARY KEY,
    dentistID INT FOREIGN KEY REFERENCES dentist(dentistID),
    treatmentID INT FOREIGN KEY REFERENCES treatment(treatmentID)
);

-- Create schedule table
CREATE TABLE schedule
(
    scheduleID INT IDENTITY(1,1) PRIMARY KEY,
    workDate DATE,
    timeSlot INT,
    status INT, -- 1 for available, 0 for booked or unavailable
    dentistID INT FOREIGN KEY REFERENCES dentist(dentistID)
);

-- Create appointmentDetails table
CREATE TABLE appointmentDetails
(
    appointmentDetailID INT IDENTITY(1,1) PRIMARY KEY,
    appointmentID INT FOREIGN KEY REFERENCES appointment(appointmentID),
    treatmentID INT FOREIGN KEY REFERENCES treatment(treatmentID),
    dentistID INT FOREIGN KEY REFERENCES dentist(dentistID),
    scheduleID INT FOREIGN KEY REFERENCES schedule(scheduleID)
);

-- Insert data into customer table
INSERT INTO customer (email, password, status)
VALUES 
    ('default@gmail.com', '123', 1),
    ('nguyen@gmail.com', '123', 1),
    ('tran@gmail.com', '456', 1),
    ('le@gmail.com', '789', 1),
    ('pham@gmail.com', '1011', 1),
    ('hoang@gmail.com', '1213', 1);

-- Insert data into patient table
INSERT INTO patient (name, yearOfBirth, address, gender, status, customerID)
VALUES 
    ('Default', 2000, '123 Street', 1, 1, 1),
    ('Nguyen', 2001, '123 Duong Chinh', 1, 1, 2),
    ('Tran', 2005, '456 Duong Huynh Van Banh', 2, 1, 3),
    ('Le', 1995, '789 Duong Le Van Sy', 2, 1, 4),
    ('Pham', 1990, '321 Duong Nguyen Thi Minh Khai', 1, 1, 5),
    ('Hoang', 2024, '654 Duong Thu Duc', 2, 1, 6);

-- Insert data into dentist table
INSERT INTO dentist (name, email, password, type, contractType, status)
VALUES 
	('Bac si Nguyen','abc@gmail.com','123', 1, 'Full-time', 1),
	('Bac si Tran', '123@gmail.com', '123', 2, 'Part-time', 1),
	('Bac si Le', '234@gmail.com', '123', 1, 'Full-time', 1),
	('Bac si Pham', '321@gmail.com', '123',  2, 'Full-time', 0),
	('Bac si Hoang', '213@gmail.com', '123',  1, 'Full-time', 0);

-- Insert data into treatment table
INSERT INTO treatment (name, price, description, status)
VALUES 
    ('Ve sinh rang', 100000, 'Ve sinh rang co ban', 1),
    ('Tram rang', 200000, 'Tram rang', 1),
    ('Nho rang', 300000, 'Nho rang', 1),
    ('Tay trang rang', 400000, 'Tay trang rang', 1),
    ('Nieng rang', 5000000, 'Nieng rang chinh nha', 1);

-- Insert data into schedule table
INSERT INTO schedule (workDate, timeSlot, status, dentistID)
VALUES 
    ('2024-05-24', 1, 1, 1),
    ('2024-05-24', 2, 1, 2),
    ('2024-05-25', 1, 1, 3),
    ('2024-05-25', 2, 1, 4),
    ('2024-05-26', 1, 1, 5);

-- Insert data into appointment table
INSERT INTO appointment (createDate, arrivalDate, timeSlot, status, bookingPrice, servicePrice, totalPrice, customerID, patientID)
VALUES 
    ('2024-05-20', '2024-05-24', 1, 1, 100000, 50000, 150000, 1, 1),
    ('2024-05-21', '2024-05-24', 2, 1, 200000, 100000, 300000, 2, 2),
    ('2024-05-22', '2024-05-25', 1, 1, 300000, 150000, 450000, 3, 3),
    ('2024-05-23', '2024-05-25', 2, 1, 400000, 200000, 600000, 4, 4),
    ('2024-05-24', '2024-05-26', 1, 1, 500000, 250000, 750000, 5, 5);

-- Insert data into appointmentDetails table
INSERT INTO appointmentDetails (appointmentID, treatmentID, dentistID, scheduleID)
VALUES 
    (1, 1, 1, 1),
    (2, 2, 2, 2),
    (3, 3, 3, 3),
    (4, 4, 4, 4),
    (5, 5, 5, 5);

-- Insert data into profession table
INSERT INTO profession (dentistID, treatmentID)
VALUES 
    (1, 1),
    (2, 2),
    (3, 3),
    (4, 4),
    (5, 5);

-- Insert data into medicalRecord table
INSERT INTO medicalRecord (diagnosis, note, status, patientID, appointmentID)
VALUES 
    ('Khong van de', 'Tinh trang tot', 1, 1, 1),
    ('Sau rang', 'Da tram rang', 1, 2, 2),
    ('Rang khon', 'Da nho rang', 1, 3, 3),
    ('Rang o mau', 'Da tay trang', 1, 4, 4),
    ('Rang lech', 'Da nieng rang', 1, 5, 5);

-- Insert data into transaction table
INSERT INTO "transaction" (price, transactionTime, status, customerID, appointmentID)
VALUES 
    (100000, '09:00:00', 1, 1, 1),
    (200000, '10:00:00', 1, 2, 2),
    (300000, '11:00:00', 1, 3, 3),
    (400000, '12:00:00', 1, 4, 4),
    (500000, '13:00:00', 1, 5, 5);
