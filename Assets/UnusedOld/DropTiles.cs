using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTiles : MonoBehaviour {


	public	DisplayArray	DisplayArrayRef;

	void	Start()
	{
	}

	void DisplayTileArray()
	{
		for (int j = 0; j < DisplayArrayRef.Height; j++)       //Rows
		{
			for (int i = 0; i < DisplayArrayRef.Width; i++) //Columns
			{
				TileObj tTO = DisplayArrayRef.mTileArray[j, i];     //Get tile at this position
				if (tTO)
				{
					tTO.UpdatePosition(j, i);       //Get Tile to reposition itself needed
				}
			}
		}
	}

	public void RemoveBottomTiles()
	{
		for (int tW = 0; tW < DisplayArrayRef.Width; tW++)
		{
			if(DisplayArrayRef.mTileArray[0, tW])
			{
				Destroy(DisplayArrayRef.mTileArray[0, tW].gameObject);
				DisplayArrayRef.mTileArray[0, tW] = null;
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
			for (int tH = 1; tH < DisplayArrayRef.Height; tH++)
			{
				for (int tW = 0; tW < DisplayArrayRef.Width; tW++)
				{
					if (DisplayArrayRef.mTileArray[tH - 1, tW] == null)
					{
						DisplayArrayRef.mTileArray[tH - 1, tW] = DisplayArrayRef.mTileArray[tH, tW];       //Move tile down one
						DisplayArrayRef.mTileArray[tH, tW]=null;        //Clear this tile
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
		for (int tW = 0; tW < DisplayArrayRef.Width; tW++)
		{
			for (int tH = DisplayArrayRef.Height - 1; tH >= 0; tH--)
			{
				if ( DisplayArrayRef.mTileArray[tH, tW] == null)
				{
					DisplayArrayRef.mTileArray[tH, tW] = TileObj.MakeTile(DisplayArrayRef.TileObjPrefab, tH, tW, Random.Range(0, 7));
				}
				else
				{
					break;      //When first tile hit in column downfill stop fill for this column
				}
			}
		}
	}

	float mTime = 0f;

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
