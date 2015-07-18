using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using Microsoft.ServiceBus.Messaging;

namespace Iremmats.ServiceBus.LargeMessage
{
    public class ReferenceMessage : ILargeMessageReference
    {
        private const string ContainerName = "messages";
        private const string ReferenceMessagesStorage = "ReferenceMessagesStorage";
        private readonly CloudBlobContainer _container;

        //Default structure for messages are 
        // /messages/messagetype/queue/filename_guid.ext

        public ReferenceMessage()
        {
            var storageConnectionString = ConfigurationManager.ConnectionStrings[ReferenceMessagesStorage].ConnectionString;
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(ContainerName);
            _container.CreateIfNotExists();
        }

        #region SaveToStorage
        public CloudBlockBlob SaveToStorage(string body, string location)
        {
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(location);
            blockBlob.UploadText(body);
            return blockBlob;
        }
        public CloudBlockBlob SaveToStorage(Stream body, string location)
        {
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(location);
            blockBlob.UploadFromStream(body);
            return blockBlob;
        }
        public CloudBlockBlob SaveToStorage(byte[] body, string location)
        {
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(location);
            blockBlob.UploadFromByteArray(body, 0, body.Length);
            return blockBlob;
        }
        #endregion

        #region CreateMessage
        public BrokeredMessage CreateMessage(string body, string queue, string messageType, CacheType cacheType)
        {
            var location = FullFilename(messageType, queue);
            var sasUri = TokenGenerator.GetBlobSasUri(_container, location);
            CloudBlockBlob blockBlob = SaveToStorage(body, location);
            LargeMessage myRefMsg = new LargeMessage
            {
                Uri = blockBlob.Uri,
                BodyLocation = blockBlob.Name,
                CacheType = CacheType.Storage,
                SasUri = new Uri(sasUri)
            };
            return new BrokeredMessage(SerializationHelper.SerializeToJsonString(myRefMsg));
        }
        public BrokeredMessage CreateMessage(string body, string queue, string messageType, CacheType cacheType, string filename)
        {
            var location = FullFilename(messageType, queue, filename);
            var sasUri = TokenGenerator.GetBlobSasUri(_container, location);
            CloudBlockBlob blockBlob = SaveToStorage(body, location);
            LargeMessage myRefMsg = new LargeMessage
            {
                Uri = blockBlob.Uri,
                BodyLocation = blockBlob.Name,
                CacheType = CacheType.Storage,
                SasUri = new Uri(sasUri)
            };
            return new BrokeredMessage(SerializationHelper.SerializeToJsonString(myRefMsg));
        }
        public BrokeredMessage CreateMessage(string body, string queue, string messageType, CacheType cacheType, string filename, Dictionary<string, string> metadata)
        {
            var location = FullFilename(messageType, queue, filename);
            var sasUri = TokenGenerator.GetBlobSasUri(_container, location);
            CloudBlockBlob blockBlob = SaveToStorage(body, location);
            if (metadata != null)
            {
                foreach (var property in metadata)
                {
                    if (property.Value != "") blockBlob.Metadata.Add(property.Key, property.Value);
                }

                blockBlob.SetMetadata();
            }

            LargeMessage myRefMsg = new LargeMessage
            {
                Uri = blockBlob.Uri,
                BodyLocation = blockBlob.Name,
                CacheType = CacheType.Storage,
                SasUri = new Uri(sasUri)
            };
            return new BrokeredMessage(SerializationHelper.SerializeToJsonString(myRefMsg));
        }
        public BrokeredMessage CreateMessage(Stream body, string queue, string messageType, CacheType cacheType)
        {
            var location = FullFilename(messageType, queue);
            var sasUri = TokenGenerator.GetBlobSasUri(_container, location);
            CloudBlockBlob blockBlob = SaveToStorage(body, location);
            LargeMessage myRefMsg = new LargeMessage
            {
                Uri = blockBlob.Uri,
                BodyLocation = blockBlob.Name,
                CacheType = CacheType.Storage,
                SasUri = new Uri(sasUri)
            };
            return new BrokeredMessage(SerializationHelper.SerializeToJsonString(myRefMsg));
        }
        public BrokeredMessage CreateMessage(Stream body, string queue, string messageType, CacheType cacheType, string filename)
        {
            var location = FullFilename(messageType, queue, filename);
            var sasUri = TokenGenerator.GetBlobSasUri(_container, location);
            CloudBlockBlob blockBlob = SaveToStorage(body, location);
            LargeMessage myRefMsg = new LargeMessage
            {
                Uri = blockBlob.Uri,
                BodyLocation = blockBlob.Name,
                CacheType = CacheType.Storage,
                SasUri = new Uri(sasUri)
            };
            return new BrokeredMessage(SerializationHelper.SerializeToJsonString(myRefMsg));
        }
        public BrokeredMessage CreateMessage(Stream body, string queue, string messageType, CacheType cacheType, string filename, Dictionary<string, string> metadata)
        {
            var location = FullFilename(messageType, queue, filename);
            var sasUri = TokenGenerator.GetBlobSasUri(_container, location);
            CloudBlockBlob blockBlob = SaveToStorage(body, location);
            if (metadata != null)
            {
                foreach (var property in metadata)
                {
                    if (property.Value != "") blockBlob.Metadata.Add(property.Key, property.Value);
                }

                blockBlob.SetMetadata();
            }

            LargeMessage myRefMsg = new LargeMessage
            {
                Uri = blockBlob.Uri,
                BodyLocation = blockBlob.Name,
                CacheType = CacheType.Storage,
                SasUri = new Uri(sasUri)
            };
            return new BrokeredMessage(SerializationHelper.SerializeToJsonString(myRefMsg));
        }
        public BrokeredMessage CreateMessage(byte[] body, string queue, string messageType, CacheType cacheType)
        {
            var location = FullFilename(messageType, queue);
            var sasUri = TokenGenerator.GetBlobSasUri(_container, location);
            CloudBlockBlob blockBlob = SaveToStorage(body, location);
            LargeMessage myRefMsg = new LargeMessage
            {
                Uri = blockBlob.Uri,
                BodyLocation = blockBlob.Name,
                CacheType = CacheType.Storage,
                SasUri = new Uri(sasUri)
            };
            return new BrokeredMessage(SerializationHelper.SerializeToJsonString(myRefMsg));
        }
        public BrokeredMessage CreateMessage(byte[] body, string queue, string messageType, CacheType cacheType, string filename)
        {
            var location = FullFilename(messageType, queue, filename);
            var sasUri = TokenGenerator.GetBlobSasUri(_container, location);
            CloudBlockBlob blockBlob = SaveToStorage(body, location);
            LargeMessage myRefMsg = new LargeMessage
            {
                Uri = blockBlob.Uri,
                BodyLocation = blockBlob.Name,
                CacheType = CacheType.Storage,
                SasUri = new Uri(sasUri)
            };
            return new BrokeredMessage(SerializationHelper.SerializeToJsonString(myRefMsg));
        }
        public BrokeredMessage CreateMessage(byte[] body, string queue, string messageType, CacheType cacheType, string filename, Dictionary<string, string> metadata)
        {
            var location = FullFilename(messageType, queue, filename);
            var sasUri = TokenGenerator.GetBlobSasUri(_container, location);
            CloudBlockBlob blockBlob = SaveToStorage(body, location);
            if (metadata != null)
            {
                foreach (var property in metadata)
                {
                    if (property.Value != "") blockBlob.Metadata.Add(property.Key, property.Value);
                }

                blockBlob.SetMetadata();
            }

            LargeMessage myRefMsg = new LargeMessage
            {
                Uri = blockBlob.Uri,
                BodyLocation = blockBlob.Name,
                CacheType = CacheType.Storage,
                SasUri = new Uri(sasUri)
            };
            return new BrokeredMessage(SerializationHelper.SerializeToJsonString(myRefMsg));
        }

