using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy03_script : EnemyClass
{

    [SerializeField] protected float distance2Player, minDistance=20;
    [SerializeField] protected bool canShoot=false;
    [SerializeField] protected float rotationSpeed= 5, shootCD = 2.0f, timeShooted = 1.0f;
    [SerializeField] protected int shootSpeed = 75;

    void FixedUpdate()
    {
        if(LevelManager.isGameActive)
        {
            // Move the enemy towards the player
            Vector3 direction = Player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // If the player is too close to the enemy it scape, otherwise it wont move but it will shoot
            distance2Player = Vector3.Distance(Player.transform.position, transform.position);

            if (distance2Player < minDistance)
            {
                ScapeFromPlayer();
            }
            else if (canShoot)
            {
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
            StopMovement();
        }
    }

    // Polymorphism:
    // Change the Bounce method because this enemy bounce away from the player
    protected override void BounceFromBorder()
    {
        // First we stop the enemy
        enemyRB.velocity *= 0.01f; //Vector3.zero;

        // The enemy 3 should be pushed away from the player
        Vector3 forceDirection = (mapCenter - transform.position).normalized;
        Vector3 direction = -1*(Player.transform.position - transform.position).normalized;            

        enemyRB.AddForce(Speed*100 * Time.deltaTime * (2*forceDirection + direction).normalized);
    }

    protected private void Shoot()
    {
        // Get an object object from the pool
        GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject("Enemy_Bullet");
        if (pooledProjectile != null)
        {
            pooledProjectile.SetActive(true); // activate it
            // position it at player
            pooledProjectile.transform.position = transform.position; 
            pooledProjectile.transform.rotation = transform.rotation;
            //Decrease the proyectile speed
            pooledProjectile.GetComponent<BulletScript>().Speed = shootSpeed;
        }

        canShoot = false;
        timeShooted = shootCD;
    }
}
