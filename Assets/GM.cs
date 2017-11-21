using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {

    [HideInInspector]
	public	TileManager	mTM;		//Set by TileManager, allows singleton instance access

    [HideInInspector]
    public Scoring mScoring;

	[HideInInspector]
    public int Score;


	AudioSource	mAudio;


	//Handy for labeling sections of code
	#region Singleton      
	public static GM sGM;       //Allows access to singleton
	//Being static means yoiu can access without knowing instance
	void Awake () {		        //Runs before Start
		if (sGM == null) {      //Has it been set up before?
			sGM = this;		    //No, its the first Time creation of Game Manager, so store our instance
			DontDestroyOnLoad(gameObject);  //Persist, now it will survive scene reloads
			mAudio = GetComponent<AudioSource> ();
		} else if (sGM != this) { //If we get called again, then destroy new version and keep old one
			Destroy (gameObject);   //Kill any subsequent one
		}

        StartCoroutine(WaitForStartup());
	}
	#endregion

    IEnumerator WaitForStartup()        //Wait for other GO's to start up before starting game
    {
        while(mScoring==null || mTM==null)
        {
            yield return new    WaitForSeconds(0.1f);
        }
		NewGame ();
    }



    public  void    UpdateUIScore()
    {
        if (mScoring != null)
        {
            mScoring.SetScore(Score);     //Update Score in UI
        }
    }

	public	void	PlayClick()
	{
		mAudio.Play ();
	}

	public void NewGame()
	{
		Score = 0;
		UpdateUIScore();
		mTM.NewTiles();
	}

}
