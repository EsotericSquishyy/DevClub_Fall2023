using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour // To attach to map
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }
    
    public GameObject overlayTilePrefab;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, GameObject> coordsToTile;
    public Dictionary<GameObject, Vector2Int> tileToCoords;

    private void Awake() { // Singleton Logic
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }
    }

    void Start() {
        coordsToTile = new Dictionary<Vector2Int, GameObject>();
        tileToCoords = new Dictionary<GameObject, Vector2Int>();

        Tilemap tileMap = gameObject.GetComponentInChildren<Tilemap>();

        BoundsInt bounds = tileMap.cellBounds;

        for(int z = bounds.max.z; z >= bounds.min.z; z--){
            for(int y = bounds.min.y; y < bounds.max.y; y++){
                for(int x = bounds.min.x; x < bounds.max.x; x++){
                    Vector3Int tileLocation = new Vector3Int(x, y, z);
                    Vector2Int tileKey      = (Vector2Int)tileLocation;

                    if(tileMap.HasTile(tileLocation) && !coordsToTile.ContainsKey(tileKey)){
                        GameObject overlayTile  = Instantiate(overlayTilePrefab, overlayContainer.transform);
                        Vector3 cellWorldPos    = tileMap.GetCellCenterWorld(tileLocation);

                        overlayTile.transform.position = new Vector3(cellWorldPos.x, cellWorldPos.y, cellWorldPos.z);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
                        
                        // Set canCross modifier / other attribs?
                        
                        coordsToTile.Add(tileKey, overlayTile);
                        tileToCoords.Add(overlayTile, tileKey);
                    }
                }
            }
        }

        //Setting player initial pos
        GameObject player = GameObject.FindWithTag("Player");
        GameObject playerTile = coordsToTile[player.GetComponent<PlayerController>().startPos];

        playerTile.GetComponent<TileOverlay>().unit = player;
        player.transform.position = playerTile.transform.position;
    }
}
