using WiseMigrateTests.TestHooks;
using Xunit;

[CollectionDefinition("Playwright collection", DisableParallelization = true)]
public class PlaywrightCollection : ICollectionFixture<GlobalTestFixture>
{
    
}