using Microsoft.Extensions.Configuration;
using Praise.Api.Logic;
using Praise.Api.Logic.Events;

namespace Tests.Praise.Api;

[TestClass]
public class TestEmailManagerLogic
{
    private readonly IConfiguration _config;    
    public string ApiKey => _config["Email:ApiKey"] ?? "";


    [TestMethod]
    public async Task GIVENIncorrectKey_WHENSendEmail_THENException()
    {
        var initialData = new Dictionary<string, string?>
        {
            ["Email:ApiKey"] = "abc123"
        };

        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(initialData)
            .Build();

        IEmailManagerLogic emailManager = new EmailManagerLogic(config);

        await Assert.ThrowsExceptionAsync<Exception>(async () => await emailManager.SendEmail("kenneth.cox@snapwire.co.uk", "Praise - Unit Test Subject", "Praise - Unit Test Body"));

        // ######################################################
        // TODO - CAN WE AUTOMATE THE CHECK OF SUCCESSFUL SEND?
        // ######################################################
    }
}
