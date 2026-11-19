USE [master]
GO
/****** Object:  Database [Hospital]    Script Date: 11/19/2026 3:03:11 PM ******/
CREATE DATABASE [Hospital]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Hospital', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.MSSQLSERVER\MSSQL\DATA\Hospital.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Hospital_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.MSSQLSERVER\MSSQL\DATA\Hospital_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Hospital] SET COMPATIBILITY_LEVEL = 170
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Hospital].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Hospital] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Hospital] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Hospital] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Hospital] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Hospital] SET ARITHABORT OFF 
GO
ALTER DATABASE [Hospital] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Hospital] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Hospital] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Hospital] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Hospital] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Hospital] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Hospital] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Hospital] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Hospital] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Hospital] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Hospital] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Hospital] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Hospital] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Hospital] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Hospital] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Hospital] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Hospital] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Hospital] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Hospital] SET  MULTI_USER 
GO
ALTER DATABASE [Hospital] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Hospital] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Hospital] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Hospital] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Hospital] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Hospital] SET OPTIMIZED_LOCKING = OFF 
GO
ALTER DATABASE [Hospital] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Hospital] SET QUERY_STORE = ON
GO
ALTER DATABASE [Hospital] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Hospital]
GO
/****** Object:  Table [dbo].[Appointment]    Script Date: 11/19/2026 3:03:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointment](
	[AppointmentID] [int] NOT NULL,
	[PatientID] [int] NULL,
	[DoctorID] [int] NULL,
	[AppointmentDate] [date] NULL,
	[AppointmentTime] [time](7) NULL,
	[Status] [varchar](20) NULL,
 CONSTRAINT [PK_Appointment] PRIMARY KEY CLUSTERED 
(
	[AppointmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Billing]    Script Date: 11/19/2026 3:03:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Billing](
	[BillID] [int] NOT NULL,
	[AppointmentID] [int] NULL,
	[Amount] [decimal](10, 2) NULL,
	[PaymentDate] [date] NULL,
	[PaymentMethod] [varchar](30) NULL,
	[Status] [varchar](20) NULL,
 CONSTRAINT [PK_Billing] PRIMARY KEY CLUSTERED 
(
	[BillID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 11/19/2026 3:03:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[DepartmentID] [int] NOT NULL,
	[DeptName] [varchar](50) NULL,
	[Location] [varchar](100) NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[DepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Doctor]    Script Date: 11/19/2026 3:03:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Doctor](
	[DoctorID] [int] NOT NULL,
	[Name] [varchar](100) NULL,
	[Specialty] [varchar](100) NULL,
	[Phone] [varchar](20) NULL,
	[Email] [varchar](100) NULL,
	[DepartmentID] [int] NULL,
 CONSTRAINT [PK_Doctor] PRIMARY KEY CLUSTERED 
(
	[DoctorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicalRecord]    Script Date: 11/19/2026 3:03:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicalRecord](
	[RecordID] [int] NOT NULL,
	[AppointmentID] [int] NULL,
	[Diagnosis] [varchar](255) NULL,
	[Notes] [text] NULL,
	[RecordDate] [date] NULL,
 CONSTRAINT [PK_MedicalRecord] PRIMARY KEY CLUSTERED 
(
	[RecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patient]    Script Date: 11/19/2026 3:03:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patient](
	[PatientID] [int] NOT NULL,
	[Name] [varchar](100) NULL,
	[DOB] [date] NULL,
	[Gender] [char](1) NULL,
	[Phone] [varchar](20) NULL,
	[Address] [varchar](200) NULL,
	[Email] [varchar](100) NULL,
 CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED 
(
	[PatientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prescription]    Script Date: 11/19/2026 3:03:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prescription](
	[PrescriptionID] [int] NOT NULL,
	[RecordID] [int] NULL,
	[MedicineName] [varchar](100) NULL,
	[Dosage] [varchar](50) NULL,
	[Instructions] [varchar](200) NULL,
 CONSTRAINT [PK_Prescription] PRIMARY KEY CLUSTERED 
(
	[PrescriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_Doctor] FOREIGN KEY([DoctorID])
REFERENCES [dbo].[Doctor] ([DoctorID])
GO
ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_Appointment_Doctor]
GO
ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_Patient] FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patient] ([PatientID])
GO
ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_Appointment_Patient]
GO
ALTER TABLE [dbo].[Billing]  WITH CHECK ADD  CONSTRAINT [FK_Billing_Appointment] FOREIGN KEY([AppointmentID])
REFERENCES [dbo].[Appointment] ([AppointmentID])
GO
ALTER TABLE [dbo].[Billing] CHECK CONSTRAINT [FK_Billing_Appointment]
GO
ALTER TABLE [dbo].[Doctor]  WITH CHECK ADD  CONSTRAINT [FK_Doctor_Department] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Department] ([DepartmentID])
ON UPDATE SET DEFAULT
GO
ALTER TABLE [dbo].[Doctor] CHECK CONSTRAINT [FK_Doctor_Department]
GO
ALTER TABLE [dbo].[MedicalRecord]  WITH CHECK ADD  CONSTRAINT [FK_MedicalRecord_Appointment] FOREIGN KEY([AppointmentID])
REFERENCES [dbo].[Appointment] ([AppointmentID])
GO
ALTER TABLE [dbo].[MedicalRecord] CHECK CONSTRAINT [FK_MedicalRecord_Appointment]
GO
ALTER TABLE [dbo].[Prescription]  WITH CHECK ADD  CONSTRAINT [FK_Prescription_MedicalRecord] FOREIGN KEY([RecordID])
REFERENCES [dbo].[MedicalRecord] ([RecordID])
GO
ALTER TABLE [dbo].[Prescription] CHECK CONSTRAINT [FK_Prescription_MedicalRecord]
GO
/*  SAMPLE DATA  */
USE [Hospital]
GO

