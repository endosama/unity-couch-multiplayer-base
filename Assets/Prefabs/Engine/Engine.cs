using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public GameObject player;
    public static Engine instance;
    private int spawned = 0;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in players)
        {
            Camera.main.GetComponent<MultipleTargetCamera>().targets.Add(p.transform);
        }
    }

    // Update is called once per frame
    public void CreatePlayer()
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        var spawn = spawnPoints[spawned++ % spawnPoints.Length];

        var playerInstance = GameObject.Instantiate(player, spawn.transform.position, Quaternion.identity);
        Camera.main.GetComponent<MultipleTargetCamera>().targets.Add(playerInstance.transform);

    }
}
