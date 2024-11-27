using MongoDB.Bson;
using MongoDB.Driver;
using Opw.PineBlog.Entities;
using System;

namespace Opw.PineBlog.MongoDb
{
    public static class CollectionHelper
    {
        public static string GetName<TEntity>()
        {
            switch (typeof(TEntity).Name)
            {
                case nameof(Author):
                    return nameof(Author) + "s";
                case nameof(Post):
                    return nameof(Post) + "s";
                case nameof(BlogSettings):
                    return nameof(BlogSettings);
            }

            throw new NotSupportedException($"Entity \"{typeof(TEntity).Name}\" is not supported.");
        }
    }
}
