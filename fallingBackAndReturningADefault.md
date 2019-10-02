# Polly Fallbacks 2

### When all else fails...return a default
Sometimes it makes sense to return a default or safe value from a fallback. In this example a quantity of zero is returned if the request fails.

``` cs --region fallingBackAndReturningADefault --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Timeouts &raquo;](../timeout.md) Previous: [Fallbacks 1 &laquo;](../fallingBack.md)