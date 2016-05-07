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

### Tables ###


```javascript
// Select all

{
  "ResourceType": "table",
  "ResourceName": "Customer",
  "ConnectionStringName": "TestDbEntities"
}
```

```javascript
// Select top 10

{
  "ResourceType": "table",
  "ResourceName": "Customer",
  "ConnectionStringName": "TestDbEntities",
  "Top": 10
}
```

```javascript
// Select top 10, a couples fields and order by field [number]

{
    "ResourceType": "table",
    "ResourceName": "Customer",
    "ConnectionStringName": "TestDbEntities",
    "Top": 10,
    "Fields": [
        "GUID",
        "number",
        "documentCode",
        "beginDate",
        "customerId"
    ],
    "Filters": [
        {
            "Name": "number",
            "OrderBy": "ASC"
        }
    ]
}
```

```javascript
// Select top 10 and order by field [number]

{
  "ResourceType": "table",
  "ResourceName": "Customer",
  "ConnectionStringName": "TestDbEntities",
  "Top": 10,
  "Filters": [
    {
      "Name": "number",
      "OrderBy": "ASC"
    }
  ]
}
```

```javascript
// Select top 10, beginDate >= 01/01/2013 and order by [number]

{
  "ResourceType": "table",
  "ResourceName": "Customer",
  "ConnectionStringName": "TestDbEntities",
  "Top": 10,
  "Fields": null,
  "Filters": [
    {
      "Name": "number",
      "DataType": null,
      "Operator": null,
      "OrderBy": "ASC",
      "Values": null
    },
    {
      "Name": "beginDate",
      "DataType": 3,
      "Operator": ">=",
      "OrderBy": "ASC",
      "Value": "12-21-2012"
    }
  ]
}
```

```javascript
// Select top 10, beginDate >= 01/01/2013, documentCode = [ "177840", "177841", "177842" ] and order by [number]
{
  "ResourceType": "table",
  "ResourceName": "Customer",
  "ConnectionStringName": "TestDbEntities",
  "Top": 10,
  "Fields": null,
  "Filters": [
    {
      "Name": "number",
      "OrderBy": "ASC",
    },
    {
      "Name": "beginDate",
      "DataType": 3,
      "Operator": ">=",
      "OrderBy": "ASC",
      "Value": "12-21-2012"
    },
    {
      "Name": "documentCode",
      "Operator": "=",
      "OrderBy": "ASC",
      "Values": [ "177840", "177841", "177842" ]
    }
  ]
}
```

```javascript
// Select top 10, beginDate >= 01/01/2013, documentCode = [ "177840"] and order by [number]
{
  "ResourceType": "table",
  "ResourceName": "Customer",
  "ConnectionStringName": "TestDbEntities",
  "Top": 10,
  "Fields": null,
  "Filters": [
    {
      "Name": "number",
      "OrderBy": "ASC",
    },
    {
      "Name": "beginDate",
      "DataType": 3,
      "Operator": ">=",
      "OrderBy": "ASC",
      "Value": "12-21-2012"
    },
    {
      "Name": "documentCode",
      "Operator": "=",
      "OrderBy": "ASC",
      "Values": [ "177840" ]
    }
  ]
}
```

```javascript
// Select top 10, beginDate >= 01/01/2013, documentCode like ["177840", "177841", "177842"] and order by [number]
{
  "ResourceType": "table",
  "ResourceName": "Customer",
  "ConnectionStringName": "TestDbEntities",
  "Top": 10,
  "Filters": [
    {
      "Name": "number",
      "OrderBy": "ASC",
    },
    {
      "Name": "beginDate",
      "DataType": 3,
      "Operator": ">=",
      "OrderBy": "ASC",
      "Value": "12-21-2012"
    },
    {
      "Name": "documentCode",
      "DataType": 2,
      "OrderBy": "ASC",
      "Values": [ "177840", "177841", "177842" ]
    }
  ]
}
```


## Stores Procedures ###
```javascript
// Select all fields of the store procedure
{
    "ResourceType": "sp",
    "ResourceName": "sp_CustomerSelect",
    "ConnectionStringName": "TestDbEntities",
    "Fields": [ "GUID", "documentCode", "beginDate", "invoiceClosedTime" ],
    "Filters": [
        {
            "Name": "documentCode",
            "Value": "177840"
        }
    ]
}
```
```javascript
// Select a couples fields of the store procedure
{
    "ResourceType": "sp",
    "ResourceName": "sp_CustomerSelect",
    "ConnectionStringName": "TestDbEntities",

    "Filters": [
        {
            "Name": "documentCode",
            "Value": "177840"
        }
    ]
}
```




Inline `code` has `back-ticks around` it.