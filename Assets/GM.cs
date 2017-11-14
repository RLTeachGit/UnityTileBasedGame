using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {

    [HideInInspector]
	public	TileManager	mTM;		//Set by TileManager, allows singleton instance access

	TileObject[,]	mTileArray;     //Tiles stored here as an array

    [HideInInspector]
	public  const int	Width=15;		//Width of game
	int	Height;					//Height of game

    [HideInInspector]
    public Scoring mScoring;


    public int Score;

	//Handy for labeling sections of code
	#region Singleton      
	public static GM sGM;       //Allows access to singleton
	//Being static means yoiu can access without knowing instance
	void Awake () {		        //Runs before Start
		if (sGM == null) {      //Has it been set up before?
			sGM = this;		    //No, its the first Time creation of Game Manager, so store our instance
			DontDestroyOnLoad(gameObject);  //Persist, now it will survive scene reloads
			Height=(int)(((float)Width/Camera.main.aspect))+3;
			mTileArray=new TileObject[Height,Width];
		} else if (sGM != this) { //If we get called again, then destroy new version and keep old one
			Destroy (gameObject);   //Kill any subsequent one
		}

        StartCoroutine(WaitForStartup());
	}
	#endregion

    IEnumerator WaitForStartup()        //Wait for other GO's to start up before starting game
    {
        while(mScoring==null || mTM==null)
        {
            yield return new    WaitForSeconds(0.1f);
        }
        NewTiles(); //Start Game
    }


    //Set up new tiles for the first time
    public  void	NewTiles() {
        Score = 0;
        UpdateUIScore();
        for (int tH = 0; tH < Height; tH++) {
			for (int tW = 0; tW < Width; tW++) {
				if (mTileArray [tH, tW] != null) {
					Destroy (mTileArray [tH, tW].gameObject);
					mTileArray [tH, tW] = null;
				}
                TileObject  tTO = mTM.MakeTileObject((int)Random.Range(0, mTM.TileCount), tW, tH);
                SetTile(tW, tH, tTO);
			}
		}

    }



    //Check valid tile position, helper function
    public bool IsValidTilePosition(int vX, int vY)
    {
        if (vX>=0 && vX < Width && vY>=0 && vY < Height)
        {
            return true;        //within array bounds
        }
        return false;
    }

    //Get tile (X,Y) from array
    public TileObject GetTile(int vX, int vY)
    {
        TileObject tTO = null;      //If not found return null
        if (IsValidTilePosition(vX,vY))
        {
            return mTileArray[vY, vX];      //Get tile, may be null if space not filled
        }
        else
        {
            Debug.LogFormat("{0:s} ({1:d},{2:d}) Invalid Tile location", System.Reflection.MethodBase.GetCurrentMethod().Name, vX, vY);
        }
        return tTO;
    }

    //Set tile (X,Y) from array
    public bool SetTile(int vX, int vY,TileObject vTO)
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
        else
        {
            Debug.LogFormat("{0:s} ({1:d},{2:d}) Invalid Tile location", System.Reflection.MethodBase.GetCurrentMethod().Name, vX, vY);
        }
        return  false;      //Invalid position
    }

    public  void    MoveTilesDown()
    {
        bool tHasMoved;
        do
        {
            tHasMoved = false;
            for (int tH = 1; tH < Height; tH++)
            {
                for (int tW = 0; tW < Width; tW++)
                {
                    if (GetTile(tW, tH - 1) == null)
                    {
                        SetTile(tW, tH - 1, GetTile(tW, tH));     //Move tile down
                        SetTile(tW, tH, null);
                        tHasMoved = true;
                    }
                }
            }
            DownFillBlankTiles();
        } while (tHasMoved);
    }


    public void ClearTaggedTiles()
    {
        foreach(TileObject tTO in mTileArray)
        {
            if(tTO!=null)
            {
                tTO.mTagged = false;
            }
        }
    }
    public void RemoveTaggedTiles()
    {
        for (int tW=0;tW<Width;tW++)
        {
            for (int tH = 0; tH < Height; tH++)
            {
                if (mTileArray[tH, tW] != null && mTileArray[tH, tW].mTagged)
                {
                    Destroy(mTileArray[tH, tW].gameObject);
                    mTileArray[tH, tW] = null;
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
                    TileObject tTO = mTM.MakeTileObject((int)Random.Range(0, mTM.TileCount), tW, tH);
                    SetTile(tW, tH, tTO);
                }
                else
                {
                    break;      //When first tile hit in column downfill stop fill for this column
                }
            }
        }
    }

    public  void    UpdateUIScore()
    {
        if (mScoring != null)
        {
            mScoring.SetScore(Score);     //Update Score in UI
        }
    }
}
