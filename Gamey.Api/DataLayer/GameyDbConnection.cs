using Azure.Identity;
using Gamey.Api.Configuration;
using Gamey.Api.DataLayer.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Container = Microsoft.Azure.Cosmos.Container;

namespace Gamey.Api.DataLayer
{
    public class GameyDbConnection : IGameyDbConnection
    {
        public Container GameContainer { get; set; }
        public Container GameEventContainer { get; set; }
        public Container AchievementContainer { get; set; }
        public Container BadgeGroupContainer { get; set; }
        public Container BadgeContainer { get; set; }
        public Container GamerContainer { get; set; }
        public Container ActionHistoryContainer { get; set; }
        public Container GamerEventDataContainer { get; set; }
        public Container PrizeGroupsContainer { get; set; }
        public Container WalletContainer { get; set; }

        protected CosmosClient client;
        protected Database database;
        protected ILogger<GameyDbConnection> _logger;

        public GameyDbConnection(IOptions<DatabaseOptions> dbOptions, ILogger<GameyDbConnection> logger)
        {
            var primaryKey = dbOptions.Value.PrimaryKey;
            var connStr = dbOptions.Value.ConnectionString;
            _logger = logger;

            DefaultAzureCredential credential = new();

            client = new(
                connectionString: connStr
            );

            database = client.GetDatabase("Gamey");
            GameContainer = database.GetContainer("game");
            GameEventContainer = database.GetContainer("GameEvents");
            AchievementContainer = database.GetContainer("Achievements");
            BadgeGroupContainer = database.GetContainer("BadgeGroups");
            BadgeContainer = database.GetContainer("Badges");
            GamerContainer = database.GetContainer("Gamers");
            ActionHistoryContainer = database.GetContainer("ActionHistory");
            GamerEventDataContainer = database.GetContainer("GamerEventData");
            PrizeGroupsContainer = database.GetContainer("PrizeGroups");
            WalletContainer = database.GetContainer("Wallet");
        }
    }
}
