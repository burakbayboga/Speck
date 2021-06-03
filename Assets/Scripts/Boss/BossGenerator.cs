using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    public static BossGenerator instance;

    public GameObject[] Bosses;

    int CurrentBossIndex;
    Boss CurrentBoss;

    private void Awake()
    {
        instance = this;
    }

    public void InitiateBoss()
    {
        CurrentBossIndex = Random.Range(0, Bosses.Length);
        CurrentBoss = Instantiate(Bosses[CurrentBossIndex]).GetComponent<Boss>();
        CurrentBoss.Initiate();
    }
}
