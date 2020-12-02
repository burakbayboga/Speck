using System.Collections.Generic;

public class ChallengeData
{
    public List<ChallengeLevel> Levels;
}

public class ChallengeWave
{
    public float Time;
    public List<ChallengeWaveEnemy> Enemies;
}

public class ChallengeLevel
{
    public int Level;
    public List<ChallengeWave> Waves;
}

public class ChallengeWaveEnemy
{
    public FryType FryType;
    public EnemyType EnemyType;
}

public class PlayerChallengeInfo
{
	public List<PlayerChallengeLevelInfo> ChallengeLevelInfoList;

	public PlayerChallengeInfo()
	{
		ChallengeLevelInfoList = new List<PlayerChallengeLevelInfo>();
	}
}

public class PlayerChallengeLevelInfo
{
	public int Level;
	public ChallengeMode Modes;
}
