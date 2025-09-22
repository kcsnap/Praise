namespace Gamey.Logic
{
    // TODO - ASSUME WE CAN DELETE THIS !
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

    // replicate this file and create tests for:
    // N per day Logic (i.e. you've sent 5 in 1 day, a new PB)
    // N per week Logic (i.e. you've sent 50 in 1 week, a new PB)
    // Received N (i.e. you've received 10 praises!)
    // Received N unique praisers (i.e. you've received praise from 10 different people)
}
