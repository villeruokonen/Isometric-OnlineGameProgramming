using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsServer)
            return;
        Move();
    }

    void Move()
    {
        transform.position += transform.forward * 0.1f;

    }

    public void SetDirection(Vector3 direction)
    {
        transform.forward = direction;
        Debug.Log("Bullet direction set to " + direction);
    }
}
