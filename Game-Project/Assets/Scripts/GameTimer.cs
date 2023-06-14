using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float startTime = 600;  // Startzeit in Sekunden. Hier auf 5 Minuten gesetzt.

    void Update()
    {
        startTime -= Time.deltaTime;  // Timer herunterzählen

        if (startTime < 0)
        {
            startTime = 0;
            // Fügen Sie hier Logik hinzu, wenn die Zeit abgelaufen ist
        }

        // Umwandlung in Minuten und Sekunden
        int minutes = Mathf.FloorToInt(startTime / 60F);
        int seconds = Mathf.FloorToInt(startTime - minutes * 60);

        string text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Aktualisieren Sie die Anzeige auf dem Bildschirm
        timerText.text = text;
    }
}
