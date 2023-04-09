using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int playerScore;
    
    [SerializeField] private TMP_Text scoreText;

    public void PointsCollected(int value)
    {
        playerScore += value;
        scoreText.text = "Points: " + playerScore.ToString();
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
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
