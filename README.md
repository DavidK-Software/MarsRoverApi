# MarsRoverApi
A programming exercise that accessess Mars Rover images from the [Nasa API portal](https://api.nasa.gov/?search=Mars+Rover+Photos+API), saves them, and serves them in a web api.

The [MarsRoverApi](https://github.com/DavidK-Software/MarsRoverApi) project downloads data about the Nasa Mars Rovers from the [Nasa API portal](https://api.nasa.gov/?search=Mars+Rover+Photos+API).

# Overview

## Application Projects

The application is divided into the following projects.

| Project | Description |
| --- | ----------- |
| MarsRoverApi | A web api application that serves information about Mars Rover Images |
| NasaApiLib | An HttpClient library that makes calls to retrieve data from the  [Nasa API portal](https://api.nasa.gov/?search=Mars+Rover+Photos+API) |
| MarsRoverApi.Test | An xUnit test library for MarsRoverApi |
| NasaApiLib.Test | An xUnit test library for NasaApiLib |

## Initialization

The following is preformed on startup:

1. Retrieves a list of rover information along with the cameras used on each rover from the [Nasa API portal](https://api.nasa.gov/?search=Mars+Rover+Photos+API).
	- The list of rovers with their associated cameras are saved in a database.
	- Initialization of rover information is only done once at the beginning of the first start of the web api.
2. Dates are read from a local file, one date at a time.
	- The dates are validated as they are read.
	- The service that reads the dates uses IEnumerable and yield, allowing the application to loop through the dates, reading only one date at a time and not needing to keep a list in memory.
3. For each rover and date combination, the [Nasa API portal](https://api.nasa.gov/?search=Mars+Rover+Photos+API) is called.
	- Calls to the [Nasa API portal](https://api.nasa.gov/?search=Mars+Rover+Photos+API) use paged retrieval accessing 25 items per page.
	- Information about each image is saved to a database.
	- For each image, a call is made to the Nasa Image api to retrieve the image.
	- Each image is saved locally in a file.
	- Initialization of Mars Rover photo images is only done once at the beginning of the first start of the web api. Data that has already been saved is not retrieved from Nasa again.

## API

This application is a Web Api. It provides API endpoints that can be called from client applications to retrieve data and images that have been retrieved from Nasa.

An example of a client application that calls this API can be found at:
[MarsRoverAngularUi](https://github.com/DavidK-Software/MarsRoverAngularUI)

### Endpoints

#### Available Endpoints

| Endpoint | Description |
| --- | ----------- |
| /api/Dates | Gets a list of dates |
| /api/Rovers | Gets a list of rovers and associated cameras |
| /api/Rovers/{roverid} | Gets information about a single rovers and its associated cameras |
| /api/Rovers/{roverid}/photos/{earthdate}?page={page}&pagesize={pagesize}" | Gets a page of information about rover images for a given day. |
| /api/Images/{imagePath}/{imageFileName} | Gets an image. This can be used in an html img tag. |

#### Example Queries

GET "http://localhost:8563/api/Dates"
<br>
GET "http://localhost:8563/api/Rovers"
<br>
GET "http://localhost:8563/api/Rovers/5"
<br>
GET "http://localhost:8563/api/Rovers/5/photos/2017-02-27?page=1&pagesize=10"
<br>
GET "http://localhost:8563/api/Images/MarsRoverImages/FLB_541484941EDR_F0611140FHAZ00341M_.JPG"
## Coding Features Demonstrated

- C#
- .NET Core 5
- Entity Framework Core 5
	- EF Core Migrations
	- Paged Query Results
- Swagger
- .NET Core Logging
- xUnit Tests
- JSon Serialization (for .NET Core 5)
- HttpClient
- yield keyword
- File and Stream IO
- Dependency Injection
- CORS
- SqlLite
- Extension Methods

## Nuget Packages

This project is written in C# using .Net Core 5.0.

The following NuGet packages are used:

[Microsoft.EntityFrameWorkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/)
<br>
Additional information can be found [here](https://docs.microsoft.com/en-us/ef/core/)
<br><br>
[Microsoft.EntityFrameWorkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/)
<br>
Additional information can be found [here](https://docs.microsoft.com/en-us/ef/core/providers/sqlite/?tabs=dotnet-core-cli)
<br><br>
[Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/)
Additional information can be found [here](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio)
<br><br>
[Automapper](https://www.nuget.org/packages/AutoMapper/)
Documentation about automapper can be found at the [Automapper Office Website](https://automapper.org/)

## License
This project is licensed with the [MIT license](LICENSE).