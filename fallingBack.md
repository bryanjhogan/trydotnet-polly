# Polly Fallbacks 1

### When all else fails...do something
There are times when no number of retires will solve a problem, the underlying service just won't respond. In this case the Fallback policy is you last resort. It can page an admin, scale a service, return a default, or perform some other arbitrary action.

This example "pages" an admin but you can do anything you want.

``` cs --region fallingBack --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Fallbacks 2 &raquo;](../fallingBackAndReturningADefault.md) Previous: [Waiting Before Retrying &laquo;](../waitAndRetry.md)