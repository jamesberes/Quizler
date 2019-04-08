---- Switch to the system (aka master) database
USE master;
GO

---- Delete the FlashCards Database (IF EXISTS)
--IF (EXISTS(select * from sys.databases where name='FlashCards'))
--	DROP DATABASE FlashCards;
--GO

---- Create a new FlashCards Database
CREATE DATABASE FlashCards;
GO

-- Switch to the FlashCards Database
USE FlashCards;
GO

-- Begin a TRANSACTION that must complete with no errors
BEGIN TRANSACTION;

create table decks
(
	id int primary key identity(1,1),
	name nvarchar(max) not null,
	description nvarchar(max) not null,
	date_created datetime default current_timestamp not null,
	is_public bit default(0) not null,
	for_review bit default(0) not null,
	users_id int not null
);

create table cards
(
	id int primary key identity(1,1),
	front nvarchar(max) not null,
	back nvarchar(max) not null,
	img nvarchar(max),
	card_order int not null,
	deck_id int not null
);

create table users
(
	id int primary key identity(1,1),
	display_name varchar(max) null,
	email varchar(max) not null,
	password varchar(max) not null,
	salt varchar(max) not null,
	is_admin bit default(0) not null
);

create table tags
(
	id int primary key identity(1,1),
	tag varchar(max) not null,
	card_id int not null
);

ALTER TABLE	decks
ADD FOREIGN KEY (users_id) References users(id);

ALTER TABLE	cards
ADD FOREIGN KEY (deck_id) References decks(id);
	
ALTER TABLE tags
ADD FOREIGN KEY (card_id) References cards(id);

COMMIT TRANSACTION;