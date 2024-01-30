using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Shoot : NetworkBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;
    private Transform _playerModel;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsLocalPlayer)
            return;

        _playerModel = transform.GetChild(0);
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
        var bullet = Instantiate(_bulletPrefab, _playerModel.position, _playerModel.rotation);
        var netObj = bullet.GetComponent<NetworkObject>();
        netObj.Spawn();
    }

}
