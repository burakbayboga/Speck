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
    Animator Animator;

    void Awake()
    {
		Animator = GetComponent<Animator>();
    }

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
            transform.Translate(Velocity * Time.deltaTime, Space.World);
        }
    }

    private void LateUpdate()
    {
        SetShaderProperties();
    }

    void SetShaderProperties()
    {
        Sprite sprite = OuterSpriteRenderer.sprite;
        Vector2 center = new Vector2((sprite.rect.xMin + sprite.pivot.x) / TextureResolution.x, (sprite.rect.yMin + sprite.pivot.y) / TextureResolution.y);
        OuterMaterial.SetVector("_Center", center);
    }

    public void OnSpinStarted()
    {
        AudioSource.Play();
        Velocity = GetVelocity();
        ShouldMove = true;

        GetComponent<Collider2D>().enabled = true;
    }

    IEnumerator Action()
    {
        yield return new WaitForSeconds(1.0f);
        Animator.SetTrigger("SpinTrigger");
    }

    Vector3 GetVelocity()
    {
        return (LilBTransform.position - transform.position) * SpeedMultiplier;
    }
}
