using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject tubeSpawn;
    [SerializeField] private GameObject regularSpawn;
    void Start()
    {
        PlayerManager playerScript = player.GetComponent<PlayerManager>();
        if (playerScript.GetGravityState() == Entity.GravityState.Regular)
        {
            player.GetComponent<Rigidbody>().position = regularSpawn.transform.position;
            Debug.Log("getting reg");
        }
        else
        {
            player.GetComponent<Rigidbody>().position = tubeSpawn.transform.position;
            Debug.Log("getting tube");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
