use DentalClinicDB
--Patient Table
INSERT INTO [dbo].[patient] (patientID, name, age, address, gender, email, password)
VALUES 
(1, 'Default', 20, '123 Street', 1, 'default@gmail.com', '123'),
(2, 'Nguyen', 30, '123 Duong Chinh', 1, 'nguyen@gmail.com', '123'),
(3, 'Tran', 25, '456 Duong Huynh Van Banh', 2, 'tran@gmail.com', '456'),
(4, 'Le', 35, '789 Duong Le Van Sy', 2, 'le@gmail.com', '789'),
(5, 'Pham', 28, '321 Duong Nguyen Thi Minh Khai', 1, 'pham@gmail.com', '1011'),
(6, 'Hoang', 40, '654 Duong Thu Duc', 2, 'hoang@gmail.com', '1213');

-- Dentist table
INSERT INTO [dbo].[dentist] (dentistID, name, email, password, type, contractType, status)
VALUES 
(1, 'Bac si Nguyen','abc@gmail.com','123', 1, 'Full-time', 1),
(2, 'Bac si Tran', '123@gmail.com', '123', 2, 'Part-time', 1),
(3, 'Bac si Le', '234@gmail.com', '123', 1, 'Full-time', 1),
(4, 'Bac si Pham', '321@gmail.com', '123',  2, 'Full-time', 0),
(5, 'Bac si Hoang', '213@gmail.com', '123',  1, 'Full-time', 0);

--  Service table
INSERT INTO [dbo].[medicalService] (medicalServiceID, name, price, description)
VALUES 
(1, 'Ve sinh rang', 100000, 'Ve sinh rang co ban'),
(2, 'Tram rang', 200000, 'Tram rang'),
(3, 'Nho rang', 300000, 'Nho rang'),
(4, 'Tay trang rang', 400000, 'Tay trang rang'),
(5, 'Nieng rang', 5000000, 'Nieng rang chinh nha');

-- Schedule table
INSERT INTO [dbo].[schedule] (scheduleID, workDate, timeSlot, status, dentistID)
VALUES 
(1, '2024-05-24', 1, 1, 1),
(2, '2024-05-24', 2, 1, 2),
(3, '2024-05-25', 1, 1, 3),
(4, '2024-05-25', 2, 1, 4),
(5, '2024-05-26', 1, 1, 5);

-- Appointment table
INSERT INTO [dbo].[appointment] (appointmentID, arrivalDate, type, status, timeSlot, patientID)
VALUES 
(1, '2024-05-24', 'New Patient', 1, 1, 1),
(2, '2024-05-24', 'Routine Check-up', 1, 2, 2),
(3, '2024-05-25', 'Emergency', 1, 1, 3),
(4, '2024-05-25', 'New Patient', 1, 2, 4),
(5, '2024-05-26', 'Emergency', 1, 1, 5);

-- AppointmentDetails table
INSERT INTO [dbo].[appointmentDetails] (appointmentDetailID, appointmentID, medicalServiceID, dentistID, scheduleID)
VALUES 
(1, 1, 1, 1, 1),
(2, 2, 2, 2, 2),
(3, 3, 3, 3, 3),
(4, 4, 4, 4, 4),
(5, 5, 5, 5, 5);

-- DentistService table
INSERT INTO [dbo].[dentistMedicalService] (dentistMedicalServiceID, dentistID, medicalServiceID)
VALUES 
(1, 1, 1),
(2, 2, 2),
(3, 3, 3),
(4, 4, 4),
(5, 5, 5);

-- MedicalRecord table
INSERT INTO [dbo].[medicalRecord] (recordID, treatment, diagnosis, note, followUpDate, patientID, appointmentID)
VALUES 
(1, 'Ve sinh rang', 'Khong van de', 'Tinh trang tot', '2024-06-24', 1, 1),
(2, 'Tram rang', 'Sau rang', 'Da tram rang', '2024-06-24', 2, 2),
(3, 'Nho rang', 'Rang khon', 'Da nho rang', '2024-06-25', 3, 3),
(4, 'Tay trang rang', 'Rang o mau', 'Da tay trang', '2024-06-25', 4, 4),
(5, 'Nieng rang', 'Rang lech', 'Da nieng rang', '2024-06-26', 5, 5);

-- Transaction table
INSERT INTO [dbo].[transaction] (transactionID, price, transactionTime, appointmentID)
VALUES 
(1, 100000, '09:00:00', 1),
(2, 200000, '10:00:00', 2),
(3, 300000, '11:00:00', 3),
(4, 400000, '12:00:00', 4),
(5, 500000, '13:00:00', 5);

use master