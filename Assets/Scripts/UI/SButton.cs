using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SButton : MonoBehaviour
{
    public float TransitionTime;
    public Graphic[] Graphics;
    public Color[] BaseColors;
    public Color[] PressedColors;
    public Color[] InactiveColors;
    public UnityEvent OnClickEvent;
    public UnityEvent InActiveOnClickEvent;
    public bool OnClickEventLoadsScene;
	public bool IsMaterialColorDst;

    public bool IsButtonActive;

	private Material Material;
	private string[] MaterialColorIds;

    private void Awake()
    {
		if (IsMaterialColorDst)
		{
			MaterialColorIds = new string[BaseColors.Length];
			Material = Instantiate(Graphics[0].material);
			Graphics[0].material = Material;
		}

		for (int i = 0; i < BaseColors.Length; i++)
		{
			if (IsMaterialColorDst)
			{
				MaterialColorIds[i] = "_Color" + i.ToString();
			}
			if (!IsButtonActive)
			{
				SetColor(InactiveColors[i], i);
			}
		}
    }

	private void SetColor(Color color, int targetIndex)
	{
		if (IsMaterialColorDst)
		{
			Material.SetColor(MaterialColorIds[targetIndex], color);
		}
		else
		{
			Graphics[targetIndex].color = color;
		}
	}

	public void SetActivity(bool isActive)
	{
		Color[] colors = isActive ? BaseColors : InactiveColors;
		for (int i = 0; i < BaseColors.Length; i++)
		{
			SetColor(colors[i], i);
		}

		IsButtonActive = isActive;
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
            lerpParameter = Mathf.Clamp((Time.unscaledTime - startTime) / TransitionTime, 0.0f, 1.0f);
            for (int i=0; i < BaseColors.Length; i++)
            {
				SetColor(Color.Lerp(BaseColors[i], PressedColors[i], lerpParameter), i);
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
            for (int i = 0; i < BaseColors.Length; i++)
            {
				SetColor(Color.Lerp(PressedColors[i], BaseColors[i], lerpParameter), i);
            }

            yield return null;
        }

        if (clicked && OnClickEventLoadsScene)
        {
            OnClickEvent.Invoke();
        }
    }
}
