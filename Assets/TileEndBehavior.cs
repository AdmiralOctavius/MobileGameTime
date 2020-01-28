using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEndBehavior : MonoBehaviour
{

    [Tooltip("Time to wait before destroying a tile")]
    public float destroyTime = 1.0f;

    public void OnTriggerEnter(Collider other)
    {
        //Check for collision with player only
        if (other.gameObject.GetComponent<PlayerBehavior>())
        {
            //spawn a new tile to make it endless
            GameObject.FindObjectOfType<GameController>().SpawnNextTile();

            //Destroy the old one
            Destroy(transform.parent.gameObject, destroyTime);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
