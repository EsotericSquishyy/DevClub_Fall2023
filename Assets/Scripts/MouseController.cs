using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MouseController : MonoBehaviour // To attach to cursor
{
    void LateUpdate() {
        RaycastHit2D focusedTileHit = GetFocusedOnTile();

        Debug.Log(focusedTileHit);

        if(focusedTileHit){
            GameObject overlayTile = focusedTileHit.collider.gameObject;

            transform.position = overlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder + 1;

            if(Input.GetMouseButtonDown(0)) {
                overlayTile.GetComponent<TileSelect>().ShowTile();
            }
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
 