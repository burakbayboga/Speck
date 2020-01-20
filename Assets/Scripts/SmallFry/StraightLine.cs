using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLine : SmallFry
{

    public GameObject EdgeParticleInitial;
    public GameObject EdgeParticleConstantCircle;

    public override void Init()
    {
        base.Init();
        EnemyType = EnemyType.StraightLine;
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
        if (!IsFrozen)
        {
            Rigidbody.velocity = OriginalVelocity * SpeedMultiplier;
            Rigidbody.angularVelocity = OriginalAngularVelocity * SpeedMultiplier;
        }
    }

    IEnumerator Action()
    {
        yield return new WaitForSeconds(1.0f);
        //OriginalAngularVelocity = Random.Range(0, 2) == 0 ? 2048.0f : -2048.0f;
        float lerpParameter = 0.0f;
        float startTime = Time.time;
        while (lerpParameter < 1.0f)
        {
            lerpParameter = (Time.time - startTime) / 2.0f;
            OriginalAngularVelocity = Mathf.Lerp(0.0f, 1440.0f, lerpParameter * lerpParameter);
            yield return null;
        }

        AudioSource.Play();

        //yield return new WaitForSeconds(2.0f);

        OriginalVelocity = GetVelocity();
        GetComponent<Collider2D>().enabled = true;

        EdgeParticleInitial.SetActive(false);
        EdgeParticleConstantCircle.SetActive(true);
    }

    Vector3 GetVelocity()
    {
        return LilBTransform.position - transform.position;
    }

    public override void ApplyFreeze(float effectTime)
    {
        if (IsFrozen)
        {
            return;
        }
        IsFrozen = true;
        StartCoroutine(FreezeCoroutine());
        StartCoroutine(Timer(effectTime, RevertFreeze));
    }

    public override void RevertFreeze()
    {
        StartCoroutine(RevertFreezeCoroutine());
    }

    IEnumerator FreezeCoroutine()
    {
        float startTime = Time.time;
        float timePassed;
        float lerpParameter;

        EdgeParticleConstantCircle.SetActive(false);
        EdgeParticleInitial.SetActive(true);

        while (true)
        {
            timePassed = Time.time - startTime;
            lerpParameter = timePassed * 2.0f;
            Rigidbody.velocity = Vector2.Lerp(OriginalVelocity, Vector2.zero, lerpParameter);
            Rigidbody.angularVelocity = Mathf.Lerp(OriginalAngularVelocity, 0.0f, lerpParameter);
            if (lerpParameter >= 1.0f)
            {
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator RevertFreezeCoroutine()
    {
        float startTime = Time.time;
        float timePassed;
        float lerpParameter;

        while (true)
        {
            timePassed = Time.time - startTime;
            lerpParameter = timePassed/* * 10.0f*/;
            Rigidbody.angularVelocity = Mathf.Lerp(0.0f, OriginalAngularVelocity, lerpParameter);
            if (lerpParameter >= 1.0f)
            {
                IsFrozen = false;

                EdgeParticleInitial.SetActive(false);
                EdgeParticleConstantCircle.SetActive(true);

                yield break;
            }
            yield return null;
        }
    }
}
