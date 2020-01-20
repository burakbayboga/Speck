using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    public LilB LilB;
    public LineRenderer SwipeLine;

    public float MaxSwipeDistanceScreenHeightRatio;
    public float MaxSwipeForce;
    public float MinSwipeForce;

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

    void Update()
    {
        if ((LilB.IsEndless && (GameController.instance.IsGameOver || GameController.instance.IsGamePaused))
            || LilB.IsChallenge && (ChallengeController.instance.IsGameOver || ChallengeController.instance.IsGamePaused))
        {
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
                LilB.ApplyForce((SwipeStartPos - SwipeEndPos).normalized, GetForce(), true);
            }
            SwipeLine.enabled = true;
            SwipeLine.SetPosition(0, LilB.transform.position);
            SwipeLine.SetPosition(1, LilB.transform.position + GetSwipeLinePosition(touch.position));
        }
        else
        {
            SwipeLine.enabled = false;
        }
    }

    public void UpdateLilB(LilB lilB)
    {
        LilB = lilB;
        SwipeLine = LilB.transform.GetChild(1).GetComponent<LineRenderer>();
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
