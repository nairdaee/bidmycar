create table UsersInfo(
UserID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
Name varchar(255) NOT NULL,
Email varchar(255) NOT NULL,
Password varchar(255) NOT NULL
)