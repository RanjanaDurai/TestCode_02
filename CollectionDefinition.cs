using WiseMigrateTests.TestHooks;
using Xunit;
[TestCaseOrderer("WiseMigrateTests.TestPriorityOrderer", "WiseMigrateTests")]
[CollectionDefinition("Playwright collection", DisableParallelization = true)]
public class PlaywrightCollection : ICollectionFixture<GlobalTestFixture>
{
    
}