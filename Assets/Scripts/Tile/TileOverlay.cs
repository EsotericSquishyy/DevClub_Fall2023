using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOverlay : MonoBehaviour //To attach to tile overlay prefab
{
    public GameObject unit = null;

    void Update()
    {

    }

    public void ShowTile() {
        //showing = true;
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void HideTile() {
        //showing = false;
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}
