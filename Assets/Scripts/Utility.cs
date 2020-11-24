public class Utility
{

    public static float ScreenFractionOffset = 10.0f;
    public static string PrefsChallengeLevelKey = "PlayerChallengeLevel";
    public static string PrefsCurrentChallengeLevelKey = "PlayerCurrentChallengeLevel";
    public static string PrefsHighScoreKey = "HighScore";
    public static string PrefsPlayedTutorialKey = "PlayedTutorial";
    public static string PrefsColorDataKey = "ColorData";


	public static ChallengeMode CurrentChallengeMode;


    // takes value in (srcMin, srcMax) interval and maps it to (destMin, destMax) interval
    public static float MapToInterval(float srcMin, float srcMax, float destMin, float destMax, float value)
    {
        float mapNormalized = (value - srcMin) / (srcMax - srcMin);
        return destMax * mapNormalized + destMin * (1f - mapNormalized);
    }

}

public enum EnemyType
{
    StraightLine = 0,
    Chaser = 1,
    Leaper = 2,
    BlackHole = 3,
    ElectricFence = 4,
    LazerBoss = 5,
    BlackHoleBoss = 6,
    SwissCheeseBoss = 7
}

public enum FryType
{
    SmallFry = 0,
    Boss = 1
}
