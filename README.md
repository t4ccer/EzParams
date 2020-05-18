# EzParams
Library for easy and fast parsing CLI parameters
## Usage
```csharp
void Main(string[] args)
{
    new ParamsParser(args)
        .WithParameter(out string param1, "--param1", "-p1", "Description", "DefaultValue")
        .WithParameter(out bool flag1, "--flag1", "-f1", "Description")
        .WithDefaultHelpParameter(out bool help);
}
```
To use in Your code just install [t4ccer.EzParams](https://www.nuget.org/packages/t4ccer.EzParams/) nuget package

## Parsing
Parser works using Finite State Machine(graph below).  

![](fsm.png)

## Contribution
Feel free to add some features or fix bugs