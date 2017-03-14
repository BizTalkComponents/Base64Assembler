[![Build status](https://ci.appveyor.com/api/projects/status/github/BizTalkComponents/Base64Assembler?branch=master)](https://ci.appveyor.com/api/projects/status/github/BizTalkComponents/Base64Assembler/branch/master)

## Description ##
The Base64Assembler component generates a base64 encoded string of the message body and adds it to a new document specified in the component parameters.


| Parameter      | Description                                               | Type | Validation|
| ---------------|-----------------------------------------------------------|------|-----------|
|DocumentSpecName|DocumentSpec name of the schema to create an instance from.|String|Required|
|DestinationXpath|Xpath expression pointing to the node to insert the base64 string in.|String|Required|

## Remarks ##
If the DocumentSpecName is not set an ArgumentException will be thrown. If the specified schema does not exist in GAC an exception will be thrown.
If the DestinationXpath is not set an ArgumentException will be thrown. If there are no node matching the xpath an exception will be thrown.
