--RESTORE FILELISTONLY
--FROM DISK = '/var/opt/mssql/backup/KarmaDB.bak'


RESTORE DATABASE KarmaDB
FROM DISK = '/var/opt/mssql/backup/KarmaDB.bak'
WITH 
MOVE 'KarmaDB' TO '/var/opt/mssql/data/KarmaDB.mdf',
MOVE 'KarmaDB_log' TO '/var/opt/mssql/data/KarmaDB_log.ldf',
REPLACE
