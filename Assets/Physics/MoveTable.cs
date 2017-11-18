using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTable : MonoBehaviour {

    float tCurrent = 0.0f;
    public float Speed = 10.0f;
    Quaternion tOriginalRotation;

    bool mShowSprites=true;

    public  GameObject[] ToggleThese;


	// Use this for initialization
	void Start () {
        tOriginalRotation = transform.rotation;
        SetVisibility();
    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = tOriginalRotation * Quaternion.Euler(0, 0, MakeRotation(tCurrent)*20.0f);
        tCurrent += Time.deltaTime;
	}


    float   MakeRotation(float tValue)
    {
        return  Mathf.Sin(tValue);
    }

    void SetVisibility()
    {
        foreach(GameObject tGO in ToggleThese)
        {
            tGO.GetComponent<SpriteRenderer>().enabled = mShowSprites;
        }
        mShowSprites =! mShowSprites;
        Invoke("SetVisibility", 5.0f);
    }

}