/* ---------- Department (6) ---------- */
INSERT INTO Department (DepartmentID, DeptName, Location) VALUES
(222, 'Cardiology',       'Floor 2'),
(333, 'Pediatrics',       'Floor 1'),
(444, 'Orthopedics',      'Floor 3'),
(555, 'Dermatology',      'Floor 1'),
(111, 'General Medicine', 'Floor 4'),
(666, 'Neurology',        'Floor 2');

/* ---------- Doctor (10) ---------- */
INSERT INTO Doctor (DoctorID, Name, Specialty, Phone, Email, DepartmentID) VALUES
(1,  'Dr. Sarah Mitchell', 'Interventional Cardiology', '(210) 555-0101', 'smitchell@cityclinic.example', 222),
(2,  'Dr. James Okafor',   'Cardiac Electrophysiology', '(210) 555-0102', 'jokafor@cityclinic.example',   222),
(3,  'Dr. Priya Nair',     'General Pediatrics',        '(210) 555-0103', 'pnair@cityclinic.example',     333),
(4,  'Dr. Carlos Ramirez', 'Pediatric Infectious Disease', '(210) 555-0104', 'cramirez@cityclinic.example', 333),
(5,  'Dr. Emily Chen',     'Sports Medicine',           '(210) 555-0105', 'echen@cityclinic.example',     111),
(6,  'Dr. Michael Brooks', 'Spine Surgery',             '(210) 555-0106', 'mbrooks@cityclinic.example',   444),
(7,  'Dr. Hannah Weiss',   'Medical Dermatology',       '(210) 555-0107', 'hweiss@cityclinic.example',    555),
(8,  'Dr. Robert Alvarez', 'Internal Medicine',         '(210) 555-0108', 'ralvarez@cityclinic.example',  111),
(9,  'Dr. Aisha Rahman',   'Family Medicine',           '(210) 555-0109', 'arahman@cityclinic.example',   111),
(10, 'Dr. Daniel Kim',     'Headache and Epilepsy',     '(210) 555-0110', 'dkim@cityclinic.example',      666);

