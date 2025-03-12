using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public bool playerDead = false;
    [SerializeField] private bool canShoot=true;
    [SerializeField] private int _moveForce = 30000, repulsionForce = 1250000, rotationSpeed = 5;
    [SerializeField] private float _shootCD = 1.0f, timeShooted=0.0f;
    [SerializeField] Vector3 mapCenter, forcedDirection;
    [SerializeField] GameObject Map, Shield;
    [SerializeField] LevelManager levelManager;
    private Rigidbody2D playerRb;
    private readonly int moveForceLimit = 75000;
    private readonly float shootCDLimit = 0.1f;

    // ENCAPSULATION:
    // There is a limit for the movement speed and the fire rate
    public int MoveForce{get{return _moveForce;}
                        set{
                            if(value > moveForceLimit)
                            {_moveForce = moveForceLimit;}
                            else{_moveForce = value;}
                        }}
    public float ShootCD{get{return _shootCD;}
                        set{
                            if(value < shootCDLimit)
                            {_shootCD = shootCDLimit;}
                            else{_shootCD = value;}
                        }}

    void Start()
    {
        // Get the player rigidbody
        playerRb = GetComponent<Rigidbody2D>();
        // And the level manager
        levelManager = GameObject.Find("Level_Manager").GetComponent<LevelManager>();

        mapCenter = Map.transform.position;

    }

    void FixedUpdate()
    {
        if(levelManager.isGameActive)
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
        else
        {
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = 0;
        }
    }

    #region Actions
    // Function to move the spaceship
    public void MovePlayer()
    {
        // To get the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Move the player
        playerRb.AddForce(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * MoveForce * Time.deltaTime);

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
        timeShooted = ShootCD;
    }
    #endregion

    #region Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.transform.name);

        // If the player tries to leave the map, the spaceship must be stopped and forced to enter again
        if(collision.transform.CompareTag("Border"))
        {
            Debug.Log("Border reached by the player");

            // First we stop the player
            playerRb.velocity *= 0.01f; //Vector3.zero;

            // Then we force the player to enter again
            forcedDirection = (mapCenter - transform.position).normalized;
            playerRb.AddForce(forcedDirection * repulsionForce * Time.deltaTime);
        } 
        else if (collision.transform.CompareTag("Power_Up") && collision.gameObject.activeSelf)
        {
            // If you pick up a shield power up, the shield should activate
            if(collision.name.Contains("Shield_PowerUp"))
            {
                if(!Shield.gameObject.activeSelf)
                {
                    Shield.SetActive(true);
                    Destroy(collision.gameObject);
                }
            }
        }
        else if(collision.transform.CompareTag("EnemyBullet") && collision.gameObject.activeSelf)
        {
            GameOver();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If an enemy or an enemy's bullet hits the player the game ends
        if (collision.transform.CompareTag("Enemy")  && collision.gameObject.activeSelf)
        {
            GameOver();
        }
    }
    #endregion

    #region PowerUps
    public void IncreaseSpeed(int speed)
    {
        MoveForce += speed;
    }

    public void IncreaseFireRate(float fireRateCDR)
    {
        ShootCD -= fireRateCDR;
    }
    #endregion

    public void GameOver()
    {
        // When the player dies the game must be stopped
        playerDead = true;
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = 0;
        levelManager.isGameActive = false;
    }

}
