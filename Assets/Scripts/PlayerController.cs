using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Vector3Int startPos;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerTile = MapManager.Instance.coordsToTile[startPos];

        playerTile.GetComponent<TileOverlay>().unit = gameObject;
        transform.position = playerTile.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
