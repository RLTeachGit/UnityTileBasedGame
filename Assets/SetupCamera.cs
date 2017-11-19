using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupCamera : MonoBehaviour {


	// Use this for initialization
	void Start () {
        ArrayBasedCameraPosition(GM.Width);
    }

    void ArrayBasedCameraPosition(int vArrayWidth)
    {
        //What Height would we need to be to get this width
        Camera.main.orthographicSize = (float)vArrayWidth / Camera.main.aspect / 2.0f;
        float tHalfHeight = Camera.main.orthographicSize;       //Reposition Camera
        float tHalfWidth = Camera.main.aspect * tHalfHeight;    
        transform.position = new Vector3(tHalfWidth - 0.5f      //Add Half tile offset
                                            ,tHalfHeight - 0.5f //& reposition camera
                                            ,transform.position.z);
    }
}
