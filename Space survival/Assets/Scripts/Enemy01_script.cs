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
}
