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
    private Collider2D Collider;

    public override void Init()
    {
        base.Init();
        EnemyType = EnemyType.Chaser;
        Renderer = GetComponent<Renderer>();
        OriginalSpeed = Speed;
        IsSpawnOver = false;
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

        Rigidbody.velocity = (LilBTransform.position - transform.position).normalized * Speed;
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
}
