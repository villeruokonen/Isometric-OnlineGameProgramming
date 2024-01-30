using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkBomb : NetworkBehaviour
{
    [SerializeField]
    private LayerMask _bombMask;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;

        StartCoroutine(ExplosionCoroutine());
    }

    private IEnumerator ExplosionCoroutine()
    {
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

            if(!Physics.Raycast(ray, out var hit, 15, _bombMask))
            {
                Debug.Log("No hit");
                continue;
            }

            lineRend.SetPosition(0, origin);
            lineRend.SetPosition(1, hit.point);

            lines[i] = lineRoot;
            startAngle += 90;
        }

        yield return null;
    }
}
