using UnityEngine;
using UnityEngine.UI;

public class ChallengeModeButton : SButton
{
	public Sprite SelectedSprite;
	public Sprite NotSelectedSprite;
	public Image SelectImage;
	public ChallengeMode Mode;

	private bool IsSelected;

	public void OnChallengeModeButtonClicked()
	{
		IsSelected = !IsSelected;
		SelectImage.sprite = IsSelected ? SelectedSprite : NotSelectedSprite;

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
	}

	public void UnselectMode()
	{
		IsSelected = false;
		SelectImage.sprite = NotSelectedSprite;
		Utility.CurrentChallengeMode |= Mode;
		Utility.CurrentChallengeMode ^= Mode;
	}


}
