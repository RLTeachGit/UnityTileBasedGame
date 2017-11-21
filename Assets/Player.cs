using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;




public class Player : MonoBehaviour {

    public	TileManager TileManagerRef;


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (EventSystem.current.IsPointerOverGameObject ())
			return;
		if(Input.GetMouseButtonDown(0))
        {
            Ray tRayIntoScene = Camera.main.ScreenPointToRay(Input.mousePosition);        //Make Ray from Camera into scene 
            RaycastHit2D tHit = Physics2D.Raycast(tRayIntoScene.origin, tRayIntoScene.direction, Mathf.Infinity);   //Send ray from camera into scene
            if(tHit.collider != null)
            {
                TileObject tTO = tHit.collider.gameObject.GetComponent<TileObject>();       //Get Tile at location
				if (tTO) 
				{
					TileManagerRef.TileClicked (tTO);
				}
            }
        }
	}


	public	void	Restart()
	{
		GM.sGM.NewGame ();
	}

}
