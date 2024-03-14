using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            audioSource.Play();

            animator.SetBool("Collected", true);
            Destroy(gameObject, 0.7f);

            var player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
            player.AddPoints(1);
        }
    }
}
