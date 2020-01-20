using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SButton : MonoBehaviour
{
    public float TransitionTime;
    public Graphic[] Graphics;
    public Color BaseColor;
    public Color PressedColor;
    public Color InactiveColor;
    public UnityEvent OnClickEvent;
    public UnityEvent InActiveOnClickEvent;
    public bool IsButtonActive;
    public bool OnClickEventLoadsScene;


    private void Start()
    {
        if (!IsButtonActive)
        {
            for (int i=0; i < Graphics.Length; i++)
            {
                Graphics[i].color = InactiveColor;
            }
        }
    }

    public void OnDown()
    {
        if (!IsButtonActive)
        {
            return;
        }

        StopAllCoroutines();
        StartCoroutine(OnDownCoroutine());
    }

    public void OnUp(bool clicked)
    {
        if (!IsButtonActive)
        {
            if (InActiveOnClickEvent != null)
            {
                InActiveOnClickEvent.Invoke();
            }
            return;
        }

        StopAllCoroutines();
        StartCoroutine(OnUpCoroutine(clicked));
    }

    private IEnumerator OnDownCoroutine()
    {
        float lerpParameter = 0.0f;
        float startTime = Time.unscaledTime;

        while (lerpParameter < 1.0f)
        {
            lerpParameter = Mathf.Clamp((Time.unscaledTime - startTime) * 1.0f / TransitionTime, 0.0f, 1.0f);
            for (int i=0; i < Graphics.Length; i++)
            {
                Graphics[i].color = Color.Lerp(BaseColor, PressedColor, lerpParameter);
            }

            yield return null;
        }
    }

    private IEnumerator OnUpCoroutine(bool clicked)
    {
        if (clicked)
        {
            if (!OnClickEventLoadsScene)
            {
                OnClickEvent.Invoke();
            }
            else
            {
                if (MenuController.instance != null)
                {
                    MenuController.instance.DisableAllSButtons();
                }
                else if (GameController.instance != null)
                {
                    GameController.instance.DisableAllSButtons();
                }
                else if (ChallengeController.instance != null)
                {
                    ChallengeController.instance.DisableAllSButtons();
                }
            }
        }

        float lerpParameter = 0.0f;
        float startTime = Time.unscaledTime;

        while (lerpParameter < 1.0f)
        {
            lerpParameter = Mathf.Clamp((Time.unscaledTime - startTime) * 1.0f / TransitionTime, 0.0f, 1.0f);
            for (int i = 0; i < Graphics.Length; i++)
            {
                Graphics[i].color = Color.Lerp(PressedColor, BaseColor, lerpParameter);
            }

            yield return null;
        }

        if (clicked && OnClickEventLoadsScene)
        {
            OnClickEvent.Invoke();
        }
    }
}
