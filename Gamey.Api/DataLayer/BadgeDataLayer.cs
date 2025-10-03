using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Models;
using Gamey.Models.Converters;
using Microsoft.Azure.Cosmos;
using System;
using System.Text.Json;

namespace Gamey.Api.DataLayer
{
    public class BadgeDataLayer(ILogger<BadgeDataLayer> _logger, IGameyDbConnection _gameyDbConnection) : IBadgeDataLayer
    {
        public async Task<IEnumerable<BaseBadgeDetails>> Read(Guid badgeGroupId, BadgeType badgeType)
        {
            var response = new List<BaseBadgeDetails>();

            try
            {
                var query = "SELECT * FROM Badges g WHERE g.BadgeGroupId = @badgeGroupId";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query).WithParameter("@badgeGroupId", badgeGroupId.ToString());

                // Query multiple items from container as raw JSON
                using var filteredFeed = _gameyDbConnection.BadgeContainer.GetItemQueryIterator<dynamic>(
                    queryDefinition: parameterizedQuery
                );

                // The problem is that Cosmos DB SDK cannot directly deserialize into an abstract
                // type(BaseBadgeDetails), even with a System.Text.Json converter.The SDK uses its
                // internal JSON serializer, which doesn’t respect the custom JsonConverter
                // automatically when deserializing abstract types.
                var serializerOptions = new JsonSerializerOptions
                {
                    Converters = { new BadgeDetailsConverter() },
                    PropertyNameCaseInsensitive = true
                };

                while (filteredFeed.HasMoreResults)
                {
                    var dbBadges = await filteredFeed.ReadNextAsync();

                    foreach (var itemJson in dbBadges)
                    {
                        // Deserialize manually using the custom converter
                        var convertedJson = Convert.ToString(itemJson);
                        var badge = JsonSerializer.Deserialize<BaseBadgeDetails>(convertedJson, serializerOptions);

                        if (badge != null)
                        {
                            response.Add(badge);
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                var error = $"Error in BadgeDataLayer.Read (badgeGroupId = {badgeGroupId})";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }
    }
}
