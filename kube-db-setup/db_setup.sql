USE [master]

GO

PRINT 'Creating login for $(APP_USER)';

IF NOT EXISTS (SELECT [loginname] FROM [master].[dbo].[syslogins] WHERE [name] = '$(APP_USER)')

BEGIN

    CREATE LOGIN [$(APP_USER)] WITH PASSWORD = '$(APP_PASS)';

    PRINT 'Login created';

END



ALTER LOGIN [$(APP_USER)] ENABLE

PRINT 'Login enabled';

ALTER LOGIN [$(APP_USER)] WITH PASSWORD=N'$(APP_PASS)'

PRINT 'Password set';

GO

GRANT CONNECT SQL TO [$(APP_USER)]

GO

DROP DATABASE IF EXISTS GLAA_Core

GO

-- CREATE DATABASE GLAA_Core

-- GO

-- PRINT 'Database created';

-- USE [GLAA_Core]

-- GO



IF NOT EXISTS (SELECT [name] FROM [sys].[database_principals] WHERE [type] = 'S' AND [name] = '$(APP_USER)')

BEGIN

    CREATE USER [$(APP_USER)] FOR LOGIN [$(APP_USER)] WITH DEFAULT_SCHEMA = [dbo];

    PRINT 'Database user created';

END



GRANT ALTER ON SCHEMA::dbo TO [$(APP_USER)]

GO

GRANT CREATE DATABASE TO [$(APP_USER)]

GO

GRANT CREATE TABLE TO [$(APP_USER)]

GO

GRANT REFERENCES TO [$(APP_USER)]

GO

GRANT INSERT TO [$(APP_USER)]

GO

GRANT UPDATE TO [$(APP_USER)]

GO

GRANT DELETE TO [$(APP_USER)]

GO

GRANT SELECT ON SCHEMA::dbo TO [$(APP_USER)]

GO

PRINT 'DB permissions granted';
