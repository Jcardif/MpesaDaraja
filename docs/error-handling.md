# Error Handling

## DarajaResult

All API methods (STK Push, Query, Reversal) return a `DarajaResult<T>`. This provides a consistent way to handle success and failure without exceptions.

```csharp
var result = await mpesaExpress.InitiateStkPush(payload);

if (result.IsSuccess)
{
    // Access the response
    var response = result.Value!;
}
else
{
    // Access error details
    var error = result.Error!;
    Console.WriteLine($"Code: {error.ErrorCode}");
    Console.WriteLine($"Message: {error.ErrorMessage}");
    Console.WriteLine($"RequestId: {error.RequestId}");
}
```

### Properties

| Property | Type | Description |
|---|---|---|
| `IsSuccess` | `bool` | `true` if the API call succeeded |
| `Value` | `T?` | The response object (null on failure) |
| `Error` | `DarajaError?` | Error details (null on success) |

## DarajaException

Authentication errors (during `InitializeDarajaAsync`) throw a `DarajaException`:

```csharp
try
{
    await gateway.InitializeDarajaAsync();
}
catch (DarajaException ex)
{
    Console.WriteLine(ex.Message);

    if (ex.Error is not null)
    {
        Console.WriteLine($"Code: {ex.Error.ErrorCode}");
        Console.WriteLine($"Message: {ex.Error.ErrorMessage}");
    }
}
```

## Common Error Scenarios

| Scenario | Behavior |
|---|---|
| Invalid credentials | `DarajaException` thrown during authentication |
| API returns error JSON | `DarajaResult` with `IsSuccess = false` and parsed `DarajaError` |
| API returns empty response | `DarajaResult` with `IsSuccess = false` and generic error message |
| API returns non-JSON error | `DarajaResult` with `IsSuccess = false` and raw content in error message |
