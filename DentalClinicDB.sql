USE master;

-- Drop the database if it exists
DROP DATABASE IF EXISTS DentalClinicDB;

-- Create the database
CREATE DATABASE DentalClinicDB;

-- Use the created database
USE DentalClinicDB;

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
DROP TABLE IF EXISTS account;
DROP TABLE IF EXISTS profession;
DROP TABLE IF EXISTS treatment;

-- Create account table
CREATE TABLE account
(
    accountID INT IDENTITY(1,1) PRIMARY KEY,
    email VARCHAR(255),
    password VARCHAR(255),
    status INT
);

-- Create patient table
CREATE TABLE patient
(
    patientID INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(255),
    age INT,
    address VARCHAR(255),
    gender INT,
    accountID INT FOREIGN KEY REFERENCES account(accountID)
);

-- Create appointment table
CREATE TABLE appointment
(
    appointmentID INT IDENTITY(1,1) PRIMARY KEY,
    createDate DATE,
    arrivalDate DATE,
    timeSlot INT,
    status INT,
    bookingPrice INT,
    servicePrice INT,
    totalPrice INT,
    accountID INT FOREIGN KEY REFERENCES account(accountID),
    patientID INT FOREIGN KEY REFERENCES patient(patientID)
);

-- Create medicalRecord table
CREATE TABLE medicalRecord
(
    recordID INT IDENTITY(1,1) PRIMARY KEY,
    diagnosis VARCHAR(255),
    note VARCHAR(255),
    patientID INT FOREIGN KEY REFERENCES patient(patientID),
    appointmentID INT FOREIGN KEY REFERENCES appointment(appointmentID)
);

-- Create transaction table
CREATE TABLE "transaction"
(
    transactionID INT IDENTITY(1,1) PRIMARY KEY,
    price INT,
    transactionTime TIME,
    accountID INT FOREIGN KEY REFERENCES account(accountID),
    appointmentID INT FOREIGN KEY REFERENCES appointment(appointmentID)
);

-- Create dentist table
CREATE TABLE dentist
(
    dentistID INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(255),
    email VARCHAR(255),
    password VARCHAR(255),
    type INT,
    contractType VARCHAR(255),
    status INT
);

-- Create treatment table
CREATE TABLE treatment
(
    treatmentID INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(255),
    price INT,
    description VARCHAR(255)
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
    status INT,
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

-- Insert data into account table
INSERT INTO account (email, password, status)
VALUES 
    ('default@gmail.com', '123', 1),
    ('nguyen@gmail.com', '123', 1),
    ('tran@gmail.com', '456', 1),
    ('le@gmail.com', '789', 1),
    ('pham@gmail.com', '1011', 1),
    ('hoang@gmail.com', '1213', 1);

-- Insert data into patient table
INSERT INTO patient (name, age, address, gender, accountID)
VALUES 
    ('Default', 20, '123 Street', 1, 1),
    ('Nguyen', 30, '123 Duong Chinh', 1, 2),
    ('Tran', 25, '456 Duong Huynh Van Banh', 2, 3),
    ('Le', 35, '789 Duong Le Van Sy', 2, 4),
    ('Pham', 28, '321 Duong Nguyen Thi Minh Khai', 1, 5),
    ('Hoang', 40, '654 Duong Thu Duc', 2, 6);

-- Insert data into dentist table
INSERT INTO dentist (name, email, password, type, contractType, status)
VALUES 
	('Bac si Nguyen','abc@gmail.com','123', 1, 'Full-time', 1),
	('Bac si Tran', '123@gmail.com', '123', 2, 'Part-time', 1),
	('Bac si Le', '234@gmail.com', '123', 1, 'Full-time', 1),
	('Bac si Pham', '321@gmail.com', '123',  2, 'Full-time', 0),
	('Bac si Hoang', '213@gmail.com', '123',  1, 'Full-time', 0);

-- Insert data into treatment table
INSERT INTO treatment (name, price, description)
VALUES 
    ('Ve sinh rang', 100000, 'Ve sinh rang co ban'),
    ('Tram rang', 200000, 'Tram rang'),
    ('Nho rang', 300000, 'Nho rang'),
    ('Tay trang rang', 400000, 'Tay trang rang'),
    ('Nieng rang', 5000000, 'Nieng rang chinh nha');

-- Insert data into schedule table
INSERT INTO schedule (workDate, timeSlot, status, dentistID)
VALUES 
    ('2024-05-24', 1, 1, 1),
    ('2024-05-24', 2, 1, 2),
    ('2024-05-25', 1, 1, 3),
    ('2024-05-25', 2, 1, 4),
    ('2024-05-26', 1, 1, 5);

-- Insert data into appointment table
INSERT INTO appointment (arrivalDate, status, timeSlot, patientID)
VALUES 
    ('2024-05-24', 1, 1, 1),
    ('2024-05-24', 1, 2, 2),
    ('2024-05-25', 1, 1, 3),
    ('2024-05-25', 1, 2, 4),
    ('2024-05-26', 1, 1, 5);

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
INSERT INTO medicalRecord (diagnosis, note, patientID, appointmentID)
VALUES 
    ('Khong van de', 'Tinh trang tot', 1, 1),
    ('Sau rang', 'Da tram rang', 2, 2),
    ('Rang khon', 'Da nho rang', 3, 3),
    ('Rang o mau', 'Da tay trang', 4, 4),
    ('Rang lech', 'Da nieng rang', 5, 5);

-- Insert data into transaction table
INSERT INTO "transaction" (price, transactionTime, accountID, appointmentID)
VALUES 
    (100000, '09:00:00', 1, 1),
    (200000, '10:00:00', 2, 2),
    (300000, '11:00:00', 3, 3),
    (400000, '12:00:00', 4, 4),
    (500000, '13:00:00', 5, 5);
