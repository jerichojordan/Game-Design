using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Concurrent;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private TextMeshProUGUI reloadPromptText;

    private Player player;
    float _tmpHealth;
    private int currentSceneNumber;
    private int _enemyCount;
    private void Start()
    {
        _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        player = GameObject.FindObjectOfType<Player>();
        currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
        // Stelle sicher, dass du deine Textfelder im Unity-Editor zuweist
        if (healthText == null || ammoText == null || missionText == null)
        {
            Debug.LogError("Ein oder mehrere UI-Elemente wurden nicht zugewiesen.");
            return;
        }
        _tmpHealth = 100f;
        // Setze den Starttext für jedes UI-Element
        healthText.text = "Health: 100";
        ammoText.text = "Ammo: 50";
        if (currentSceneNumber == 3) missionText.text = "Mission: Eliminate the boss";
        else missionText.text = "Mission: Eliminate all the enemies";
    }

    // Update is called once per frame
    void Update()
    {
        _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (player.HitPoint < 0f)
            _tmpHealth = 0f;
        else
            _tmpHealth = player.HitPoint;

        healthText.text = "Health: " + _tmpHealth;
        ammoText.text = "Ammo: " + player.currentAmmo;
        if (currentSceneNumber == 2 && _enemyCount == 0) missionText.text = "Mission: Go to the next area";
        
        if (player.currentAmmo <= 2)
        {
            reloadPromptText.enabled = true;
        }
        else
        {
            reloadPromptText.enabled = false;
        }
    }
}
