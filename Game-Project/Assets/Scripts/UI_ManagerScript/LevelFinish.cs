using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private Text time;
    [SerializeField] private GameObject TutorialUI;
    [SerializeField] private GameObject PlayerUI;
    [SerializeField] GameObject Campos;
    private GameObject Player;
    private GameTimer gameTimer;
    private bool canCompleted;
    private float currentSceneNumber;

    //EnemyCounter
    private EnemyCounter enemyCounter;
    // Start is called before the first frame update
    void Start()
    {
        currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
        gameTimer = GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameTimer>();
        canCompleted = true;
        if (currentSceneNumber == 1)
        {
            activateTutorial();
        }
        else if (currentSceneNumber == 2 || currentSceneNumber == 4) canCompleted = false;
        GameOverUI.SetActive(false);
        levelCompleteUI.SetActive(false);
        Campos.SetActive(true);
        Player = GameObject.FindGameObjectWithTag("Player");
        enemyCounter = GameObject.FindGameObjectWithTag("UIManager").GetComponent<EnemyCounter>();
        
        
    }
    private void FixedUpdate()
    {
        if (enemyCounter.enemyCount<=0 && canCompleted)
         {
            Invoke("LevelComplete", 1f);
         }
    }

    // Update is called once per frame
    public void GameOver()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        GameOverUI.SetActive(true);
        Campos.SetActive(false);
        Player.GetComponent<Player>().enabled = false;
    }

    public void LevelComplete()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        levelCompleteUI.SetActive(true);
        float level = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetFloat("Level", level);
        float ftime = gameTimer.startTime;
        int minutes = Mathf.FloorToInt(ftime / 60F);
        int seconds = Mathf.FloorToInt(ftime - minutes * 60);

        string text = string.Format("{0:00}:{1:00}", minutes, seconds);
        time.text = text;
        //Campos.SetActive(false);
        //Player.GetComponent<Player>().enabled = false;
    }
    public void activateTutorial()
    {
        TutorialUI.SetActive(true);
        PlayerUI.SetActive(false);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        //Player.GetComponent<Player>().enabled = false;
        Campos.SetActive(false);
    }
    public void deactivateTutorial()
    {
        TutorialUI.SetActive(false);
        PlayerUI.SetActive(true);
    }
}
