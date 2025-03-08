using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    
    [SerializeField] private bool playerShield = false;

    private void Awake()
    {
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
        if ((collision.CompareTag("Bullet") && playerShield == false) || (collision.CompareTag("EnemyBullet") && playerShield == true))
        {
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
