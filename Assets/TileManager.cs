using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Offset Helper class
public class TileOffset
{
	public int oX;      //Helper class to store a relative tile offset
	public int oY;
	public  TileOffset(int vOX,int vOY)
	{
		oX = vOX;
		oY = vOY;
	}
}


public class TileManager : MonoBehaviour {


	//Valid Offsets, from clicked tile      //These are the valid "next to" positions for a tile, ie left, right, top, bottom
	private TileOffset[] mOffsetArray = { 
		 new TileOffset(0, 1)
		,new TileOffset(0, -1)
		,new TileOffset(-1, 0)
		,new TileOffset(1, 0)
	};




    //Link to tile sprites in IDE
    public Sprite[]	GemSprites;             //Sprites we will be using for tiles
	public	TileObject	TileObjectPrefab;   //Prefab used for a tile, we link this to a sprite at runtime

	uint 	mGemTypeCount;

    #region TileArrayHelpers

    //Keep it private and use helpers to access
    private TileObject[,] mTileArray=new TileObject[10,7];

	public	int Width //Get Width of array
	{
		get 
		{
			return	mTileArray.GetLength (1); //Width is Dimension 1
		}
	}

	public	int Height //Get Height of array
	{
		get 
		{
			return	mTileArray.GetLength (0);	//Height is Dimension 0
		}	
	}

    //Check valid tile position, helper function
    bool IsValidTilePosition(int vX, int vY)
    {
        if ((vX >= 0 && vX < Width) && (vY >= 0 && vY < Height))
        {
            return true;        //within array bounds
        }
        return false;
    }

    //Get tile (X,Y) from array
    TileObject GetTile(int vX, int vY)
    {
        TileObject tTO = null;      //If not found return null
        if (IsValidTilePosition(vX, vY))
        {
            return mTileArray[vY, vX];      //Get tile, may be null if space not filled
        }
        else
        {
            Debug.LogFormat("{0:s} ({1:d},{2:d}) Invalid Tile location", System.Reflection.MethodBase.GetCurrentMethod().Name, vX, vY);
        }
        return tTO;
    }

    //Set tile (X,Y) from array, pass null to clear and destroy old tile
    bool SetTile(int vX, int vY, TileObject vNewTO)
    {
        if (IsValidTilePosition(vX, vY))
        {
            mTileArray[vY, vX] = vNewTO;      //Set New tile in array, could be null if clearing tile
            if (vNewTO)  //If its not a null tile
            {
                vNewTO.SetXY(vX, vY);          //Make sure screen position reflects this
            }
            return true;
        }
        else
        {
            Debug.LogFormat("{0:s} ({1:d},{2:d}) Invalid Tile location", System.Reflection.MethodBase.GetCurrentMethod().Name, vX, vY);
        }
        return false;      //Invalid position
    }

    bool    MoveTile(TileObject vTile, int vX,int vY)
    {
        if (IsValidTilePosition(vX, vY) && vTile!=null)     //Valid posstion & Tile
        {
            SetTile(vTile.X, vTile.Y, null);        //Remove tile from old position
            SetTile(vX, vY, vTile);                 //Put it in new position
            return true;
        }
        return false;
    }

    bool ClearTile(int vX, int vY)
    {
        if (IsValidTilePosition(vX, vY))
        {
            TileObject tOldTO = mTileArray[vY, vX];     //Was something there before?
            if (tOldTO != null)
            {
                mTileArray[vY, vX] = null;      //Clear tile from array
                Destroy(tOldTO.gameObject);     //Destroy old game object
            }
            return true;
        }
        else
        {
            Debug.LogFormat("{0:s} ({1:d},{2:d}) Invalid Tile location", System.Reflection.MethodBase.GetCurrentMethod().Name, vX, vY);
        }
        return false;      //Invalid position
    }

    #endregion

    //Get Max tilecount for this set & link to Game Manager

    void Start ()
    {
		ArrayBasedCameraPosition();
		mGemTypeCount = (uint)GemSprites.Length;		//Number of tile sprites we can choose from
		GM.sGM.mTM=this;		//Link TileManager to GM, for global access
	}

	TileObject	MakeTileObject(int vID,int vX, int vY)
    {
		TileObject	tTO = null;
		if (vID < mGemTypeCount)
        {
			tTO=Instantiate(TileObjectPrefab);		//Make New Tile Object
            tTO.transform.SetParent(gameObject.transform); //Makes sure new wiles apear under TileManger in IDE
			tTO.Initialise(vID,vX,vY,GemSprites[vID]);		//Set it to desired sprite
		}
        else
        {
			Debug.LogFormat ("Invalid Tile ID {0:d}", vID);
		}
		return	tTO;
	}


