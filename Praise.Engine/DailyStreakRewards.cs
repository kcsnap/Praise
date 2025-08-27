public class DailyStreakRewards
{
    public async Task<DailyStreakRewardNamesEnum> IsStreakReward(int streakNumber)
    {
        if(streakNumber == 1) { return DailyStreakRewardNamesEnum.Day1Reward; }
        // TODO change to switch statement for multiple

        return DailyStreakRewardNamesEnum.NoReward;
    }
}

public enum DailyStreakRewardNamesEnum
{
    NoReward,
    Day1Reward,
    Day2Reward,
    Day3Reward,
    Day5Reward
    // TODO create more values and related tests 
}