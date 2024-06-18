using System.Text;
using VidiView.Api.WSMessaging;

namespace VidiView.Api.Helpers.Test.Extension;

[TestClass]
public class WSMessageExtensionTest
{
    [TestMethod]
    public void TestSerializeMessage()
    {
        var authMessage = new AuthenticateMessage("abcd", "1234")
        { MessageId = "2d4122306afd4cdb946f6e2ecbb5a99f" };

        var buffer = authMessage.Serialize();

        // Assert
        string result = Encoding.UTF8.GetString(buffer);
        Assert.AreEqual("{\"api-key\":\"abcd\",\"authorization\":\"1234\",\"message-type\":\"AuthenticateMessage\",\"message-id\":\"2d4122306afd4cdb946f6e2ecbb5a99f\"}", result);
    }

    [TestMethod]
    public void TestDeserializeMessage()
    {
        string data = "{\"api-key\":\"abcd\",\"authorization\":\"1234\",\"message-type\":\"AuthenticateMessage\",\"message-id\":\"2d4122306afd4cdb946f6e2ecbb5a99f\"}";
        var buffer = Encoding.UTF8.GetBytes(data);
        var success = WSMessageSerializer.TryDeserialize(buffer, out var message);

        // Assert
        Assert.IsTrue(success);
        Assert.IsInstanceOfType<AuthenticateMessage>(message);
        Assert.AreEqual("2d4122306afd4cdb946f6e2ecbb5a99f", message.MessageId);

        var auth = (AuthenticateMessage)message;
        Assert.AreEqual("abcd", auth.ApiKey);
        Assert.AreEqual("1234", auth.Authorization);
    }
}