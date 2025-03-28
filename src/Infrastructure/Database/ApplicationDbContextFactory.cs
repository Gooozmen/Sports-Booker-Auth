﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Database;

public class ApplicationDbContextFactory<TContext> : IDbContextFactory<TContext> where TContext : DbContext
{
    private readonly IServiceProvider _provider;

    public ApplicationDbContextFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public TContext CreateDbContext()
    {
        if (_provider == null)
            throw new InvalidOperationException("You must configure an instance of IServiceProvider");

        return ActivatorUtilities.CreateInstance<TContext>(_provider);
    }
}