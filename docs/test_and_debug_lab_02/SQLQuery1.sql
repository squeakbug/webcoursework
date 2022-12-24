use master;
go

IF EXISTS(SELECT * FROM sys.databases WHERE name = 'testdb')
	DROP DATABASE testdb;

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'the_dotfactory_test')
BEGIN
    CREATE DATABASE [the_dotfactory_test]
END

CREATE DATABASE testdb
ON ( NAME = the_dotfactory_test,
     FILENAME = '/var/opt/mssql/data/testdb.ss' )
AS SNAPSHOT OF the_dotfactory_test;
go

use the_dotfactory_test;
go

/*
create table dbo.UserInfo (
	id integer identity(1, 1),
	n nvarchar(MAX) null,
	pass nvarchar(MAX) null,
	ln nvarchar(MAX) null,
	loggined integer null,
);

create table dbo.UserConfig (
	id integer identity(1, 1),
	displayName nvarchar(MAX) null
);

create table dbo.Font (
	id integer identity(1, 1),
	sz integer null,
	n nvarchar(MAX) null
);
*/

insert into dbo.UserInfo
(n, pass, ln, loggined)
values
('default', 'password', 'user_001', 0)

-- Login
declare @userId int;

select @userId = id
from dbo.UserInfo
where ln like 'user_001'

update dbo.UserInfo
set loggined = 1
where id = @userId

select *
from dbo.UserInfo
where ln like 'user_001'

-- add font
insert into dbo.Font
(sz, n)
values
(10, 'Verdana')

select *
from dbo.Font
where n like 'Verdana'

-- add config
insert into dbo.UserConfig
(displayName)
values
('default')

select *
from dbo.UserConfig
where displayName like 'default'

select *
from dbo.Font
where n like 'Verdana'

-- Logout

select @userId = id
from dbo.UserInfo
where ln like 'user_001'

update dbo.UserInfo
set loggined = 0
where id = @userId

select *
from dbo.UserInfo
where ln like 'user_001'

use master;
go

RESTORE DATABASE the_dotfactory_test
FROM DATABASE_SNAPSHOT = 'testdb';
DROP DATABASE testdb;
go
