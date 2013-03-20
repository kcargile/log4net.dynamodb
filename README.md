# log4net.dynamodb
================

An Amazon Web Services DynamoDb log4net appender.

## Installation
Install the package via nuget using the following command:
```
Install-Package log4net.dynamodb
```

## Configuration
You can set up the appender by adding it to the <appenders> section of your existing log4net configuraiton in App.config or Web.config. 
For example:
```
<log4net debug="true">
	<root>
		<level value="ALL" />
		<appender-ref ref="DynmoDbAppender" />
	</root>
	<appender name="DynmoDbAppender" type="log4net.Appender.DynamoDbAppender, log4net.dynamodb">
		<tableName value="log-app" />
		<tablePrefix value="unittest-" />
		<serializeExceptions value="false" />
		<layout type="log4net.Layout.PatternLayout">
		<param name="ConversionPattern" value="%-5p %d{yyyy-MM-dd hh:mm:ss} :: %m%n" />
		</layout>
	</appender>
</log4net>
```

Addtionally, you must set the **AWSAccessKey** and **AWSSecretKey** configuration elements in the appConfig section of your App.config or 
Web.config file for authentication to DynamoDb. See the AWS documentation for more information. [These release notes](http://aws.amazon.com/releasenotes/.NET/7526512651260522) 
are a good starting point.

See App.config in the test project for a more complete configuration example.

You must configure a table in DynamoDb using the AWS Console or CLI tools, the appender will not do this for you. The appender expects 
that you will have both a hash key named "Id" (type string) and a range key named "TimeStamp" (type string). Other columns will be 
added as needed when a LoggingEvent is sent to your DynamoDb table.

The following parameters are available in the appender configuration:
```
<tableName value="log-app" />
The name of the table that messages will be written to.

<tablePrefix value="unittest-" />
A table prefix that will be automatically appended to the tableName property. This can be an empty string or can be 
ommitted altogther. Useful for varying configuration transformations used during unit testing, etc.

<serializeExceptions value="false" />
If set to true, a binary serialized version of any actual exception messages will be written to the database along 
with other log data.
``` 

## Usage 
Using the appender is easy, just log using log4net as you normally would. Note that this is a buffered appender, so log messages may not 
be written immediately. You can override this behavior at the expense of performace using the log4net **immediateFlush** configuration 
element. See [this[(http://logging.apache.org/log4net/release/sdk/log4net.Appender.TextWriterAppender.ImmediateFlush.html) page for more information.

## Building the Project Yourself
Clone the repository, open log4net.dynamodb.sln in Visual Studio, select the desired configuration, and click Build -> 
Build Solution (F6). Alternatively, you can build from the command line using csc or MSBuild.

## Dependencies
log4net.correlationPatternConverter is written in C# and requires the MS.NET Framework version 4.0. The only other external dependency
is log4net, which will be installed automatically by Nuget when you build the solution.

## Issues
If you find an bug please [open an issue](https://github.com/kcargile/log4net.dynamodb/issues) or alternatively contact 
me via my [blog](http://www.kriscargile.com).

## License
See LICENSE. Copyright (c) 2013, Cargile Technology Group, LLC.
