USE [master]

GO

PRINT 'Creating login for $(DB_USER)';

IF NOT EXISTS (SELECT [loginname] FROM [master].[dbo].[syslogins] WHERE [name] = '$(DB_USER)')

BEGIN

    CREATE LOGIN [$(DB_USER)] WITH PASSWORD = '$(DB_PASS)';

    PRINT 'Login created';

END



ALTER LOGIN [$(DB_USER)] ENABLE

PRINT 'Login enabled';

ALTER LOGIN [$(DB_USER)] WITH PASSWORD=N'$(DB_PASS)'

PRINT 'Password set';

GO

GRANT CONNECT SQL TO [$(DB_USER)]

GO

DROP DATABASE IF EXISTS GLAA_Core

GO

CREATE DATABASE GLAA_Core

GO

PRINT 'Database created';

USE [GLAA_Core]

GO



IF NOT EXISTS (SELECT [name] FROM [sys].[database_principals] WHERE [type] = 'S' AND [name] = '$(DB_USER)')

BEGIN

    CREATE USER [$(DB_USER)] FOR LOGIN [$(DB_USER)] WITH DEFAULT_SCHEMA = [dbo];

    PRINT 'Database user created';

END



GRANT ALTER ON SCHEMA::dbo TO [$(DB_USER)]

GO

GRANT CREATE TABLE TO [$(DB_USER)]

GO

GRANT INSERT TO [$(DB_USER)]

GO

GRANT UPDATE TO [$(DB_USER)]

GO

GRANT DELETE TO [$(DB_USER)]

GO

GRANT SELECT ON SCHEMA::dbo TO [$(DB_USER)]

GO

PRINT 'DB permissions granted';
