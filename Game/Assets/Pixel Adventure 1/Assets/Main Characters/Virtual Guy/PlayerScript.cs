using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Joystick joystick;
    [SerializeField] private Button jumpBtn;
    [SerializeField] private Text pointsText;
    [SerializeField] private Text endGameText;

    [SerializeField] private float movespeed;
    [SerializeField] private float jumpforce;
    [SerializeField] private float hitPoints;
    [SerializeField] private bool jumped;
    [SerializeField] private int jumpCount;
    [SerializeField] private int playerPoints;

    void Start()
    {
        FindReferences();

        rigidbody.mass = 2.0f;
        rigidbody.freezeRotation = true;

        jumpBtn.onClick.AddListener(jump);

        pointsText.text = "POINTS: 0";
        endGameText.enabled = false;

        movespeed = 5.0f;
        jumpforce = 15.0f;
        jumped = false;
        jumpCount = 0;
        playerPoints = 0;
        hitPoints = 100.0f;
    }

    void Update()
    {
        Run();

        if (playerPoints == 8)
        {
            StartCoroutine(EndGame(true));
        }

        if (hitPoints <= 0)
        {
            StartCoroutine(EndGame(false));
        }
    }

    void jump()
    {
        if (!jumped && jumpCount == 0)
        {
            rigidbody.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            jumped = true;
            animator.SetBool("jumping", true);

            jumpCount = 1;
        }
        else if (jumped && jumpCount == 1)
        {
            animator.SetBool("doubleJump", true);
            rigidbody.AddForce(Vector2.up * (jumpforce * 0.6f), ForceMode2D.Impulse);
            jumpBtn.interactable = false;
            StartCoroutine(StopDoubleJump());
        }
    }

    void Run()
    {
        float horizontalInput = joystick.Horizontal;

        animator.SetBool("moving", true);
        transform.Translate(horizontalInput * Vector3.right * Time.deltaTime * movespeed);

        if (horizontalInput < 0)
            spriteRenderer.flipX = true;
        else if (horizontalInput > 0)
            spriteRenderer.flipX = false;
        else
            animator.SetBool("moving", false);
    }

    void FindReferences()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        joystick = GameObject.Find("Floating Joystick").GetComponent<Joystick>();
        jumpBtn = GameObject.Find("JumpButton").GetComponent<Button>();
        pointsText = GameObject.Find("Points").GetComponent<Text>();
        endGameText = GameObject.Find("EndGame").GetComponent<Text>();
    }

    public void TakeDamage(float amount)
    {
        hitPoints -= amount;
        StartCoroutine(SleepTakeDmg(0.5f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Block")
        {
            jumped = false;
            jumpCount = 0;
            animator.SetBool("jumping", false);
        }
    }

    public void AddPoints(int amount)
    {
        playerPoints += amount;
        pointsText.text = "POINTS: " + playerPoints;
    }

    IEnumerator StopDoubleJump()
    {
        yield return new WaitForSeconds(0.18f);

        jumpCount = 0;
        jumpBtn.interactable = true;
        animator.SetBool("doubleJump", false);
    }
    IEnumerator EndGame(bool didWin)
    {
        this.enabled = false;

        yield return new WaitForSeconds(1.0f);

        Time.timeScale = 0;
        pointsText.enabled = false;
        endGameText.text = didWin ? "You win!" : "Game Over!";
        endGameText.enabled = true;
    }

    IEnumerator SleepTakeDmg(float amount)
    {
        animator.SetBool("hit", true);

        yield return new WaitForSeconds(amount);

        animator.SetBool("hit", false);
    }
}