using System.Collections;
using UnityEngine;
using System;



public class SmallFry : MonoBehaviour
{

    [HideInInspector] public Rigidbody2D Rigidbody;
    [HideInInspector] public Vector2 OriginalVelocity;
    [HideInInspector] public float OriginalAngularVelocity;

    public bool IsFrozen;
    public float SpeedMultiplier;

    protected float CurrentTheta;

    public EnemyType EnemyType;

    public Transform LilBTransform;

    protected Camera MainCamera;

    protected Vector2 ScreenPos;

    protected AudioSource AudioSource;

    public virtual void Init()
    {
        AudioSource = GetComponent<AudioSource>();
        Rigidbody = GetComponent<Rigidbody2D>();
        LilBTransform = GameObject.FindWithTag("LilB").transform;

        MainCamera = Camera.main;
        IsFrozen = false;
        SpeedMultiplier = 1.0f;
    }

    public virtual void HandleDeath()
    {
        SmallFryManager.instance.RemoveFromSmallFryList(this);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    protected void DetermineStartTheta()
    {
        float helperAngle = Vector3.Angle(transform.position, Vector3.right);
        if (transform.position.y < 0.0f)
        {
            CurrentTheta = 360.0f - helperAngle;
        }
        else
        {
            CurrentTheta = helperAngle;
        }
    }

    public virtual void ApplyFreeze(float effectTime){ }

    public virtual void RevertFreeze() { }

    protected IEnumerator Timer(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
