using Microsoft.Security.Authentication.OAuth;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VidiView.Api.Authentication;
using VidiView.Api.Helpers;

namespace VidiView.Api.Helpers.Test.ReverseProxy;
[TestClass]
public partial class UnitTest1
{
    static HttpClient _http = null!;

    /// <summary>
    /// Connect to VidiView Server and ensure device is registered.
    /// </summary>
    /// <param name="context"></param>
    [ClassInitialize]
    public static async Task ClassInitialize(TestContext context)
    {
        var handler = new SocketsHttpHandler
        {
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.All,
            UseCookies = false
        };
        
        handler.AcceptLegacyLicenseCertificate(true);

        _http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(10) };
        _http.SetApiKey(TestConfig.ApiKey());
    }


    /// <summary>
    /// Try to connect
    /// </summary>
    /// <returns></returns>
    [UITestMethod]
    public async Task Connect_And_Receive_PreAuthentication()
    {
        _http.ClearAuthentication(); // Start fresh

        var parentWindowId = UnitTestApp.TestWindow.AppWindow.Id;

        var state = await _http.ConnectAsync(TestConfig.ServerHostName, CancellationToken.None);

        Assert.IsInstanceOfType<PreauthenticateRequired>(state);

        var oidc = (PreauthenticateRequired)state;
        Assert.AreEqual("oidc", oidc.IdP);

        AuthRequestParams authRequestParams = AuthRequestParams.CreateForAuthorizationCodeRequest(oidc.ClientId);
        authRequestParams.Scope = oidc.Scope;

        AuthRequestResult authRequestResult = await OAuth2Manager.RequestAuthWithParamsAsync(parentWindowId,
            oidc.AuthEndPoint, authRequestParams);


    }

    // Use the UITestMethod attribute for tests that need to run on the UI thread.
    //[UITestMethod]
    //public void TestMethod2()
    //{
    //    var grid = new Grid();
    //    Assert.AreEqual(0, grid.MinWidth);
    //}
}
