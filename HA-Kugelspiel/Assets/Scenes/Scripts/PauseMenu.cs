using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuUI;
    private bool isPaused;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            isPaused = !isPaused;
        }
        if(isPaused){
            ActivateMenu();
        }
        else{
            DeactivateMenu();
        }
    }
    void ActivateMenu(){
        Time.timeScale = 0;
        AudioListener.pause = true;
        PauseMenuUI.SetActive(true);
    }

    public void DeactivateMenu(){
        Time.timeScale = 1;
        AudioListener.pause = false;
        PauseMenuUI.SetActive(false);
        isPaused = false;
    }

    public void MainMenu()
    {
        DeactivateMenu();
        SceneManager.LoadScene(0);
    }
}
