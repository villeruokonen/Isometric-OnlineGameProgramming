using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Shoot : NetworkBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsLocalPlayer)
            return;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShootServerRpc();
            }
        }
    }

    [ServerRpc]
    private void ShootServerRpc()
    {
        // Make a bullet
        var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        
        var bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(transform.forward);
            
        }

        var netObj = bullet.GetComponent<NetworkObject>();
        netObj.Spawn();
    }

}
