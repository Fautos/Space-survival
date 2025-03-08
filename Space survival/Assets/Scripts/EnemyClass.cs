using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

// INHERITANCE:
// General enemies behaviour
public class EnemyClass : MonoBehaviour
{
    public int speed = 50000, dropProbability = 50;
    [SerializeField] protected GameObject Player, DropObject, Map;
    [SerializeField] protected Vector3 mapCenter;
    protected Rigidbody2D enemyRB;

    void Awake()
    {
        Player = GameObject.Find("Spaceship");
        enemyRB = GetComponent<Rigidbody2D>();
        Map = GameObject.Find("Map_limits");
        mapCenter = Map.transform.position;
    }

    protected virtual void MoveToPayer()
    {
        Vector3 direction = (Player.transform.position - transform.position).normalized;

        enemyRB.AddForce(direction* speed * Time.deltaTime);
        //transform.Translate(direction * speed * Time.deltaTime);

    }

    protected virtual void ScapeFromPlayer()
    {
        Vector3 direction = -1*(Player.transform.position - transform.position).normalized;
        
        enemyRB.AddForce(speed * Time.deltaTime * direction);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Bullet"))
        {
            Debug.Log("Enemy killed"); 

            KillEnemy();        
        }
    }

    // If the enemy reaches the map limits it will be pushed towards the center but in the player's direction
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.transform.name);
        if(collision.transform.CompareTag("Border"))
        {
            Debug.Log("Enemy out of limits");

            // First we stop the enemy
            enemyRB.velocity *= 0.01f; //Vector3.zero;

            // Then we force the enemy to enter again and push it towards the player
            Vector3 forceDirection = (mapCenter - transform.position).normalized;
            Vector3 direction = (Player.transform.position - transform.position).normalized;            

            enemyRB.AddForce(speed*100 * Time.deltaTime * (forceDirection + direction).normalized);
        }
    }

    protected virtual void KillEnemy()
    {
        // We add the score to the player


        // If the enemy can drop we roll the dice
        if (Random.Range(0,100) <= dropProbability)
        {
            Debug.Log("Somthing dropped");
        }

        // At the end we destroy/disable the game object
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
