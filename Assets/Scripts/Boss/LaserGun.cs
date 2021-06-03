using System.Collections;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public GameObject Laser;
    public GameObject LaserBase;
	public Animator Animator;

    public float FireTime;
	public int FireCount;

	public int TraceInterval;
	public Sprite TraceSprite;
	public Material TraceMaterial;

    private Vector3 Rotation;
    private Vector3 TargetScale = new Vector3(1.0f, 1.0f, 1.0f);
    private Collider2D LaserBaseCollider;

	private int[] TracePoolIndexes;
	private GameObject[] TraceObjects;
	private SpriteRenderer[] Renderers;
	private int ShotsFired;
	private float TraceLength = 19f;
	private int TraceCount;

	private LaserBoss Boss;

    private void Start()
    {
        Rotation = new Vector3(0.0f, 0.0f, Random.Range(0, 2) == 0 ? -30.0f : 30.0f);
        LaserBaseCollider = LaserBase.GetComponent<Collider2D>();
		GetTrace();

		LaserFireSequence();
    }

	public void AssignBoss(LaserBoss _boss)
	{
		Boss = _boss;
	}

	private void GetTrace()
	{
		TraceCount = (int)(TraceLength / TraceInterval);
		TraceObjects = TracePool.instance.GetTrace(TraceCount, out TracePoolIndexes, out Renderers);
		if (TraceObjects == null)
		{
			// TODO: not enough trace pooled
			Debug.LogError("NOT ENOUGH TRACE");
		}
	}

	private void LaserFireSequence()
	{
		StartCoroutine(DrawTrace());
        StartCoroutine(FireLaser());
        StartCoroutine(LaserCountdown());	
	}

	private void ReleaseTrace()
	{
		TracePool.instance.ReturnTrace(TracePoolIndexes);
	}

    private IEnumerator FireLaser()
    {
        float lerpParameter = 0.0f;
        float timePassed;
        float startTime = Time.time;

        while (lerpParameter < 1.0f)
        {
            timePassed = Time.time - startTime;
            lerpParameter = Mathf.Clamp01(timePassed / 1.5f);
            LaserBase.transform.localScale = TargetScale * lerpParameter;

            yield return null;
        }

		for (int i = 0; i < TraceCount; i++)
		{
			TraceObjects[i].SetActive(false);
		}

        LaserBaseCollider.enabled = true;
        Laser.SetActive(true);
		Animator.Play("lazerGun");
    }

	private IEnumerator DrawTrace()
	{
		WaitForSeconds delay = new WaitForSeconds(0.05f);
		Vector3 laserBase = LaserBase.transform.position;

		for (int i = 0; i < TraceCount; i++)
		{
			GameObject trace = TraceObjects[i];
			Vector3 forward = -transform.up;
			Vector3 traceVector = forward * TraceInterval * i;
			Vector3 tracePosition = laserBase + traceVector;

			Renderers[i].material = TraceMaterial;
			Renderers[i].sprite = TraceSprite;
			TraceObjects[i].transform.position = tracePosition;
			TraceObjects[i].SetActive(true);

			yield return delay;
		}
	}

	private IEnumerator Rotate()
	{
		float startTime = Time.time;
		while (Time.time - startTime < 2f)
		{
			transform.Rotate(Rotation * Time.deltaTime);
			yield return null;
		}
		LaserFireSequence();
	}

    private IEnumerator LaserCountdown()
    {
        yield return new WaitForSeconds(FireTime);
		ShotsFired++;
		if (ShotsFired < FireCount)
		{
			Animator.Play("lazerGunIdle");
			Laser.SetActive(false);
			StartCoroutine(Rotate());
		}
		else
		{
			Boss.OnLaserDestroyed();
			Destroy(gameObject);
		}
    }
}
