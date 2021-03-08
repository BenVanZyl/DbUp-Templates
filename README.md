# DbUp-Templates

## Introduction 
Providing a base template for use with DbUp that allows you to track changes to your database in a more structured way and the ability to setup re-running scripts that do contain new changes.

This template focus on MS SQL Server T-SQL scripts.  DbUp do support othe databases.  Please see the DbUp documentation for more details.

This template provides sample code using .Net 4.8 and .Net Core 3.1
- Net48 => .Net 4.8 (*Should be compatible with previous versions of .Net*)
- NetCore => .Net Core 3.1  (*Should be compatible with .Net 5*)

## Disclaimer
Like all things in life, this code does not come with any warranty at all.  Please take care of what you are doing and how you use this information (and code) as it might not apply to you requirements or circumstances.

## DbUp

DbUp is a .NET library that helps you to deploy changes to SQL Server databases. It tracks which SQL scripts have been run already, and runs the change scripts that are needed to get your database up to date.
( *https://dbup.readthedocs.io/en/latest/* )


Package can be obtained from NuGet.org:
* https://www.nuget.org/packages/dbup

For more information see:

* https://dbup.github.io/
* https://dbup.readthedocs.io/en/latest/
* https://github.com/DbUp/DbUp


## Getting Started

### Basic concepts and requirements
* Console application
* 1 Application with scripts = Deployment to 1 database
* Database scripts are added in the appropriate folder in the "Scripts" folder of the console application.
* All scripts must be "**Embedded Resources**" 
* Connectionstring can be provided using a 
  * app.config file or 
  * with a command line argument.
* **Important:** All scripts should apply the following principles:
  * Embeded Resource
  * Do not use "Use SomeDbName" statement as scripts are executed in the context of the connectionstring
  * All scripts must be re-runable and apply "If Exists" checks for objecs and data to preent duplication and errors if objects already exists.

### Basic How-to-use
* DbObjects contains a ***single*** script per object.

* Changes to an object requires the script to be run again.  To do this add a script to the folder [Scripts\00_Predeployment] that does one of the following actions:
  * Delete From SchemaVersions table Where script name = '[Net48.DbMigrations.Scripts._01_DbObjects._01_Tables.20200520-0001-SampleTable.sql]'
  * Update ScriptName From SchemaVersions table Where script name = '[Net48.DbMigrations.Scripts._01_DbObjects._01_Tables.20200520-0001-SampleTable.sql]' to indicate a reset 
    * like adding the text '--Reset' to the end of the script name value
  * Any of the above changes will trigger the Db Object script to run again
    
* Data.[environment] contains scripts, usually data inserts and updates, related all environments or to a specified environment.
  * ex.; localdev, uat, production

* DevOps Release Pipeline runs executable file and pass the following command line arguments
  * -e [EnvironmentName]  (ex.; localdev, uat, production)
  * -c [The Actual Conections String to the database]  (recomend you use Key Vault secrets to store passwords)

## NuGet Packages to Install

* dbup-sqlserver (includes dbup-core)
* System.Configuration.ConfigurationManager
* Microsoft.Extensions.Configuration
* Microsoft.Extensions.Configuration.Abstractions


 ## Folder & File structure 

* Provide the sequence to run scripts in an alfabetic order
* Add scripts to the appropriate object folder
* Tip: Add a script to the predeployment folder to re-run a previous run script.
  * Like when you update a stored proc
  * For more info and a sample see: "*Scripts\00_Predeployment\00000000-0000-0000-Reset-readme.md*"

 ## File naming standard 

* is based on date time format to provide a alfabetic sequence to be used to run the scripts in the appropriate order
  * 00000000-0000-0000
    * YYYYMMDD-HHmm-0000


## Build and Test
Unit tests are added to the solution and run as part of the build process which will also trigger with Pull Requests.  This will check that:
* Scripts are Embeded Resources
* Do not use "Use SomeDbName" statement 


## Sample scripts:

### Schema

    If Not Exists(Select 1 From sys.schemas Where name = 'DevOps')
    Begin
	    Execute('Create Schema DevOps')
    End
    GO


### Tables

    If Not Exists(Select 1 From sys.tables Where name = 'App')
    Begin
	    Create Table DevOps.App
	    (
		    Id					bigint	Identity(1,1) NOT NULL,
		    CreatedOn			dateTime NOT NULL,
		    ModifiedOn			dateTime NOT NULL,
		    Name				varchar(1024) NOT NULL,
		    CONSTRAINT [PK_App] PRIMARY KEY CLUSTERED
		    (
			    [Id] ASC
		    )
	    )
    End
    GO

    If Not Exists(select 1 from sys.indexes Where name = 'IX_DevOps_App_Name')
    Begin
	    CREATE NONCLUSTERED INDEX [IX_DevOps_App_Name] ON [DevOps].[App]
	    (
		    [Name]	ASC
	    )
    End
    Go


### Data

    Declare @rowId int, @rowCount int, @nameValue varchar(1024)

    Declare @data table (RowID int identity, NameValue varchar(1024))
    Insert Into @data (NameValue) Values
        ('MyDemoCode.WebApi'),
        ('MyDemoCode.WebApp'),
        ('MyDemoCode.WebAppNet46')

    Select @rowId = 1, @rowCount = Count(*) From @data

    While (@rowId <= @rowCount)
    Begin
        Select @nameValue = NameValue
        From @data
        Where rowId = @rowId

        if Not Exists(Select 1 From DevOps.App Where Name = @nameValue)
        Begin
            Insert Into DevOps.App
                (   
                    CreatedOn,
                    ModifiedOn,
                    Name
                )
                Values
                (   
                    GetDate(),
                    GetDate(),
                    @nameValue
                )
        End

        Set @rowId = @rowId + 1
    End


## Unit Test Project

This will be part of the DevOps build to ensure that the DbUp project have scripts as embedded resources and the Use SomeDbName statement is not used.

Use standard Unit Test project and add the following NuGet packages:

* xunit
* xunit.runner.visualstudio
* Shouldly


