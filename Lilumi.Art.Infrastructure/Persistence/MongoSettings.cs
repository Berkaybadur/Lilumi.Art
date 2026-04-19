namespace Lilumi.Art.Infrastructure.Persistence;

public class MongoSettings
{
    public const string SectionName = "MongoSettings";
    public string ConnectionString { get; set; } = "mongodb://mongo:27017";
    public string DatabaseName { get; set; } = "LilumiArtDb";
}
