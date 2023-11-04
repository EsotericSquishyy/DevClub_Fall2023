using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }
    
    public GameObject overlayTilePrefab;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, GameObject> map;
    public bool ignoreBottomTiles;

    private void Awake(){ // Singleton Logic
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }
    }

    void Start(){
        var tileMap = gameObject.GetComponentInChildren<Tilemap>();
        map = new Dictionary<Vector2Int, GameObject>();

        BoundsInt bounds = tileMap.cellBounds;

        for(int z = bounds.max.z; z >= bounds.min.z; z--){
            if(z == bounds.min.z && ignoreBottomTiles){
                return;
            }

            for(int y = bounds.min.y; y < bounds.max.y; y++){
                for(int x = bounds.min.x; x < bounds.max.x; x++){
                    Vector3Int tileLocation = new Vector3Int(x, y, z);
                    Vector2Int tileKey      = new Vector2Int(x, y);

                    if(tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey)){
                        GameObject overlayTile  = Instantiate(overlayTilePrefab, overlayContainer.transform);
                        Vector3 cellWorldPos    = tileMap.GetCellCenterWorld(tileLocation);

                        overlayTile.transform.position = new Vector3(cellWorldPos.x, cellWorldPos.y, cellWorldPos.z+1);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
                        map.Add(tileKey, overlayTile);
                    }
                }
            }
        }
    }
}
