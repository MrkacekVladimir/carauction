# Car Auction

The application is still under development, sometimes you can see missing or WIP stuff.

Car Auction App is a web application written mostly in .NET technologies with focus on modern practices and frameworks.

## Mentionable technologies

- ASP.NET Core
- ReactJS
- Typescript

- PostgreSQL as main relational database
- RabbitMQ as message broker 
- SQLite for unit testing

- WebSockets for real time bidding feedback

- Docker, Docker Compose

## Mentionable libraries

- MassTransit
- EntityFramework Core
- SignalR
- xUnit

- @tanstack/react-query

## Used principles

- Domain Driven Design
- Clean Architecture
- Background processing
- Message queueing, Outbox pattern
- Asynchronous programming
- Dependency injection

## Project structure

### /src/

- carauctionapp-auctionsite = React project with Typescript and CSR, used for bidding and viewing auctions
- CarAuctionApp.WebApi = REST api ASP.NET Core project using minimal API approach
- CarAuctionApp.Application = Application specific logic for serving specific use-cases and handling input validation
- CarAuctionApp.Domain = Core of our domain processes and logic
- CarAuctionApp.Infrastructure = Interaction with external services such as RabbitMQ for messaging queues
- CarAuctionApp.Persistence = Split from Infrastructure layer to clearly seperate persistence in EF Core
- CarAuctionApp.SharedKernel = Shared abstractions, utility classes across our whole application

### /tests/

- CarAuctionApp.Domain.UnitTests = xUnit project covering unit tests for Domain processes
- CarAuctionApp.Infrastructure.UnitTests = xUnit project covering unit tests for infrastructure communication
- CarAuctionApp.Persistence.UnitTests = xUnit project covering unit tests for specific for persisting data
- CarAuctionApp.TestUtilities = Shared library for all tests containing utility classes and helper methods
