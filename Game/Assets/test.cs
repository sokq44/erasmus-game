using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    
    public float movespeed;
    public float jumpforce;
    private bool jumped;

    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        rigidbody.mass = 2.0f;
        
        movespeed = 15.0f;
        jumpforce = 20f;
        jumped = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(Input.GetAxis("Horizontal") * Vector3.right * Time.deltaTime * movespeed);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !jumped)
        {
            rigidbody.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            jumped = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") 
        {
            jumped = false;
        }
    }
}
