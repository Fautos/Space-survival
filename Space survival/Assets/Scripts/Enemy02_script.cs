using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02_script : EnemyClass
{

    [SerializeField] public GameObject Shield;
    [SerializeField] public float cdShield = 5.0f, timelapsed = 0.0f;
    [SerializeField] public bool shieldBroken = false;

    // Start is called before the first frame update
    void Start()
    {
        // The shield must start enable
        if(!Shield.activeInHierarchy)
        {
            Shield.SetActive(true);
        }
    }

    void FixedUpdate()
    {

        // First we check the state of the shield
        CheckShield();

        // If the shield is up the enemy chase the player
        if (shieldBroken == false)
        {
            MoveToPayer();
            timelapsed = 0.0f;
        }

        // Otherwise it will run away from the player until the shield is restored
        else
        {
            ScapeFromPlayer();
            timelapsed += Time.deltaTime;

            if (timelapsed >= cdShield)
            {
                Shield.SetActive(true);
            }
        }
        
    }

    private void CheckShield()
    {
        if(!Shield.activeInHierarchy)
        {
            shieldBroken = true;
        }
        else
        {
            shieldBroken = false;
        }
    }

}
