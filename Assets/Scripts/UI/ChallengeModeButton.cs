using UnityEngine;
using UnityEngine.UI;

public class ChallengeModeButton : SButton
{
	public Image SelectImage;
	public Graphic ModeName;
	public ChallengeMode Mode;
	public Color ModeBaseColor;
	public Color ModeSelectedColor;
	public GameObject SelectedSpeck;
	public GameObject Chain;

	public void Select()
	{
		SelectImage.color = ModeSelectedColor;
		ModeName.color = ModeSelectedColor;
		SelectedSpeck.SetActive(true);

		Utility.CurrentChallengeMode = Mode;
	}

	public void Unselect()
	{
		SelectImage.color = ModeBaseColor;
		ModeName.color = ModeBaseColor;
		SelectedSpeck.SetActive(false);
	}

	public void LockMode()
	{
		SetActivity(false);
		Chain.SetActive(true);
	}

	public void Reset()
	{
		SelectImage.color = ModeBaseColor;
		ModeName.color = ModeBaseColor;
		SelectedSpeck.SetActive(false);
		SetActivity(true);
		Chain.SetActive(false);
	}
}
