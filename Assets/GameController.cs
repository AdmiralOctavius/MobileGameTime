using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls the main gameplay
/// </summary>
public class GameController : MonoBehaviour
{
    [Tooltip("A reference to the tile we want to spawn")]
    public Transform tile;

    [Tooltip("Reference to the obstacle we want to spawn")]
    public Transform obstacle;

    public Transform obstacle2;

    [Tooltip("Where the first tile should be placed at")]
    public Vector3 startPoint = new Vector3(0, 0, -5);

    [Tooltip("How many tiles should we create in advance")]
    [Range(1, 15)]
    public int initSpawnNum = 10;

    [Tooltip("How many tile to spawn without obstacles")]
    [Range(0, 5)]
    public int initNoObstacles = 4;
    /// <summary>
    /// Where the next tile should be spawned at.
    /// </summary>
    private Vector3 nextTileLocation;
    /// <summary>
    /// How should the next tile be rotated?
    /// </summary>
    private Quaternion nextTileRotation;
    /// <summary>
    /// Used for initialization
    /// </summary>
    void Start()
    {
        // Set our starting point
        nextTileLocation = startPoint;  
        nextTileRotation = Quaternion.identity;
        for (int i = 0; i < initSpawnNum; ++i)
        {
            SpawnNextTile(i>= initNoObstacles);
        }
    }
    /// <summary>
    /// Will spawn a tile at a certain location and setup the next position
    /// </summary>
    public void SpawnNextTile(bool spawnObstacles=true)
    {
        var newTile = Instantiate(tile, nextTileLocation,
        nextTileRotation);
        // Figure out where and at what rotation we should spawn
        // the next item
        var nextTile = newTile.Find("Next Spawn Point");
        nextTileLocation = nextTile.position;
        nextTileRotation = nextTile.rotation;

        if (!spawnObstacles)
        {
            return;
        }

        var obstacleSpawnPoints = new List<GameObject>();
        foreach (Transform item in newTile)
        {
            if (item.CompareTag("ObstacleSpawn"))
            {
                obstacleSpawnPoints.Add(item.gameObject);
            }
        }
        Transform newObstacle;
        if (obstacleSpawnPoints.Count > 0)
        {
            var spawnPoint = obstacleSpawnPoints[UnityEngine.Random.Range(0, obstacleSpawnPoints.Count)];
            var spawnPos = spawnPoint.transform.position;
            if(UnityEngine.Random.Range(0,2) == 1)
            {
                newObstacle = Instantiate(obstacle, spawnPos, Quaternion.identity);

            }
            else
            {
                newObstacle = Instantiate(obstacle2, spawnPos, obstacle2.transform.rotation);
            }

            newObstacle.SetParent(spawnPoint.transform);
        }
    }
}

