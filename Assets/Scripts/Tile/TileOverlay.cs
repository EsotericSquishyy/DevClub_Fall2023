using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOverlay : MonoBehaviour //To attach to tile overlay prefab
{
    private GameObject unit = null;

    private TileData tileData;
    public void setTileData(TileData tileData)
    {
        this.tileData = tileData;
    }

    public GameObject getUnit() { return unit; }
    public void setUnit(GameObject unit) {  this.unit = unit; }

    public bool isCrossable()
    {
        return tileData.crossable;
    }

    public void showCrossable()
    {
        if(isCrossable())
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.75f);
        else
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.75f);
    }

    public void showPath() 
    {
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void hideTile() 
    {
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}
