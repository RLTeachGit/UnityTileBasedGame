using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj : MonoBehaviour {

    //Assign possible TileSprites in IDE, from spritesheet
    public Sprite[] TileSprites;

    int mRow;       //Store Tile position
    int mColumn;

    //Static maker function, can be called before  Instance exists
    public static TileObj   MakeTile(GameObject vTilePrefab, int vRow, int vColumn,int vType)
    {
        TileObj tTO=null;
        GameObject tGO = Instantiate(vTilePrefab); //Make a tile object from Prefab & position
        tTO = tGO.GetComponent<TileObj>();      //Get The TileObj script to talk to
        SpriteRenderer tSR = tGO.GetComponent<SpriteRenderer>();        //Get Sprire renderer to set sprite type, base don what was requested
		tTO.UpdatePosition(vRow,vColumn);
        tSR.sprite = tTO.TileSprites[vType];        //Display correct Sprite
        return  tTO;
    }

    public  void    UpdatePosition(int vRow,int vColumn)
    {
        mColumn = vColumn;
        mRow = vRow;
        transform.position = new    Vector3(mColumn, mRow, transform.position.z);       //Update Screen Position
    }
}
