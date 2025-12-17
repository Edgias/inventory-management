namespace EdgIQ.Inventory.Infrastructure.Identity;

public class RefreshToken
{
    public int Id { get; private set; }
    public string Token { get; private set; } 
    public string UserId { get; private set; } 
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }

    public RefreshToken(string token, string userId, DateTime expiresAt)
    {
        Token = token;
        UserId = userId;
        ExpiresAt = expiresAt;
    }

    public void Revoke()
    {
        IsRevoked = true;
    }
}
