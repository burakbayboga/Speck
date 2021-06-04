using UnityEngine;
using System.Collections;

public class ScreenEdgeController : MonoBehaviour
{
    public float EdgeForce;

    Camera MainCamera;

    float ScreenLimitOffset = 2.0f;
	Transform LilBTransform;
	bool CanPush = true;
	WaitForSeconds cooldown = new WaitForSeconds(0.1f);
	float height;
	float width;

    void Awake()
    {
        MainCamera = Camera.main;
		width = Screen.width;
		height = Screen.height;
    }

	void Start()
	{
		LilBTransform = LilB.instance.transform;
	}

	IEnumerator Cooldown()
	{
		yield return cooldown;
		CanPush = true;
	}

    void Update()
    {
		if (!CanPush)
		{
			return;
		}

		Vector2 screenPoint = MainCamera.WorldToScreenPoint(LilBTransform.position);
        if (screenPoint.x < ScreenLimitOffset)
        {
            //hit left border
			Vector2 resetVelocity = LilB.instance.Rigidbody.velocity;
			resetVelocity.x = 0f;
			LilB.instance.Rigidbody.velocity = resetVelocity;
			PushSpeckBack(Vector2.right);
        }

        else if (screenPoint.x > width - ScreenLimitOffset)
        {
            //hit right border
			Vector2 resetVelocity = LilB.instance.Rigidbody.velocity;
			resetVelocity.x = 0f;
			LilB.instance.Rigidbody.velocity = resetVelocity;
			PushSpeckBack(Vector2.left);
        }

        if (screenPoint.y < ScreenLimitOffset)
        {
            //hit lower border
			Vector2 resetVelocity = LilB.instance.Rigidbody.velocity;
			resetVelocity.y = 0f;
			LilB.instance.Rigidbody.velocity = resetVelocity;
			PushSpeckBack(Vector2.up);
        }

        else if (screenPoint.y > height - ScreenLimitOffset)
        {
            //hit upper border
			Vector2 resetVelocity = LilB.instance.Rigidbody.velocity;
			resetVelocity.y = 0f;
			LilB.instance.Rigidbody.velocity = resetVelocity;
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
		CanPush = false;
		StartCoroutine(Cooldown());
	}

}
