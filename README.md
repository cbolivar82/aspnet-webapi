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
|:--------------------------|:----------------------|:---------------------|
| ResourceType              | You must indicate if it is a table or store procedure         | "sp" or "table"               |
| ResourceName              | You must indicate the name of table or store procedure| "Customer" or "sp_CustomerSelect" |
| ConnectionStringName      | You must indicate the connection string name that stroge on the web.config |- |
| Top                       | It is to do a select with top on select statements | - |
| Fields                    | It is a string array with field to select if it is null the service do a select * | ["field1", "field2", "field3"] |
| Filters                   | It is an object array with filter to where clausulae if it is null the service do a select * | - |
| Filters.Name              | It is the name to filter or parameter either table or store procedure | |
| Filters.OrderBy           | It is the definition to order by to query, if you set use null the sort will be omit | "ASC", "DSC" | 
| Filters.DataType          | This is for define the type to filter| -|
| Filters.Operator          | This is an operator | ">,>=,<,<=,=" |
| Filters.Value             | Single value to query| |
| Filters.Values            | String array to set multiples values on query | ["value1", "value2" ,"value3"] |


####DataType allow the following values:####

`[1] Represent a Numeric value.`

`[2] Represent Date value with the format mm/dd/yyyy.`

`[3] When you use this type, the where clausulae will be a LIKE operator (i.e. LIKE '%value%').`

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
Response example:
```javascript
  [
      {
      "id": 1034833,
      "GUID": "637d80bc-4930-4b18-93f6-bf49bca99881",
      "number": 2293,
      "documentCode": "348806",
      "beginDate": "2012-11-30",
      "customerId": "5520099",
      "invoiceClosedTime": "03:59:12 A.M.",
      "totalAmount": "4508",
      "customerName": "MARIA J CAMELIO DE RUIZ",
      "serialCode": "CVRF3",
      "companyCodeInSerialCode": "V",
      "customerType": "R",
      "productCode": "F",
      "key": "3",
      "companyCode": "V",
      "customerEmail": "cbolivar82@gmail.com",
      "customerTelephone": "",
      "customerAddress": "COLONIA TOVAR SECT CAMBURAL  PROPATRIA DTTOFEDERALCaracas     111",
      "isCopyDocument": true
   },
      {
      "id": 1026930,
      "GUID": "8141694c-5442-4a97-bf33-bf0dc7c218ad",
      "number": 1072,
      "documentCode": "67614",
      "beginDate": "2012-12-01",
      "customerId": "11800410",
      "invoiceClosedTime": "05:59:52 A.M.",
      "totalAmount": "14000",
      "customerName": "SUAREZ MIQUELENA ABELINA DEL CARMEN",
      "serialCode": "BIRF1",
      "companyCodeInSerialCode": "I",
      "customerType": "R",
      "productCode": "F",
      "key": "1",
      "companyCode": "I",
      "customerEmail": "cbolivar82@gmail.com",
      "customerTelephone": "",
      "customerAddress": "CRR.4 / CLL.4 Y 5. CASA 05-00. MONTEZUMA 2.,CERCADELA PARADADE RAPIDITO MARIA ESTRELLA.Barquisimeto     111",
      "isCopyDocument": true
   }
]
```
___

### Request examples to get date from [Tables] ###

Request:
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


## Request examples to get date from [Stores Procedures] ###
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