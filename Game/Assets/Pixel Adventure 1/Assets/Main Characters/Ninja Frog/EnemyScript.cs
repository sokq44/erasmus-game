using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private Rigidbody2D rigidbody;

    [SerializeField] private float moveSpeed;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        moveSpeed = 2;
        rigidbody.gravityScale = 0;
    }

    void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyPath"))
        {
            transform.Rotate(0, 180, 0);
        }

        if (collision.gameObject.tag == "Player")
        {
            playerScript.TakeDamage(1);
        }
    }
}
