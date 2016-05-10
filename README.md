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

## Request Object ###

That is the object properties, definition and examples values:


| Property                  | Definition            | Example               |
| --------------------------|:---------------------:| ---------------------:|
| ResourceType              | You must indicate if it is a table or store procedure         | "sp" or "table"               |
| ResourceName              | You must indicate the name of table or store procedure| "Customer" or "sp_CustomerSelect" |
| ConnectionStringName      | You must indicate the connection string name on the web.config | |
| Top                       | It is to do a select with top on select statements | - |
| Fields                    | It is a string array with field to select if it is null the service do a select * | ["field1", "field2", "field3"] |
| Filters                   | It is an object array with filter to where clausule if it is null the service do a select * | - |
| Filters.Name              | It is the name to filter or parameter either table or store procedure | |
| Filters.OrderBy           | It is the definition to order by to query, if you set use null the sort will be omit | "ASC", "DSC" | 
| Filters.DataType          | This is for define the type to filter| -|
| Filters.Operator          | This is an operator | ">,>=,<,<=,=" |
| Filters.Value             | Single value to query| |
| Filters.Values            | String array to set multiples values on query | ["value1", "value2" ,"value3"] |


####DataType allow the following values:####

`[1] Represent a Numeric value.`

`[2] Represent Date value with the format mm/dd/yyyy.`

`[3] When you use this type, the where clausule will be a LIKE operator (i.e. LIKE '%value%').`

##That is the full structure object for request:##

```javascript
// Object structure definition
{
  "ResourceType": string,
  "ResourceName": string,
  "ConnectionStringName": string,
  "Top": int,
  "Fields": string array,
  "Filters": [
    {
      "Name": string,
      "OrderBy": string,
    },
    {
      "Name": string,
      "DataType": int,
      "Operator": string,
      "OrderBy": string,
      "Value": string
    },
    {
      "Name": string,
      "DataType": int,
      "OrderBy": string,
      "Values": string array
    }
  ]
}
```

### Examples to get date from [Tables] ###


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


## Examples to get date from [Stores Procedures] ###
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