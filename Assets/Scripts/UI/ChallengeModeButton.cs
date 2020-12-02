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
	public Color ModeSelectLockedColor;


	private bool IsSelected;

	public void OnChallengeModeButtonClicked()
	{
		IsSelected = !IsSelected;
		SelectImage.sprite = IsSelected ? SelectedSprite : NotSelectedSprite;
		ModeNameImage.color = IsSelected ? ModeSelectedColor : ModeBaseColor;

		Utility.CurrentChallengeMode ^= Mode;

		if (Utility.IsHardcore(Mode))
		{
			MenuController.instance.OnChallengeModeHardcoreClicked(IsSelected);
		}
	}	

	public void SelectMode()
	{
		IsSelected = true;
		SelectImage.sprite = SelectedSprite;
		Utility.CurrentChallengeMode |= Mode;
		ModeNameImage.color = ModeSelectLockedColor;
	}

	public void UnselectMode()
	{
		IsSelected = false;
		SelectImage.sprite = NotSelectedSprite;
		Utility.CurrentChallengeMode |= Mode;
		Utility.CurrentChallengeMode ^= Mode;
		ModeNameImage.color = ModeBaseColor;
	}


}
