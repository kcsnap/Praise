using Praise.WebApp.Logic;

namespace Praise.Tests.Logic
{
    [TestClass]
    public sealed class PraiseTypeLogic
    {
        [TestMethod]
        public async Task WHENGetPraiseType_THENReturnDictionary()
        {
            WebApp.Logic.PraiseTypeLogic praiseType = new WebApp.Logic.PraiseTypeLogic(new PraiseApi());
            var types = await praiseType.GetPraiseTypes();
            Assert.AreEqual(3, types.Count);

            //DailyStreakLogic rewards = new DailyStreakLogic();
            //var award = rewards.IsStreakReward(1);
            //Assert.AreEqual(DailyStreakRewardNamesEnum.Day1Reward, award.Result);
        }
    }
}
