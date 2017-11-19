using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayTest : MonoBehaviour {

    //Assign in editor
    public GameObject TileObjPrefab;
    TileObj[,] mTileArray;
    int Width = 7;
    int Height = 10;

    float mTime = 0f;

    void Start()
    {
        mTileArray = new TileObj[Height, Width];        //Allocate space for Array
        FillArray();
    }

    void FillArray()
    {
        for (int j = 0; j < Height; j++)       //Rows
        {
            for (int i = 0; i < Width; i++) //Coloumns
            {
                //Get TileObject to make itself
                mTileArray[j, i] = TileObj.MakeTile(TileObjPrefab, j, i, Random.Range(0, 7));
            }
        }
    }

    void DisplayTileArray()
    {
        for (int j = 0; j < Height; j++)       //Rows
        {
            for (int i = 0; i < Width; i++) //Coloumns
            {
                TileObj tTO = mTileArray[j, i];     //Get tile at this position
                if (tTO)
                {
                    tTO.UpdatePosition(j, i);       //Get Tile to reposition itself needed
                }
            }
        }
    }



    public void RemoveBottomTiles()
    {
        for (int tW = 0; tW < Width; tW++)
        {
            if(mTileArray[0, tW])
            {
                Destroy(mTileArray[0, tW].gameObject);
                mTileArray[0, tW] = null;
            }
        }
    }


    public void MoveTilesDown()
    {
		int	tSafe = 0;
        bool tHasMoved;
        do
        {
            tHasMoved = false;
            for (int tH = 1; tH < Height; tH++)
            {
                for (int tW = 0; tW < Width; tW++)
                {
                    if (mTileArray[tH - 1, tW] == null)
                    {
                        mTileArray[tH - 1, tW] = mTileArray[tH, tW];       //Move tile down one
                        mTileArray[tH, tW]=null;        //Clear this tile
                        tHasMoved = true;
                    }
                }
				DownFillBlankTiles();
            }
		} while (tHasMoved && tSafe++<100);
		Debug.LogFormat ("Lines {0:d}", tSafe);
    }

    void DownFillBlankTiles()     //Downfill the array by column, stop and move to next coloum when non null tile hit
    {
        for (int tW = 0; tW < Width; tW++)
        {
            for (int tH = Height - 1; tH >= 0; tH--)
            {
				if ( mTileArray[tH, tW] == null)
                {
					mTileArray[tH, tW] = TileObj.MakeTile(TileObjPrefab, tH, tW, Random.Range(0, 7));
                }
                else
                {
                    break;      //When first tile hit in column downfill stop fill for this column
                }
            }
        }
    }

    void Update()
    {

        mTime += Time.deltaTime;        //Every Second
        if(mTime>1.0f)
        {
            mTime = 0f;
            RemoveBottomTiles();
            MoveTilesDown();
            DisplayTileArray();
        }
    }
}
