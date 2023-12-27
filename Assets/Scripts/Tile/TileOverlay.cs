using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOverlay : MonoBehaviour //To attach to tile overlay prefab
{
    public GameObject unit = null;

    public TileData tileData;

    private void Start()
    {

    }

    void Update()
    {

    }

    public bool isCrossable()
    {
        return tileData.crossable && unit == null;
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
