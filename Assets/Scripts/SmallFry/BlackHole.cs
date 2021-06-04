using System.Collections;
using UnityEngine;

public class BlackHole : SmallFry
{
    public float SingularityForceMultiplier;
    public float LifeTime;

    public GameObject PreSpawnParticle;
    public GameObject PreSpawnParticleScatter;

	[HideInInspector]
	public float WaveRadiusStart;
	public float WaveThickness;

	[HideInInspector]
	public Vector2 WaveRadiusData;

    private Rigidbody2D LilBRigidBody;
    private CircleCollider2D Collider;

    bool Active;
	bool CanPull = true;
	WaitForSeconds cooldown = new WaitForSeconds(0.1f);

	BlackHoleScreenEffect ScreenEffect;

    public override void Init()
    {
        base.Init();
		ScreenEffect = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BlackHoleScreenEffect>();
        LilBRigidBody = LilBTransform.gameObject.GetComponent<Rigidbody2D>();
        Collider = GetComponent<CircleCollider2D>();
        StartCoroutine(PreSpawnCoroutine());
    }

    private IEnumerator PreSpawnCoroutine()
    {
        Active = false;
        Collider.enabled = false;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
		Animator.enabled = false;
        GameObject particle = Instantiate(PreSpawnParticleScatter, transform);
        yield return new WaitForSeconds(1.5f);
        Destroy(particle);
        particle = Instantiate(PreSpawnParticle, transform);
        yield return new WaitForSeconds(2.0f);
        Destroy(particle);
        Active = true;
        renderer.enabled = true;
        Collider.enabled = true;
		Animator.enabled = true;
        StartCoroutine(DeathCountdown(LifeTime));

		//Background.instance.StartBlackHoleWave(transform.position, LifeTime, this);
		ScreenEffect.StartBlackHoleEffect(transform.position, LifeTime, this);
    }

    private IEnumerator DeathCountdown(float delay)
    {
        yield return new WaitForSeconds(delay);
        HandleDeath();
    }

	public override void HandleDeath()
	{
		LilB speck = LilBTransform.GetComponent<LilB>();
		if (speck.IsEndless)
		{
			SmallFryManager.instance.CurrentBlackHoleCount--;
		}
		base.HandleDeath();
	}

    private void Update()
    {
        if (Active && CanPull)
        {
            //ApplySingularity();
            ApplyWaveSingularity();
        }
    }

	IEnumerator Cooldown()
	{
		yield return cooldown;
		CanPull = true;
	}

    void ApplySingularity()
    {
		Vector2 forceBase = transform.position - LilBTransform.position;
		float forceBaseMagnitude = forceBase.magnitude;
		Vector3 forceDir = forceBase / forceBaseMagnitude;
        Vector2 force = forceDir * SingularityForceMultiplier / forceBaseMagnitude * Time.deltaTime;

        LilBRigidBody.AddForce(force);
    }

	void ApplyWaveSingularity()
	{
		Vector2 forceBase = transform.position - LilBTransform.position;
		float speckRadius = forceBase.magnitude;
		if (speckRadius > WaveRadiusData.x && speckRadius < WaveRadiusData.y)
		{
			Vector2 force = forceBase / speckRadius * SingularityForceMultiplier;
			LilBRigidBody.AddForce(force);

			CanPull = false;
			StartCoroutine(Cooldown());
		}
	}

    //void ApplyWaveSingularity()
    //{
        //float sineValue = Mathf.Clamp01(CalculateSine());
        //LilBRigidBody.AddForce((transform.position - LilBTransform.position).normalized * sineValue * SingularityForceMultiplier * Time.deltaTime);
    //}
//
    //float CalculateSine()
    //{
        //Vector3 radiusVector = LilBTransform.position - transform.position;
        //radiusVector = new Vector3(radiusVector.x / 50f, radiusVector.y / 28f, 0f);
        //float radius = radiusVector.magnitude;
//
//
        //return Mathf.Sin(radius * 4f * Mathf.PI / LenghtMapped(radius) - Time.time * 8f) + OffsetMapped(radius);
    //}
//
    //float LenghtMapped(float radius)
    //{
        //float mapNormalized = radius / 1.4f;
        //return 0.51f * (1f - mapNormalized);
    //}
//
    //float OffsetMapped(float radius)
    //{
        //float mapNormalized = radius / 1.4f;
        //return -1.33f * mapNormalized;
    //}



}
