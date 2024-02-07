using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkBomb : NetworkBehaviour
{
    [SerializeField]
    private float _timeToExplode = 3;

    [SerializeField]
    private LayerMask _bombMask;

    private Renderer _bombBodyRenderer;

    public override void OnNetworkSpawn()
    {
        _bombBodyRenderer = GetComponent<Renderer>();
        StartCoroutine(ExplosionCoroutine());
    }

    private IEnumerator ExplosionCoroutine()
    {
        var originalColor = _bombBodyRenderer.material.color;
        var targetColor = Color.red;
        var timer = 0f;
        while(timer < _timeToExplode)
        {
            var lerpedColor = Color.Lerp(originalColor, targetColor, timer / _timeToExplode);
            _bombBodyRenderer.material.color = lerpedColor;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _bombBodyRenderer.enabled = false;

        GameObject[] lines = new GameObject[4];
        float startAngle = 0;
        var template = transform.Find("LineTemplate").gameObject;
        for(int i = 0; i < 4; i++)
        {
            var lineRoot = Instantiate(template, transform);
            lineRoot.SetActive(true);

            var lineRend = lineRoot.GetComponent<LineRenderer>() ?? lineRoot.AddComponent<LineRenderer>();
            var origin = transform.position;
            var dir = Quaternion.Euler(0, startAngle, 0) * Vector3.forward;
            var ray = new Ray(origin, dir);

            // We are using a boxcast instead of a raycast because raycast is too thin
            // and might not hit the players
            if(!Physics.BoxCast(origin, Vector3.one * 0.1f, dir, out var hit, Quaternion.identity, 100, _bombMask))
            {
                Debug.Log("No hit");
                continue;
            }

            lineRend.SetPosition(0, origin);
            lineRend.SetPosition(1, hit.point);

            lines[i] = lineRoot;
            startAngle += 90;

            if (!IsServer)
                continue;

            if(hit.collider.CompareTag("Player"))
            {
                Debug.Log("Hit player");
                var player = hit.collider.GetComponent<Player>();

                // This is a server RPC so it will only run on the server,
                // even if this player is not the host player
                player.BlowUpServerRpc();
            }
        }

        yield return new WaitForSeconds(0.5f);

        foreach(var line in lines)
        {
            Destroy(line);
        }

        if(IsServer)
            NetworkObject.Despawn(true);

        yield return null;
    }
}
