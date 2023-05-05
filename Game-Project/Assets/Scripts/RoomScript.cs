using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    private Vector2 x_range;
    private Vector2 y_range;
    [SerializeField] private float RoomNumber;
    void Start()
    {
        x_range = new Vector2(transform.position.x-(0.5f*transform.localScale.x), transform.position.x + (0.5f * transform.localScale.x));
        y_range = new Vector2(transform.position.y - (0.5f * transform.localScale.y), transform.position.y + (0.5f * transform.localScale.y));
    }

    // Update is called once per frame
    void Update()
    {
        detectEnemies(); 
    }

    public void detectEnemies()
    {
        var Characters = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in Characters)
        {
            if (enemy.transform.position.x >= x_range[0] && enemy.transform.position.x <= x_range[1])
            {
                if (enemy.transform.position.y >= y_range[0] && enemy.transform.position.y <= y_range[1])
                {
                    enemy.GetComponent<EnemyAI>().setLocation(RoomNumber);
                }
            }
        }
        var Player = GameObject.FindGameObjectWithTag("Player");
        if (Player.transform.position.x >= x_range[0] && Player.transform.position.x <= x_range[1])
        {
            if (Player.transform.position.y >= y_range[0] && Player.transform.position.y <= y_range[1])
            {
                Player.GetComponent<Player>().location=RoomNumber;
            }
        }
    }
}
