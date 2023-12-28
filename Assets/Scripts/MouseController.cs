using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseController : MonoBehaviour // To attach to cursor
{
    private static MouseController _instance;
    public static MouseController Instance { get { return _instance; } }

    private Dictionary<string, MouseControllerState> MCStates;
    private MouseControllerState currMCS;
    public MouseControllerState getState(string name) { return MCStates[name]; }

    private void Awake()
    { // Singleton Logic
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        MCStates = new Dictionary<string, MouseControllerState>();
        MCStates.Add("General", new GeneralMCS());
        MCStates.Add("Path Select", new PathSelectMCS());
        MCStates.Add("Movement", new MovementMCS());

        currMCS = MCStates["General"];
    }

    private void LateUpdate() {
        RaycastHit2D focusedTileHit = GetFocusedOnTile();

        if (focusedTileHit)
        {
            //Move cursor to the right place
            GetComponent<SpriteRenderer>().enabled = true;

            GameObject overlayTile = focusedTileHit.collider.gameObject;

            transform.position = overlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder + 1;

            //Debug.Log("Hovering over tile " + MapManager.Instance.tileToCoords[overlayTile]);

            //Handling FSM
            MouseControllerState nextMCS = currMCS.step(overlayTile);
            if (nextMCS != currMCS)
            {
                currMCS.end();
                currMCS = nextMCS;
                currMCS.start();
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private RaycastHit2D GetFocusedOnTile() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = (Vector2) mousePos;

        /*RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Map"));

        if(hits.Length > 0){
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
        */

        return Physics2D.Raycast(mousePos2d, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Map"));
    }
}

public abstract class MouseControllerState
{
    public abstract void start();

    public abstract void end();

    public abstract MouseControllerState step(GameObject tile);
}

public class GeneralMCS : MouseControllerState
{
    public override void start()
    {
        Debug.Log("General START");
    }

    public override MouseControllerState step(GameObject tile)
    {
        //Debug.Log("General");

        if (Input.GetMouseButtonDown(0) && tile.GetComponent<TileOverlay>().getUnit() != null) 
        {
            string tag = tile.GetComponent<TileOverlay>().getUnit().tag;

            if (tag == "Player")
            {
                ((PathSelectMCS)MouseController.Instance.getState("Path Select")).playerTile = tile;

                return MouseController.Instance.getState("Path Select");
            }
        }

        return this;
    }

    public override void end()
    {
        Debug.Log("General END");
    }
}

public class PathSelectMCS : MouseControllerState
{
    public GameObject playerTile;

    List<GameObject> path = new List<GameObject>();

    public override void start()
    {
        Debug.Log("Path Select START");

        path.Clear();
        path.Add(playerTile);
        
        MapManager.Instance.showCrossableOverlay();
    }

    public override MouseControllerState step(GameObject tile)
    {
        if (Input.GetMouseButtonDown(1))
            return MouseController.Instance.getState("General");
        else if (Input.GetMouseButtonDown(0) && path.Count > 1)
        {
            ((MovementMCS)MouseController.Instance.getState("Movement")).playerController
                = playerTile.GetComponent<TileOverlay>().getUnit().GetComponent<PlayerController>();

            ((MovementMCS)MouseController.Instance.getState("Movement")).path = path;

            return MouseController.Instance.getState("Movement");
        }

        if (tile == playerTile || tile.GetComponent<TileOverlay>().isCrossable())
        {
            if (path.Contains(tile))
            {
                while (path.Last() != tile)
                {
                    path.Last().GetComponent<TileOverlay>().showCrossable();
                    path.RemoveAt(path.Count - 1);
                }

            }
            else if (MapManager.Instance.isAdjacent(tile, path.Last()))
            {
                path.Add(tile);
                tile.GetComponent<TileOverlay>().showPath();
            }
        } 

        return this;
    }

    public override void end()
    {
        Debug.Log("Path Select END");

        MapManager.Instance.hideOverlay();
    }
}

public class MovementMCS : MouseControllerState
{
    public PlayerController playerController;
    public List<GameObject> path;
    
    public override void start()
    {
        Debug.Log("Movement START");

        playerController.moveAlongPath(path);
    }

    public override MouseControllerState step(GameObject tile)
    {
        if (playerController.isMoving())
            return this;
        else
            return MouseController.Instance.getState("General");
    }

    public override void end()
    {
        Debug.Log("Movement END");
    }
}