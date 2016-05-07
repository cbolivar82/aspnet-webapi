#ASP.Net Web API Resful Service

##Purpose
It is a service to get data through a POST method a connection string you can get date from any table with any structure or a store procedure. The result data is serialize like Json Object.

##Getting the project

1. Clone the repository and open the solution on Visual Studio 2015
```git
    git clone https://github.com/cbolivar/aspnet-webapi.git
``` 
2. Open any rest client, I am use [SOAP UI OpenSource](https://www.soapui.org/downloads/soapui.html) and you can find the project with all examples in the folder `\SOAPUITest\oapUI_Project.xml`

##How can I get data?

With the rest client (SOAP UI for example), you must invoke the POST method `../api/DataSource/GetData`, and you must use some these object, according to resource type:

## Tables ##
---
```javascript
{
  "ResourceType": "table",
  "ResourceName": "Customer",
  "ConnectionStringName": "TestDbEntities"
}
```




## Stores Procedures ###
---




Inline `code` has `back-ticks around` it.