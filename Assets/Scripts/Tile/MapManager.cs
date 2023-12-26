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

    public Dictionary<Vector3Int, GameObject> coordsToTile;
    public Dictionary<GameObject, Vector3Int> tileToCoords;
    
    public List<TileData> tileDatas;

    private void Awake() { // Singleton Logic
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }

        createOverlayTiles();

        assignTileData();
    }

    void Start() {

    }

    void createOverlayTiles()
    {
        coordsToTile = new Dictionary<Vector3Int, GameObject>();
        tileToCoords = new Dictionary<GameObject, Vector3Int>();

        Tilemap tileMap = gameObject.GetComponentInChildren<Tilemap>();

        BoundsInt bounds = tileMap.cellBounds;

        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    Vector3Int tileKey = new Vector3Int(x, y, z);

                    if (tileMap.HasTile(tileKey) && !coordsToTile.ContainsKey(tileKey))
                    {
                        GameObject overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                        Vector3 cellWorldPos = tileMap.GetCellCenterWorld(tileKey);

                        overlayTile.transform.position = new Vector3(cellWorldPos.x, cellWorldPos.y, cellWorldPos.z);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;

                        coordsToTile.Add(tileKey, overlayTile);
                        tileToCoords.Add(overlayTile, tileKey);
                    }
                }
            }
        }
    }

    void assignTileData()
    {
        Dictionary<TileBase, TileData>  tileToData = new Dictionary<TileBase, TileData>();

        foreach(TileData tileData in tileDatas)
        {
            foreach(TileBase tileBase in tileData.tiles)
                tileToData.Add(tileBase, tileData);
        }

        Tilemap tileMap = gameObject.GetComponentInChildren<Tilemap>();

        foreach(KeyValuePair<Vector3Int, GameObject> entry in coordsToTile)
        {
            TileBase tile = tileMap.GetTile(entry.Key);

            entry.Value.GetComponent<TileOverlay>().tileData = Instantiate(tileToData[tile]);
        }
    }

    public void showCrossableOverlay()
    {
        foreach(GameObject overlayTile in coordsToTile.Values)
            overlayTile.GetComponent<TileOverlay>().showCrossable();
    }

    public void hideOverlay()
    {
        foreach (GameObject overlayTile in coordsToTile.Values)
            overlayTile.GetComponent<TileOverlay>().hideTile();
    }
}
