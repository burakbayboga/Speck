using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
