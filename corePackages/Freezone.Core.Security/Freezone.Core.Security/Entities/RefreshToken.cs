using Freezone.Core.Persistence.Repositories;

namespace Freezone.Core.Security.Entities;

public class RefreshToken: Entity
{
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresDate { get; set; }
    public DateTime? RevokedDate { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? RevokedReason { get; set; }
    
    public string CreatedByIp { get; set; }
    public string? RevokedByIp { get; set; }
    
    public virtual User User { get; set; }
}