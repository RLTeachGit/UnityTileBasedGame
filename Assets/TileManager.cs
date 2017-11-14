using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {


	//Link to tile sprites in IDE
	public	Sprite[]	TileSprites;
	public	TileObject	TileObjectPrefab;

	uint 	mTileCount;

	public	uint	TileCount {		//Public Getter for tilecount
		get {
			return	mTileCount;
		}
	}


	//Gte Max tilecount for this set & link to Game Manager

	void Start () {
		mTileCount = (uint)TileSprites.Length;		//Number of tile sprites we can choose from
		GM.sGM.mTM=this;		//Link TileManager to GM, for global access
	}

	public	TileObject	MakeTileObject(uint vID,uint vX, uint vY) {
		TileObject	tTO = null;
		if (vID < mTileCount) {
			tTO=Instantiate(TileObjectPrefab);		//Make New Tile Object
			tTO.Initialise(vID,vX,vY,TileSprites[vID]);		//Set it to desired sprite
		} else {
			Debug.LogFormat ("Invalid Tile ID {d}", vID);
		}
		return	tTO;
	}
}
