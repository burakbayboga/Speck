using UnityEngine;

public class LilB : MonoBehaviour
{
	public static LilB instance;

    public GameObject DeathEffect;

    private Rigidbody2D Rigidbody;
    private Collider2D Collider;
    private AudioSource AudioSource;

    public bool IsTutorial;
    public bool IsChallenge;
    public bool IsEndless;

	public bool InputEnabled;

    private float DefaultForce = 10000;

	public bool IsDead;

    void Awake()
    {
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (instance.IsDead)
		{
			Destroy(instance.gameObject);
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}

        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
        AudioSource = GetComponent<AudioSource>();
    }

    public void ApplyForce(Vector2 direction, float multiplier, bool fromPlayerInput)
    {
		if (fromPlayerInput && !InputEnabled)
		{
			return;
		}

        if (IsTutorial && fromPlayerInput)
        {
            TutorialController.instance.OnSwipe();
        }
        
        Rigidbody.AddForce(direction * multiplier * Time.deltaTime);
    }

    public void HandleDeath()
    {
        Instantiate(DeathEffect, transform.position, Quaternion.identity);
        GetComponent<Renderer>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        Collider.enabled = false;
        AudioSource.Play();
		IsDead = true;
    }

#if UNITY_EDITOR
    void Update()
    {
        HandleKeyboardInput();
    }
#endif

	void HandleKeyboardInput()
	{
		if (Input.GetKeyDown(KeyCode.W))
        {
            ApplyForce(Vector2.up, DefaultForce, true);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ApplyForce(Vector2.left, DefaultForce, true);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ApplyForce(Vector2.right, DefaultForce, true);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ApplyForce(Vector2.down, DefaultForce, true);
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("SmallFry") || collision.collider.CompareTag("BossProjectile") || collision.collider.CompareTag("Hazard"))
        {
            if (IsChallenge)
            {
                ChallengeController.instance.ChallengeDeath();
            }
            else
            {
                GameController.instance.EndGame();
            }
        }
    }

}
