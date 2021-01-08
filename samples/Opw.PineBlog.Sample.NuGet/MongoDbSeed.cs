using MongoDB.Driver;
using Opw.PineBlog.Entities;
using Opw.PineBlog.MongoDb;
using System;
using System.Linq;

namespace Opw.PineBlog.Sample.NuGet
{
    internal class MongoDbSeed : DbSeedBase
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<Author> _authorCollection;
        private readonly IMongoCollection<Post> _postCollection;

        public MongoDbSeed(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;

            _authorCollection = _mongoDatabase.GetCollection<Author>(CollectionHelper.GetName<Author>());
            _postCollection = _mongoDatabase.GetCollection<Post>(CollectionHelper.GetName<Post>());
        }

        public void Run()
        {
            if (DateTime.UtcNow.Day % 2 == 0)
                CreateAuthor("John Smith", "images/avatar-male.png");
            else
                CreateAuthor("Mary Smith", "images/avatar-female.png");

            CreatePosts();
        }

        void CreateAuthor(string name, string imagePath)
        {
            if (_authorCollection.CountDocuments(Builders<Author>.Filter.Empty) > 0) return;

            var email = ApplicationConstants.UserEmail;
            if (_authorCollection.CountDocuments(a => a.UserName.Equals(email)) > 0) return;

            _authorCollection.InsertOne(GetAuthor(name, email, imagePath));
        }

        void CreatePosts()
        {
            if (_postCollection.CountDocuments(Builders<Post>.Filter.Empty) > 0) return;

            var email = ApplicationConstants.UserEmail;
            var author = _authorCollection.Find(a => a.UserName.Equals(email)).Single();

            for (int i = 1; i < 40; i++)
            {
                _postCollection.InsertOne(GetPost(i, author.Id));
            }
        }
    }
}
