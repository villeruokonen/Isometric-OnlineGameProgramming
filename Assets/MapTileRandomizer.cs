using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileRandomizer : MonoBehaviour
{
    public class MapTile
    {
        public GameObject tile;
        public Vector3 position;
        public GameObject[] neighbors;

        public MapTile(GameObject tile, Vector3 position)
        {
            this.tile = tile;
            this.position = position;
            neighbors = new GameObject[4];
        }
    }

    [SerializeField]
    private int seed;
    GameObject[] tileObjects;
    List<Vector3> freePositions;
    public MapTile[] mapTilesArray;
    // Start is called before the first frame update
    void Start()
    {
        tileObjects = new GameObject[transform.childCount];
        freePositions = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            tileObjects[i] = transform.GetChild(i).gameObject;
        }

        RandomizeTiles();
    }

    void RandomizeTiles()
    {
        Random.InitState(seed);
        
        for (int i = 0; i < tileObjects.Length; i++)
        {
            float rand = Random.Range(0, 100);
            if (rand < 57)
            {
                // Second chance
                rand = Random.Range(0, 100);
                if (rand < 4)
                    continue;

                tileObjects[i].SetActive(false);
                freePositions.Add(tileObjects[i].transform.position);
                mapTilesArray[i] = new MapTile(tileObjects[i], tileObjects[i].transform.position);
            }
        }        
    }

    public bool IsPositionFree(Vector3 pos)
    {
        return freePositions.Contains(pos);
    }
}
