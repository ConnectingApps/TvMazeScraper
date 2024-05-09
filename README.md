# TvMaze Scraper

## Description
TvMaze Scraper is a .NET application designed to scrape and store cast information from TV shows provided by the TVMaze API. The application persists the data in a PostgreSQL database and exposes a REST API that serves paginated lists of TV shows along with their cast information, ordered by the cast members' birthdays in descending order.

## Prerequisites
Before you begin, ensure you have the following installed:
- Docker
- .NET 8 SDK

## Installation

### Setting up PostgreSQL
First, run the following Docker command to set up a PostgreSQL container:

```bash
docker run --name postgres-db -e POSTGRES_PASSWORD=mysecretpassword -d -p 5432:5432 postgres
```

This command initializes a PostgreSQL database with the required settings.

### Cloning the Repository
Clone the repository using the following command:

```bash
git clone https://github.com/ConnectingApps/TvMazeScraper
```

### Running the Application
Navigate to the project directory and build the application using:

```bash
dotnet restore
dotnet build
```

Then, run the application:

```bash
cd TvMazeScraper.Api
dotnet run
```

## API Usage
The REST API can be accessed at `http://localhost:5059/swagger`.
Alternatively, open this url
`http://localhost:5059/Show?pageNumber=80&pageSize=2`
in your website to see a direct result.
