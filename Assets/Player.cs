using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TileOffset
{
    public int oX;
    public int oY;
    public  TileOffset(int vOX,int vOY)
    {
        oX = vOX;
        oY = vOY;
    }
}


public class Player : MonoBehaviour {

    TileManager mFirstInSequence;

    static TileOffset[] mOffsetArray = { new TileOffset(0, 1)
                                        ,new TileOffset(0, -1)
                                        ,new TileOffset(-1, 0)
                                        ,new TileOffset(1, 0)
                                        };


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (EventSystem.current.IsPointerOverGameObject ())
			return;
		if(Input.GetMouseButtonDown(0))
        {
            Ray tRayIntoScene = Camera.main.ScreenPointToRay(Input.mousePosition);        //Make Ray from Camera into scene 
            RaycastHit2D tHit = Physics2D.Raycast(tRayIntoScene.origin, tRayIntoScene.direction, Mathf.Infinity);   //Send ray from camera into scene
            GM.sGM.ClearTaggedTiles();
            if(tHit.collider != null)
            {
                TileObject tTO = tHit.collider.gameObject.GetComponent<TileObject>();       //Get Tile at location
                if(tTO)
                {
                    Debug.LogFormat("Hit ID{0:d} ({1:d},{2:d})", tTO.ID,tTO.X,tTO.Y);
                    tTO.mTagged = true;
                    TileObject  tArrayTO = GM.sGM.GetTile(tTO.X, tTO.Y);
                    if(tArrayTO)
                    {
                        int tCount = 1;     //The Clicked tile counts as first one
                        tCount+=FindMatchingTiles(tArrayTO);
                        Debug.LogFormat("{0:d} found", tCount);
                        if(tCount>=3)
                        {
							GM.sGM.PlayClick ();
                            GM.sGM.RemoveTaggedTiles();
                            GM.sGM.MoveTilesDown();
                            GM.sGM.Score += 10 * tCount;
                            GM.sGM.UpdateUIScore();
                        }
                    }
                }
            }
        }
	}

    //Recursivly look up tiles which match ours
    int    FindMatchingTiles(TileObject vTO)
    {
        int tCount = 0;
        foreach(TileOffset tOffset in mOffsetArray)
        {
            int tX = vTO.X + tOffset.oX;
            int tY = vTO.Y + tOffset.oY;
            TileObject tCheckTile = GM.sGM.GetTile(tX, tY);
            if (tCheckTile)
            {
                if(tCheckTile.ID == vTO.ID)
                {
                    if (!tCheckTile.mTagged)        //Don't double count ones flagged already
                    {
                        tCheckTile.mTagged = true;
                        tCount++;
                        tCount+=FindMatchingTiles(tCheckTile);      //Now check tiles next to this one
                    }
                }
            }

        }
        return  tCount;
    }

	public	void	Restart()
	{
		GM.sGM.NewTiles();
	}

}
