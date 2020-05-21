/*
 # IMPORTANT !!! Also remember to set the script file build action = "Embedded resource"
 # IMPORTANT !!! Implement object validation checks before executing changes to avoid crashes.  For ex. If Not Exists(....).....

 # Folder structure provide the sequesnce to run scripts in alfabetic order
 # File naming standard is based on date time format to provide a alfabetic sequence to be used to run the scripts in the appropriate order
    00000000-0000-0000
    YYYYMMDD-HHmm-0000
*/

/*
### Sample update statement to reset a script that has been already run like a update to a table, stored proc, etc.   ###

Update dbo.SchemaVersions
	Set ScriptName = ScriptName + ' --Dt '+ Format(GetDate(), 'yyyy-MM-dd HH:mm:ss') + ' --Reset'
	Where ScriptName = 'NetCore.DbMigrations.Scripts._01_Deployment._01_Schema_Tables.20190906-1001-Tbl-Repository.sql'

*/