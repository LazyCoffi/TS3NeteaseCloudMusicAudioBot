using MusicBotPlugin;

namespace PluginTest;

[TestClass]
public class PluginTest
{
    [TestMethod]
    public void GetLoginKeyUrlTest()
    {
        Porter porter = new Porter("http://117.50.187.207:3000");
        string result = porter.GetLoginKeyUrl();
        Assert.AreEqual(result, "http://117.50.187.207:3000/login/qr/key");
    }

    [TestMethod]
    public void GetLoginQrUrlTest()
    {
        Porter porter = new Porter("http://117.50.187.207:3000");
        string result = porter.GetLoginQrUrl("sample_key");
        Assert.AreEqual(result, "http://117.50.187.207:3000/login/qr/create?key=sample_key&qrimg=true");
    }

    [TestMethod]
    public void GetLoginStatusUrlCheck()
    {
        Porter porter = new Porter("http://117.50.187.207:3000");
        string result = porter.GetLoginStatusUrl("sample_key");
        Assert.AreEqual(result, "http://117.50.187.207:3000/login/qr/check?key=sample_key");
    }
}