﻿using Bogus;
using PetCareClub.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace PetCareClub.IntegrationTests.Base;
public class BaseFixture
{
    protected Faker Faker { get; set; }

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
    }

    public PetCareClubDbContext CreateDbContext(
        bool preserveData = false
    )
    {
        var context = new PetCareClubDbContext(
            new DbContextOptionsBuilder<PetCareClubDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
        );

        if (!preserveData)
            context.Database.EnsureDeleted();

        return context;

    }


}
