using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTypeBlackHole : MonoBehaviour
{

    public float ActiveLifeTime;
    public float SingularityForceMultiplier;
    public GameObject PreSpawnParticle;
    public GameObject PreSpawnParticleScatter;

    private Rigidbody2D LilBRigidBody;
    private bool BlackHoleActive;
    private Transform LilBTransform;
    private CircleCollider2D Collider;

    private void Awake()
    {
        LilBRigidBody = FindObjectOfType<LilB>().GetComponent<Rigidbody2D>();
        LilBTransform = LilBRigidBody.transform;
        BlackHoleActive = false;
        Collider = GetComponent<CircleCollider2D>();
        StartCoroutine(PreSpawnCoroutine());
    }

    private void Update()
    {
        if (!BlackHoleActive)
        {
            return;
        }

        Vector2 forceBase = new Vector2(transform.position.x - LilBTransform.position.x, transform.position.y - LilBTransform.position.y);
        Vector2 force = (forceBase.normalized * SingularityForceMultiplier) / (forceBase.magnitude) * Time.deltaTime;

        LilBRigidBody.AddForce(force);

    }

    private IEnumerator PreSpawnCoroutine()
    {
        BlackHoleActive = false;
        Collider.enabled = false;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        GameObject particle = Instantiate(PreSpawnParticleScatter, transform);
        yield return new WaitForSeconds(1.5f);
        Destroy(particle);
        particle = Instantiate(PreSpawnParticle, transform);
        yield return new WaitForSeconds(2.0f);
        Destroy(particle);
        BlackHoleActive = true;
        renderer.enabled = true;
        Collider.enabled = true;
        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(ActiveLifeTime);
        Destroy(gameObject);
    }

}