/* ---------- Patient (20) ---------- */
INSERT INTO Patient (PatientID, Name, DOB, Gender, Phone, Address, Email) VALUES
(1,  'Olivia Martinez',   '1988-03-14', 'F', '(210) 555-0201', '412 Maple Ave, San Antonio, TX 78212',     'olivia.martinez@example.com'),
(2,  'Liam Johnson',      '1975-11-02', 'M', '(210) 555-0202', '88 Cedar Ln, San Antonio, TX 78209',       'liam.johnson@example.com'),
(3,  'Sophia Nguyen',     '2016-06-21', 'F', '(210) 555-0203', '1507 Oak Hollow Dr, San Antonio, TX 78230', 'nguyen.family@example.com'),
(4,  'Noah Williams',     '2019-09-09', 'M', '(210) 555-0204', '230 Pecan St, San Antonio, TX 78204',      'williams.family@example.com'),
(5,  'Emma Davis',        '1992-01-30', 'F', '(210) 555-0205', '9 Willow Creek Rd, San Antonio, TX 78248', 'emma.davis@example.com'),
(6,  'Mason Garcia',      '1965-07-18', 'M', '(210) 555-0206', '641 Alamo Heights Blvd, San Antonio, TX 78209', 'mason.garcia@example.com'),
(7,  'Ava Thompson',      '1953-12-05', 'F', '(210) 555-0207', '75 Brookside Ct, San Antonio, TX 78217',   'ava.thompson@example.com'),
(8,  'Ethan Brown',       '1999-04-25', 'M', '(210) 555-0208', '320 College Park Dr, San Antonio, TX 78228', 'ethan.brown@example.com'),
(9,  'Isabella Lopez',    '1981-08-12', 'F', '(210) 555-0209', '1180 Vista Del Sol, San Antonio, TX 78232', 'isabella.lopez@example.com'),
(10, 'Lucas Hernandez',   '2013-02-17', 'M', '(210) 555-0210', '57 Rosewood Ave, San Antonio, TX 78201',   'hernandez.family@example.com'),
(11, 'Mia Patel',         '1996-10-03', 'F', '(210) 555-0211', '902 Zarzamora St, San Antonio, TX 78207',  'mia.patel@example.com'),
(12, 'Benjamin Scott',    '1948-05-29', 'M', '(210) 555-0212', '14 Sunset Ridge, San Antonio, TX 78216',   'ben.scott@example.com'),
(13, 'Charlotte Kim',     '1970-09-14', 'F', '(210) 555-0213', '466 Broadway St, San Antonio, TX 78215',   'charlotte.kim@example.com'),
(14, 'Henry Wilson',      '1985-06-06', 'M', '(210) 555-0214', '2201 Culebra Rd, San Antonio, TX 78228',   'henry.wilson@example.com'),
(15, 'Amelia Flores',     '2021-03-11', 'F', '(210) 555-0215', '38 Magnolia Pl, San Antonio, TX 78212',    'flores.family@example.com'),
(16, 'Alexander Reyes',   '1959-01-22', 'M', '(210) 555-0216', '705 Hildebrand Ave, San Antonio, TX 78212', 'alex.reyes@example.com'),
(17, 'Harper Lee',        '2002-11-27', 'F', '(210) 555-0217', '119 Bandera Rd, San Antonio, TX 78228',    'harper.lee@example.com'),
(18, 'Daniel Cooper',     '1978-02-08', 'M', '(210) 555-0218', '3340 Nacogdoches Rd, San Antonio, TX 78217', 'daniel.cooper@example.com'),
(19, 'Evelyn Turner',     '1944-08-19', 'F', '(210) 555-0219', '81 Lakeview Terrace, San Antonio, TX 78209', 'evelyn.turner@example.com'),
(20, 'Jacob Rivera',      '1990-12-12', 'M', '(210) 555-0220', '555 Commerce St, San Antonio, TX 78205',   'jacob.rivera@example.com');

