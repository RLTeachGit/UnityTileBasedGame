using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {


	public	TileManager	mTM;		//Set by TileManager, allows singleton instance access

	public	TileObject[,]	mTileArray;

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
				mTileArray [tH, tW] = mTM.MakeTileObject ((uint)Random.Range (0, mTM.TileCount), tW, tH);
			}
		}
	}




}
