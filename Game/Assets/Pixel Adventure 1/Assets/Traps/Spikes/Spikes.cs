using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerScript player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
            player.TakeDamage(25.0f);
        }
    }
}
