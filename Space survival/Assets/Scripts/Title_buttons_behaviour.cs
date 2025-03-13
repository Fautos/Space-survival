using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For SceneManager.LoadScene
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.UIElements;



// For EditorApplication (only used in edition mode)
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NewBehaviourScript : MonoBehaviour
{
    // Input field gameObject
    [SerializeField] TMP_InputField userNameField;
    [SerializeField] TMP_Text playerText, levelText, scoreText;
    [SerializeField] Toggle musicToggle;
    public GameObject loadingScreen, OptionsScreen;

    public void Start()
    {
        if(GameManager.Instance!=null && GameManager.Instance.userName != null)
        {
            userNameField.text = GameManager.Instance.userName;
        }    
    }

    public void LateUpdate()
    {
        LeaderboardUpdate();        
    }

    // To create the leaderboard
    public void LeaderboardUpdate()
    {
        playerText.text = GameManager.Instance.Leaderboard.GetElement("names");
        levelText.text = GameManager.Instance.Leaderboard.GetElement("levels");
        scoreText.text = GameManager.Instance.Leaderboard.GetElement("scores");
    }

    // Main game scene will be load when you click on the play button
    public void PlayButton()
    {
        if (userNameField.text.Length == 0)
        {
            GameManager.Instance.userName = "Pilot";
        }
        //SceneManager.LoadScene(1);
        LoadScene(1);
    }

    public void SettingsButton()
    {
        if(OptionsScreen.activeInHierarchy)
        {
            musicToggle.isOn = GameManager.Instance.musicOn;
            OptionsScreen.SetActive(false);
        }
        else
        {
            OptionsScreen.SetActive(true);
            musicToggle.isOn = GameManager.Instance.musicOn;
        }
    }

    public void MusicOnOff()
    { 
        GameManager.Instance.musicOn = !GameManager.Instance.musicOn;
    }

    // You will exit the game when you press the exit button
    public void ExitButton()
    {
        // And then we close the game
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit();
        #endif
    }

    // To get the user name
    public void UserNameInput()
    {
        GameManager.Instance.userName = userNameField.text;
        userNameField.text = GameManager.Instance.userName;
        Debug.Log("User name changed to: " + GameManager.Instance.userName);   
    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
