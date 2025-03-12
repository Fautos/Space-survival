using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{ 
    private GameObject Map, levelManager;
    private readonly int maxDistace=50; 

    // ENCAPSULATION:
    // Bullet speed must be positive
    private int _speed = 100;
    public int Speed{get{
                            return _speed;
                        } 
                    set{
                        if (value < 0)
                        {
                            _speed = 0;
                        }
                        else{
                            _speed = value;
                        }}}

    void Awake()
    {
        Map = GameObject.Find("Map_center");
        levelManager = GameObject.Find("Level_Manager");    
    }

    void FixedUpdate()
    {

        if (levelManager.GetComponent<LevelManager>().isGameActive)
        {
            // The bullet moves forward except if it reach a distance limit
            transform.Translate(Speed * Time.deltaTime * Vector3.up);
            
            if(Vector3.Distance(Map.transform.position, transform.position) >= maxDistace)
            {
                gameObject.SetActive(false);
            }
        }

    }

    // If it gets out of the limits it should be deactivated
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            gameObject.SetActive(false);
        }
    }

    // If the bullet hits another bullet both are deactivated
    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((gameObject.transform.CompareTag("EnemyBullet") && collision.CompareTag("Bullet")) || (gameObject.transform.CompareTag("Bullet") && collision.CompareTag("EnemyBullet")))
        {
            gameObject.SetActive(false);
        }
    }

}
