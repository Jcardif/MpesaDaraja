# M-Pesa Daraja SDK

C# M-Pesa SDK leveraging the Daraja API 2.0 allowing easy integration of M-Pesa Payments into your Applications

# Installation & Setup
Available on NuGet: https://www.nuget.org/packages/MpesaDarajaSDK/
Add to your project via .Net CLI
```
dotnet add package MpesaDarajaSDK
```

## Build Status
**Build Status:** [![Build Status](https://dev.azure.com/Jcardif/M-Pesa%20Daraja%20SDK/_apis/build/status/Publish%20Nuget?branchName=master)](https://dev.azure.com/Jcardif/M-Pesa%20Daraja%20SDK/_build/latest?definitionId=12&branchName=master)

# Usage
To use this package ensure you add the following ```using``` statement into your project files. 

```C#
using MpesaDaraja.Models;
using MpesaDaraja.Services;
```
The package need to be configured with your [Daraja App Credentials](https://developer.safaricom.co.ke/MyApps) (Consumer Secret, consumerKey and your pass key)

```C#
var gateway = new DarajaGateway(consumerKey, consumerSecret, passKey, false);

var darajaClient = await gateway.GetDarajaClientAsync(false);
```
The ```DarajaGateway``` authenticates with the daraja api to get you a timebound access token which is used to create the ```DarajaClient``` which you use to accesss the various [Daraja APIs](https://developer.safaricom.co.ke/APIs)

# Make an STK Push (M-Pesa Express)
To make an online payment on behalf of the customer:

Create an ```STKData``` object

```C#
var stkData = new StkData
{
    BusinessShortCode = 174379,
    Timestamp = "20230116043457",
    TransactionType = "CustomerPayBillOnline",
    Amount = 1,
    PartyA = receiver,
    PartyB = 174379,
    PhoneNumber = receiver,
    CallBackUrl = new Uri("https://mydomain.com/path"),
    AccountReference = "CompanyXLTD",
    TransactionDesc = "Payment of X"
};
```
Get the password that is used for encrypting the request sent

```C#
stkData.Password = darajaGateway.GetStkPushPassword(stkData.BusinessShortCode, stkData.Timestamp);
```
Make the STK Push
```C#
var result = await darajaClient.SendStkPushAsync(stkData);
```
Query the status of the stk push
```C#
var (isCompleted, pushQueryResponse) = await darajaClient.QueryStkPushStatus(pushResponse, stkData);
```


# Find this repository useful? :heart:
Support it by joining [stargazers](https://github.com/Jcardif/MpesaDaraja/stargazers) for this repository. 

Also [follow](https://github.com/Jcardif) me for my next creations!

# License
Under MIT (See [LicenseFile](https://github.com/Jcardif/MpesaDaraja/blob/master/LICENSE.txt))

