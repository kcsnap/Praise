public class DailyStreakRewards
{
    public async Task<DailyStreakRewardNamesEnum> IsStreakReward(int streakNumber)
    {
        // TODO - check if streakNumber is negative and throw error

        // TODO change to switch statement for multiple
        if (streakNumber == 1) { return DailyStreakRewardNamesEnum.Day1Reward; }
        
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
    Day5Reward
    // TODO create more values and related tests 
}

