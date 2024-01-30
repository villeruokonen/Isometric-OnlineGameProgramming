using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private float _bulletSpeed = 10f;
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
        transform.position += transform.forward * _bulletSpeed*0.01f;

    }
}
