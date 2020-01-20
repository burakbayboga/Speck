using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTarget : MonoBehaviour
{

    public float RotationSpeed;
    public float Radius;
    public GameObject Effect;

    private CircleCollider2D Collider;
    private float CurrentAngle = 0.0f;
    private Vector3 Center;

    private void Awake()
    {
        Center = transform.position;
        Effect.transform.position = Center + new Vector3(Radius, 0.0f, 0.0f);
        Effect.SetActive(true);
        Collider = GetComponent<CircleCollider2D>();
        Collider.radius = Radius;
    }

    private void Update()
    {
        CurrentAngle += RotationSpeed;

        float x = Mathf.Cos(CurrentAngle);
        float y = Mathf.Sin(CurrentAngle);

        Effect.transform.position = Center + new Vector3(x, y, 0.0f) * Radius * Random.Range(0.95f, 1.05f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LilB"))
        {
            BasicTutorialController.instance.TutorialTargetReached();
            gameObject.SetActive(false);
        }
    }

}
