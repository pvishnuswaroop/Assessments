--Q-1:Provide a SQL script that initializes the database for the Job Board scenario “CareerHub”.

CREATE DATABASE CareerHub;
USE CareerHub;

--Q-2:Create tables for Companies, Jobs, Applicants and Applications. 

CREATE TABLE Companies (
    CompanyID INT PRIMARY KEY IDENTITY(1,1),
    CompanyName VARCHAR(30),
    Location VARCHAR(30)
);

CREATE TABLE Jobs (
    JobID INT PRIMARY KEY IDENTITY(1,1),
    CompanyID INT,
    JobTitle VARCHAR(30),
    JobDescription TEXT,
    JobLocation VARCHAR(30),
    Salary FLOAT,
    JobType VARCHAR(30),
    PostedDate DATETIME,
    FOREIGN KEY (CompanyID) REFERENCES Companies(CompanyID)
);

CREATE TABLE Applicants (
    ApplicantID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(30),
    LastName VARCHAR(30),
    Email VARCHAR(30),
    Phone VARCHAR(30),
    Resume TEXT
);

CREATE TABLE Applications (
    ApplicationID INT PRIMARY KEY IDENTITY(1,1),
    JobID INT,
    ApplicantID INT,
    ApplicationDate DATETIME,
    CoverLetter TEXT,
    FOREIGN KEY (JobID) REFERENCES Jobs(JobID),
    FOREIGN KEY (ApplicantID) REFERENCES Applicants(ApplicantID)
);


-- Inserting dummy data into Companies
INSERT INTO Companies (CompanyName, Location) VALUES
('Tech Solutions', 'New York'),
('Innovative Systems', 'San Francisco'),
('Global Marketing', 'Chicago'),
('Data Analytics Inc.', 'Los Angeles'),
('Web Development Co.', 'Austin');

-- Inserting dummy data into Jobs
INSERT INTO Jobs (CompanyID, JobTitle, JobDescription, JobLocation, Salary, JobType, PostedDate) VALUES
(1, 'Software Engineer', 'Develop and maintain software applications.', 'New York', 95000, 'Full-time', GETDATE()),
(1, 'Junior Software Engineer', 'Assist in software development tasks.', 'New York', 70000, 'Full-time', GETDATE()),
(2, 'Data Analyst', 'Analyze and interpret complex data sets.', 'San Francisco', 80000, 'Full-time', GETDATE()),
(2, 'Data Scientist', 'Build models and algorithms for data analysis.', 'San Francisco', 120000, 'Full-time', GETDATE()),
(3, 'Marketing Manager', 'Oversee marketing strategies and campaigns.', 'Chicago', 85000, 'Full-time', GETDATE()),
(4, 'Business Analyst', 'Gather and analyze business requirements.', 'Los Angeles', 90000, 'Full-time', GETDATE()),
(5, 'Web Developer', 'Create and maintain websites.', 'Austin', 75000, 'Contract', GETDATE()),
(5, 'Front End Developer', 'Develop user interfaces for web applications.', 'Austin', 70000, 'Full-time', GETDATE());

-- Inserting dummy data into Applicants
INSERT INTO Applicants (FirstName, LastName, Email, Phone, Resume) VALUES
('John', 'Doe', 'john.doe@example.com', '123-456-7890', 'Resume of John Doe.'),
('Jane', 'Smith', 'jane.smith@example.com', '098-765-4321', 'Resume of Jane Smith.'),
('Emily', 'Johnson', 'emily.johnson@example.com', '555-555-5555', 'Resume of Emily Johnson.'),
('Michael', 'Brown', 'michael.brown@example.com', '333-444-5555', 'Resume of Michael Brown.'),
('Sarah', 'Davis', 'sarah.davis@example.com', '222-333-4444', 'Resume of Sarah Davis.');

