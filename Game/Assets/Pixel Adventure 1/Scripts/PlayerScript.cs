using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private GameObject HPPrefab;

    [SerializeField] private Joystick joystick;
    [SerializeField] private Button jumpBtn;
    [SerializeField] private Text pointsText;
    [SerializeField] private Text timerText;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource backgroundMusic;

    [SerializeField] private float movespeed;
    [SerializeField] private float jumpforce;
    [SerializeField] private float knockbackForce;
    [SerializeField] private int playerPoints;
    [SerializeField] private bool jumped;
    [SerializeField] private int jumpCount;
    [SerializeField] private int fruitsOnScene;

    [SerializeField] private float timer;

    public static float hitPoints;

    void Start()
    {
        FindReferences();
        DisplayHitPoints();

        rigidbody.mass = 2.0f;
        rigidbody.freezeRotation = true;

        jumpBtn.onClick.AddListener(jump);

        movespeed = 5.0f;
        jumpforce = 15.0f;
        jumped = false;
        jumpCount = 0;
        knockbackForce = 5.0f;
        playerPoints = 0;
        timer = 25.0f;
        fruitsOnScene = GameObject.FindGameObjectsWithTag("Fruit").Length;
    }

    void Update()
    {
        Run();
        UpdateTimer();

        if (playerPoints == fruitsOnScene)
            StartCoroutine(EndGame(true));

        if (PlayerScript.hitPoints <= 0 || timer <= 0)
            StartCoroutine(EndGame(false));
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

    void UpdateTimer()
    {
        timer -= Time.deltaTime;
        timerText.text = "Time: " + (int)timer;
    }

    void FindReferences()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        joystick = GameObject.Find("Floating Joystick").GetComponent<Joystick>();
        jumpBtn = GameObject.Find("JumpButton").GetComponent<Button>();
        pointsText = GameObject.Find("Points").GetComponent<Text>();
        timerText = GameObject.Find("Timer").GetComponent<Text>();

        hitSound = GameObject.Find("HitSound").GetComponent<AudioSource>();
        backgroundMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
    }

    public void TakeDamage(float amount)
    {
        StartCoroutine(SleepTakeDmg(0.5f, amount));
    }

    public void AddPoints(int amount)
    {
        playerPoints += amount;
        pointsText.text = "POINTS: " + playerPoints;
    }

    public void DisplayHitPoints()
    {
        RectTransform hitPointsCanvas = GameObject.Find("HitPoints").GetComponent<RectTransform>();
        hitPointsCanvas.sizeDelta = new Vector2(150 * hitPoints, hitPointsCanvas.sizeDelta.y);
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
        backgroundMusic.Stop();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            enemy.SetActive(false);

        yield return new WaitForSeconds(0.5f);  

        if (didWin)
            ScenesLogic.NextLevel(false);
        else
            ScenesLogic.NextLevel(true);
    }

    IEnumerator SleepTakeDmg(float timeAmount, float hpAmount)
    {
        animator.Play("Hit");   
        hitSound.Play();
        PlayerScript.hitPoints -= hpAmount;

        rigidbody.AddForce(Vector2.up * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(timeAmount);

        animator.SetBool("hit", false);
        DisplayHitPoints();
    }
}