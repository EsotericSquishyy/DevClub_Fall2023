using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelect : MonoBehaviour //To attach to tile overlay prefab
{
    //bool showing = false;

    // Update is called once per frame
    void Update()
    {
        if(/*showing &&*/ Input.GetMouseButtonDown(0)){
            HideTile();
        }
    }

    public void ShowTile(){
        //showing = true;
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void HideTile(){
        //showing = false;
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}
