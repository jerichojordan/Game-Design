using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuUI;
    public bool isPaused;
    private GameObject Player;
    [SerializeField] GameObject Campos;
    // Update is called once per frame
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
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
        Time.timeScale = 0f;
        AudioListener.pause = true;
        PauseMenuUI.SetActive(true);
        Player.GetComponent<Player>().enabled = false;
        Campos.SetActive(false);
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        PauseMenuUI.SetActive(false);
        isPaused = false;
        Player.GetComponent<Player>().enabled = true;
        Campos.SetActive(true);
    }

    public void MainMenu()
    {
        DeactivateMenu();
        SceneManager.LoadScene(0);
    }
}
