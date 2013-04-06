# log4net.dynamodb
================

An Amazon Web Services DynamoDb log4net appender. The appender is templated so that custom DynamoDb schemas can be used, 
and is buffered to optimize performance. Three DynamoDb data types (S, N, B) are supported.

## Installation
Install the package via nuget using the following command:
```
Install-Package log4net.dynamodb
```

## Configuration
You can set up the appender by adding it to the <appenders> section of your existing log4net configuraiton in App.config or Web.config. 
For example, using the standard log4net layout:
```
<log4net debug="true">
  <root>
    <level value="ALL" />
    <appender-ref ref="DynmoDbAppender" />
  </root>
  <appender name="DynmoDbAppender" type="log4net.Appender.DynamoDbAppender, log4net.dynamodb">
    <tableName value="log4net" />
    <bufferSize value="512" />
    <threshold value="DEBUG" />
    <parameter>
      <name value="Id" />
      <type value="S" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%GUID" />
        <converter>
          <name value="GUID" />
          <type value="log4net.Layout.PatternLayout.NewGuidPatternLayoutConverter, log4net.dynamodb" />
        </converter>
      </layout>
    </parameter>
    <parameter>
      <name value="TimeStamp" />
      <type value="S" />
      <layout type="log4net.Layout.PatternLayout" value="%date{yyyy'-'MM'-'dd HH':'mm':'ss'.'fff}" />
    </parameter>
    <parameter>
      <name value="Message" />
      <type value="S" />
      <layout type="log4net.Layout.PatternLayout" value="%message" />
    </parameter>
    <parameter>
      <name value="Level" />
      <type value="S" />
      <layout type="log4net.Layout.PatternLayout" value="%level" />
    </parameter>
    <parameter>
      <name value="Username" />
      <type value="S" />
      <layout type="log4net.Layout.PatternLayout" value="%username" />
    </parameter>
    <parameter>
      <name value="MachineName" />
      <type value="S" />
      <layout type="log4net.Layout.PatternLayout" value="%property{log4net:HostName}" />
    </parameter>
    <parameter>
      <name value="ThreadName" />
      <type value="S" />
      <layout type="log4net.Layout.PatternLayout" value="%thread" />
    </parameter>
    <parameter>
      <name value="Domain" />
      <type value="S" />
      <layout type="log4net.Layout.PatternLayout" value="%appdomain" />
    </parameter>
    <parameter>
      <name value="Identity" />
      <type value="S" />
      <layout type="log4net.Layout.PatternLayout" value="%identity" />
    </parameter>
  </appender>
</log4net>
```

For each parameter element:

```
<name value="Id" />
```
The **name** element specifies the corresponding DynamoDb column, and,

```
<type value="S" />
```
The **type** element specifies the corresponding DynamoDb type. Currently the S, N, and B types are supported. Omitting this element 
will default the column to the S (string) type.

Additionally, custom parameters can be writen to Dynamo using **ThreadContext.Properties[]**. For example, you could write a binary 
object by adding the following line to of code to your application (the "myapp:" predicate has no special meaning to log4net; it is 
used only for disambiguation):
```
ThreadContext.Properties["myapp:ImportantObject"] = new Tuple<string, int>("Number", 42);
```

This object will then be serialized and captured in log output if the following corresponding parameter exists in your application 
configuration:
```
<parameter>
  <name value="SomeObjectField" />
  <type value="B" />
  <layout type="log4net.Layout.PatternLayout" value="%property{myapp:ImportantObject}" />
</parameter>
```

Complete exception objects can be serialized to Dynamo in much the same way. For example:
```
<parameter>
  <name value="Exception" />
  <type value="B" />
  <layout type="log4net.Layout.PatternLayout" value="%exception" />
</parameter>
```

Note that you must set the **AWSAccessKey** and **AWSSecretKey** configuration elements in the appConfig section of your App.config or 
Web.config file for authentication to DynamoDb. See the AWS documentation for more information. [These release notes](http://aws.amazon.com/releasenotes/.NET/7526512651260522) 
are a good starting point.

The following addtional parameters are available in the appender configuration:
```
<tableName value="log-app" />
```
This is the name of the table that messages will be written to.

```
<serviceEndpoint value="anAwsEndpoint" />
```
The service endpoint to use for AWS DynamoDb connections. This element is optiona; ommitting it will force the appender to use the default 
DynamoDb region (http://dynamodb.us-east-1.amazonaws.com).

See App.config in the test project for a complete configuration example.

## AWS Setup
You must configure a table in DynamoDb using the AWS Console or CLI tools, the appender will not do this for you. The appender does not expect 
any specific schema, you control this by defining parameters in your configuration file. Columns will be added as needed when a LoggingEvent is 
sent to your DynamoDb table, and your schema can be jagged.

## Usage 
Using the appender is easy, just log using log4net as you normally would. Note that this is a buffered appender, so log messages may not 
be written immediately. You can override this behavior at the (substantial) expense of performace using log4net's **immediateFlush** configuration 
element. See [this](http://logging.apache.org/log4net/release/sdk/log4net.Appender.TextWriterAppender.ImmediateFlush.html) page for more information.

For example:
```
<immediateFlush value="true" />
```

## Building the Project Yourself
Clone the repository, open log4net.dynamodb.sln in Visual Studio, select the desired configuration, and click Build -> 
Build Solution (F6). Alternatively, you can build from the command line using csc or MSBuild.

## Dependencies
log4net.dynamodb is written in C# and requires the MS.NET Framework version 4.0. The only other external dependency
is log4net, which will be installed automatically by Nuget when you build the solution.

## Issues
If you find an bug please [open an issue](https://github.com/kcargile/log4net.dynamodb/issues).

## License
See LICENSE. Copyright (c) 2013, Cargile Technology Group, LLC.
