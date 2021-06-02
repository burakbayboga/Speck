using UnityEngine;

public class SmallFry : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D Rigidbody;

    protected Camera MainCamera;
	protected Animator Animator;
    protected float CurrentTheta;
    protected Transform LilBTransform;

    public virtual void Init()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
		Animator = GetComponent<Animator>();
        LilBTransform = LilB.instance.transform;

        MainCamera = Camera.main;
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
}
