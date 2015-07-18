# sb-largemessage
Lib designed to let you send large messages over the Service Bus

The basic usage is the following:

Put a connection string to a Storage Account in your app.config
<connectionStrings>
	<add name="ReferenceMessagesStorage" connectionString="{storage connection string goes here}" />
</connectionStrings>

Create an instance of the interface
ReferenceMessage rfMsg = new ReferenceMessage();

Create a BrokeredMessage from an object/stream/string/bytearray
BrokeredMessage myMsg = rfMsg.CreateMessage(Stream, Queue, MessageType, CacheType.Storage, Filename);

The Stream will now end up in your storage account under /messages/MessageType/Queue/date/
Send myMsg to your service bus queue/topic in any way. What will end up on the service bus looks like this:

{
	"BodyLocation" : "messages/MessageType/Queue/Date/blobname",
	"CacheType"	: "0",
	"Uri" : "https://xxx.blob.core.windows.net/messages/MessageType/Queue/2015-07-05/filename_with_guid",
	"SasUri" : "https://xxx.blob.core.windows.net/messages/MessageType%2FQueue%2F2015-07-05%2Ffilename_with_guid?sr=b&sv=2015-02-21&st=2015-07-18T06%3A39%3A22Z&se=2015-07-18T07%3A39%3A22Z&sp=rwd&sig=tnfkyTU6iNrjq1sdfsdeAsKEowGTUMjfTDNOGwX%2BZe2r%2BpofI%3D"
}

The receiver has to create an instance of the interface too and then retrieve the body. The receiver has to have a connection string to storage too but since the SasUri is used, it does not have to be the same one.
ReferenceMessage rfMsg = new ReferenceMessage();
string body = rfMsg.RetrieveBodyAsString(receivedMsg);
