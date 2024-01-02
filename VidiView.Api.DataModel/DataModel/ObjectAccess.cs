namespace VidiView.Api.DataModel;

public record ObjectAccess
{
    /// <summary>
    /// The unique id of the user or group this ACL applies to
    /// </summary>
    public Guid? UserId { get; init; }

    /// <summary>
    /// Department (if this instance applies to a department)
    /// </summary>
    public IdAndName? Department { get; init; }

    /// <summary>
    /// The granted flags
    /// </summary>
    public long Granted { get; init; }

    /// <summary>
    /// The explicitly denied flags
    /// </summary>
    public long Denied { get; init; }

    /// <summary>
    /// The effective permission
    /// </summary>
    public long Effective => Granted & ~Denied;

    /// <summary>
    /// Check if this acl item grants the required permission(s)
    /// </summary>
    /// <param name="requiredPermission"></param>
    /// <returns></returns>
    public bool IsGranted(long requiredPermission)
    {
        return (Effective & requiredPermission) == requiredPermission; // We can accept or:ing permissions together with this construct
    }
}
