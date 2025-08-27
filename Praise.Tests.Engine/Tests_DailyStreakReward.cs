using Gamey.Logic;
using Microsoft.AspNetCore.Http.Connections;

namespace Praise.Tests.Engine
{
    [TestClass]
    public sealed class Tests_DailyStreakReward
    {
        [TestMethod]
        public void WHENStreakNumberIs1_THENReturnDay1Reward()
        {
            DailyStreakLogic rewards = new DailyStreakLogic();
            var award = rewards.IsStreakReward(1);
            Assert.AreEqual(DailyStreakRewardNamesEnum.Day1Reward, award.Result);
        }

        [TestMethod]
        public void WHENStreakNumberIs2_THENReturnDay2Reward()
        {
            DailyStreakLogic rewards = new DailyStreakLogic();
            var award = rewards.IsStreakReward(2);
            Assert.AreEqual(DailyStreakRewardNamesEnum.Day2Reward, award.Result);
        }
       
        [TestMethod]
        public void WHENStreakNumberIs3_THENReturnDay3Reward()
        {
            DailyStreakLogic rewards = new DailyStreakLogic();
            var award = rewards.IsStreakReward(3);
            Assert.AreEqual(DailyStreakRewardNamesEnum.Day3Reward, award.Result);
        }

        [TestMethod]
        public void WHENStreakNumberIs5_THENReturnDay5Reward()
        {
            DailyStreakLogic rewards = new DailyStreakLogic();
            var award = rewards.IsStreakReward(5);
            Assert.AreEqual(DailyStreakRewardNamesEnum.Day5Reward, award.Result);
        }

        // TODO - add more tests for other streak numbers

        [TestMethod]
        public void WHENStreakNumberIsNotAReward_THENReturnNoReward()
        {
            DailyStreakLogic rewards = new DailyStreakLogic();
            var award = rewards.IsStreakReward(4);
            Assert.AreEqual(DailyStreakRewardNamesEnum.NoReward, award.Result);
        }

        [TestMethod]
        public async Task WHENStreakNumberIsNegative_THENThrowError()
        {
            try
            {
                DailyStreakLogic rewards = new DailyStreakLogic();
                var award = await rewards.IsStreakReward(-3);
                Assert.Fail("Should Not Reach This Point :( ");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Negative values not allowed!", ex.Message);
            }
            
        }

        [TestMethod]
        public void WHENStreakNumberIs10_THENReturnDay10Reward()
        {
            DailyStreakLogic rewards = new DailyStreakLogic();
            var award = rewards.IsStreakReward(10);
            Assert.AreEqual(DailyStreakRewardNamesEnum.Day10Reward, award.Result);
        }

        // TODO - any other tests you can think of?
    }
}
