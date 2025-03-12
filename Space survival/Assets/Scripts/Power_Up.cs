using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Power_Up : MonoBehaviour
{
    public int timer = 15;

    // The power up will vanish if it's not picked within 10 seconds
    private void Awake()
    {
        StartCoroutine(VanishTimer());
    }

    IEnumerator VanishTimer()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }

}
