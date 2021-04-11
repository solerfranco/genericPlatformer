using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, transform.position.z);
        transform.position = targetPos;
    }
}
