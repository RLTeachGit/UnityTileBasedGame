using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileOffset
{
    public uint oX;
    public uint oY;
    TileOffset(uint vOX,uint vOY)
    {
        oX = vOX;
        oY = vOY;
    }
}


public class Player : MonoBehaviour {

    TileManager mFirstInSequence;

    static  TileOffset[] = {{0,1},{0,-1},{-1,0},{1,0}};


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButtonDown(0))
        {
            Ray tRayIntoScene = Camera.main.ScreenPointToRay(Input.mousePosition);        //Make Ray from Camera into scene 
            RaycastHit2D tHit = Physics2D.Raycast(tRayIntoScene.origin, tRayIntoScene.direction, Mathf.Infinity);   //Send ray from camera into scene
            if(tHit.collider != null)
            {
                TileObject tTO = tHit.collider.gameObject.GetComponent<TileObject>();       //Get Tile at location
                if(tTO)
                {
                    Debug.LogFormat("Hit ID{0:d} ({1:d},{2:d})", tTO.ID,tTO.X,tTO.Y);
                    TileObject  tArrayTO = GM.sGM.GetTile(tTO.X, tTO.Y);
                    if(tArrayTO)
                    {
                        GM.sGM.SetTile(tArrayTO.X, tArrayTO.Y, null); //Wipe old tile from array
                        Destroy(tArrayTO.gameObject);       //Delete it off screen
                        GM.sGM.MoveTilesDown();
                    }
                }
            }
        }
	}


    uint    FindMatchingTiles(TileObject vTO)
    {

    }
}
