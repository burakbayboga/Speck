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

    void FixedUpdate()
    {
        if (MainCamera.WorldToScreenPoint(LilBTransform.position).x < ScreenLimitOffset)
        {
            //hit left border
			PushSpeckBack(Vector2.right);
        }

        else if (MainCamera.WorldToScreenPoint(LilBTransform.position).x > Screen.width - ScreenLimitOffset)
        {
            //hit right border
			PushSpeckBack(Vector2.left);
        }

        if (MainCamera.WorldToScreenPoint(LilBTransform.position).y < ScreenLimitOffset)
        {
            //hit lower border
			PushSpeckBack(Vector2.up);
        }

        else if (MainCamera.WorldToScreenPoint(LilBTransform.position).y > Screen.height - ScreenLimitOffset)
        {
            //hit upper border
			PushSpeckBack(Vector2.down);
        }
    }

	void PushSpeckBack(Vector2 dir)
	{
		LilB.ApplyForce(dir, EdgeForce, false);
		if (LilB.IsTutorial)
		{
			TutorialController.instance.OnEdgePushBack();
		}
	}

}
