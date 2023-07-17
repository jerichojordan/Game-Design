using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    public TextMeshProUGUI enemyCountText;
    public int enemyCount;
    //[SerializeField] GameObject LastEnemyArrow;
    private GameObject lastEnemy;
    private GameObject player;
    private float arrowOffset = 0f;
    private GameObject LastEnemyArrow;
    void Start()
    {
        // Zählen Sie die anfängliche Anzahl der Gegner in der Szene
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        UpdateEnemyCountText();
        player = GameObject.FindGameObjectWithTag("Player");
        LastEnemyArrow = GameObject.FindGameObjectWithTag("LastEnemyArrow");
        LastEnemyArrow.SetActive(false);
    }

    public void EnemyKilled()
    {
        // Reduzieren Sie die Anzahl der Gegner, wenn einer getötet wird
        enemyCount--;
        UpdateEnemyCountText();
        //Debug.Log("EnemyKilled called");
    }

    void UpdateEnemyCountText()
    {
        // Aktualisieren Sie die Anzeige auf dem Bildschirm
        if (enemyCount == 1)
        {
            enemyCountText.text = enemyCount + " Enemy remaining";
        }
        else enemyCountText.text = enemyCount + " Enemies remaining";
    }

    private void Update()
    {
        if (enemyCount == 1)
        {
            if (lastEnemy == null) lastEnemy = GameObject.FindGameObjectWithTag("Enemy");
            if (!LastEnemyArrow.activeSelf) LastEnemyArrow.SetActive(true);
            var dir = lastEnemy.transform.position - LastEnemyArrow.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            LastEnemyArrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}