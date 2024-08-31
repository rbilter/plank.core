namespace Plank.Core.Tests.Fixtures
{
    [CollectionDefinition("PlankConfigurationCollection", DisableParallelization = true)]
    public sealed class PlankConfigurationCollection : ICollectionFixture<PlankConfigurationFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}