-- Inserting dummy data into Applications
INSERT INTO Applications (JobID, ApplicantID, ApplicationDate, CoverLetter) VALUES
(1, 1, GETDATE(), 'I am very interested in the Software Engineer position.'),
(1, 3, GETDATE(), 'I would love to contribute as a Software Engineer.'),
(2, 2, GETDATE(), 'I believe I am a great fit for the Junior Software Engineer role.'),
(3, 4, GETDATE(), 'I have a strong background in data analysis.'),
(4, 1, GETDATE(), 'I am eager to bring my data science skills to your team.'),
(5, 5, GETDATE(), 'I am excited about the opportunity to work as a Marketing Manager.'),
(6, 3, GETDATE(), 'I am a qualified applicant for the Business Analyst position.'),
(7, 2, GETDATE(), 'I am passionate about web development and would love to join your team.'),
(8, 4, GETDATE(), 'I am interested in the Front End Developer position.');


--Q-5:Query to Count Applications for Each Job Listing:
SELECT J.JobID, J.JobTitle, COUNT(A.ApplicationID) AS ApplicationCount
FROM Jobs J
LEFT JOIN Applications A ON J.JobID = A.JobID
GROUP BY J.JobID, J.JobTitle
ORDER BY J.JobID;


-- Q-6:Retrieve Job Listings Within a Specified Salary Range
SELECT MIN(Salary) AS MinSalary, MAX(Salary) AS MaxSalary 
FROM Jobs;
SELECT J.JobTitle, C.CompanyName, J.JobLocation, J.Salary
FROM Jobs J
INNER JOIN Companies C ON J.CompanyID = C.CompanyID
WHERE J.Salary BETWEEN 60000 AND 95000;

--Q-7:Query to Retrieve Job Application History for a Specific Applicant
SELECT * FROM Applicants;

INSERT INTO Applicants (FirstName, LastName, Email, Phone, Resume) VALUES
('Alice', 'Green', 'alice.green@example.com', '777-888-9999', 'Resume of Alice Green.');

INSERT INTO Applications (JobID, ApplicantID, ApplicationDate, CoverLetter) VALUES
(8, 7, GETDATE(), 'I am very interested in the Front End Developer position.');


SELECT J.JobTitle, C.CompanyName, A.ApplicationDate 
FROM Applications A
INNER JOIN Jobs J ON A.JobID = J.JobID
INNER JOIN Companies C ON J.CompanyID = C.CompanyID 
WHERE A.ApplicantID = 7;

--Q-8:Query to Calculate Average Salary (Ignoring Zero Salaries)
SELECT AVG(Salary) AS AverageSalary
FROM Jobs
WHERE Salary > 0;

--Q-9:Query to Find the Company that Posted the Most Jobs

WITH CompanyJobCount AS (
    SELECT 
        C.CompanyName, 
        COUNT(J.JobID) AS JobCount
    FROM  Companies C
    LEFT JOIN Jobs J ON C.CompanyID = J.CompanyID
    GROUP BY 
        C.CompanyName
)
SELECT 
    CompanyName, 
    JobCount
FROM CompanyJobCount
WHERE JobCount = (SELECT MAX(JobCount) FROM CompanyJobCount);


--Q-10:Query to Find Applicants with 3+ Years Experience in 'CityX'
ALTER TABLE Applicants 
ADD ExperienceYears INT;  

UPDATE Applicants SET ExperienceYears = 3 WHERE ApplicantID = 1; 
UPDATE Applicants SET ExperienceYears = 3 WHERE ApplicantID = 2;  
UPDATE Applicants SET ExperienceYears = 4 WHERE ApplicantID = 3; 
UPDATE Applicants SET ExperienceYears = 3 WHERE ApplicantID = 4;  
UPDATE Applicants SET ExperienceYears = 4 WHERE ApplicantID = 5;  
UPDATE Applicants SET ExperienceYears = 3 WHERE ApplicantID = 6;  

SELECT DISTINCT 
    A.FirstName, 
    A.LastName, 
    A.Email 
FROM Applicants A
INNER JOIN Applications Ap ON A.ApplicantID = Ap.ApplicantID
INNER JOIN Jobs J ON Ap.JobID = J.JobID
INNER JOIN Companies C ON J.CompanyID = C.CompanyID
WHERE C.Location = 'CityX' AND A.ExperienceYears >= 3;



--Q-11:Query to List Distinct Job Titles with Salaries Between $60,000 and $80,000
SELECT DISTINCT JobTitle
FROM Jobs
WHERE Salary BETWEEN 60000 AND 80000;

