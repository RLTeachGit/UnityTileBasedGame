using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour {


    public  Text ScoreText;     //Link in IDE

	// Use this for initialization
	void Start () {
        GM.sGM.mScoring = this;     //Link this to GameManager;
        SetScore(0);
	}

    public  void    SetScore(int vScore)
    {
        if(ScoreText!=null)
        {
            ScoreText.text = string.Format("{0:d}", vScore);
        }
    }

}
