using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCamera : MonoBehaviour {


	// Use this for initialization
	void Start () {
		ArrayBasedCameraPosition ();
	}


	//Make it easy on ourselves bu moving Camera so (0,0) is in bottom left corner
	//Also make it fit max width of tile array, to allow easy array->Screen mapping
	void	ArrayBasedCameraPosition() {
		Camera.main.orthographicSize = (float)GM.Width/Camera.main.aspect/2.0f;

		float	tHalfHeight = Camera.main.orthographicSize;
		float	tHalfWidth = Camera.main.aspect * tHalfHeight;
		transform.position = new Vector3 (tHalfWidth - 0.5f, tHalfHeight - 0.5f,transform.position.z);
	}
}
