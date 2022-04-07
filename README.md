[![Build status](https://waal.visualstudio.com/BizTalk%20Components/_apis/build/status/SetTypedProperty)](https://waal.visualstudio.com/BizTalk%20Components/_build/latest?definitionId=0)

## Description
Promotes a specified value to a specified property.

This component is useful when you need to promote a hard coded value.

| Parameter | Description | Type | Validation |
|-|-|-|-|
|Property Path|The property path where the specified value will be promoted to, i.e. http://temupuri.org#MyProperty.|String|Required, Format = namespace#property|
|Promote Property|Specifies whether the property should be promoted or just written to the context.|Bool|Required|
|Value|The value that should be promoted to the specified property.|String|Required|
|Value Type|The type of value being assigned to the specified property.|String|Required, must be the text of System.TypeCode enumeration|



## Remarks ##
- If ValueType is empty or null, ValueType is set to String.

The component throws exception of type:
- System.ArgumentException if any of the required parameters is not specified, or if the ValueType is not a named value of System.TypeCode enumeration.
- System.FormatException if the value format does not match the specified value type.
- System.OverflowException if the value exceeds the type's range.
- System.InvalidCastException if the value cannot be casted to the specified ValueType.
