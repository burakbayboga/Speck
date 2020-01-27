using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadertest : MonoBehaviour
{

    public SpriteRenderer SpriteRenderer;

    Material Material;
    float pingpongtimer = 0f;

    private void Awake()
    {
        Material = SpriteRenderer.material;
    }

    private void Start()
    {
        StartCoroutine(PingpongThreshold());
    }

    private void LateUpdate()
    {
        //print(SpriteRenderer.sprite.rect.center + " " + SpriteRenderer.sprite.rect.x + " " + SpriteRenderer.sprite.rect.y);
        //print(SpriteRenderer.sprite.texture.height + " " + SpriteRenderer.sprite.texture.width);

        //Vector2 center = new Vector2(SpriteRenderer.sprite.rect.center.x / 1920f, SpriteRenderer.sprite.rect.center.y / 1080f);
        Vector2 center = new Vector2((SpriteRenderer.sprite.rect.xMin + SpriteRenderer.sprite.pivot.x) / 1920f, (SpriteRenderer.sprite.rect.yMin + SpriteRenderer.sprite.pivot.y) / 1080f);

        Material.SetVector("_Center", new Vector4(center.x, center.y, 0f, 0f));
        //print(SpriteRenderer.sprite.pivot);
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
