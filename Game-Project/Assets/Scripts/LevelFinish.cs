using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject levelCompleteUI;
    // Start is called before the first frame update
    void Start()
    {
        GameOverUI.SetActive(false);
        levelCompleteUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        GameOverUI.SetActive(true);
    }

    public void LevelComplete()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        levelCompleteUI.SetActive(true);
    }
}
