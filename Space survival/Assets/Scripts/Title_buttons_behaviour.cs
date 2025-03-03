using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For SceneManager.LoadScene
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


// For EditorApplication (only used in edition mode)
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NewBehaviourScript : MonoBehaviour
{
    // Input field gameObject
    [SerializeField] TMP_InputField userNameField;

    // Main game scene will be load when you click on the play button
    public void PlayButton()
    {
        /*if (userNameField.text.Length == 0)
        {
            Persistent_manager.Instance.userName = "Player";
        }*/
        SceneManager.LoadScene(1);
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
        /*Persistent_manager.Instance.userName = userNameField.text;
        Debug.Log("User name: " + Persistent_manager.Instance.userName);*/
        Debug.Log("User name: " + userNameField.text);
    }
}
