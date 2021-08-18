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

        public DocumentChangeEventArgs(ChangeStreamDocument<BsonDocument> document)
        {
            Document = document;
        }
    }
}
