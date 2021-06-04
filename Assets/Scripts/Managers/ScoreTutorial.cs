using System.Collections;
using UnityEngine;

public class ScoreTutorial : MonoBehaviour
{
	public GameObject[] TutorialObjects;
	public Transform[] SpriteIndicatorTransforms;
	public RectTransform[] UITextTransforms;
	public Animator[] UITextAnimators;
	public Animator[] SpriteIndicatorAnimators;
	public GameObject PlayButton;

	public void Initiate(Vector2 screenCenter, float radius0Sqr, float radius1Sqr, Camera camera)
	{
		SetScalesAndPositions(screenCenter, radius0Sqr, radius1Sqr, camera);
		StartCoroutine(TutorialSequence());
	}

	void SetScalesAndPositions(Vector2 screenCenter, float radius0Sqr, float radius1Sqr, Camera camera)
	{
		Vector2 screenSize = new Vector2(Screen.width, Screen.height);
		float worldRadius = camera.ScreenToWorldPoint(new Vector3(screenCenter.x + Mathf.Sqrt(radius0Sqr), screenCenter.y, 25f)).x;
		float scaleFactor = 1f / 2.3f;
		Vector3 scale = new Vector3(worldRadius * scaleFactor, worldRadius * scaleFactor, 1f);
		SpriteIndicatorTransforms[0].localScale = scale;
		worldRadius = camera.ScreenToWorldPoint(new Vector3(screenCenter.x + Mathf.Sqrt(radius1Sqr), screenCenter.y, 25f)).x;
		scale = new Vector3(worldRadius * scaleFactor, worldRadius * scaleFactor, 1f);
		SpriteIndicatorTransforms[1].localScale = scale;

		float screenWidth = Screen.width;
		Vector3 pos = new Vector3(screenWidth / 20f + screenWidth / 8f, 0f, 0f);
		UITextTransforms[0].anchoredPosition = pos;
		pos.x *= -1f;
		UITextTransforms[1].anchoredPosition = pos;
		pos.x = screenWidth / 4f + screenWidth / 8f;
		UITextTransforms[2].anchoredPosition = pos;
		pos.x *= -1f;
		UITextTransforms[3].anchoredPosition = pos;
	}

	IEnumerator TutorialSequence()
	{
		yield return null;
		WaitForSeconds delay = new WaitForSeconds(0.5f);

		TutorialObjects[0].SetActive(true);
		TutorialObjects[1].SetActive(true);

		SpriteIndicatorAnimators[0].Play("score_sprite_appear");
		UITextAnimators[5].Play("score_text_appear");
		yield return delay;
		SpriteIndicatorAnimators[1].Play("score_sprite_appear");
		yield return delay;
		UITextAnimators[0].Play("score_ui_inner_appear");
		yield return delay;
		UITextAnimators[1].Play("score_ui_middle_appear");
		UITextAnimators[2].Play("score_ui_middle_appear");
		yield return delay;
		UITextAnimators[3].Play("score_ui_outer_appear");
		UITextAnimators[4].Play("score_ui_outer_appear");

		yield return new WaitForSeconds(10f);
		UITextAnimators[5].gameObject.SetActive(false);
		PlayButton.SetActive(true);
		PlayerPrefs.SetInt(Utility.PrefsSeenScoreTutorialKey, 1);
	}


}
