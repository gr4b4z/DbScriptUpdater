DbScriptUpdater
================
A lightweight database updater written in C#.  
Currenlty support MSSQL database, but can be quickly extended to others databases.


## Should I used it?
If you have database first project, you store db changes in files, and you want to quickly add those changes to model, it's for you.

## How it works?

All changes are stored in files with version prefix.
For example:

    1.Updated database.sql
	2.Merged with dev.sql
	3.Fix123.sql

Database store version number. in this example this number is 0.
So AppUpdater will execute each script, and after it, it will store last script name in db.

## Multiple branches

In this example, there are two branches: dev and prod.
In dev branch we got:

	1.Updated database.sql
	2.Merged with dev.sql
	3.Fix123.sql
	4.Fix133.sql
	5.Fix153.sql

In prod we got: 

	1.Updated database.sql
	2.Merged with dev.sql

So what to do if we want to add some script to prod? We can't add script with prefix 3, because we would have conflicts in dev brach.

The simplest solution it to add scripts with two numbers. So on prod we will have:

	1.Updated database.sql
	2.Merged with dev.sql
	2.1.Issue.sql
	2.2.Issue.sql

And after merge with dev:

	1.Updated database.sql
	2.Merged with dev.sql
	2.1.Issue.sql
	2.2.Issue.sql
	3.Fix123.sql
	4.Fix133.sql
	5.Fix153.sql

This app supports "four dots numbers" ex: **5.3.4.1**

## Grouping scripts
In scripts folder, scripts files can be grouped in folders. For example:

    \sql
    	\Release1	
    		1.Updated database.sql
    		2.Merged with dev.sql
    		2.1.Issue.sql
    	\Release2
    		2.2.Issue.sql
    		3.Fix123.sql
    		4.Fix133.sql
    		5.Fix153.sql
When app look for files, it search all folders also.

## Run

###Autonumeration
	You can drag, and drop sql files on DbUpdater.exe to add files to folder specied in app.config path.
	This file will be prexied with the next number. So if you have 1.2.file.sql, next number is 1.3.new_file.sql
	
	You can also add new files runing this:
	DbUpdater.exe  "file1.sql" "file3.sql" "file3.sql"


### Run with parameters
	DbUpdater.exe -cs "some connection string" -path "C:\path-to-files\" 
	DbUpdater.exe -cs "some connection string" -path "C:\path-to-zipped-files.zip" 
	DbUpdater.exe -cs "some connection string" -path "C:\path-to-files\"  -max "xx.update to this version only.sql"
### Multiple databases	
	DbUpdater.exe -cs "db1 connection string" "db2 connection string" "db3 connection string" -path "C:\path-to-files\" 
	
### Run whitout parameters
When running without parameters, app take configuration form app.config.

    <configuration>
      <appSettings>
        <add key="Path" value="C:\path_to_dir\"/>
      </appSettings>
      <connectionStrings>
        <add name="Update_FirstCS" connectionString="exmaple_csstrn"/>
        <!-- You can add more databases, just prefix them with Update_ string-->
        <add name="Update_FirstCS1" connectionString="exmaple_csstrn"/> 
        <add name="Update_FirstCS2" connectionString="exmaple_csstrn"/>
      </connectionStrings>
      <startup useLegacyV2RuntimeActivationPolicy="true">
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.1"/>
        </startup>
    </configuration>

