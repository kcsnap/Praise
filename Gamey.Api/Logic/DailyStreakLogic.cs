namespace Gamey.Logic
{
    public class DailyStreakLogic
    {
        public async Task<DailyStreakRewardNamesEnum> IsStreakReward(int streakNumber)
        {
            switch(streakNumber)
            {
                case < 0: throw new Exception("Negative values not allowed!");
                case 1: return DailyStreakRewardNamesEnum.Day1Reward;
                case 2: return DailyStreakRewardNamesEnum.Day2Reward;
                case 3: return DailyStreakRewardNamesEnum.Day3Reward;
                case 5: return DailyStreakRewardNamesEnum.Day5Reward;
                case 10:return DailyStreakRewardNamesEnum.Day10Reward;
            }

            // if streaknumber not matched then return no reward
            return DailyStreakRewardNamesEnum.NoReward;
        }
    }

    public enum DailyStreakRewardNamesEnum
    {
        NoReward,
        Day1Reward,
        Day2Reward,
        Day3Reward,
        Day5Reward,
        Day10Reward
        // TODO create more values and related tests 
    }
}
