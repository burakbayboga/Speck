using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadertest : MonoBehaviour
{

    public SpriteRenderer SpriteRenderer;

    Material Material;
    Animator Animator;
    float pingpongtimer = 0f;
    float animparam = 0f;

    private void Awake()
    {
        Material = SpriteRenderer.material;
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(PingpongThreshold());
    }

    private void Update()
    {
        animparam += Time.deltaTime;
        if (animparam > 1f)
        {
            animparam -= 1f;
        }

        Animator.SetFloat("normtime", animparam);
    }



    private void LateUpdate()
    {
        Vector2 center = new Vector2((SpriteRenderer.sprite.rect.xMin + SpriteRenderer.sprite.pivot.x) / 1920f, (SpriteRenderer.sprite.rect.yMin + SpriteRenderer.sprite.pivot.y) / 1080f);

        Material.SetVector("_Center", new Vector4(center.x, center.y, 0f, 0f));
    }
    
    IEnumerator PingpongThreshold()
    {
        WaitForSeconds seconds = new WaitForSeconds(0.1f);

        while (true)
        {
            pingpongtimer += 0.001f;
            Material.SetFloat("_Threshold", Mathf.PingPong(pingpongtimer, 0.009f) + 0.001f);

            yield return seconds;
        }
    }

}
