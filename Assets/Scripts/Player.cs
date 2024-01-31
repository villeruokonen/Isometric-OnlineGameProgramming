using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

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

    [ClientRpc]
    public void BlowUpClientRpc(NetworkObjectReference playerReference, ClientRpcParams receiveParams = default)
    {
        // We can try getting the player object from the reference
        if(playerReference.TryGet(out NetworkObject player))
        {
            if(player.IsLocalPlayer)
            {
                Debug.Log("I blew up!");
            }
            else
            {
                Debug.Log($"{player.OwnerClientId} blew up!");
            }
        }
    }

    [ServerRpc]
    public void BlowUpServerRpc(ServerRpcParams receiveParams = default)
    {
        Debug.Log("BlowUpServerRpc called");

        // Only the server should be able to call this method
        if (!IsServer)
            return;

        // The player who blew up is the one who this RPC is called on
        // so we can check OwnerClientId as player objects are owned by clients
        var player = OwnerClientId;

        // We find the player object using the client id
        var playerObject = NetworkManager.Singleton.ConnectedClients[player].PlayerObject;

        // We make a new NetworkObjectReference that we can send across the network
        var playerReference = new NetworkObjectReference(playerObject);

        // Finally we call the client rpc on all clients with the player reference
        BlowUpClientRpc(playerReference);
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

    [ServerRpc(RequireOwnership =false)]
    public void DieServerRpc()
    {
        gameObject.SetActive(false);
    }
}
