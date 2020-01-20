using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFenceShowcase : MonoBehaviour
{

    public bool MainBody;

    public GameObject Helper;
    public GameObject DeathCollider;
    public GameObject Electric;

    private void Start()
    {
        if (MainBody)
        {
            StartCoroutine(Showcase());
        }
    }

    IEnumerator Showcase()
    {
        yield return new WaitForSeconds(1.5f);

        DeathCollider.transform.GetChild(0).gameObject.SetActive(false);
        Electric.SetActive(true);

        float lerpParameter = 0.0f;
        float startTime = Time.time;
        Electric.transform.position = Helper.transform.position;

        ShowcaseController.instance.Electric = Electric.GetComponent<ParticleSystem>();

        yield return null;

        while (Time.time - startTime < 5f)
        {
            lerpParameter = Mathf.PingPong(Time.time - startTime, 1.0f);
            Vector3 electricPosition = Vector3.Lerp(transform.position, Helper.transform.position, lerpParameter * lerpParameter);
            Electric.transform.position = electricPosition;
            yield return null;
        }

        ResetShowcase();
    }

    void ResetShowcase()
    {
        DeathCollider.transform.GetChild(0).gameObject.SetActive(true);
        Electric.SetActive(false);
        StartCoroutine(Showcase());
    }

}
