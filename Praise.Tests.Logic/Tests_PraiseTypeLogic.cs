using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Praise.Models.Configuration;
using Praise.WebApp.Logic;

namespace Praise.Tests.Logic
{
    [TestClass]
    public sealed class PraiseTypeLogic
    {
        [TestMethod]
        public async Task WHENGetPraiseType_THENReturnDictionary()
        {
            HttpContextAccessor accessor = new HttpContextAccessor();
            var praiseApiOptions = Options.Create(new PraiseApiOptions());
            var authorisationOptions = Options.Create(new AuthorisationOptions());
            WebApp.Logic.PraiseTypeLogic praiseType = new WebApp.Logic.PraiseTypeLogic(new PraiseApi(authorisationOptions, praiseApiOptions, accessor));
            var types = await praiseType.GetPraiseTypes();
            Assert.AreEqual(3, types.Count);

            //DailyStreakLogic rewards = new DailyStreakLogic();
            //var award = rewards.IsStreakReward(1);
            //Assert.AreEqual(DailyStreakRewardNamesEnum.Day1Reward, award.Result);
        }
    }
}
