using Microsoft.AspNetCore.Http;
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
            WebApp.Logic.PraiseTypeLogic praiseType = new WebApp.Logic.PraiseTypeLogic(new PraiseApi(accessor));
            var types = await praiseType.GetPraiseTypes();
            Assert.AreEqual(3, types.Count);

            //DailyStreakLogic rewards = new DailyStreakLogic();
            //var award = rewards.IsStreakReward(1);
            //Assert.AreEqual(DailyStreakRewardNamesEnum.Day1Reward, award.Result);
        }
    }
}
