using EA.Migrator;

var factory = new OLDBContextFactory();

await using (var db = factory.CreateDbContext(args))
{
    if (await db.Database.CanConnectAsync())
        await db.Database.EnsureCreatedAsync();

    await db.SaveChangesAsync();
}