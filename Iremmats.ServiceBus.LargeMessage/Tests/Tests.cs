using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using Iremmats.ServiceBus.LargeMessage;
using Microsoft.ServiceBus.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        public CloudBlobClient BlobClient;
        public CloudBlobContainer Container;
        public Stream TestStream;
        public string TestString = "Im a stupid string...";
        public byte[] TestByteArray;
        public string MessageType = "messageType";
        public string Queue = "queue";
        public string Filename = "filename.xml";


        [TestInitialize]
        public void SetupBlobClient()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["ReferenceMessagesStorage"].ConnectionString);
            BlobClient = storageAccount.CreateCloudBlobClient();
            Container = BlobClient.GetContainerReference("messages");

            TestStream = File.OpenRead("./Resources/testfile.txt");
            TestByteArray = Encoding.UTF8.GetBytes(TestString);
        }

        [TestMethod]
        public void SendAndReceiveString()
        {
            ReferenceMessage rfMsg = new ReferenceMessage();
            BrokeredMessage myMsg = rfMsg.CreateMessage(TestString, Queue, MessageType, CacheType.Storage, Filename);

            string connectionString = ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var messagingFactory = MessagingFactory.CreateFromConnectionString(connectionString);
            var sender = messagingFactory.CreateMessageSender("testing");
            sender.Send(myMsg);

            var receiver = messagingFactory.CreateMessageReceiver("testing", ReceiveMode.ReceiveAndDelete);
            BrokeredMessage receivedMsg = receiver.Receive();

            string body = rfMsg.RetrieveBodyAsString(receivedMsg);
            Assert.AreEqual(TestString, body);
        }

        [TestMethod]
        public void SendAndReceiveBytes()
        {
            var rfMsg = new ReferenceMessage();
            var myMsg = rfMsg.CreateMessage(TestByteArray, Queue, MessageType, CacheType.Storage);

            var connectionString = ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var messagingFactory = MessagingFactory.CreateFromConnectionString(connectionString);
            var sender = messagingFactory.CreateMessageSender("testing");
            sender.Send(myMsg);

            var receiver = messagingFactory.CreateMessageReceiver("testing", ReceiveMode.ReceiveAndDelete);
            var receivedMsg = receiver.Receive();

            var body = rfMsg.RetrieveBodyAsString(receivedMsg);
            Assert.AreEqual(TestString, body);
        }

        [TestMethod]
        public void TestSaveToStorage()
        {

            var rfMsg = new ReferenceMessage();

            var myBlob = rfMsg.SaveToStorage(TestStream, "test1" + Guid.NewGuid().ToString());
            Assert.IsTrue(myBlob.Exists());
            myBlob = rfMsg.SaveToStorage(TestString, "test2" + Guid.NewGuid().ToString());
            Assert.IsTrue(myBlob.Exists());
            myBlob = rfMsg.SaveToStorage(TestByteArray, "test3" + Guid.NewGuid().ToString());
            Assert.IsTrue(myBlob.Exists());
        }

        [TestMethod]
        public void GetSasToken()
        {
            var sasuri = TokenGenerator.GetBlobSasUri(Container, "messageType/queue/2015-06-20/filename_21-58-40_fc5ebd3b-ca42-41fb-abec-842db13ccd4b.xml");

            CloudBlockBlob cloudBlockBlob = new CloudBlockBlob(new Uri(sasuri));

            string text = cloudBlockBlob.DownloadText();

        }

        [TestMethod]
        public void BlobMetadata()
        {

            var dict = new Dictionary<string, string> {{"metadata1", "value1"}, {"metadata2", "value2"}};

            var rfMsg = new ReferenceMessage();

            BrokeredMessage msg = rfMsg.CreateMessage(TestStream, "queue", "messageType",
                CacheType.Storage);
            TestStream.Position = 0;
            msg = rfMsg.CreateMessage(TestStream, "queue", "messageType",
                CacheType.Storage, "filename123.txt");

            TestStream.Position = 0;
            msg = rfMsg.CreateMessage(TestStream, "queue", "messageType",
                CacheType.Storage, "filename123.txt", dict);

            msg = rfMsg.CreateMessage(TestString, "queue", "messageType",
                CacheType.Storage);
            msg = rfMsg.CreateMessage(TestString, "queue", "messageType",
                CacheType.Storage, "filename123.txt");
            msg = rfMsg.CreateMessage(TestString, "queue", "messageType",
                CacheType.Storage, "filename123.txt", dict);

            msg = rfMsg.CreateMessage(TestByteArray, "queue", "messageType",
                CacheType.Storage);
            msg = rfMsg.CreateMessage(TestByteArray, "queue", "messageType",
                CacheType.Storage, "filename123.txt");
            msg = rfMsg.CreateMessage(TestByteArray, "queue", "messageType",
                CacheType.Storage, "filename123.txt", dict);

            //Check the blobs in storage...

        }
    }
}
