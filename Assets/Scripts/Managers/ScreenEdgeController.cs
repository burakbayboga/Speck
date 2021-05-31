using UnityEngine;

public class ScreenEdgeController : MonoBehaviour
{
    public float EdgeForce;

    Camera MainCamera;

    float ScreenLimitOffset = 2.0f;
	Transform LilBTransform;

    void Awake()
    {
        MainCamera = Camera.main;
    }

	void Start()
	{
		LilBTransform = LilB.instance.transform;
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
		LilB.instance.ApplyForce(dir, EdgeForce, false);
		if (LilB.instance.IsTutorial)
		{
			TutorialController.instance.OnEdgePushBack();
		}
	}

}
