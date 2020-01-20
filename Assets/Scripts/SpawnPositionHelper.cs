using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionHelper
{
    private static float SpawnLimitXMax = 22.0f;
    private static float SpawnLimitXMin = -22.0f;
    private static float SpawnLimitYMax = 10.0f;
    private static float SpawnLimitYMin = -10.0f;

    public static Vector3 GetSpawnPositionForChaser()
    {
        float theta = Random.Range(0.0f, 2.0f * Mathf.PI);
        float xCoord = Mathf.Cos(theta) * 3.0f;
        float yCoord = Mathf.Sin(theta) * 3.0f;
        return new Vector3(xCoord, yCoord, 0.0f);
    }

    public static Vector3 GetSpawnPositionForLeaper()
    {
        float theta = Random.Range(0.0f, 2.0f * Mathf.PI);
        float xCoord = Mathf.Cos(theta) * 20.0f;
        float yCoord = Mathf.Sin(theta) * 10.0f;
        return new Vector3(xCoord, yCoord, 0.0f);
    }

    public static Vector3 GetSpawnPositionForElectricFence()
    {
        float theta = Random.Range(0.0f, 2.0f * Mathf.PI);
        float xCoord = Mathf.Cos(theta) * 15.0f;
        float yCoord = Mathf.Sin(theta) * 10.0f;
        return new Vector3(xCoord, yCoord, 0.0f);
    }

    public static Vector3 GetSpawnPositionForBlackHole()
    {
        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0:
                //up
                return new Vector3(0.0f, Random.Range(2.0f, 11.0f), 0.0f);
            case 1:
                //right
                return new Vector3(Random.Range(5.0f, 20.0f), 0.0f, 0.0f);
            case 2:
                //down
                return new Vector3(0.0f, Random.Range(-11.0f, -2.0f), 0.0f);
            case 3:
                //left
                return new Vector3(Random.Range(-20.0f, -5.0f), 0.0f, 0.0f);
            default:
                return Vector3.zero;
        }
    }

    public static Vector3 GetSpawnPositionForEnemy(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.StraightLine:
                return GetSpawnPositionForStraightLine();
            case EnemyType.Chaser:
                return GetSpawnPositionForChaser();
            case EnemyType.Leaper:
                return GetSpawnPositionForLeaper();
            case EnemyType.BlackHole:
                return GetSpawnPositionForBlackHole();
            case EnemyType.ElectricFence:
                return GetSpawnPositionForElectricFence();
            case EnemyType.LazerBoss:
                return Vector3.zero;
            case EnemyType.BlackHoleBoss:
                return Vector3.zero;
            case EnemyType.SwissCheeseBoss:
                return Vector3.zero;
            default:
                return Vector3.zero;
        }
    }

    public static Vector3 GetSpawnPositionForStraightLine()
    {
        int side = Random.Range(0, 4);
        float x;
        float y;

        switch (side)
        {
            case 0:
                //upper edge
                x = Random.Range(SpawnLimitXMin, SpawnLimitXMax);
                y = SpawnLimitYMax;
                return new Vector3(x, y, 0.0f);
            case 1:
                //right edge
                x = SpawnLimitXMax;
                y = Random.Range(SpawnLimitYMin, SpawnLimitYMax);
                return new Vector3(x, y, 0.0f);
            case 2:
                //lower edge
                x = Random.Range(SpawnLimitXMin, SpawnLimitXMax);
                y = SpawnLimitYMin;
                return new Vector3(x, y, 0.0f);
            case 3:
                //left edge
                x = SpawnLimitXMin;
                y = Random.Range(SpawnLimitYMin, SpawnLimitYMax);
                return new Vector3(x, y, 0.0f);
        }

        return Vector3.zero;
    }

}
