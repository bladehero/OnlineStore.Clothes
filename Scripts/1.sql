create database ClothesDB;
go

use ClothesDB;
go

drop table if exists dbo.Newsletters;
drop table if exists dbo.[Messages];
drop table if exists dbo.ProductPictures;
drop table if exists dbo.TaggedProducts;
drop table if exists dbo.OrderProducts;
drop table if exists dbo.Orders;
drop table if exists dbo.Carts;
drop table if exists dbo.Users;
drop table if exists dbo.Tags;
drop table if exists dbo.Products;
drop table if exists dbo.Categories;
go

create table dbo.Categories
(
Id int not null primary key identity,
IsDeleted bit not null default (0),
[Name] nvarchar(128) not null unique,
[Image] varchar(1024) null
);
go

create table dbo.Products
(
Id int not null primary key identity,
IsDeleted bit not null default (0),
[Name] nvarchar(256) not null unique,
[Description] nvarchar(max) not null default (N'No description yet...'),
Price float not null,
ReviewCount int not null default (0),
Mark int not null default (5) check (Mark between 0 and 5),
[Image] varchar(1024) null,
CategoryId int not null foreign key references dbo.Categories(Id)
);
go

create table dbo.Tags
(
Id int not null primary key identity,
IsDeleted bit not null default (0),
[Name] nvarchar(128) not null unique,
[Image] varchar(2048) null
);
go

create table dbo.Users
(
Id int not null primary key identity,
IsDeleted bit not null default (0),
FirstName nvarchar(128) not null,
LastName nvarchar(128) null,
Email varchar(1024) not null unique,
[Password] varchar(32) not null,
);
go

create table dbo.Carts
(
Id int not null primary key identity,
IsDeleted bit not null default (0),
UserId int not null foreign key references dbo.Users(Id) unique,
OrderBody nvarchar(max) null,
[DateTime] datetime not null default (getdate())
);
go

create table dbo.Orders
(
Id int not null primary key identity,
IsDeleted bit not null default (0),
UserId int not null foreign key references dbo.Users(Id),
[DateTime] datetime not null default (getdate())
);
go

create table dbo.OrderProducts
(
Id int not null primary key identity,
IsDeleted bit not null default (0),
ProductId int not null foreign key references dbo.Products(Id),
OrderId int not null foreign key references dbo.Orders(Id),
Quantity int not null default(1)

unique (ProductId, OrderId)
);
go

create table dbo.TaggedProducts
(
Id int not null primary key identity,
IsDeleted bit not null default (0),
ProductId int not null foreign key references dbo.Products(Id),
TagId int not null foreign key references dbo.Tags(Id),

unique (ProductId, TagId)
);
go

create table dbo.ProductPictures
(
Id int not null primary key identity,
IsDeleted bit not null default (0),
ProductId int not null foreign key references dbo.Products(Id),
[Image] varchar(1024) not null

unique ([Image], ProductId)
);
go

create table dbo.[Messages]
(
Id int not null primary key identity,
IsDeleted bit not null default (0),
[Name] nvarchar(1024) not null,
Email varchar(1024) not null,
Phone varchar(15) null,
[Text] nvarchar(2048) not null
);
go

create table dbo.Newsletters
(
Id int not null primary key identity,
IsDeleted bit not null default (0),
Email varchar(1024) not null unique
);
go