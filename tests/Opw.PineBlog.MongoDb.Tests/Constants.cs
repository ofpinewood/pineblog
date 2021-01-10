
namespace Opw.PineBlog.MongoDb
{
    public static class Constants
    {
        // tests are not skipped since they are now working on the build agent as well
        // TODO: re-enable MongoDb tests
        public const string SkipMongoDbTests = "Requires MongoDb in memory database (that does not work on the build server).";
        public const string SkipMongoDbBlogSettingsConfigurationProviderTests = "These tests do not work on the build server.";
    }
}
