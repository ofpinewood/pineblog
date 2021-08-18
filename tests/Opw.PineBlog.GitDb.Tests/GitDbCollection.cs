using Xunit;

namespace Opw.PineBlog.GitDb
{
    [CollectionDefinition(nameof(GitDbCollection))]
    public class GitDbCollection : ICollectionFixture<GitDbFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
