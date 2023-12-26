namespace VidiView.Api.DataModel;

public class ObjectAccess
{
    /// <summary>
    /// The unique id of the user or group this ACL applies to
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Department id (if this instance applies to a department)
    /// </summary>
    public Guid? DepartmentId { get; set; }

    /// <summary>
    /// Name of entity this instance applies to
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The granted flags
    /// </summary>
    public long Granted { get; set; }

    /// <summary>
    /// The explicitly denied flags
    /// </summary>
    public long Denied { get; set; }

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
