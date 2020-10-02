CREATE TABLE Clients (
	client_id int IDENTITY(1,1) PRIMARY KEY,
	client_first_name varchar(50) NOT NULL,
	client_last_name varchar(100) NOT NULL,
	client_age int NOT NULL,
	client_phone_number int NOT NULL,
	client_address varchar(250)
)

CREATE TABLE Rooms (
	room_id int IDENTITY(1,1) PRIMARY KEY,
	room_number int NOT NULL,
	room_people_capacity int NOT NULL,
	room_bathroom int NOT NULL
)

CREATE TABLE Room_reservation (
	client_id int FOREIGN KEY REFERENCES Clients(client_id),
	room_id int FOREIGN KEY REFERENCES Rooms(room_id),
	price decimal NOT NULL,
	check_in date NOT NULL,
	check_out date NOT NULL,
)

CREATE TABLE Employees (
	employee_id int IDENTITY(1,1) PRIMARY KEY,
	employee_first_name varchar(50) NOT NULL,
	employee_last_name varchar(100) NOT NULL,
	employee_salary decimal NOT NULL,
	employee_position varchar(50) NOT NULL
)

CREATE TABLE Accounts (
	account_id int IDENTITY(1,1) PRIMARY KEY,
	account_login varchar(50) NOT NULL,
	account_password varchar(50) NOT NULL,
	account_type varchar(50) NOT NULL
)