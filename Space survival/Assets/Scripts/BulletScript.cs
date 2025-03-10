using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int speed = 100, maxDistace=50;
    [SerializeField] GameObject Map;

    void Awake()
    {
        Map = GameObject.Find("Map_center");    
    }

    void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.up);
        
        if(Vector3.Distance(Map.transform.position, transform.position) >= maxDistace)
        {
            gameObject.SetActive(false);
        } 

    }

    // If it gets out of the limits
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("EnemyBullet"))
        {
            gameObject.SetActive(false);
        }
    }

}
