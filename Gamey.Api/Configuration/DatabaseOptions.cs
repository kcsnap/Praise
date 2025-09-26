namespace Gamey.Api.Configuration
{
    public class DatabaseOptions
    {
        public static string ConfigNode => "Database";
        public string PrimaryKey { get; set; }
        public string ConnectionString { get; set; }
    }
}
