# Polly Retries Part 1

### Retrying when an exception occurs
The policy retries the request up to three times if an exception occurs.

``` cs --region retryIfException --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
Console.WriteLine("status!");
```

#### Next: [Retrying Based on a Result  &raquo;](./retryIfIncorrectStatus.md) Previous: [Home &laquo;](../README.md)