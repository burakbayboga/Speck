using UnityEngine;

public class ScreenEdgeController : MonoBehaviour
{

    public GameObject LilBGO;
    public float EdgeForce;

    Camera MainCamera;

    Transform LilBTransform;
    LilB LilB;

    float ScreenLimitOffset = 2.0f;

    void Awake()
    {
        MainCamera = Camera.main;
        LilBTransform = LilBGO.GetComponent<Transform>();
        LilB = LilBGO.GetComponent<LilB>();
    }

    public void UpdateLilB(LilB lilB)
    {
        LilB = lilB;
        LilBTransform = LilB.transform;
    }

    void Update()
    {
        if (MainCamera.WorldToScreenPoint(LilBTransform.position).x < ScreenLimitOffset)
        {
            //hit left border
            LilB.ApplyForce(Vector2.right, EdgeForce, false);
        }

        else if (MainCamera.WorldToScreenPoint(LilBTransform.position).x > Screen.width - ScreenLimitOffset)
        {
            //hit right border
            LilB.ApplyForce(Vector2.left, EdgeForce, false);
        }

        if (MainCamera.WorldToScreenPoint(LilBTransform.position).y < ScreenLimitOffset)
        {
            //hit lower border
            LilB.ApplyForce(Vector2.up, EdgeForce, false);
        }

        else if (MainCamera.WorldToScreenPoint(LilBTransform.position).y > Screen.height - ScreenLimitOffset)
        {
            //hit upper border
            LilB.ApplyForce(Vector2.down, EdgeForce, false);
        }
    }

}
