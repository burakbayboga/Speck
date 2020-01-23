using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : SmallFry
{
    public float Speed;
    public float TargetAlpha;
    public float LifeTime;

    public Color FarColor;
    public Color NearColor;

    private Renderer Renderer;

    private float OriginalSpeed;
    private bool IsSpawnOver;
    private Animator Animator;
    private Collider2D Collider;

    public override void Init()
    {
        base.Init();
        EnemyType = EnemyType.Chaser;
        Renderer = GetComponent<Renderer>();
        OriginalSpeed = Speed;
        IsSpawnOver = false;
        Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider2D>();
    }

    void Update()
    {

        if (!IsSpawnOver && !ShouldChase())
        {
            return;
        }

        float distance = (transform.position - LilBTransform.position).magnitude;
        float lerpParameter = Mathf.InverseLerp(0.0f, 30.0f, distance);

        Color outlineColor = Color.Lerp(NearColor, FarColor, lerpParameter);
        Renderer.material.SetColor("_OutlineColor", outlineColor);

        Rigidbody.velocity = (LilBTransform.position - transform.position).normalized * Speed * SpeedMultiplier;
        float zRotation = Vector3.SignedAngle(LilBTransform.position - transform.position, Vector3.up, Vector3.forward * -1.0f);
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, zRotation));

    }

    bool ShouldChase()
    {
        bool spawnOver = Animator.GetCurrentAnimatorStateInfo(0).IsName("chaser_moving");
        if (spawnOver != IsSpawnOver)
        {
            IsSpawnOver = spawnOver;
            StartCoroutine(DeathCountdown(LifeTime));
            Collider.enabled = true;
            AudioSource.Play();
        }
        return spawnOver;
    }

    IEnumerator DeathCountdown(float delay)
    {
        yield return new WaitForSeconds(delay);
        HandleDeath();
    }

    public override void ApplyFreeze(float effectTime)
    {
        if (IsFrozen)
        {
            return;
        }
        IsFrozen = true;
        StartCoroutine(FreezeCoroutine());
        Timer(effectTime, RevertFreeze);
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
        while (true)
        {
            timePassed = Time.time - startTime;
            lerpParameter = timePassed * 2.0f;
            Speed = Mathf.Lerp(OriginalSpeed, 0.0f, lerpParameter);
            if (lerpParameter >= 1.0f)
            {
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator RevertFreezeCoroutine()
    {
        float starTime = Time.time;
        float timePassed;
        float lerpParameter;
        while (true)
        {
            timePassed = Time.time - starTime;
            lerpParameter = timePassed * 10.0f;
            Speed = Mathf.Lerp(0.0f, OriginalSpeed, lerpParameter);
            if (lerpParameter >= 1.0f)
            {
                IsFrozen = false;
                yield break;
            }
            yield return null;
        }
    }
}
