using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed = 10f;
    void Start()
    {
        
    }

    void Update()
    {
       
        Move();
    }

    void Move()
    {
        transform.position += transform.forward * _bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponent<Player>();
            player.DieServerRpc();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            return;
        }
        Destroy(gameObject);
    }
    
}
