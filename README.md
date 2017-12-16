# TodoApi

The api supports content negotiation with the following formats: json, xml and csv. The project shows two examples of action filters which is used to avoid code repetition and centralised validation logic in the controllers. The error messages is customized and has been structured for consistency.

## IntegrationTests
The IntegrationTests uses a TestServer class in the Microsoft.AspNetCore.TestHost library which can be used to simulate ASP.NET Core applications, serving test requests without the need for a real web host.

