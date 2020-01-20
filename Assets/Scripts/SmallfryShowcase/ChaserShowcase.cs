using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserShowcase : MonoBehaviour
{

    Animator Animator;

    Vector3 OriginalPosition;
    Vector3 TargetPosition;


    private void Awake()
    {
        Animator = GetComponent<Animator>();

        OriginalPosition = transform.position;
        TargetPosition = OriginalPosition + Vector3.up * 15f;
    }

    private void Start()
    {
        StartCoroutine(Showcase());
    }

    IEnumerator Showcase()
    {
        yield return null;

        while (!Animator.GetCurrentAnimatorStateInfo(0).IsName("chaser_moving"))
        {
            yield return null;
        }

        float startTime = Time.time;
        float lerpParameter = 0f;

        while (lerpParameter < 1f)
        {
            lerpParameter = (Time.time - startTime) / 2f;
            transform.position = Vector3.Lerp(OriginalPosition, TargetPosition, lerpParameter);
            yield return null;
        }

        ResetShowcase();
    }

    void ResetShowcase()
    {
        Animator.Play("chaser_spawning");
        transform.position = OriginalPosition;
        StartCoroutine(Showcase());
    }
}
