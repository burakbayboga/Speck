using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance;

	public GameObject EdgeWarningsParent;
	public GameObject SwipeText;
	public GameObject SwipeDistanceParent;
	public Image SwipeDistanceBg;

	public LineRenderer[] SpeckTails;
	public Transform[] SpeckTailTargets;
	public LilB[] Specks;
	public Transform[] SpeckTransforms;
	public Transform[] SpeckInitialTransforms;
	public Rigidbody2D[] SpeckRbs;

	Coroutine SwipeDistanceCoroutine;
	State CurrentState;

	int SwipeCount = 0;

	void Awake()
	{
		instance = this;
		LilB.instance.IsTutorial = true;
		LilB.instance.IsChallenge = false;
		LilB.instance.IsEndless = false;
	}

	void Start()
	{
		CurrentState = State.Start;
	}

	public void OnEdgePushBack()
	{
		if (CurrentState == State.Start)
		{
			CurrentState = State.EdgePushback;
			EdgeWarningsParent.SetActive(true);
			StartCoroutine(SwitchToSwipeTimer());
		}
	}

	IEnumerator SwitchToSwipeTimer()
	{
		yield return new WaitForSeconds(3f);
		EdgeWarningsParent.SetActive(false);
		SwipeText.SetActive(true);
		CurrentState = State.Swipe;
		LilB.instance.InputEnabled = true;
	}

	public void OnSwipe()
	{
		if (CurrentState == State.Swipe)
		{
			SwipeCount++;
			if (SwipeCount >= 3)
			{
				SwipeText.SetActive(false);
				CurrentState = State.SwipeDistance;
				SwipeDistanceCoroutine = StartCoroutine(SwipeDistanceSequence());
				StartCoroutine(SwipeDistanceTimer());
			}
		}
	}

	IEnumerator SwipeDistanceTimer()
	{
		yield return new WaitForSeconds(15f);
		StopCoroutine(SwipeDistanceCoroutine);
		CurrentState = State.Finito;
		SwipeDistanceParent.SetActive(false);
		PlayerPrefs.SetInt(Utility.PrefsPlayedTutorialKey, 1);
		SceneManager.LoadScene("menu");
	}

	IEnumerator SwipeDistanceSequence()
	{
		SwipeDistanceParent.SetActive(true);

		while (true)
		{
			SpeckRbs[0].gravityScale = 0f;
			SpeckRbs[1].gravityScale = 0f;
			SpeckRbs[0].velocity = Vector2.zero;
			SpeckRbs[1].velocity = Vector2.zero;
			SpeckTransforms[0].position = SpeckInitialTransforms[0].position;
			SpeckTransforms[1].position = SpeckInitialTransforms[1].position;
			SpeckTails[0].enabled = true;
			SpeckTails[1].enabled = true;

			float startTime = Time.time;
			float t = 0f;
			while (t < 1f)
			{
				t = (Time.time - startTime) / 1.5f;

				Vector3 point = Vector3.Lerp(SpeckTransforms[0].position, SpeckTailTargets[0].position, t);
				SpeckTails[0].SetPosition(0, SpeckTransforms[0].position);
				SpeckTails[0].SetPosition(1, point);

				point = Vector3.Lerp(SpeckTransforms[1].position, SpeckTailTargets[1].position, t);
				SpeckTails[1].SetPosition(0, SpeckTransforms[1].position);
				SpeckTails[1].SetPosition(1, point);

				yield return null;
			}
			SpeckTails[0].enabled = false;
			SpeckTails[1].enabled = false;
			SpeckRbs[0].gravityScale = 1f;
			SpeckRbs[1].gravityScale = 1f;
			Specks[0].ApplyForce(Vector3.right, 8000f, false);
			Specks[1].ApplyForce(Vector3.right, 15000f, false);

			yield return new WaitForSeconds(2.5f);
		}
	}


	enum State
	{
		Start,
		EdgePushback,
		Swipe,
		SwipeDistance,
		Finito
	}

}
