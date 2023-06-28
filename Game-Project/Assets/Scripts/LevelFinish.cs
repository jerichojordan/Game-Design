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
    public int _enemyCount;
    public int _enemyKilled;
    bool isCounted;
    // Start is called before the first frame update
    void Start()
    {
        gameTimer = GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameTimer>();
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            activateTutorial();
        }
        GameOverUI.SetActive(false);
        levelCompleteUI.SetActive(false);
        Campos.SetActive(true);
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(CountEnemiesCoroutine());
        isCounted = false;
        //_enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        _enemyKilled = 0;
    }
    private void Update()
    {
        if (!isCounted)
        {
            _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            isCounted = true;
            Debug.Log("Counted" + _enemyCount);

        }
        if (_enemyCount == _enemyKilled && _enemyKilled != 0)
        {
            Invoke(nameof(LevelComplete), 2f);
        }
        Debug.Log("count"+_enemyCount);
        Debug.Log("kill" + _enemyKilled);
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
        Player.GetComponent<Player>().enabled = false;
        Campos.SetActive(false);
    }
    public void deactivateTutorial()
    {
        TutorialUI.SetActive(false);
        PlayerUI.SetActive(true);
    }
    public void enemyKilledInc()
    {
        _enemyKilled++;
    }

    IEnumerator CountEnemiesCoroutine()
    {
        yield return 1f;
        _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
