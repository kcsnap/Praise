using Gamey.Api.Models;
using Gamey.Api.Models.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Praise.Api.Configuration;
using Praise.Api.Logic;
using Praise.Api.Logic.Interfaces;
using Praise.Models;

namespace Tests.Praise.Api;

[TestClass]
public class TestGameWrapperLogic
{
    protected IOptions<GameApiOptions> options;
    protected Mock<ILogger<GameWrapperLogic>> logger;
    protected Mock<IHttpMethodWrapper> httpWrapper;
    protected Mock<IPraiseUserLogic> praiseUserLogic;
    protected GameWrapperLogic gameWrapperLogic;
    protected PraiseUser mockUser;

    [TestInitialize]
    public async Task TestInitialize()
    {
        options = Options.Create(new GameApiOptions());
        logger = new Mock<ILogger<GameWrapperLogic>>();
        httpWrapper = new Mock<IHttpMethodWrapper>();
        praiseUserLogic = new Mock<IPraiseUserLogic>();
        mockUser = new PraiseUser
        {
            UserId = Guid.NewGuid().ToString(),
            Email = "Unit@TestUser.com",
            DisplayName = "Unit Test User"
        };
        praiseUserLogic.Setup(p => p.PraiseUser).Returns(mockUser);
        gameWrapperLogic = new GameWrapperLogic(options, logger.Object, httpWrapper.Object);
    }

    [TestMethod]
    public async Task GIVENNewUser_WHENRegister_THENCallGamerUrl()
    {   
        // ACT
        await gameWrapperLogic.RegisterGamer(praiseUserLogic.Object);

        // ASSERT
        httpWrapper.Verify(w => w.PostAsync<NewGamer, Gamer>(praiseUserLogic.Object, It.IsAny<string>(), It.IsAny<NewGamer>()), Times.Once);
        httpWrapper.Verify(w => w.PostAsync<NewGameEvent, ResponseGameBadges>(praiseUserLogic.Object, It.IsAny<string>(), It.IsAny<NewGameEvent>()), Times.Exactly(2));
    }

    [TestMethod]
    public async Task GIVENExistingUser_WHENRegister_THENCallGamerUrl()
    {
        httpWrapper.Setup(w => w.GetAsync<Gamer>(praiseUserLogic.Object, "gamers/Unit@TestUser.com"))
            .ReturnsAsync(new Gamer { GamerName = "Unit Test User" });

        // ACT
        await gameWrapperLogic.RegisterGamer(praiseUserLogic.Object);

        // ASSERT
        httpWrapper.Verify(w => w.PostAsync<NewGamer, Gamer>(praiseUserLogic.Object, It.IsAny<string>(), It.IsAny<NewGamer>()), Times.Never);
        httpWrapper.Verify(w => w.PostAsync<NewGameEvent, ResponseGameBadges>(praiseUserLogic.Object, It.IsAny<string>(), It.IsAny<NewGameEvent>()), Times.Exactly(2));
    }

    [TestMethod]
    public async Task GIVENUser_WHENPraiseEvent_THENCallGameEventUrl()
    {
        httpWrapper.Setup(w => w.GetAsync<Gamer>(praiseUserLogic.Object, "gamers/Unit@TestUser.com"))
            .ReturnsAsync(new Gamer { GamerName = "Unit Test User" });

        // ACT
        await gameWrapperLogic.SendPraiseEvent(praiseUserLogic.Object);

        // ASSERT
        
        httpWrapper.Verify(w => w.PostAsync<NewGameEvent, ResponseGameBadges>(praiseUserLogic.Object, GameWrapperLogic._gameEventUrl, It.IsAny<NewGameEvent>()), Times.Exactly(1));
    }
}
