using UnityEngine;
using System;

public class LilB : MonoBehaviour
{

    public GameObject DeathEffect;

    private Rigidbody2D Rigidbody;
    private Collider2D Collider;
    private AudioSource AudioSource;

    public bool IsTutorial;
    public bool IsChallenge;
    public bool IsEndless;

    private float DefaultForce = 10000;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
        AudioSource = GetComponent<AudioSource>();
    }

    public void ApplyForce(Vector2 direction, float multiplier, bool fromPlayerInput)
    {
        if (IsTutorial && fromPlayerInput)
        {
            BasicTutorialController.instance.OnLilBApplyForce();
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
    }

    void Update()
    {
        //if (GameController.instance.IsGameOver)
        //{
        //    return;
        //}

        if (Input.GetKeyDown(KeyCode.W))
        {
            Rigidbody.AddForce(Vector2.up * DefaultForce * Time.deltaTime);
            if (IsTutorial)
            {
                BasicTutorialController.instance.OnLilBApplyForce();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Rigidbody.AddForce(Vector2.left * DefaultForce * Time.deltaTime);
            if (IsTutorial)
            {
                BasicTutorialController.instance.OnLilBApplyForce();
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Rigidbody.AddForce(Vector2.right * DefaultForce * Time.deltaTime);
            if (IsTutorial)
            {
                BasicTutorialController.instance.OnLilBApplyForce();
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Rigidbody.AddForce(Vector2.down * DefaultForce * Time.deltaTime);
            if (IsTutorial)
            {
                BasicTutorialController.instance.OnLilBApplyForce();
            }
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
