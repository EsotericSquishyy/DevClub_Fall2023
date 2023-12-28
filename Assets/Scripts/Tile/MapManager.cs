using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour // To attach to map
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    [SerializeField]
    private GameObject overlayContainer;

    [SerializeField]
    private GameObject overlayTilePrefab;

    [SerializeField]
    private List<TileData> tileDatas;

    private Dictionary<Vector3Int, GameObject> coordToTile;
    private Dictionary<GameObject, Vector3Int> tileToCoord;

    private void Awake() { // Singleton Logic
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        createOverlayTiles();

        assignTileData();
    }

    void Start() {

    }

    void createOverlayTiles()
    {
        coordToTile = new Dictionary<Vector3Int, GameObject>();
        tileToCoord = new Dictionary<GameObject, Vector3Int>();

        Tilemap tileMap = gameObject.GetComponentInChildren<Tilemap>();

        BoundsInt bounds = tileMap.cellBounds;

        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    Vector3Int tileKey = new Vector3Int(x, y, z);

                    if (tileMap.HasTile(tileKey) && !coordToTile.ContainsKey(tileKey))
                    {
                        GameObject overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                        Vector3 cellWorldPos = tileMap.GetCellCenterWorld(tileKey);

                        overlayTile.transform.position = new Vector3(cellWorldPos.x, cellWorldPos.y, cellWorldPos.z);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;

                        coordToTile.Add(tileKey, overlayTile);
                        tileToCoord.Add(overlayTile, tileKey);
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

        foreach(KeyValuePair<Vector3Int, GameObject> entry in coordToTile)
        {
            TileBase tile = tileMap.GetTile(entry.Key);

            entry.Value.GetComponent<TileOverlay>().setTileData(Instantiate(tileToData[tile]));
        }
    }

    public void showCrossableOverlay()
    {
        foreach(GameObject overlayTile in coordToTile.Values)
            overlayTile.GetComponent<TileOverlay>().showCrossable();
    }

    public void hideOverlay()
    {
        foreach (GameObject overlayTile in coordToTile.Values)
            overlayTile.GetComponent<TileOverlay>().hideTile();
    }

    public GameObject getTileAtCoord(Vector3Int coord)
    {
        return coordToTile[coord];
    }

    public Vector3Int getCoordAtTile(GameObject tile)
    {
        return tileToCoord[tile];
    }

    public bool isAdjacent(GameObject a, GameObject b)
    {
        Vector3Int aPos = tileToCoord[a], bPos = tileToCoord[b];

        return Mathf.Abs(aPos.x - bPos.x) + Mathf.Abs(aPos.y - bPos.y) == 1;
    }
}
