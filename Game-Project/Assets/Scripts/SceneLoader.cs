using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void CreditsGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        //Application.Quit();
        EditorApplication.ExitPlaymode();
        Debug.Log("Quit!");
    }
}
