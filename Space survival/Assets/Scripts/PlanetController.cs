using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    [SerializeField] public Transform maskTransform;
    [SerializeField] Vector3 initMaskScale;
    [SerializeField] private float time2complete = 5.0f, elapsedTime=0.0f, percentage=0.0f;
    [SerializeField] public bool validPlanet = false;
    [SerializeField] private LevelManager LevelManager;
    private int _points = 5;
    public int Points{ get {return _points;} 
                        set { 
                            if(value<0)
                            {_points = 0;}
                            else
                            {_points = value;} 
                            }}

    // Start is called before the first frame update
    void Start()
    {
        LevelManager = GameObject.Find("Level_Manager").GetComponent<LevelManager>();

        // Save the "completed" values of the mask
        initMaskScale = maskTransform.localScale;

        // The mask stars hidden
        maskTransform.localScale = new Vector3(0, 0, 0);
    }

    // When the spaceship enters the planet it progress towards its conquest
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && validPlanet == false)
        {
            elapsedTime += Time.deltaTime;
            percentage = elapsedTime/time2complete;

            maskTransform.localScale = initMaskScale * percentage;

            if (elapsedTime >= time2complete)
            {
                PlanetCompleted();
            }
        }
    }

    // If the spaceship leaves before the planet is completed, the timer must be restarted
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && validPlanet != true)
        {
            elapsedTime = 0.0f;
            maskTransform.localScale = new Vector3(0, 0, 0);
        }      
    }

    // Function to set the behaviour of the planet onces it's completed
    void PlanetCompleted()
    {
        LevelManager.AddPoints(Points);
        validPlanet = true;
    }

    public void ResetPlanet()
    {
        validPlanet = false;
        maskTransform.localScale = new Vector3(0, 0, 0);
        elapsedTime = 0.0f;
    }
}
