using System.Collections;
using UnityEngine;

public class StraightLine : SmallFry
{
    public float SpeedMultiplier;

    bool ShouldMove = false;
    Vector3 Velocity;

	public float OuterSpawnParticleRadius0;
	public float OuterSpawnParticleRadius1;
	public float InnerSpawnParticleRadius0;
	public float InnerSpawnParticleRadius1;

	public float AngleIncrement;
	
	public Transform OuterSpawnParticle;
	public Transform InnerSpawnParticle;

	float OuterSpawnParticleCurrentRadius;
	float InnerSpawnParticleCurrentRadius;

	Coroutine[] SpinSpawnParticleCoroutines = new Coroutine[2];

    public override void Init()
    {
        base.Init();
        StartCoroutine(Action());
        GetComponent<Collider2D>().enabled = false;
    }

    void Update()
    {
        ScreenPos = MainCamera.WorldToScreenPoint(transform.position);
        if (ScreenPos.x < 0 || ScreenPos.x > Screen.width || ScreenPos.y < 0 || ScreenPos.y > Screen.height)
        {
            HandleDeath();
        }
        if (ShouldMove)
        {
            transform.Translate(Velocity * Time.deltaTime, Space.World);
        }
    }

    public void OnSpinStarted()
    {
        AudioSource.Play();
        Velocity = GetVelocity();
		Animator.speed = Mathf.Clamp(Velocity.magnitude / 10f, 1f, 1.9f);
        ShouldMove = true;

        GetComponent<Collider2D>().enabled = true;
    }

    IEnumerator Action()
    {
		SpinSpawnParticleCoroutines[0] = StartCoroutine(SpinOuterSpawnParticle());
		SpinSpawnParticleCoroutines[1] = StartCoroutine(SpinInnerSpawnParticle());
		yield return StartCoroutine(HandleSpawnParticleRadiusChanges());
		Animator.SetTrigger("SpinTrigger");
		OuterSpawnParticle.gameObject.SetActive(false);
		InnerSpawnParticle.gameObject.SetActive(false);
    }

	IEnumerator HandleSpawnParticleRadiusChanges()
	{
		yield return new WaitForSeconds(1f);
		float lerpParameter = 0f;
		float startTime = Time.time;
		while (lerpParameter < 1f)
		{
			lerpParameter = (Time.time - startTime) / 1f;
			lerpParameter = Mathf.Clamp01(lerpParameter);
			OuterSpawnParticleCurrentRadius = Mathf.Lerp(OuterSpawnParticleRadius0,
														OuterSpawnParticleRadius1,
														lerpParameter);
			InnerSpawnParticleCurrentRadius = Mathf.Lerp(InnerSpawnParticleRadius0,
														InnerSpawnParticleRadius1,
														lerpParameter);
			yield return null;
		}

		yield return new WaitForSeconds(0.5f);

		lerpParameter = 0f;
		startTime = Time.time;
		while (lerpParameter < 1f)
		{
			lerpParameter = (Time.time - startTime) / 0.15f;
			lerpParameter = Mathf.Clamp01(lerpParameter);
			OuterSpawnParticleCurrentRadius = Mathf.Lerp(OuterSpawnParticleRadius1,
														OuterSpawnParticleRadius0,
														lerpParameter);
			InnerSpawnParticleCurrentRadius = Mathf.Lerp(InnerSpawnParticleRadius1,
														OuterSpawnParticleRadius0,
														lerpParameter);
			yield return null;
		}
		yield return new WaitForSeconds(0.7f);
	}

	IEnumerator SpinInnerSpawnParticle()
	{
		InnerSpawnParticleCurrentRadius = InnerSpawnParticleRadius0;
		float theta = 3f * Mathf.PI / 2f;
		Vector3 position = Vector3.zero;
		float twoPi = Mathf.PI * 2f;
		while (true)
		{
			position.Set(Mathf.Cos(theta) * InnerSpawnParticleCurrentRadius,
						Mathf.Sin(theta) * InnerSpawnParticleCurrentRadius,
						0f);
			InnerSpawnParticle.position = transform.position + position;
			theta += 2f * AngleIncrement / InnerSpawnParticleCurrentRadius;
			theta = theta > twoPi ? theta - twoPi : theta;

			yield return null;
		}
	}

	IEnumerator SpinOuterSpawnParticle()
	{
		OuterSpawnParticleCurrentRadius = OuterSpawnParticleRadius0;
		float theta = Mathf.PI / 2f;
		Vector3 position = Vector3.zero;
		float twoPi = Mathf.PI * 2f;
		while (true)
		{
			position.Set(Mathf.Cos(theta) * OuterSpawnParticleCurrentRadius,
						Mathf.Sin(theta) * OuterSpawnParticleCurrentRadius,
						0f);
			OuterSpawnParticle.position = transform.position + position;
			theta += 2f * AngleIncrement / OuterSpawnParticleCurrentRadius;
			theta = theta > twoPi ? theta - twoPi : theta;

			yield return null;
		}
	}

    Vector3 GetVelocity()
    {
        return (LilBTransform.position - transform.position) * SpeedMultiplier;
    }
}
