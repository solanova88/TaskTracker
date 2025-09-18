namespace TaskTracker.Infrastructure.Settings;

public class AuthSettings
{
	public static readonly string SectionName = "AuthSettings";
	public bool IsEssential { get; set; }
	public bool HttpOnly { get; set; }
	public bool Secure { get; set; }
	public int LifetimeInDays { get; set; }
	public string JwtSigningKey { get; set; }
}