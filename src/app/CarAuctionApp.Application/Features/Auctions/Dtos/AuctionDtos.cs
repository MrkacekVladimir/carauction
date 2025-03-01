using System;
using System.Collections.Generic;

namespace CarAuctionApp.Application.Features.Auctions.Dtos;

public record AuctionBidUserDto(
     Guid Id,
     string Username
    );

public record AuctionBidDto(
     Guid Id,
     decimal Amount,
     DateTime CreatedOn,
     AuctionBidUserDto User
    );
public record AuctionListItemDto(
     Guid Id,
     string Title,
     DateTime StartsOn,
     DateTime EndsOn,
     IEnumerable<AuctionBidDto> bids
    );
public record AuctionDto(
     Guid Id,
     string Title,
     DateTime StartsOn,
     DateTime EndsOn,
     IEnumerable<AuctionBidDto> Bids
    );
