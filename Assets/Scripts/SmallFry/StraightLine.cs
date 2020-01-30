using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLine : SmallFry
{
    public float SpeedMultiplier = 1f;

    public SpriteRenderer OuterSpriteRenderer;
    Material OuterMaterial;
    Vector2 TextureResolution;

    bool ShouldMove = false;
    Vector3 Velocity;

    private void Start()
    {
        Texture2D texture = OuterSpriteRenderer.sprite.texture;
        TextureResolution = new Vector2(texture.width, texture.height);
        OuterMaterial = OuterSpriteRenderer.material;
        OuterMaterial.SetFloat("_HalfSliceSize", (OuterSpriteRenderer.sprite.rect.height / TextureResolution.y) / 2f);
    }

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
        if (ShouldMove)
        {
            transform.position += Velocity * Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        SetShaderProperties();
    }

    void SetShaderProperties()
    {
        Vector2 center = new Vector2((OuterSpriteRenderer.sprite.rect.xMin + OuterSpriteRenderer.sprite.pivot.x) / TextureResolution.x, (OuterSpriteRenderer.sprite.rect.yMin + OuterSpriteRenderer.sprite.pivot.y) / TextureResolution.y);
        OuterMaterial.SetVector("_Center", center);
    }

    IEnumerator Action()
    {
        yield return new WaitForSeconds(1.0f);

        AudioSource.Play();
        Velocity = GetVelocity();
        ShouldMove = true;

        //Rigidbody.velocity = GetVelocity();
        GetComponent<Collider2D>().enabled = true;
    }

    Vector3 GetVelocity()
    {
        return (LilBTransform.position - transform.position) * SpeedMultiplier;
    }
}