        #endregion

        public Stream RetrieveBodyAsStream(BrokeredMessage msg)
        {
            CloudBlockBlob blockBlob = GetReferenceToBlob(msg);
            MemoryStream ms = new MemoryStream();
            blockBlob.DownloadToStream(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public string RetrieveBodyAsString(BrokeredMessage msg)
        {
            CloudBlockBlob blockBlob = GetReferenceToBlob(msg);
            return blockBlob.DownloadText();
        }

        public byte[] RetrieveBodyAsBytes(BrokeredMessage msg)
        {
            CloudBlockBlob blockBlob = GetReferenceToBlob(msg);
            blockBlob.FetchAttributes();
            long fileByteLength = blockBlob.Properties.Length;
            byte[] target = new byte[fileByteLength];
            blockBlob.DownloadToByteArray(target, 0);
            return target;
        }


        private CloudBlockBlob GetReferenceToBlob(BrokeredMessage msg)
        {
            LargeMessage myRef = SerializationHelper.DeserializeFromJsonString<LargeMessage>(msg.GetBody<string>());
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(myRef.BodyLocation);
            return blockBlob;
        }

        #region FullFileName
        //Creates a full blob location from different inputs 
        private static string FullFilename(string messageType, string queue, string filename)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            string fullFilename = Path.Combine(messageType, queue, today, InsertTimeAndGuidOnFilename(filename));
            return fullFilename;
        }

        private static string FullFilename(string messageType, string queue)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string fullFilename = Path.Combine(messageType, queue, today, Guid.NewGuid().ToString());
            return fullFilename;
        }
        #endregion

        public static string InsertTimeAndGuidOnFilename(string filename)
        {
            return
                $"{Path.GetFileNameWithoutExtension(filename)}_{DateTime.Now.ToString("HH-mm-ss")}_{Guid.NewGuid().ToString()}{Path.GetExtension(filename)}";
        }
    }
}
