using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class TileObject : MonoBehaviour {

	private SpriteRenderer	mSR;        //Cached Sprite Renderer
        
    int	mID;
	public	int	ID 
    {		//Public Getter for TileID, dont want a setter as the tile ID should not change
		get 
        {
			return	mID;
		}
	}

    [HideInInspector]
    public  bool mTagged;      //Is Tile tagged

	int	mX;     //X position in tile space
    public  int    X   //Getter & Setter for position
    {
        get 
        {
            return  mX;
        }
        set 
        {
            mX = value;
            UpdateScreenPosition();  //If object moves in tile space, also update screen position
        }
    }

    int    mY; //y position in tile space
    public  int Y //Getter & Setter for position
    {
        get 
        {
            return mY;
        }

        set 
        {
            mY = value;
            UpdateScreenPosition();     //If object moves in tile space, also update screen position
        }
    }


    //Helper function to position object in TileSpace and reflect this in screen position
    public void    SetXY(int vX,int vY)     
    {
        mX = vX;
        mY = vY;
        UpdateScreenPosition();     //If object moves in tile space, also update screen position
    }

    //Set up a Tile Object with a sprite renderer
    public void	Initialise(int vID,int vX,int vY,Sprite vSprite) {
		mSR = gameObject.AddComponent<SpriteRenderer> (); //Add Sprite Renderer
        BoxCollider2D tBC=gameObject.AddComponent<BoxCollider2D>(); //Add 2D Boxcollider
        tBC.isTrigger = true;       //Turn it into a trigger
        tBC.size = Vector2.one*0.8f;    //Add a colider just smaller than the 1x1 tile
        mID =vID;
		mSR.sprite = vSprite;
        SetXY(vX, vY);
		name = string.Format ("TileID {0:d}", mID);
	}

    //Calculate the Screen Position from this TileSpace position
	void	UpdateScreenPosition() {
		transform.position = new Vector2 (mX, mY);
	}
}