--Q-12:Query to Find Jobs That Have Not Received Applications
SELECT * FROM  Jobs J
LEFT JOIN Applications A ON J.JobID = A.JobID
WHERE A.ApplicationID IS NULL; 


--Q-13:Retrieve a List of Job Applicants with the Companies and Positions
SELECT 
    A.FirstName, 
    A.LastName, 
    A.Email, 
    C.CompanyName, 
    J.JobTitle 
FROM Applicants A
INNER JOIN Applications Ap ON A.ApplicantID = Ap.ApplicantID
INNER JOIN Jobs J ON Ap.JobID = J.JobID
INNER JOIN Companies C ON J.CompanyID = C.CompanyID
ORDER BY A.LastName, A.FirstName;  


--Q-14:Retrieve Companies and the Count of Jobs Posted (Even Without Applications)
SELECT 
    C.CompanyName, 
    COUNT(J.JobID) AS JobCount
FROM  Companies C
LEFT JOIN Jobs J ON C.CompanyID = J.CompanyID
GROUP BY C.CompanyID, C.CompanyName
ORDER BY C.CompanyName; 

--Q-15:List All Applicants Along with Companies and Positions
SELECT * FROM  Applicants A
LEFT JOIN Applications Ap ON A.ApplicantID = Ap.ApplicantID
LEFT JOIN Jobs J ON Ap.JobID = J.JobID
LEFT JOIN Companies C ON J.CompanyID = C.CompanyID
ORDER BY A.LastName, A.FirstName;  

--Q-16:Find Companies that Posted Jobs with Salaries Higher than the Average
WITH AverageSalary AS (
    SELECT AVG(Salary) AS AvgSalary
    FROM Jobs
)
SELECT DISTINCT  C.CompanyName 
FROM  Companies C
INNER JOIN Jobs J ON C.CompanyID = J.CompanyID
WHERE J.Salary > (SELECT AvgSalary FROM AverageSalary);

--Q-17:Display Applicants with a Concatenated String of City and State
ALTER TABLE Applicants 
ADD City VARCHAR(50), 
    State VARCHAR(50);

UPDATE Applicants SET City = 'New York', State = 'NY' WHERE ApplicantID = 1;
UPDATE Applicants SET City = 'San Francisco', State = 'CA' WHERE ApplicantID = 2;
UPDATE Applicants SET City = 'Los Angeles', State = 'CA' WHERE ApplicantID = 3;
UPDATE Applicants SET City = 'Chicago', State = 'IL' WHERE ApplicantID = 4;
UPDATE Applicants SET City = 'Austin', State = 'TX' WHERE ApplicantID = 5;
UPDATE Applicants SET City = 'Seattle', State = 'WA' WHERE ApplicantID = 6;

SELECT 
    A.FirstName, 
    A.LastName, 
    CONCAT(A.City, ', ', A.State) AS Location 
FROM Applicants A;


--Q-18: Retrieve Jobs with Titles Containing 'Developer' or 'Engineer'
SELECT * FROM Jobs
WHERE 
    JobTitle LIKE '%Developer%' OR JobTitle LIKE '%Engineer%';

--Q-19: Applicants and Jobs (Including Those Without Applications)
SELECT 
    A.FirstName, 
    A.LastName, 
    A.Email, 
    J.JobTitle, 
    C.CompanyName
FROM Applicants A
LEFT JOIN  Applications Ap ON A.ApplicantID = Ap.ApplicantID
LEFT JOIN Jobs J ON Ap.JobID = J.JobID
LEFT JOIN  Companies C ON J.CompanyID = C.CompanyID
ORDER BY  A.LastName, A.FirstName; 

--Q-20: List All Combinations of Applicants and Companies in a Specific City:
SELECT 
    A.FirstName, 
    A.LastName, 
    A.Email, 
    C.CompanyName, 
    C.Location
FROM Applicants A
INNER JOIN Applications Ap ON A.ApplicantID = Ap.ApplicantID
INNER JOIN  Jobs J ON Ap.JobID = J.JobID
INNER JOIN  Companies C ON J.CompanyID = C.CompanyID
WHERE C.Location = 'Chennai' AND A.ExperienceYears > 2;





