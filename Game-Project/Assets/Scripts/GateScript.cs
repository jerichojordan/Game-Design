using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GateScript : MonoBehaviour
{
    [SerializeField] int scenenumber;
    private int enemiesnumber;
    private bool isPlaying = false;
    private Player player;
    private GameTimer gameTimer;
    void Start()
    {
        gameTimer = GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameTimer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemiesnumber = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && enemiesnumber == 0)
        {
            PlayerPrefs.SetFloat(scenenumber.ToString()+"time", gameTimer.startTime);
            PlayerPrefs.SetFloat(scenenumber.ToString()+"HP", player.HitPoint);
            SceneManager.LoadScene(scenenumber);
            Debug.Log("Loading Next Scene");
        } else if (collision.CompareTag("Player") && enemiesnumber > 0) Debug.Log("Enemies Remaining : " + enemiesnumber);
       
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length != enemiesnumber)
        {
            enemiesnumber =  GameObject.FindGameObjectsWithTag("Enemy").Length;
        }
        if(!isPlaying && enemiesnumber == 0)
        {
            isPlaying = true;
            this.gameObject.GetComponent<ParticleSystem>().Play();
        }
    }
}
