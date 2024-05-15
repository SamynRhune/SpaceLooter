using ActionCommandGame.Model;
using ActionCommandGame.RestApi.Service.Helpers;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.RestApi.Service.Extensions
{
    public static class PlayerExtensions
    {
        public static int GetLevel(this Player result)
        {
            return PlayerLevelHelper.GetLevelFromExperience(result.Experience);
        }

        public static int GetExperienceForNextLevel(this Player result)
        {
            return PlayerLevelHelper.GetExperienceForNextLevel(result.Experience);
        }

        public static int GetLevelFromExperience(this Player result)
        {
            return PlayerLevelHelper.GetLevelFromExperience(result.Experience);
        }

        public static int GetRemainingExperienceUntilNextLevel(this Player result)
        {
            return PlayerLevelHelper.GetRemainingExperienceUntilNextLevel(result.Experience);
        }
    }
}
