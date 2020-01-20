using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeShowcase : MonoBehaviour
{

    public GameObject PreSpawnParticle;
    public GameObject PreSpawnParticleScatter;

    SpriteRenderer Renderer;

    private void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(Showcase());
    }

    IEnumerator Showcase()
    {
        Renderer.enabled = false;
        PreSpawnParticleScatter.SetActive(true);
        yield return new WaitForSeconds(2f);
        PreSpawnParticleScatter.SetActive(false);
        PreSpawnParticle.SetActive(true);
        yield return new WaitForSeconds(2f);
        PreSpawnParticle.SetActive(false);
        Renderer.enabled = true;

        yield return new WaitForSeconds(4f);
        ResetShowcase();
    }

    void ResetShowcase()
    {
        StartCoroutine(Showcase());
    }

}
