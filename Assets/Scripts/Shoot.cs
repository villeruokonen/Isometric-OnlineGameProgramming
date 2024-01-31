using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Shoot : NetworkBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;
    private Transform _playerModel;
    private Transform _shootingPoint;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsLocalPlayer)
            return;

        _playerModel = transform.GetChild(0);
        _shootingPoint = transform.GetChild(0).GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShootServerRpc(_shootingPoint.position,_playerModel.rotation);
            }
        }
    }

    [ServerRpc]
    private void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        // Make a bullet
        var bullet = Instantiate(_bulletPrefab, position, rotation);
        var netObj = bullet.GetComponent<NetworkObject>();
        netObj.Spawn();
    }

   
        
    
}
