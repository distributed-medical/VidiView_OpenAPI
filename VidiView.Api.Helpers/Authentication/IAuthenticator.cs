using VidiView.Api.DataModel;

namespace VidiView.Api.Authentication;

/// <summary>
/// This interface is implemented on authenticator classes.
/// Note that the actual Authenticate request has different signatures
/// in all implementations and is therefore not part of the interface
/// </summary>
public interface IAuthenticator
{
    // Task AuthenticateAsync();

    /// <summary>
    /// Returns true if the authentication method is supported
    /// </summary>
    /// <returns></returns>
    /// <exception cref="E1007_DeviceNotGrantedAccessException">Thrown if not registered first</exception>
    Task<bool> IsSupportedAsync();

    /// <summary>
    /// Authenticated user. This is set after a successful authentication request
    /// </summary>
    User? User { get; }

    /// <summary>
    /// Authentication token. This is set after a successful authentication request
    /// </summary>
    AuthToken? Token { get; }

    /// <summary>
    /// Optional token request options
    /// </summary>
    TokenRequest? Options { get; set; }
}
