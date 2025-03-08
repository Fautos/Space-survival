using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int speed = 100;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.up);
    }

    // If it gets out of the limits
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            gameObject.SetActive(false);
        }
    }

    // If it hits a shield
    /*void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shield"))
        {
            gameObject.SetActive(false);
        }
    }*/

    // If it hits an enemy
    void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);    
    }
}
