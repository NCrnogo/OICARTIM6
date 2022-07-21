
CREATE table Teams
(
	IDTeam int primary key identity(1,1),
	Team nvarchar(50) unique not null,
	Created char(10) not null,
	OwnerID int FOREIGN KEY REFERENCES Users(IDUser) not null,
	TeacherID int FOREIGN KEY REFERENCES Users(IDUser)
)
go

CREATE TABLE TeamMembers
(
	IDTeamMembers int not null identity(1,1) primary key,
	TeamID int FOREIGN KEY REFERENCES Teams(IDTeam) not null,
	UserID int FOREIGN KEY REFERENCES Users(IDUser) not null
);
go

CREATE TABLE TeamInvites(
	IDTeamInvites int not null identity(1,1) primary key,
	TeamID int FOREIGN KEY REFERENCES Teams(IDTeam) not null,
	UserID int FOREIGN KEY REFERENCES Users(IDUser) not null,
	Invited int
)
go

create table Users(
	IDUser int primary key identity(1,1),
	[PasswordHash] [binary](64) NOT NULL,
	[Salt] [nvarchar](36) NOT NULL,
	LoginName nvarchar(50) unique not null, --email
	DateCreated char(10) not null, --YYYY-MM-DD date format
)
go

create table UserRoles(
	IDUserRole int primary key identity(1,1),
	Roll nvarchar(50) not null
)
go

create table UserRollMappings(
	IDUserRoleMapping int primary key identity(1,1),
	UserFK int foreign key references Users(IDUser) not null,
	UserRoleFK int foreign key references UserRoles(IDUserRole) not null
)
go

CREATE TABLE UserWork(
	IDUserWork int primary key identity(1,1),
	DateCreated char(10) not null, --YYYY-MM-DD date format
	Name nvarchar(100),
	Details nvarchar(500),
	UserFK int foreign key references Users(IDUser),
	TeamFK int FOREIGN KEY REFERENCES Teams(IDTeam) not null,
	StartWork char(10) not null, --HH-MM-SS date format
	EndWork char(10) not null, --HH-MM-SS date format
)
go



--DELETED TABLES--
create table Deleted_Teams
(
	IDTeam int primary key identity(1,1),
	Team nvarchar(50) not null,
	Created char(10) not null,
	OwnerID int,
	TeacherID int
)
go
CREATE TABLE Deleted_TeamMembers
(
	IDTeamMembers int not null identity(1,1) primary key,
	TeamID int,
	UserID int
);
go
CREATE TABLE Deleted_TeamInvites(
	IDTeamInvites int not null identity(1,1) primary key,
	TeamID int,
	UserID int,
	Invited int
)
go
CREATE table Deleted_Users(
	IDUser int primary key identity(1,1),
	[PasswordHash] [binary](64) NOT NULL,
	[Salt] [nvarchar](36) NOT NULL,
	LoginName nvarchar(50) not null, --email
	DateCreated char(10) not null, --YYYY-MM-DD date format
)
go
create table Deleted_Dailys(
	IDDaily int primary key identity(1,1),
	DateCreated char(10) not null, --YYYY-MM-DD date format
	Details nvarchar(500),
	UserFK int
)
go

CREATE table Deleted_Activities(
	IDActivities int primary key identity(1,1),
	"Start" char(8) not null, --hh:mm:ss
	"End" char(8),
	UserID int
)
go

CREATE TABLE Deleted_UserWork(
	IDUserWork int primary key identity(1,1),
	DateCreated char(10) not null, --YYYY-MM-DD date format
	Name nvarchar(100),
	Details nvarchar(500),
	UserFK int,
	TeamFK int,
	StartWork char(10) not null, --HH-MM-SS date format
	EndWork char(10) not null, --HH-MM-SS date format
)
go

