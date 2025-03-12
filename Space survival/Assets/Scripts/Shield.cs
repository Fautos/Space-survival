using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private bool playerShield = false;

    private void Awake()
    {
        // At first it checks if its owner is the player or the enemy
        if(transform.parent != null)
        {
            if (transform.parent.gameObject.name == "Spaceship")
            {
                playerShield = true;
            }
            else
            {
                playerShield = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the owner is the player and it's hit by an enemy bullet, the shield is destroyed but the player is saved and vice versa with the enemies
        if ((collision.transform.CompareTag("Bullet") && playerShield == false) || (collision.transform.CompareTag("EnemyBullet") && playerShield == true))
        {
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Debug.Log("Colisión eliminada");
        }
    }
}
