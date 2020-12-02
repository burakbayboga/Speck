using UnityEngine;
using UnityEngine.UI;

public class ChallengeButton : SButton
{
	public Image ButtonImage;
	public Image DoubleImg;
	public Image FastImg;
	public Image HardcoreImg;
	public Text LevelText;
	public Sprite[] ButtonSprites;

	private int Level;
	private ChallengeMode Modes;

	public void InitChallengeButton(ChallengeMode modes, int level, bool isActive)
	{
		Modes = modes;
		IsButtonActive = isActive;
		Level = level;
		LevelText.text = (Level+1).ToString();

		if (IsButtonActive)
		{
			InitPassedImage(DoubleImg, ChallengeMode.Double);
			InitPassedImage(FastImg, ChallengeMode.Fast);
			InitPassedImage(HardcoreImg, ChallengeMode.Hardcore);
		}
	}

	private void InitPassedImage(Image image, ChallengeMode comparedMode)
	{
		Color color;
		float alpha;

		color = image.color;
		alpha = ((Modes & comparedMode) == comparedMode) ? 1f : 0f;
		color.a = alpha;
		image.color = color;
	}

	public void OnChallengeButtonClicked()
	{
		Utility.CurrentChallengeIndex = Level;
		MenuController.instance.OnChallengeLevelClicked(Modes);
	}

}
