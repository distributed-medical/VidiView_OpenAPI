namespace VidiView.Api.Configuration;

public class UserAccountFilter
{
    /// <summary>
    /// If true, only user accounts will be returned. 
    /// This excludes groups and other non-user accounts. If false, all accounts will be returned.
    /// </summary>
    public bool OnlyUserAccounts { get; set; }

    /// <summary>
    /// Include implicit accounts in the result. Implicit accounts are accounts that are not 
    /// explicitly created in the system, but are created automatically based on other information, 
    /// such as a user being a member of an Active Directory group.
    /// </summary>
    public bool IncludeImplicitAccounts { get; set; }

    /// <summary>
    /// Set to true to include deleted user accounts
    /// </summary>
    public bool IncludeDeletedAccounts { get; set; }

    /// <summary>
    /// Name filter. Wildcards are supported
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Limit the returned number of hits. Set to 0 to return all hits.
    /// </summary>
    public int MaximumHits { get; set; } = 0;
}
