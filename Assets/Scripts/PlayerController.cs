using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Vector3Int startPos;

    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerTile = MapManager.Instance.getTileAtCoord(startPos);

        playerTile.GetComponent<TileOverlay>().unit = gameObject;
        transform.position = playerTile.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool isMoving()
    {
        return moving;
    }
}
