public class Utility
{

    public static float ScreenFractionOffset = 10.0f;
    public static string PrefsChallengeLevelKey = "PlayerChallengeLevel";
    public static string PrefsCurrentChallengeLevelKey = "PlayerCurrentChallengeLevel";
    public static string PrefsHighScoreKey = "HighScore";
    public static string PrefsPlayedTutorialKey = "PlayedTutorial";
    public static string PrefsColorDataKey = "ColorData";
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
