using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy03_script : EnemyClass
{

    [SerializeField] protected float distance2Player, minDistance=20;
    [SerializeField] protected bool canShoot=true;
    [SerializeField] protected float rotationSpeed= 5, shootCD = 1.0f, timeShooted=0.0f;
    // Update is called once per frame
    void FixedUpdate()
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
        }

        canShoot = false;
        timeShooted = shootCD;
    }
}