/* ---------- Appointment (30) ----------*/
INSERT INTO Appointment (AppointmentID, PatientID, DoctorID, AppointmentDate, AppointmentTime, Status) VALUES
(1,  7,  1,  '2026-10-01', '09:00', 'Completed'),
(2,  3,  3,  '2026-10-01', '09:30', 'Completed'),
(3,  6,  2,  '2026-10-02', '10:00', 'Completed'),
(4,  8,  5,  '2026-10-02', '11:00', 'Completed'),
(5,  11, 7,  '2026-10-05', '13:00', 'Completed'),
(6,  2,  8,  '2026-10-05', '14:00', 'Completed'),
(7,  4,  4,  '2026-10-06', '09:00', 'Completed'),
(8,  13, 10, '2026-10-06', '10:30', 'Completed'),
(9,  12, 1,  '2026-10-08', '09:30', 'Completed'),
(10, 5,  9,  '2026-10-08', '15:00', 'Completed'),
(11, 14, 6,  '2026-10-09', '11:30', 'Completed'),
(12, 10, 3,  '2026-10-12', '10:00', 'Completed'),
(13, 17, 7,  '2026-10-13', '14:30', 'Completed'),
(14, 16, 2,  '2026-10-14', '09:00', 'Completed'),
(15, 9,  10, '2026-10-15', '13:30', 'Completed'),
(16, 19, 8,  '2026-10-16', '08:30', 'Completed'),
(17, 15, 4,  '2026-10-19', '10:00', 'Completed'),
(18, 18, 5,  '2026-10-20', '16:00', 'Completed'),
(19, 1,  9,  '2026-10-21', '11:00', 'Completed'),
(20, 20, 8,  '2026-10-23', '09:00', 'Completed'),
(21, 14, 7,  '2026-10-22', '10:00', 'Cancelled'),
(22, 8,  6,  '2026-10-23', '14:00', 'No-Show'),
(23, 11, 9,  '2026-10-26', '09:30', 'Cancelled'),
(24, 7,  1,  '2026-10-29', '09:00', 'Scheduled'),
(25, 3,  3,  '2026-10-29', '09:30', 'Scheduled'),
(26, 12, 1,  '2026-10-30', '10:00', 'Scheduled'),
(27, 1,  7,  '2026-11-02', '13:00', 'Scheduled'),
(28, 6,  2,  '2026-11-03', '10:00', 'Scheduled'),
(29, 13, 10, '2026-11-04', '11:00', 'Scheduled'),
(30, 19, 8,  '2026-11-05', '08:30', 'Scheduled');

