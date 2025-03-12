using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private TMP_Text playerName, playerLevel, playerScore, messages;
    [SerializeField] private List<GameObject> level_configs, currentPlanets, currentSpawners;
    [SerializeField] public bool isGameActive;
    [SerializeField] private int gameState=0, level=1;

    // ENCAPSULATION:
    private int _score;
    public int Score{get{return _score;} set{if(value < 0) {_score = 0;} else{_score = value;}}}
    
    // Start is called before the first frame update
    void Start()
    {
        // Player's GameObject
        player = GameObject.Find("Spaceship");

        // The level_config is the GameObject where the different configurations are stored
        level_configs = GetChildGameObjects("Level_Manager/Game_configs");

        // Imprimir los nombres de los GameObjects hijos para verificar
        foreach (GameObject child in level_configs)
        {
            Debug.Log("Hijo encontrado: " + child.name);
        }

        // Get the user's name
        if (GameManager.Instance != null)
        {
            playerName.text = "Player: " + GameManager.Instance.userName;
        }
        else
        {
            playerName.text = "Player: Pilot";
        }

        // Initialize some variables
        gameState = 0;
        Score = 0;
        isGameActive = true;
        level = 1;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If the game is active you can play
        if (isGameActive)
        {
            switch (gameState)
            {
                case 0:
                {
                    // At the start the level must be generated
                    GenerateLevel();
                    gameState = 1;
                    break;
                }
                case 1:
                {
                    // Once the level is generated, we check if the planets are completed to pass to the next level
                    if(AllPlanetsCompleted())
                    {
                        gameState = 2;
                    }

                    break;
                }
                case 2:
                {
                    // If the level is completed we reward the player and pass to the next level
                    FinishLevel();
                    gameState = 0;

                    break;
                }
            }
            
        }
        else
        {
            // If player dies
            GameOver();
        } 
        
        if (Input.GetKey(KeyCode.Escape))
        {
            isGameActive = false;
        }

        // Update the level and score
        playerLevel.text = "Level: "+ level;
        playerScore.text = "Score: " + Score;

    }

    #region Level creation
    private void GenerateLevel()
    {
        int config = 0, activeSpawners = 1, activePlanets = 1;

        // Check if any configuration is active. If so, disable it
        if (!level_configs.All(item => item.activeSelf))
        {
            level_configs.ForEach(item => item.SetActive(false));
        }

        // The level is generated acording the "level" variable
        if (level == 1 || level == 3)
        {
            config = 0;
            activeSpawners = 1;
            activePlanets = 1;
        }
        else if (level == 2 || level == 4)
        {
            config = 1;
            activeSpawners = 1;
            activePlanets = 2;
        }
        else if (level == 5)
        {
            config = 0;
            activeSpawners = 2;
            activePlanets = 3;
        }
        else if (level > 5 && level < 10)
        {
            config = Random.Range(0,2);
            activeSpawners = 2;
            activePlanets = 3;
        }
        else if (level == 10)
        {
            config = 0;
            activeSpawners = 3;
            activePlanets = 5;
        }
        else if (level > 10 && level < 20)
        {
            config = Random.Range(0,2);
            activeSpawners = Random.Range(2,5);
            activePlanets = Random.Range(3,6);
        }
        else if (level ==20)
        {
            config = 0;
            activeSpawners = 5;
            activePlanets = 5;
        }
        else
        {
            config = Random.Range(0,2);
            activeSpawners = Random.Range(3,6);
            activePlanets = Random.Range(3,6);
        }

        LevelCreation(config, activeSpawners, activePlanets);

    }

    private void LevelCreation(int config, int activeSpawners, int activePlanets)
    {
        GameObject planetsGO = null, spawnersGO = null;

        // First we active the level configuration
        level_configs[config].SetActive(true);
        
        // Then we get the planets and spawners game object
        List<GameObject> childComponents = GetChildGameObjects(level_configs[config]);

        foreach (GameObject component in childComponents)
        {
            if(component.name.Contains("Planets"))
            {
                planetsGO = component;
            }
            else if(component.name.Contains("Spawners"))
            {
                spawnersGO = component;
            }
        }

        if (planetsGO == null || spawnersGO == null)
        {
            Debug.Log("Error, no spawners or planets found in the game configuration");
        }

        List<GameObject> planets = GetChildGameObjects(planetsGO);
        List<GameObject> spawners = GetChildGameObjects(spawnersGO);

        // And deactive all their children
        DeactiveAllElements(planets);
        DeactiveAllElements(spawners);

        // Then we only active the planets we want
        // If it's the first configuration
        if (config == 0)
        {
            // The center planet must be allways active
            currentPlanets.Add(planets[0]);
            planets.RemoveAt(0);

            // If we want between 2 and 4 planets no matter the number but we will active 2 planets
            if (activePlanets > 1 && activePlanets <= 4)
            {
                for(int i = 0; i < 2; i++)
                {
                    int index = Random.Range(0, planets.Count);

                    currentPlanets.Add(planets[index]);
                    planets.RemoveAt(index);
                }
            }
            else if (activePlanets > 4)
            {
                currentPlanets.AddRange(planets);
            }
        }
        else if (config ==1)
        {
            if (activePlanets > 0 && activePlanets <= 3)
            {
                for(int i = 0; i < 2; i++)
                {
                    int index = Random.Range(0, planets.Count);

                    currentPlanets.Add(planets[index]);
                    planets.RemoveAt(index);
                }
            }
            else
            {
                currentPlanets.AddRange(planets);
            }
        }

        // The spawners can be chosen randomly
        for(int i = 0; i < activeSpawners; i++)
        {
            int index = Random.Range(0, spawners.Count);

            currentSpawners.Add(spawners[index]);
            spawners.RemoveAt(index);
        }

        // Lastly we active all the planets and spawners
        ActiveAllElements(currentPlanets);
        //currentPlanets.ForEach(item => item.GetComponent<PlanetController>().ResetPlanet());

        ActiveAllElements(currentSpawners);

    }

    void FinishLevel()
    {
        string msg="Level completed!!!";
        Debug.Log("Level completed");

        // First we deactive the elements
        DeactiveAllElements(level_configs);

        // Then we level up
        level ++;

        // And reward the player
        if(level % 2 == 0)
        {
            player.GetComponent<Player_Controller>().IncreaseSpeed(5000);
            msg += "\nMovement speed increased";
        }
        else if(level % 5 == 0)
        {
            player.GetComponent<Player_Controller>().IncreaseFireRate(0.1f);
            msg += "\nFire rate increased";
        }
        
        StartCoroutine(WriteMessage(msg));
        
    }
    #endregion

    private void GameOver()
    {
        // Stop game

        // Write user's level and score

        // GameOver screen
    }

    #region Auxiliary functions
    // METHOD OVERLOAD:
    // In this case I want this function to work either with a string or a GameObject
    List<GameObject> GetChildGameObjects(string parentName)
    {
        List<GameObject> childObjects = new List<GameObject>();
        // First we search for the parent
        GameObject parent = GameObject.Find(parentName);

        if (parent != null)
        {
            // Then we run through all the parent children and add them to a list
            foreach (Transform child in parent.transform)
            {
                childObjects.Add(child.gameObject);
            }
        }
        else
        {
            Debug.LogError("GameObject '" + parentName + "' cannot be found.");
        }

        return childObjects;
    }

    List<GameObject> GetChildGameObjects(GameObject parent)
    {
        List<GameObject> childObjects = new();
        // First we search for the parent

        if (parent != null)
        {
            // Then we run through all the parent children and add them to a list
            foreach (Transform child in parent.transform)
            {
                childObjects.Add(child.gameObject);
            }
        }
        else
        {
            Debug.LogError("GameObject '" + parent.name + "' cannot be found.");
        }

        return childObjects;
    }

    private void ActiveAllElements(List<GameObject> ElementsList)
    {
        if (!ElementsList.All(item => item.activeSelf))
        {
            ElementsList.ForEach(item => item.SetActive(true));
        }
    }

    private void DeactiveAllElements(List<GameObject> ElementsList)
    {
        if (!ElementsList.All(item => !item.activeSelf))
        {
            ElementsList.ForEach(item => item.SetActive(false));
        }
    }

    private bool AllPlanetsCompleted()
    {
        bool levelComplete = true;

        foreach (GameObject planet in currentPlanets)
        {
            if (!planet.GetComponent<PlanetController>().validPlanet)
            {
                levelComplete = false;
                break;
            }
        }

        return levelComplete;
    }

    public void AddPoints(int points)
    {
        Score += points;
    }

    IEnumerator WriteMessage(string message)
    {
        messages.text = message;
        yield return new WaitForSeconds(15);
        if (messages.text == message)
        {
            messages.text = "";
        }
    }

    #endregion

}
