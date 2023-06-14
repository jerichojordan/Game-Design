using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private TextMeshProUGUI reloadPromptText;

    private Player player;
    float _tmpHealth = 100f;

    private void Start()
    {

        player = GameObject.FindObjectOfType<Player>();

        // Stelle sicher, dass du deine Textfelder im Unity-Editor zuweist
        if (healthText == null || ammoText == null || missionText == null)
        {
            Debug.LogError("Ein oder mehrere UI-Elemente wurden nicht zugewiesen.");
            return;
        }

        // Setze den Starttext für jedes UI-Element
        healthText.text = "Health: 100";
        ammoText.text = "Ammo: 50";
        missionText.text = "Mission: Eliminate all the enemies";
    }

    // Update is called once per frame
    void Update()
    {

        if (player.HitPoint < 0f)
            _tmpHealth = 0f;
        else
            _tmpHealth = player.HitPoint;

        healthText.text = "Health: " + _tmpHealth;
        ammoText.text = "Ammo: " + player.currentAmmo;

        if (player.currentAmmo <= 0)
        {
            reloadPromptText.enabled = true;
        }
        else
        {
            reloadPromptText.enabled = false;
        }
    }
}
