using System.Text;
using VidiView.Api.WSMessaging;

namespace VidiView.Api.Helpers.Test.Helpers;

[TestClass]
public class WSMessageTest
{
    private class PrivateMessageType : IWSMessage
    {
        public string MessageType { get; init; }
        public string MessageId { get; init; }
    }

    private class GenericMessageType<T1,T2> : IWSMessage
    {
        public string MessageType { get; init; }
        public string MessageId { get; init; }
        public T1? T1Value { get; set; }
        public T2? T2Value { get; set; }
    }

    [TestMethod]
    public void CreateInstanceUsingFactory()
    {
        var authMessage = WSMessage.Factory<AuthenticateMessage>();

        Assert.AreEqual("VidiView.Api.WSMessaging.AuthenticateMessage", authMessage.MessageType, "Expected namespace qualified name (not assembly qualified)");
        Assert.IsNotNull(authMessage.MessageId);
        Assert.IsTrue(Guid.TryParse(authMessage.MessageId, out Guid id));
    }

    [TestMethod]
    public void CreateReplyUsingFactory()
    {
        var authMessage = WSMessage.Factory<AuthenticateMessage>();
        var replyMessage = WSMessage.Factory<AuthenticateReplyMessage>(authMessage);

        Assert.AreEqual("VidiView.Api.WSMessaging.AuthenticateReplyMessage", replyMessage.MessageType);
        Assert.IsNotNull(replyMessage.MessageId);
        Assert.IsTrue(Guid.TryParse(replyMessage.MessageId, out Guid id));
        Assert.AreEqual(authMessage.MessageId, replyMessage.InReplyTo);
        Assert.AreNotEqual(replyMessage.MessageId, replyMessage.InReplyTo);
    }

    [TestMethod]
    public void CreateGenericUsingFactory()
    {
        var customMessage = WSMessage.Factory<GenericMessageType<int, double>>();
        Assert.AreEqual("VidiView.Api.Helpers.Test.Helpers.WSMessageTest+GenericMessageType`2[System.Int32,System.Double]", customMessage.MessageType);
    }

    [TestMethod]
    public void CreateGenericArrayUsingFactory()
    {
        var customMessage = WSMessage.Factory<GenericMessageType<int[], List<string>>>();
        Assert.AreEqual("VidiView.Api.Helpers.Test.Helpers.WSMessageTest+GenericMessageType`2[System.Int32[],System.Collections.Generic.List`1[System.String]]", customMessage.MessageType);
    }

    [TestMethod]
    public void TestSerializeMessage()
    {
        var authMessage = new AuthenticateMessage
        {
            MessageType = "VidiView.Api.WSMessaging.AuthenticateMessage",
            MessageId = "2d4122306afd4cdb946f6e2ecbb5a99f",
            ApiKey = "abcd",
            Authorization = "1234",
        };

        // Act
        var buffer = authMessage.Serialize();

        // Assert
        string result = Encoding.UTF8.GetString(buffer);
        Assert.AreEqual("{\"message-type\":\"VidiView.Api.WSMessaging.AuthenticateMessage\",\"message-id\":\"2d4122306afd4cdb946f6e2ecbb5a99f\",\"api-key\":\"abcd\",\"authorization\":\"1234\"}", result);
    }

    [TestMethod]
    public void TestDeserializeMessage()
    {
        string data = "{\"message-type\":\"VidiView.Api.WSMessaging.AuthenticateMessage\",\"message-id\":\"2d4122306afd4cdb946f6e2ecbb5a99f\",\"api-key\":\"abcd\",\"authorization\":\"1234\"}";
        var buffer = Encoding.UTF8.GetBytes(data);
        var success = WSMessage.TryDeserialize(buffer, out var message);

        // Assert
        Assert.IsTrue(success);
        Assert.IsInstanceOfType<AuthenticateMessage>(message);
        var auth = (AuthenticateMessage)message;

        Assert.AreEqual("2d4122306afd4cdb946f6e2ecbb5a99f", message.MessageId);
        Assert.AreEqual("abcd", auth.ApiKey);
        Assert.AreEqual("1234", auth.Authorization);
    }

    [TestMethod]
    public void SerializePrivateMessageType()
    {
        var customMessage = WSMessage.Factory<PrivateMessageType>();
        Assert.AreEqual("VidiView.Api.Helpers.Test.Helpers.WSMessageTest+PrivateMessageType", customMessage.MessageType);

        var serialized = WSMessage.Serialize(customMessage);
        Assert.IsNotNull(serialized);

        var success = WSMessage.TryDeserialize(serialized, out var deserializedMessage);
        Assert.IsTrue(success);
        Assert.AreEqual(customMessage.MessageId, deserializedMessage.MessageId);
    }

    [TestMethod]
    public void SerializeAndDeserializeGenericMessageType()
    {
        var customMessage = new GenericMessageType<int, double>
        {
            MessageId = "ff627f27167447a2a696541c7f7ab546",
            MessageType = "VidiView.Api.Helpers.Test.Helpers.WSMessageTest+GenericMessageType`2[System.Int32,System.Double]",
            T1Value = 666,
            T2Value = 66.6
        };

        var serialized = WSMessage.Serialize(customMessage);

        var json = Encoding.UTF8.GetString(serialized);
        Assert.AreEqual("{\"message-type\":\"VidiView.Api.Helpers.Test.Helpers.WSMessageTest\\u002BGenericMessageType\\u00602[System.Int32,System.Double]\",\"message-id\":\"ff627f27167447a2a696541c7f7ab546\",\"t1-value\":666,\"t2-value\":66.6}", json);

        var success = WSMessage.TryDeserialize(serialized, out var deserializedMessage);
        Assert.IsTrue(success);
        Assert.AreEqual(customMessage.MessageId, deserializedMessage.MessageId);
        Assert.AreEqual(666, ((GenericMessageType<int, double>) deserializedMessage).T1Value);
        Assert.AreEqual(66.6, ((GenericMessageType<int, double>)deserializedMessage).T2Value);
    }

    [TestMethod]
    public void SerializeAndDeserializeGenericListType()
    {
        var customMessage = WSMessage.Factory<GenericMessageType<List<int>, double>>();
        customMessage.T1Value = new List<int> { 10, 20, 30, 40 };

        var serialized = WSMessage.Serialize(customMessage);
        var json = Encoding.UTF8.GetString(serialized);

        var success = WSMessage.TryDeserialize(serialized, out var deserializedMessage);
        Assert.IsTrue(success);
        var d = (GenericMessageType<List<int>, double>)deserializedMessage;
        CollectionAssert.AreEqual(customMessage.T1Value, d.T1Value);
    }


    [TestMethod]
    public void SerializeAndDeserializeGenericArrayType()
    {
        var customMessage = WSMessage.Factory<GenericMessageType<int[], bool>>();
        customMessage.T1Value = [10, 20, 30, 40];
        customMessage.T2Value = true;

        var serialized = WSMessage.Serialize(customMessage);
        var json = Encoding.UTF8.GetString(serialized);

        var success = WSMessage.TryDeserialize(serialized, out var deserializedMessage);
        Assert.IsTrue(success);
        var d = (GenericMessageType<int[], bool>)deserializedMessage;
        CollectionAssert.AreEqual(customMessage.T1Value, d.T1Value);
        Assert.AreEqual(customMessage.T2Value, d.T2Value);
    }
}