/* ---------- MedicalRecord (20) ---------- */
INSERT INTO MedicalRecord (RecordID, AppointmentID, Diagnosis, Notes, RecordDate) VALUES
(1,  1,  'Hypertension, stage 1',            'BP 142/90 on two readings. Started low-dose amlodipine. Advised reduced sodium intake. Recheck BP in 4 weeks.', '2026-10-01'),
(2,  2,  'Acute otitis media, right ear',    'Fever 100.8 F for 2 days, ear pain, bulging tympanic membrane. Amoxicillin for 10 days. Return if no improvement in 48 hours.', '2026-10-01'),
(3,  3,  'Atrial fibrillation, rate controlled', 'Resting HR 74. ECG confirms AF. Continue rate control and anticoagulation. Echocardiogram ordered.', '2026-10-02'),
(4,  4,  'Ankle sprain, grade II',           'Inversion injury during a basketball game. Swelling and bruising over lateral ligaments. RICE protocol, brace for 3 weeks.', '2026-10-02'),
(5,  5,  'Atopic dermatitis flare',          'Itchy patches on both forearms. Topical corticosteroid prescribed. Moisturize twice daily and avoid harsh soaps.', '2026-10-05'),
(6,  6,  'Type 2 diabetes mellitus, follow-up', 'A1c 7.4 percent, slightly above target. Continue metformin and diet counseling. Repeat A1c in 3 months.', '2026-10-05'),
(7,  7,  'Seasonal allergic rhinitis',       'Sneezing, nasal congestion, and itchy eyes for 2 weeks. Daily antihistamine recommended.', '2026-10-06'),
(8,  8,  'Migraine without aura',            'Recurrent throbbing headaches with light sensitivity, about 3 per month. Abortive therapy prescribed. Keep a headache diary.', '2026-10-06'),
(9,  9,  'Hyperlipidemia',                   'LDL 168 mg/dL. Started statin therapy. Lipid panel to be repeated in 8 weeks.', '2026-10-08'),
(10, 10, 'Viral upper respiratory infection', 'Cough, sore throat, and low-grade fever for 3 days. No signs of bacterial infection. Supportive care and fluids.', '2026-10-08'),
(11, 11, 'Lumbar muscle strain',              'Lower back pain after lifting. No neurological deficits. Short course of muscle relaxant plus NSAID. Physical therapy referral.', '2026-10-09'),
(12, 12, 'Well-child checkup',               'Height and weight on the 60th percentile. Vaccinations up to date. No concerns.', '2026-10-12'),
(13, 13, 'Acne vulgaris, moderate',          'Inflammatory papules on the face and jaw. Started topical retinoid and oral antibiotic. Follow up in 8 weeks.', '2026-10-13'),
(14, 14, 'Coronary artery disease, stable',  'No chest pain since last visit. Continue antiplatelet and statin therapy. Stress test scheduled.', '2026-10-14'),
(15, 15, 'Tension-type headache',            'Bilateral pressing headaches linked to stress and poor sleep. Neuro exam normal. NSAID as needed and sleep hygiene advice.', '2026-10-15'),
(16, 16, 'Hypothyroidism, follow-up',        'TSH 6.1 mIU/L. Increased levothyroxine dose. Recheck TSH in 6 weeks.', '2026-10-16'),
(17, 17, 'Viral gastroenteritis',            'Vomiting and loose stools for 2 days, mildly dehydrated. Oral rehydration and anti-nausea medication prescribed.', '2026-10-19'),
(18, 18, 'Rotator cuff tendinitis, right shoulder', 'Pain with overhead reaching for 6 weeks. Positive impingement sign. NSAID course and physical therapy.', '2026-10-20'),
(19, 19, 'Uncomplicated urinary tract infection', 'Dysuria and urinary frequency for 2 days. Urinalysis positive for leukocytes. 5-day antibiotic course.', '2026-10-21'),
(20, 20, 'Annual physical, no abnormal findings', 'Routine exam and labs within normal limits. Counseled on exercise and diet. Return in 12 months.', '2026-10-23');

