using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {


	public	TileManager	mTM;		//Set by TileManager, allows singleton instance access

	TileObject[,]	mTileArray;     //Tiles stored here as an array

	public	const	uint	Width=10;		//Width of game
	public	uint	Height;					//Height of game


	//Handy for labeling sections of code
	#region Singleton      
	public static GM sGM;       //Allows access to singleton
	//Being static means yoiu can access without knowing instance
	void Awake () {		        //Runs before Start
		if (sGM == null) {      //Has it been set up before?
			sGM = this;		    //No, its the first Time creation of Game Manager, so store our instance
			DontDestroyOnLoad(gameObject);  //Persist, now it will survive scene reloads
			Height=(uint)(((float)Width/Camera.main.aspect))+3;
			mTileArray=new TileObject[Height,Width];
		} else if (sGM != this) { //If we get called again, then destroy new version and keep old one
			Destroy (gameObject);   //Kill any subsequent one
		}
		Invoke ("NewTiles", 2.0f);
	}
	#endregion

	void	NewTiles() {
		for (uint tH = 0; tH < Height; tH++) {
			for (uint tW = 0; tW < Width; tW++) {
				if (mTileArray [tH, tW] != null) {
					Destroy (mTileArray [tH, tW].gameObject);
					mTileArray [tH, tW] = null;
				}
                TileObject  tTO = mTM.MakeTileObject((uint)Random.Range(0, mTM.TileCount), tW, tH);
                SetTile(tW, tH, tTO);
			}
		}
	}



    //Check valid tile position, helper function
    public bool IsValidTilePosition(uint vX, uint vY)
    {
        if (vX < Width && vY < Height)
        {
            return true;
        }
        Debug.LogFormat("{0:s} ({1:d},{2:d}) Invalid Tile location", vX, vY, System.Reflection.MethodBase.GetCurrentMethod().Name, vX, vY);
        return false;
    }

    public TileObject GetTile(uint vX, uint vY)
    {
        TileObject tTO = null;      //If not found return null
        if (IsValidTilePosition(vX,vY))
        {
            return mTileArray[vY, vX];      //Get tile, may be null if space not filled
        }
        return tTO;
    }

    public bool SetTile(uint vX, uint vY,TileObject vTO)
    {
        if (IsValidTilePosition(vX, vY))
        {
            mTileArray[vY, vX]=vTO;      //Set tile in array
            if(vTO)
            {
                vTO.SetXY(vX, vY);          //Make sure screen position reflects this
            }
            return true;
        }
        return  false;      //Invalid position
    }

    public  void    MoveTilesDown()
    {
        for(uint tH=1;tH<Height;tH++)
        {
            for(uint tW=0;tW<Width;tW++)
            {
                if(GetTile(tW,tH-1)==null)
                {
                    SetTile(tW, tH - 1,GetTile(tW,tH));     //Move tile down
                    SetTile(tW, tH, null);
                }
            }
        }
        DownFillBlankTiles();
    }

    public  void    DownFillBlankTiles()     //Downfill the array by column, stop and move to next coloum when non null tile hit
    {
        for(uint tW=0;tW<Width;tW++)
        {
            for(int tH=(int)Height-1;tH>=0;tH--)
            {
                if(GetTile(tW,(uint)tH)==null)
                {
                    TileObject tTO = mTM.MakeTileObject((uint)Random.Range(0, mTM.TileCount), tW, (uint)tH);
                    SetTile(tW, (uint)tH, tTO);
                }
                else
                {
                    break;      //When first tile hit in column downfill stop fill for this column
                }
            }
        }
    }
}
