using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTerrain : MonoBehaviour
{
    public Vector3 lineOfGravity;
    void Start()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.transform.position, lineOfGravity);
        Gizmos.DrawRay(transform.transform.position, -lineOfGravity);
    }
    void Update()
    {
        
    }
}
