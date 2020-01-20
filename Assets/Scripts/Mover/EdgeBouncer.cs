using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeBouncer : MonoBehaviour
{
    Camera MainCamera;
    float ScreenLimitOffset = 2.0f;
    Rigidbody2D Rigidbody;

    Vector2 CurrentVelocity;
    Vector2 ScreenPos;

    void Awake()
    {
        MainCamera = Camera.main;
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ScreenPos = MainCamera.WorldToScreenPoint(transform.position);

        if (ScreenPos.x < ScreenLimitOffset)
        {
            //hit left border
            CurrentVelocity = Rigidbody.velocity;
            Rigidbody.velocity = new Vector2(Mathf.Abs(CurrentVelocity.x), CurrentVelocity.y);
        }
        else if (ScreenPos.x > Screen.width - ScreenLimitOffset)
        {
            //hit right border
            CurrentVelocity = Rigidbody.velocity;
            Rigidbody.velocity = new Vector2(Mathf.Abs(CurrentVelocity.x) * -1.0f, CurrentVelocity.y);
        }
        else if (ScreenPos.y < ScreenLimitOffset)
        {
            //hit lower border
            CurrentVelocity = Rigidbody.velocity;
            Rigidbody.velocity = new Vector2(CurrentVelocity.x, Mathf.Abs(CurrentVelocity.y));
        }
        else if (ScreenPos.y > Screen.height - ScreenLimitOffset)
        {
            //hit upper border
            CurrentVelocity = Rigidbody.velocity;
            Rigidbody.velocity = new Vector2(CurrentVelocity.x, Mathf.Abs(CurrentVelocity.y) * -1.0f);
        }
    }

}
