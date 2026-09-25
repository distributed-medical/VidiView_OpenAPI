using VidiView.Api.DataModel;

namespace VidiView.Api.Configuration;

public class ADAccountFilter
{
    /// <summary>
    /// A specific domain to search for accounts. If not specified, the current domain's Global Catalog will be queried
    /// </summary>
    public string? DomainName { get; set; }

    /// <summary>
    /// The name of the account to search for. If not specified, all accounts will be returned.
    /// </summary>
    public string? AccountName { get; set; }

    /// <summary>
    /// The type of account to search for
    /// </summary>
    public ADAccountType Type { get; set; } = ADAccountType.Group | ADAccountType.User;

    /// <summary>
    /// Maximum number of accounts to return
    /// </summary>
    public int MaximumHits { get; set; } = 500;
}
