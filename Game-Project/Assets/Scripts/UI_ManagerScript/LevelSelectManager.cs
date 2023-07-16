using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private GameObject LevelSelectUI;
    [SerializeField] private Button Level2;
    [SerializeField] private GameObject Lock2;
    [SerializeField] private Button Level3;
    [SerializeField] private GameObject Lock3;

    void Start()
    {
        LevelSelectUI.SetActive(false);

        if (PlayerPrefs.HasKey("Level"))
        {
            if (PlayerPrefs.GetFloat("Level") >= 3)
            {
                Level2.enabled = true;
                Lock2.gameObject.SetActive(false);
                Level3.enabled = true;
                Lock3.gameObject.SetActive(false);
            }else if (PlayerPrefs.GetFloat("Level") >= 1)
            {
                Level2.enabled = true;
                Lock2.gameObject.SetActive(false);
                Level3.enabled = false;
                Lock3.gameObject.SetActive(true);
            }
            
        }
        else
        {
            Level2.enabled = false;
            Lock2.gameObject.SetActive(true);
            Level3.enabled = false;
            Lock3.gameObject.SetActive(true);
        }
    }
    public void openLevelSelect()
    {
        LevelSelectUI.SetActive(true);
    }
    public void closeLevelSelect()
    {
        LevelSelectUI.SetActive(false);
    }
    public void startLevel(int lvl)
    {
        SceneManager.LoadScene(lvl);
    }
    
    


}
