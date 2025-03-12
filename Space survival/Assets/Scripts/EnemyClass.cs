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
    // ENCAPSULATION:
    // Speed, dropProbability and points must be positive
    private int _speed = 50000, _dropProbability = 20, _points = 1;
    public int Speed{get{return _speed;} 
                        set{
                            if(value < 0)
                            {
                                _speed = 0;
                            }
                            else{
                                _speed = value;
                            }
                        }}
    public int DropProbability{get{return _dropProbability;} 
                        set{
                            if(value < 0)
                            {
                                _dropProbability = 0;
                            }
                            else if(value > 100)
                            {
                                _dropProbability = 100;
                            }
                            else {
                                _dropProbability = value;
                            }
                        }}

    public int Points{get{return _points;}
                    set{
                      if(value < 0)
                      {
                        _points = 0;
                      }
                      else{_points=value;}
                    }}

    // Some protected variables so they can be accesss when inheritance
    [SerializeField] protected GameObject Player, DropObject, Map;
    [SerializeField] protected LevelManager LevelManager;
    [SerializeField] protected Vector3 mapCenter;
    protected Rigidbody2D enemyRB;

    void Awake()
    {
        Player = GameObject.Find("Spaceship");
        LevelManager = GameObject.Find("Level_Manager").GetComponent<LevelManager>();
        enemyRB = GetComponent<Rigidbody2D>();
        Map = GameObject.Find("Map_center");
        mapCenter = Map.transform.position;
    } 

    # region Movement related function
    // With this function the enemy is move towards the player
    protected virtual void MoveToPayer()
    {
        Vector3 direction = (Player.transform.position - transform.position).normalized;
        enemyRB.AddForce(Speed * Time.deltaTime * direction);
    }

    // With this function the enemy runs away from the player
    protected virtual void ScapeFromPlayer()
    {
        Vector3 direction = -1*(Player.transform.position - transform.position).normalized;
        enemyRB.AddForce(Speed * Time.deltaTime * direction);
    }

    // If the enemy hits the border, it will be pushed towards the player, except if it's Enemy 3 (this behavior will be overridden)
    protected virtual void BounceFromBorder()
    {
        // First we stop the enemy
        enemyRB.velocity *= 0.01f;

        // Then we force the enemy to enter again and push it towards the player
        Vector3 forceDirection = (mapCenter - transform.position).normalized;
        Vector3 direction = (Player.transform.position - transform.position).normalized;            

        enemyRB.AddForce(Speed*100 * Time.deltaTime * (forceDirection + direction).normalized);
    }
    #endregion

    // If the enemy reaches the map limits it will be pushed towards the center but in the player's direction
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Border"))
        {
            // ABSTRACTION
            BounceFromBorder();

        } else if(collision.CompareTag("Bullet") && collision.gameObject.activeSelf)
        {
            // The bullet must be destroyed
            collision.gameObject.SetActive(false);

            // And the enemy dies (ABSTRACTION)
            KillEnemy();        
        }
    }

    protected virtual void KillEnemy()
    {
        // We add the score to the player
        LevelManager.AddPoints(Points);
        Debug.Log(DropProbability);

        // If the enemy can drop we roll the dice
        if (Random.Range(0,100) <= DropProbability && DropObject != null)
        {
            Instantiate(DropObject, transform.position, Quaternion.identity);
        }

        // At the end we destroy/disable the game object
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
