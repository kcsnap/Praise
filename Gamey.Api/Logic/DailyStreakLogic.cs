namespace Gamey.Logic
{
    public class DailyStreakLogic
    {
        public async Task<DailyStreakRewardNamesEnum> IsStreakReward(int streakNumber)
        {

            // TODO change to switch statement for multiple
            if (streakNumber < 0)
            {
                throw new Exception("Negative values not allowed!");
            }
            else if (streakNumber == 1) 
            { 
                return DailyStreakRewardNamesEnum.Day1Reward; 
            }
            else if (streakNumber == 2)
            {
                return DailyStreakRewardNamesEnum.Day2Reward;
            }

            else if (streakNumber == 3)
            {
                return DailyStreakRewardNamesEnum.Day3Reward;
            }

            else if (streakNumber == 5)
            {
                return DailyStreakRewardNamesEnum.Day5Reward;
            }

            else if (streakNumber == 10)
            {
                return DailyStreakRewardNamesEnum.Day10Reward;
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
