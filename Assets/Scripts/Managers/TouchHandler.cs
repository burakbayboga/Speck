using UnityEngine;

public class TouchHandler : MonoBehaviour
{

    public float MaxSwipeDistanceScreenHeightRatio;
    public float MaxSwipeForce;
    public float MinSwipeForce;

    LineRenderer SwipeLine;
    Camera MainCamera;
    float MaxSwipeDistance;

    Vector2 SwipeStartPos;
    Vector2 SwipeEndPos;
    float MainCameraOffsetZ;

    void Awake()
    {
        MainCamera = Camera.main;
        MainCameraOffsetZ = -1.0f * MainCamera.transform.position.z;
        MaxSwipeDistance = Screen.height * MaxSwipeDistanceScreenHeightRatio;
    }

	void Start()
	{
		SwipeLine = LilB.instance.GetComponentInChildren<LineRenderer>();
	}

    void Update()
    {
        if (!LilB.instance.InputEnabled ||
			(LilB.instance.IsEndless && (GameController.instance.IsGameOver || GameController.instance.IsGamePaused))
            || LilB.instance.IsChallenge && (ChallengeController.instance.IsGameOver || ChallengeController.instance.IsGamePaused))
        {
			// TODO: srsly...
            SwipeLine.enabled = false;
            return;
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                SwipeStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                SwipeEndPos = touch.position;
                LilB.instance.ApplyForce((SwipeStartPos - SwipeEndPos).normalized, GetForce(), true);
            }
            SwipeLine.enabled = true;
            SwipeLine.SetPosition(0, LilB.instance.transform.position);
            SwipeLine.SetPosition(1, LilB.instance.transform.position + GetSwipeLinePosition(touch.position));
        }
        else
        {
            SwipeLine.enabled = false;
        }
    }

    private float GetForce()
    {
        float swipeDistance = Mathf.Clamp((SwipeEndPos - SwipeStartPos).magnitude, 0.0f, MaxSwipeDistance);
        float lerpParameter = Mathf.InverseLerp(0.0f, MaxSwipeDistance, swipeDistance);
        float swipeForce = Mathf.Lerp(MinSwipeForce, MaxSwipeForce, lerpParameter);
        return swipeForce;
    }

    private Vector3 GetSwipeLinePosition(Vector2 currentTouchPos)
    {
        Vector3 dummyEndPoint = new Vector3(currentTouchPos.x, currentTouchPos.y, MainCameraOffsetZ);
        Vector3 dummyStartPoint = new Vector3(SwipeStartPos.x, SwipeStartPos.y, MainCameraOffsetZ);
        Vector3 swipeVector = MainCamera.ScreenToWorldPoint(dummyEndPoint) - MainCamera.ScreenToWorldPoint(dummyStartPoint);
        return swipeVector;
    }

}
