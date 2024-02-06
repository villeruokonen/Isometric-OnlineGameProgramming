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

        _playerModel = transform.Find("Model");
        _shootingPoint = _playerModel.Find("ShootingPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShootServerRpc(_shootingPoint.position,_playerModel.rotation);
                //Debug.Log("Clicked");
            }
        }
    }

    [ServerRpc]
    private void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        // Instantiate the bullet on the server

        ShootClientRpc(position,rotation);

        Debug.Log("ShootServerRpc called");
    }

    [ClientRpc]
    private void ShootClientRpc(Vector3 bulletPosition, Quaternion bulletRotation)
    {
        var bullet = Instantiate(_bulletPrefab, bulletPosition, bulletRotation);

        Debug.Log("ShootClientRpc called");
    }

}
