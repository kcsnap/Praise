namespace Gamey.Api.Models
{
    public enum BadgeType
    {
        Streak, // e.g., for maintaining a daily login streak (7 days, 30 days, etc.)
        Count, // e.g., for completing a specific number of tasks (total number of praises)
        Special, // e.g., for unique achievements or events (Registration, First Win, etc.)

        // Future types could include:
        //Sum, // e.g., for accumulating a certain number of points
        //Level, // e.g., for reaching a new level or rank in the game
        //Event, // e.g., for participating in special events or challenges
    }
}
