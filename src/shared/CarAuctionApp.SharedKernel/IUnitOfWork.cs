﻿namespace CarAuctionApp.SharedKernel;


public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
