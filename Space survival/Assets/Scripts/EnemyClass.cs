using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyClass : MonoBehaviour
{
    public int speed = 50000;
    [SerializeField] GameObject Player;
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
            gameObject.SetActive(false);
        }
    }
}
