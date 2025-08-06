using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeCameraScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float terrainOffset;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float playerBackDistance;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
    }
    void UpdateRotation()
    {
        MainTerrain mainTerrain = player.GetComponent<PlayerManager>().GetCurrentTerrain().GetComponent<MainTerrain>(); //this should be done via unityevent/action in game manager
        Vector3 rotationAxis = mainTerrain.lineOfGravity;
        Vector3 pointOfGravity = player.GetComponent<PlayerManager>().GetGravityDirection() + player.transform.position; //this should be the point along lineofgravity
        transform.position = pointOfGravity + -player.GetComponent<PlayerManager>().GetGravityDirection().normalized * terrainOffset;
        transform.position -= rotationAxis.normalized * playerBackDistance;

        Quaternion targetRotation = Quaternion.LookRotation(rotationAxis, -player.GetComponent<PlayerManager>().GetGravityDirection());
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
