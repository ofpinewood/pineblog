using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Opw.PineBlog.MongoDb
{
    /// <summary>
    /// Event arguments for events relating to tracked MongoDB.Driver.ChangeStreamDocuments.
    /// </summary>
    public class DocumentChangeEventArgs : EventArgs
    {
        /// <summary>
        /// The ChangeStreamDocument for the document.
        /// </summary>
        public ChangeStreamDocument<BsonDocument> Document { get; }

        /// <summary>
        /// This API supports the MongoDb infrastructure and is not intended
        /// to be used directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public DocumentChangeEventArgs(ChangeStreamDocument<BsonDocument> document)
        {
            Document = document;
        }
    }
}
