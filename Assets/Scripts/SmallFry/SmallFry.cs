using UnityEngine;



public class SmallFry : MonoBehaviour
{

    [HideInInspector] public Rigidbody2D Rigidbody;
    [HideInInspector] public Vector2 OriginalVelocity;
    [HideInInspector] public float OriginalAngularVelocity;

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
