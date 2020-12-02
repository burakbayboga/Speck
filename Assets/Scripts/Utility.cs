public class Utility
{

    public static string PrefsHighScoreKey = "HighScore";
    public static string PrefsPlayedTutorialKey = "PlayedTutorial";
    public static string PrefsColorDataKey = "ColorData";
	public static string PrefsChallengeInfoKey = "ChallengeInfo";

	public static ChallengeMode CurrentChallengeMode;
	public static int CurrentChallengeIndex;

	public static PlayerChallengeInfo ChallengeInfo;

    // takes value in (srcMin, srcMax) interval and maps it to (destMin, destMax) interval
    public static float MapToInterval(float srcMin, float srcMax, float destMin, float destMax, float value)
    {
        float mapNormalized = (value - srcMin) / (srcMax - srcMin);
        return destMax * mapNormalized + destMin * (1f - mapNormalized);
    }

	public static bool IsDouble(ChallengeMode mode)
	{
		return !((mode & ChallengeMode.Double) == 0);
	}

	public static bool IsFast(ChallengeMode mode)
	{
		return !((mode & ChallengeMode.Fast) == 0);
	}

	public static bool IsHardcore(ChallengeMode mode)
	{
		return !((mode & ChallengeMode.Hardcore) == 0);
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
