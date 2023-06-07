using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuUI;
    public bool isPaused;
    private GameObject Player;
    private GameObject Camera;
    // Update is called once per frame
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        isPaused = false;
        DeactivateMenu();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }
        if (isPaused)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }
    public void ActivateMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        PauseMenuUI.SetActive(true);
        Player.GetComponent<Player>().enabled = false;
        Camera.GetComponent<CameraTargetting>().enabled = false;
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        PauseMenuUI.SetActive(false);
        isPaused = false;
        Player.GetComponent<Player>().enabled = true;
        Camera.GetComponent<CameraTargetting>().enabled = true;
    }

    public void MainMenu()
    {
        DeactivateMenu();
        SceneManager.LoadScene(0);
    }
}
