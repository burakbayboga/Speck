using UnityEngine;
using UnityEngine.UI;

public class ChallengeModeButton : SButton
{
	public Sprite SelectedSprite;
	public Sprite NotSelectedSprite;
	public Image SelectImage;
	public Image ModeNameImage;
	public ChallengeMode Mode;
	public Color ModeBaseColor;
	public Color ModeSelectedColor;
	public GameObject Chain;
	public GameObject SelectedSpeck;

	private bool IsSelected;

	public void OnChallengeModeButtonClicked()
	{
		IsSelected = !IsSelected;

		if (IsSelected)
		{
			SelectImage.color = ModeSelectedColor;
			ModeNameImage.color = ModeSelectedColor;
			SelectedSpeck.SetActive(true);
		}
		else
		{
			SelectImage.color = ModeBaseColor;
			ModeNameImage.color = ModeBaseColor;
			SelectedSpeck.SetActive(false);
		}

		Utility.CurrentChallengeMode ^= Mode;

		if (Utility.IsHardcore(Mode))
		{
			MenuController.instance.OnChallengeModeHardcoreClicked(IsSelected);
		}
	}	

	public void ForceSelectMode()
	{
		IsSelected = true;
		SelectImage.color = ModeSelectedColor;
		ModeNameImage.color = ModeSelectedColor;
		SelectedSpeck.SetActive(true);
		Utility.CurrentChallengeMode |= Mode;
		Chain.SetActive(true);
	}

	public void ForceUnselectMode()
	{
		IsSelected = false;
		SelectImage.color = ModeBaseColor;
		ModeNameImage.color = ModeBaseColor;
		SelectedSpeck.SetActive(false);
		Utility.CurrentChallengeMode |= Mode;
		Utility.CurrentChallengeMode ^= Mode;
		ModeNameImage.color = ModeBaseColor;
		Chain.SetActive(false);
	}
}
