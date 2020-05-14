# EzParams
Library for easy and fast parsing CLI parameters
## Usage
```csharp
void Main(string args)
{
    new ParamsParser(args)
        .WithParameter(out string param1, "--param1", "-p1", "Description", "DefaultValue")
        .WithParameter(out bool flag1, "--flag1", "-f1", "Description")
        .WithDefaultHelpParameter(out bool help);
}
```
## Parsing
Parser works using Finite State Machine(graph below).  

![](fsm.png)