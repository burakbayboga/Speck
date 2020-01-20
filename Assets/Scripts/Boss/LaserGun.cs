using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{

    public GameObject Laser;
    public GameObject LaserBase;
    public LaserBoss Boss;

    public float LifeTime;

    private Vector3 Rotation;
    private Vector3 TargetScale = new Vector3(1.0f, 1.0f, 1.0f);
    private Collider2D LaserBaseCollider;

    public void Init(LaserBoss _boss)
    {
        Boss = _boss;
    }

    private void Start()
    {
        Rotation = new Vector3(0.0f, 0.0f, Random.Range(0, 2) == 0 ? -30.0f : 30.0f);
        LaserBaseCollider = LaserBase.GetComponent<Collider2D>();
        StartCoroutine(FireLaser());
        StartCoroutine(DeathCountdown());
    }

    private void Update()
    {
        transform.Rotate(Rotation * Time.deltaTime);
    }

    private IEnumerator FireLaser()
    {
        float lerpParameter = 0.0f;
        float timePassed;
        float startTime = Time.time;



        while (lerpParameter < 1.0f)
        {
            timePassed = Time.time - startTime;
            lerpParameter = Mathf.Clamp(timePassed / 3.0f, 0.0f, 1.0f);
            LaserBase.transform.localScale = TargetScale * lerpParameter;

            yield return null;
        }

        LaserBaseCollider.enabled = true;

        Laser.SetActive(true);

        Boss.TriggerFireSound();
    }

    private IEnumerator DeathCountdown()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }
}
