using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Opw.PineBlog.Entities;

namespace Opw.PineBlog.MongoDb
{

    internal static class BsonClassMappings
    {
        public static void Register()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(BlogSettings)))
            {
                BsonClassMap.RegisterClassMap<BlogSettings>(m =>
                {
                    m.AutoMap();
                    m.MapIdProperty(bs => bs.Created);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Post)))
            {
                BsonClassMap.RegisterClassMap<Post>(m =>
                {
                    m.AutoMap();
                    m.MapIdProperty(p => p.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                    m.MapProperty(p => p.AuthorId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                    m.UnmapProperty(p => p.Author);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Author)))
            {
                BsonClassMap.RegisterClassMap<Author>(m =>
                {
                    m.AutoMap();
                    m.MapIdProperty(a => a.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                    m.UnmapProperty(a => a.Posts);
                });
            }
        }
    }
}
