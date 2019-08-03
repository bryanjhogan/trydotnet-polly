# Polly Retries Part 1

### Retrying When an Exception Occurs
The policy retries the request up to three times if an exception occurs.

``` cs --region retryIfException --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Retrying Based on a Result  &raquo;](./retryIfIncorrectStatus.md) Previous: [Home &laquo;](../README.md)