using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] bool canShoot=true;
    [SerializeField] int moveForce = 30000, repulsionForce = 1250000, rotationSpeed = 5;
    [SerializeField] float shootCD = 1.0f, timeShooted=0.0f;
    [SerializeField] Vector3 mapCenter, forcedDirection;
    [SerializeField] GameObject Map;
    private Rigidbody2D playerRb;

    void Start()
    {
        // Get the player rigidbody
        playerRb = GetComponent<Rigidbody2D>();

        mapCenter = Map.transform.position;

    }

    void FixedUpdate()
    {
        // ABSTRACTION:
        // Function to move the player
        MovePlayer();

        // Shooting behaviour
        if (canShoot && Input.GetMouseButton(0))
        {
            // ABSTRACTION:
            // Function to shoot
            Shoot();

        }
        else if (!canShoot)
        {
            timeShooted -= Time.deltaTime;

            if (timeShooted <= 0)
            {
                canShoot = true;
            }
        }
    }

    // Function to move the spaceship
    public void MovePlayer()
    {
        // To get the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Move the player
        playerRb.AddForce(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * moveForce * Time.deltaTime);

        // Rotate the player towards the mouse direction
        Vector3 direction = mousePosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }

    public void Shoot()
    {   
        // Get an object object from the pool
        GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject("Bullet");
        if (pooledProjectile != null)
        {
            // Activate it
            pooledProjectile.SetActive(true); 
            
            // Position it at player
            pooledProjectile.transform.position = transform.position; 
            pooledProjectile.transform.rotation = transform.rotation;
        }

        canShoot = false;
        timeShooted = shootCD;
    }

    // If the player tries to leave the map, the spaceship must be stopped and forced to enter again
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.transform.name);
        if(collision.transform.CompareTag("Border"))
        {
            Debug.Log("Border reached by the player");

            // First we stop the player
            playerRb.velocity *= 0.01f; //Vector3.zero;

            // Then we force the player to enter again
            forcedDirection = (mapCenter - transform.position).normalized;
            playerRb.AddForce(forcedDirection * repulsionForce * Time.deltaTime);

        }
    }

}
