using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ArrayBasedCameraPosition();

    }
    void ArrayBasedCameraPosition()
    {
        Camera.main.orthographicSize = (float)GM.Width / Camera.main.aspect / 2.0f;

        float tHalfHeight = Camera.main.orthographicSize;
        float tHalfWidth = Camera.main.aspect * tHalfHeight;
        transform.position = new Vector3(tHalfWidth - 0.5f, tHalfHeight - 0.5f, transform.position.z);
    }
}
