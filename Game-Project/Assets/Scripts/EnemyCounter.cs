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
        if (enemyCount == 1) { 
            enemyCountText.text = enemyCount + " Enemy remaining";
        } 
        else enemyCountText.text = enemyCount + " Enemies remaining";
    }

    private void Update()
    {
        if (enemyCount == 1) {
            LastEnemyArrow.SetActive(true);
            lastEnemy = GameObject.FindGameObjectWithTag("Enemy");
            Vector3 direction = lastEnemy.transform.position - player.transform.position;

            // Apply the offset to the direction vector
            Vector3 targetPosition = player.transform.position + direction.normalized * arrowOffset;

            // Calculate the angle between the direction vector and arrow's forward vector
            float angle = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x) * Mathf.Rad2Deg;

            // Set the rotation of the arrow towards the last enemy
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
