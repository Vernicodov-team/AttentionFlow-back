namespace API.Modules;

public class UserSeedOptions
{
    public const string SectionName = "SeedUsers";

    public List<UserSeedEntry> Users { get; set; } = new List<UserSeedEntry>();
}

public class UserSeedEntry
{
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
