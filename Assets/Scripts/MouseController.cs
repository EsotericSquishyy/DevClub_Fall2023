using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class MouseController : MonoBehaviour // To attach to cursor
{
    private static MouseController _instance;
    public static MouseController Instance { get { return _instance; } }

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

    public Dictionary<string, MouseControllerState> MCStates;
    private MouseControllerState currMCS;

    private void Start()
    {
        MCStates = new Dictionary<string, MouseControllerState>();
        MCStates.Add("General", new GeneralMCS());
        MCStates.Add("Movement", new MovementMCS());

        currMCS = MCStates["General"];
    }

    void LateUpdate() {
        RaycastHit2D focusedTileHit = GetFocusedOnTile();

        if(focusedTileHit){
            GetComponent<SpriteRenderer>().enabled = true;

            GameObject overlayTile = focusedTileHit.collider.gameObject;

            transform.position = overlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder + 1;

            //Debug.Log("Hovering over tile " + MapManager.Instance.tileToCoords[overlayTile]);

            currMCS = currMCS.step(overlayTile);
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public RaycastHit2D GetFocusedOnTile() {
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
    public abstract MouseControllerState step(GameObject tile);
}

public class GeneralMCS : MouseControllerState
{
    public override MouseControllerState step(GameObject tile)
    {
        Debug.Log("General");

        if (Input.GetMouseButtonDown(0) && tile.GetComponent<TileOverlay>().unit != null) {
            string tag = tile.GetComponent<TileOverlay>().unit.tag;

            if (tag == "Player")
            {
                return MouseController.Instance.MCStates["Movement"];
            }
        }

        return this;
    }
}

public class MovementMCS : MouseControllerState
{
    public override MouseControllerState step(GameObject tile)
    {
        Debug.Log("Movement");

        if(Input.GetMouseButtonDown(1))
        {
            return MouseController.Instance.MCStates["General"];
        }

        return this;
    }
}