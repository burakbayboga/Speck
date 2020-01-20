using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : SmallFry
{

    public float SingularityForceMultiplier;
    public float LifeTime;

    public GameObject PreSpawnParticle;
    public GameObject PreSpawnParticleScatter;

    private Rigidbody2D LilBRigidBody;
    private CircleCollider2D Collider;

    bool Active;

    public override void Init()
    {
        base.Init();
        EnemyType = EnemyType.BlackHole;
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
        GameObject particle = Instantiate(PreSpawnParticleScatter, transform);
        yield return new WaitForSeconds(1.5f);
        Destroy(particle);
        particle = Instantiate(PreSpawnParticle, transform);
        yield return new WaitForSeconds(2.0f);
        Destroy(particle);
        Active = true;
        renderer.enabled = true;
        Collider.enabled = true;
        AudioSource.Play();
        StartCoroutine(DeathCountdown(LifeTime));
    }

    private IEnumerator DeathCountdown(float delay)
    {
        yield return new WaitForSeconds(delay);
        HandleDeath();
    }

    private void Update()
    {
        if (Active)
        {
            ApplySingularity();
        }
    }

    void ApplySingularity()
    {
        Vector2 forceBase = new Vector2(transform.position.x - LilBTransform.position.x, transform.position.y - LilBTransform.position.y);
        Vector2 force = (forceBase.normalized * SingularityForceMultiplier) / (forceBase.magnitude) * Time.deltaTime;

        LilBRigidBody.AddForce(force);
    }

}
