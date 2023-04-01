use BidMyCar;
create table UsersInfo(
UserID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
Name varchar(255) NOT NULL,
Email varchar(255) NOT NULL,
Password varchar(255) NOT NULL
)

create table CarDetails(
CarID int IDENTITY(000001,1) NOT NULL PRIMARY KEY,
YOM int CHECK (YOM >=1950  AND YOM <=2023) NOT NULL,
Make varchar(255) NOT NULL,
Model varchar(255) NOT NULL,
BodyType varchar(255) NOT NULL,
Condition varchar(255) NOT NULL,
Category varchar(255) NOT NULL,
Features varchar(5000) NOT NULL,
CarDescription varchar(7500) NOT NULL,
CarLocation varchar(255) NOT NULL,
UploadDate date NOT NULL,
Price int NOT NULL,
UserID int NOT NULL FOREIGN KEY REFERENCES UsersInfo(UserID),
Color VARCHAR(255) NOT NULL,
Miles INT NOT NULL,
Transmission VARCHAR(255) NOT NULL,
EngineSize VARCHAR(255) NOT NULL,
PowerOutput VARCHAR(255) NOT NULL
)
/*add status colums in the above table*/



