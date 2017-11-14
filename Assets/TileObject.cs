using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour {


	uint	mID;
	public	uint	ID {		//Public Getter for TileID
		get {
			return	mID;
		}
	}

	SpriteRenderer	mSR;
	uint	mX;
	uint	mY;

	//Set up a Tile Object with a sprite renderer

	public	void	Initialise(uint vID,uint vX,uint vY,Sprite vSprite) {
		mSR = gameObject.AddComponent<SpriteRenderer> (); //Add Sprite Renderer
		mID=vID;
		mSR.sprite = vSprite;
		mX = vX;
		mY = vY;
		name = string.Format ("TileID {0:d}", mID);
		UpdateScreenPosition ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void	UpdateScreenPosition() {
		transform.position = new Vector2 (mX, mY);
	}
}
