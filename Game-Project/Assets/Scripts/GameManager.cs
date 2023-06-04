using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ChangeLevel(int id)
    {
        StartCoroutine(DoLevelChange(id));
    }

    private IEnumerator DoLevelChange(int id)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(id);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        EditorApplication.isPlaying = false;
    }
}
