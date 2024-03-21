using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource collectSound;
    void Start()
    {
        animator = GetComponent<Animator>();
        collectSound = GameObject.Find("CollectSound").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        { 
            StartCoroutine(DestroyCouroutine(0.25f));
        }
    }

    IEnumerator DestroyCouroutine(float amount)
    {
        animator.Play("Collected");
        collectSound.Play();

        yield return new WaitForSeconds(amount);

        Destroy(gameObject);
        PlayerScript player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        player.AddPoints(1);
    }
}
