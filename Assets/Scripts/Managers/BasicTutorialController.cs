using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicTutorialController : MonoBehaviour
{
    public static BasicTutorialController instance;

    public GameObject SwipeTutorialText;
    public GameObject TutorialFinishedText;
    public GameObject TargetTutorialText;
    public LilB Speck;
    public int TutorialSwipeRequirement;

    public GameObject[] TutorialTargets;

    private int SwipeCount = 0;
    private bool SwipeTutorialActive = true;
    private int CurrentTutorialTargetIndex = 0;

    private void Awake()
    {
        instance = this;
        Speck.IsTutorial = true;
    }

    public void OnLilBApplyForce()
    {
        if (!SwipeTutorialActive)
        {
            return;
        }

        SwipeCount++;
        
        if (SwipeCount >= TutorialSwipeRequirement)
        {
            SwipeTutorialActive = false;
            SwipeTutorialText.SetActive(false);

            TargetTutorialText.SetActive(true);

            TutorialTargets[CurrentTutorialTargetIndex++].SetActive(true);

        }
    }

    private IEnumerator EndTutorial()
    {
        PlayerPrefs.SetInt(Utility.PrefsPlayedTutorialKey, 1);
        TutorialFinishedText.SetActive(true);
        yield return new WaitForSeconds(3.5f);
		Utility.CurrentChallengeIndex = 0;
		Utility.CurrentChallengeMode = ChallengeMode.None;
        SceneManager.LoadScene("challenge");
    }

    public void TutorialTargetReached()
    {
        if (CurrentTutorialTargetIndex == TutorialTargets.Length)
        {
            TargetTutorialText.SetActive(false);
            StartCoroutine(EndTutorial());
        }
        else
        {
            TutorialTargets[CurrentTutorialTargetIndex++].SetActive(true);
        }
    }

}