/* ---------- Prescription (22) ---------- */
INSERT INTO Prescription (PrescriptionID, RecordID, MedicineName, Dosage, Instructions) VALUES
(1,  1,  'Amlodipine',            '5 mg',          'Take 1 tablet by mouth once daily in the morning'),
(2,  2,  'Amoxicillin suspension','250 mg/5 mL',   'Give 5 mL by mouth twice daily for 10 days'),
(3,  3,  'Metoprolol succinate',  '50 mg',         'Take 1 tablet by mouth once daily'),
(4,  3,  'Apixaban',              '5 mg',          'Take 1 tablet by mouth twice daily'),
(5,  4,  'Ibuprofen',             '400 mg',        'Take 1 tablet every 8 hours with food for 7 days'),
(6,  5,  'Hydrocortisone cream',  '2.5 percent',   'Apply a thin layer to affected areas twice daily for 14 days'),
(7,  6,  'Metformin',             '500 mg',        'Take 1 tablet twice daily with meals'),
(8,  7,  'Cetirizine',            '5 mg',          'Take 1 tablet once daily'),
(9,  8,  'Sumatriptan',           '50 mg',         'Take 1 tablet at onset of migraine; may repeat once after 2 hours; max 200 mg per day'),
(10, 9,  'Atorvastatin',          '20 mg',         'Take 1 tablet by mouth nightly'),
(11, 10, 'Acetaminophen',         '500 mg',        'Take 1 tablet every 6 hours as needed for fever or pain; max 3,000 mg per day'),
(12, 11, 'Cyclobenzaprine',       '5 mg',          'Take 1 tablet at bedtime as needed for 7 days'),
(13, 11, 'Ibuprofen',             '400 mg',        'Take 1 tablet every 8 hours with food for 5 days'),
(14, 13, 'Tretinoin cream',       '0.025 percent', 'Apply a pea-sized amount to the face every night'),
(15, 13, 'Doxycycline',           '100 mg',        'Take 1 capsule twice daily with food and water for 8 weeks'),
(16, 14, 'Aspirin',               '81 mg',         'Take 1 tablet by mouth once daily'),
(17, 14, 'Atorvastatin',          '40 mg',         'Take 1 tablet by mouth nightly'),
(18, 15, 'Naproxen',              '250 mg',        'Take 1 tablet twice daily as needed for up to 5 days'),
(19, 16, 'Levothyroxine',         '75 mcg',        'Take 1 tablet on an empty stomach every morning'),
(20, 17, 'Ondansetron ODT',       '2 mg',          'Dissolve 1 tablet on tongue every 8 hours as needed for vomiting, up to 2 days'),
(21, 18, 'Naproxen',              '500 mg',        'Take 1 tablet twice daily with food for 10 days'),
(22, 19, 'Nitrofurantoin',        '100 mg',        'Take 1 capsule twice daily for 5 days');

/* ---------- Billing (21) ----------*/
INSERT INTO Billing (BillID, AppointmentID, Amount, PaymentDate, PaymentMethod, Status) VALUES
(1,  1,  250.00, '2026-10-01', 'Insurance',   'Paid'),
(2,  2,  120.00, '2026-10-01', 'Credit Card', 'Paid'),
(3,  3,  280.00, '2026-10-03', 'Insurance',   'Paid'),
(4,  4,  200.00, '2026-10-02', 'Debit Card',  'Paid'),
(5,  5,  175.00, '2026-10-05', 'Credit Card', 'Paid'),
(6,  6,  150.00, '2026-10-07', 'Insurance',   'Paid'),
(7,  7,  120.00, '2026-10-08', 'Insurance',   'Paid'),
(8,  8,  275.00, '2026-10-09', 'Insurance',   'Paid'),
(9,  9,  250.00, '2026-10-08', 'Credit Card', 'Paid'),
(10, 10, 150.00, '2026-10-08', 'Cash',        'Paid'),
(11, 11, 200.00, NULL,         NULL,          'Overdue'),
(12, 12, 120.00, '2026-10-14', 'Insurance',   'Paid'),
(13, 13, 175.00, '2026-10-13', 'Debit Card',  'Paid'),
(14, 14, 280.00, '2026-10-16', 'Insurance',   'Paid'),
(15, 15, 275.00, '2026-10-15', 'Credit Card', 'Paid'),
(16, 16, 150.00, '2026-10-18', 'Insurance',   'Paid'),
(17, 17, 120.00, '2026-10-19', 'Cash',        'Paid'),
(18, 18, 200.00, NULL,         NULL,          'Pending'),
(19, 19, 150.00, NULL,         NULL,          'Pending'),
(20, 20, 150.00, NULL,         NULL,          'Pending'),
(21, 22, 25.00,  NULL,         NULL,          'Pending');
GO
USE [master]
GO
ALTER DATABASE [Hospital] SET  READ_WRITE 
GO
