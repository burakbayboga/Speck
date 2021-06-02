using System.Collections;
using UnityEngine;

public class Chaser : SmallFry
{
    public float Speed;
    public float LifeTime;

	public float InitialSpeedBoost;
	public float InitialSpeedBoostEaseTime;

    private Renderer Renderer;

    private float OriginalSpeed;
    private bool IsSpawnOver;
    private Collider2D Collider;
	private float BoostMultiplier;

	private bool DeadMovement;
	private bool IsVelocityLocked;
	private Vector3 LockedVelocity;

    public override void Init()
    {
        base.Init();
        Renderer = GetComponent<Renderer>();
        OriginalSpeed = Speed;
        IsSpawnOver = false;
        Collider = GetComponent<Collider2D>();
    }

    void Update()
    {
		RotateToSpeck();;
        if (!IsSpawnOver && !ShouldChase())
        {
            return;
        }

		Vector2 velocity;
		if (!DeadMovement)
		{
			velocity = (LilBTransform.position - transform.position).normalized * Speed * BoostMultiplier;
		}
		else
		{
			if (!IsVelocityLocked)
			{
				LockedVelocity = (LilBTransform.position - transform.position).normalized * Speed * BoostMultiplier / 1.5f;
				IsVelocityLocked = true;
			}
			velocity = LockedVelocity;
		}

        Rigidbody.velocity = velocity;
    }

	void RotateToSpeck()
	{
        float zRotation = Vector3.SignedAngle(LilBTransform.position - transform.position, Vector3.up, Vector3.forward * -1.0f);
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, zRotation));
	}

	IEnumerator EaseInitialSpeedBoost()
	{
		float startTime = Time.time;
		float t = 0f;
		while (t < 1f)
		{
			t = Mathf.Clamp01((Time.time - startTime) / InitialSpeedBoostEaseTime);
			BoostMultiplier = Mathf.Lerp(InitialSpeedBoost, 1f, t);
			
			yield return null;
		}
	}

    bool ShouldChase()
    {
        bool spawnOver = Animator.GetCurrentAnimatorStateInfo(0).IsName("chaser_moving");
        if (spawnOver != IsSpawnOver)
        {
            IsSpawnOver = spawnOver;
            StartCoroutine(DeathCountdown(LifeTime));
            Collider.enabled = true;
			StartCoroutine(EaseInitialSpeedBoost());
        }
        return spawnOver;
    }

    IEnumerator DeathCountdown(float delay)
    {
        yield return new WaitForSeconds(delay);
		Animator.Play("chaser_death");
		Collider.enabled = false;
		DeadMovement = true;
		StartCoroutine(CheckDeathAnimation());
    }

	IEnumerator CheckDeathAnimation()
	{
		while (true)
		{
			if (Animator.GetCurrentAnimatorStateInfo(0).IsName("death"))
			{
				HandleDeath();
			}

			yield return null;
		}
	}
}
