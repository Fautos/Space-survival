using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE:
// General behaviour applied to "Enemy 01"
public class Enemy01_script : EnemyClass
{

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveToPayer();        
    }

    // Polymorphism:
    // Change the kill method because this enemy doesn't drop anything
    protected override void KillEnemy()
    {
        // We add the score to the player

        // At the end we destroy/disable the game object
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
