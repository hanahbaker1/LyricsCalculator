# LyricsCalculator
This is an application that accepts an artist name in the request and finds the average number of words for each of their songs it has found.

#Prerequisites
.Net Core SDK 3.1 (https://dotnet.microsoft.com/download/dotnet-core/3.1)

## Usage
This application can be built and run using the dotnet CLI.

- Navigate to the root directory of the repository using a terminal window.
- Run the project using the following command: `dotnet run --project LyricsCalculator\LyricsCalculator.Api.csproj`
- Browse to https://localhost:5001/swagger/index.html
- Click /LyricsStatistics/Search/Search and click Try it out, enter the desired artist's name in the textbox and press "Execute"

### Visual Studio
Alternatively, open the project in Visual Studio and select the LyricsCalculator.Api as the startup project project, start the app using IIS Express or Kestrel.
