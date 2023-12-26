using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOverlay : MonoBehaviour //To attach to tile overlay prefab
{
    public GameObject unit = null;
    public TileData tileData;

    private void Start()
    {
        if(tileData == null)
            tileData = new TileData();
    }

    void Update()
    {

    }

    public void showCrossable()
    {
        if(tileData.crossable && unit == null)
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.75f);
        else
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.75f);
    }

    public void showTile() {
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void hideTile() {
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}
