using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

// INHERITANCE:
// General enemies behaviour
public class EnemyClass : MonoBehaviour
{
    public int speed = 50000, dropProbability = 50;
    [SerializeField] GameObject Player, DropObject;
    private Rigidbody2D enemyRB;

    void Awake()
    {
        Player = GameObject.Find("Spaceship");
        enemyRB = GetComponent<Rigidbody2D>();
    }

    public void MoveToPayer()
    {
        Vector3 direction = (Player.transform.position - transform.position).normalized;

        enemyRB.AddForce(direction* speed * Time.deltaTime);
        //transform.Translate(direction * speed * Time.deltaTime);

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Bullet"))
        {
            Debug.Log("Enemy killed");

            // If the enemy has any drop assigned it may drop
            if (DropObject != null)
            {
                Drop(Random.Range(0,100));
            } 

            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void Drop(int Probability)
    {
        if (Probability <= dropProbability)
        {
            Debug.Log("Somthing dropped");
        }
    }
}
