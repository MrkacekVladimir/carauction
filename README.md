# Car Auction

Car Auction App is a web application written mostly in .NET technologies with focus on modern practices and frameworks.

## Project structure

### /src/

- carauctionapp-auctionsite = React project with Typescript and CSR, used for bidding and viewing auctions
- CarAuctionApp.WebApi = REST api ASP.NET Core project using minimal API approach
- CarAuctionApp.Application = Application specific logic for serving specific use-cases and handling input validation
- CarAuctionApp.Domain = Core of our domain processes and logic
- CarAuctionApp.Infrastructure = Interaction with external services such as RabbitMQ for messaging queues
- CarAuctionApp.SharedKernel = Shared abstractions, utility classes across our whole application

### /tests/

- CarAuctionApp.Domain.UnitTests = xUnit project covering unit tests for Domain processes
