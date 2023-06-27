using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float _enemyCount;
    public float _enemyKilled;
    LevelFinish levelFinish;
   

    void Start()
    {
        _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        _enemyKilled = 0;
        levelFinish = GameObject.FindGameObjectWithTag("Finish").GetComponent<LevelFinish>();
    }

    private void Update()
    {
        if(_enemyCount == _enemyKilled)
        {
            levelFinish.LevelComplete();
        }
    }
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
        if (AudioListener.pause) AudioListener.pause = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f; 
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
    public void enemyKilledInc()
    {
        _enemyKilled++;
    }
    
}
