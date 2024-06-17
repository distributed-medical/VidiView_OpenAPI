using System.Text;

namespace VidiView.Api.WSMessaging.Test;

[TestClass]
public class MessageSerializerTest
{
    [TestMethod]
    public void TestSerializeMessage()
    {
        var authMessage = new AuthenticateMessage("abcd", "1234")
            { MessageId = "2d4122306afd4cdb946f6e2ecbb5a99f" };

        var buffer = MessageSerializer.Serialize(authMessage);

        // Assert
        string result = Encoding.UTF8.GetString(buffer);
        Assert.AreEqual("{\"ApiKey\":\"abcd\",\"Authorization\":\"1234\",\"MessageType\":\"AuthenticateMessage\",\"MessageId\":\"2d4122306afd4cdb946f6e2ecbb5a99f\"}", result);
    }

    [TestMethod]
    public void TestDeserializeMessage()
    {
        string data = "{\"ApiKey\":\"abcd\",\"Authorization\":\"1234\",\"MessageType\":\"AuthenticateMessage\",\"MessageId\":\"2d4122306afd4cdb946f6e2ecbb5a99f\"}";
        var buffer = Encoding.UTF8.GetBytes(data);
        var success = MessageSerializer.TryDeserialize(buffer, out var message);

        // Assert
        Assert.IsTrue(success);
        Assert.IsInstanceOfType<AuthenticateMessage>(message);
        Assert.AreEqual("2d4122306afd4cdb946f6e2ecbb5a99f", message.MessageId);

        var auth = (AuthenticateMessage)message;
        Assert.AreEqual("abcd", auth.ApiKey);
        Assert.AreEqual("1234", auth.Authorization);
    }
}