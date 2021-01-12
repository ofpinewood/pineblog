
namespace Opw.PineBlog.MongoDb
{
    public static class Constants
    {
        // tests are not skipped since they are now working on the build agent as well
        public const string SkipMongoDbTests = null; //"Requires MongoDb in memory database (that does not work on the build server).";
        public const string SkipMongoDbBlogSettingsConfigurationProviderTests = "These tests do not work on the build server.";
    }
}
