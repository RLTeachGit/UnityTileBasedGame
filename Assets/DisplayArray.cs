using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayArray : MonoBehaviour {

    //Assign in editor
    public GameObject TileObjPrefab;

	public	TileObj[,] mTileArray=new TileObj[10,7];

	#region ArrayHelpers
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

	public	TileObj[,] TileArray
	{
		get
		{
			return mTileArray;
		}	
	}
	#endregion

    void Start()
    {
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
}
