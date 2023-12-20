using VidiView.Api.DataModel;

namespace VidiView.Api.Access.Authentication;

public interface IAuthenticator
{
    bool IsSupported { get; }
    User? User { get; }
    AuthToken? Token { get; }
}

