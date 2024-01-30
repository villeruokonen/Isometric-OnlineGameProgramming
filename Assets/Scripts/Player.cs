using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            CreateBombServerRpc();
        }
    }

    [ServerRpc]
    void CreateBombServerRpc()
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        var bomb = Instantiate(Resources.Load<GameObject>("Prefabs/Bomb"));
        bomb.transform.position = transform.position;
        var netObj = bomb.GetComponent<NetworkObject>();
        netObj.Spawn();
    }
}
