using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int speed = 100;

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

    // If it gets out of the limits
    void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }

    // If it hits an enemy
    void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);    
    }
}