	//Set up new tiles for the first time
	public  void	NewTiles()
    {
		for (int tH = 0; tH < Height; tH++)
        {
			for (int tW = 0; tW < Width; tW++)
            {
			    ClearTile(tW,tH);        //Clear tile out
                int tNewTileID = Mathf.FloorToInt(Random.Range(0, mGemTypeCount));      //Get new random TileIndex
                TileObject  tNewTO = MakeTileObject(tNewTileID, tW, tH);   //Make tile
				SetTile(tW, tH, tNewTO);        //Set New Tile
			}
		}

	}


	void    MoveTilesDown()
	{
		bool tHasMoved;
		do
		{
			tHasMoved = false;
			for (int tH = 1; tH < Height; tH++)     //Go through array bottom up
			{
				for (int tW = 0; tW < Width; tW++)
				{
                    if (GetTile(tW, tH - 1) == null)        //If there is a space below, drop into it
					{
                        TileObject tTO = GetTile(tW, tH);
                        if(tTO!=null)               //Only drop if there is somthing to drop
                        {
                            MoveTile(tTO, tW, tH-1);
                        }
						tHasMoved = true;
					}
				}
			}
			DownFillBlankTiles();
		} while (tHasMoved);
	}


	void ClearTaggedTiles()
	{
		foreach(TileObject tTO in mTileArray)   //As I am doing whole array this is faster
        {                                       //its just ckearing tagged tiles should be safe
            if (tTO!=null)
			{
				tTO.mTagged = false;
			}
		}
	}
	void RemoveTaggedTiles()
	{
		for (int tW=0;tW<Width;tW++)
		{
			for (int tH = 0; tH < Height; tH++)
			{
                TileObject tTO = GetTile(tW, tH);
				if (tTO != null && tTO.mTagged)     //This is safe as if the first condition is false, the second wont be evaluated
				{
					ClearTile(tW, tH);
				}
			}
		}
	}

	void    DownFillBlankTiles()     //Downfill the array by column, stop and move to next coloum when non null tile hit
	{
		for(int tW=0;tW<Width;tW++)
		{
			for(int tH=Height-1;tH>=0;tH--)
			{
				if(GetTile(tW,tH)==null)
				{
					TileObject tTO = MakeTileObject((int)Random.Range(0, mGemTypeCount), tW, tH);
					SetTile(tW, tH, tTO);
				}
				else
				{
					break;      //When first tile hit in column downfill stop fill for this column
				}
			}
		}
	}


	public	void TileClicked(TileObject vTO) 
	{
		ClearTaggedTiles();
		vTO.mTagged = true;	//Tag clicked tile
		TileObject  tArrayTO = GetTile(vTO.X, vTO.Y);
		if(tArrayTO)
		{
			int tCount = 1;     //The Clicked tile counts as first one
			tCount+=FindMatchingTiles(tArrayTO);
			Debug.LogFormat("{0:d} found", tCount);
			if(tCount>=3)
			{
				GM.sGM.PlayClick ();
				RemoveTaggedTiles();
				MoveTilesDown();
				GM.sGM.Score += 10 * tCount;
				GM.sGM.UpdateUIScore();
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
			TileObject tCheckTile = GetTile(tX, tY);
			if (tCheckTile)
			{
				if(tCheckTile.ID == vTO.ID)
				{
					if (!tCheckTile.mTagged)        //Don't double count ones flagged already
					{
						tCheckTile.mTagged = true;
						tCount++;
						tCount+=FindMatchingTiles(tCheckTile);      //Now check tiles next to this one, this routine calls itself
					}
				}
			}

		}
		return  tCount;
	}



	void ArrayBasedCameraPosition()
	{
		//What Height would we need to be to get this width
		Camera.main.orthographicSize = (float)Width / Camera.main.aspect / 2.0f;
		float tHalfHeight = Camera.main.orthographicSize;       //Reposition Camera
		float tHalfWidth = Camera.main.aspect * tHalfHeight;    
		Camera.main.transform.position = new Vector3(tHalfWidth - 0.5f      //Add Half tile offset
			,tHalfHeight - 0.5f //& reposition camera
			,Camera.main.transform.position.z);
	}

}
