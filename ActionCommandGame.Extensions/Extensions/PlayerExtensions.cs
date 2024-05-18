using ActionCommandGame.Model;
using ActionCommandGame.Helpers;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Extensions
{
    public static class PlayerExtensions
    {
        public static int GetLevel(this PlayerResult result)
        {
            return PlayerLevelHelper.GetLevelFromExperience(result.Experience);
        }

        public static int GetExperienceForNextLevel(this PlayerResult result)
        {
            return PlayerLevelHelper.GetExperienceForNextLevel(result.Experience);
        }

        public static int GetLevelFromExperience(this PlayerResult result)
        {
            return PlayerLevelHelper.GetLevelFromExperience(result.Experience);
        }

        public static int GetRemainingExperienceUntilNextLevel(this PlayerResult result)
        {
            return PlayerLevelHelper.GetRemainingExperienceUntilNextLevel(result.Experience);
        }

        public static int GetExperienceFromCurrentLevel(this PlayerResult result)
        {
            return PlayerLevelHelper.GetExperienceFromLevel(result.GetLevel());
        }
    }
}
