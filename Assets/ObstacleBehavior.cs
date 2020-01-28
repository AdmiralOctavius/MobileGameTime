using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ObstacleBehavior : MonoBehaviour
{

    [Tooltip("How long to wait before restarting the game")]
    public float waitTime = 3.0f;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<PlayerBehavior>())
        {
            //Destroy the player
            Destroy(other.gameObject);

            //Restart the level
            Invoke("ResetGame", waitTime);
        }
    }

    void ResetGame()
    {
        //Reload level by loading the current scene again
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
