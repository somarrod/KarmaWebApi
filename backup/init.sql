
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'KarmaDB')
BEGIN
    RESTORE DATABASE KarmaDB
    FROM DISK = '/var/opt/mssql/backup/KarmaDB.bak'
    WITH 
        MOVE 'KarmaDB' TO '/var/opt/mssql/data/KarmaDB.mdf',
        MOVE 'KarmaDB_log' TO '/var/opt/mssql/data/KarmaDB.ldf',
        REPLACE
END
