using UnityEngine;
using System.Collections;

public class HighScore : Singleton<HighScore> {

	private int userHighScore = 0;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.I.DeadState == true && userHighScore < GameManager.I.UserScore){
			userHighScore = GameManager.I.UserScore;
		}
	}

	public int UserHighScore {
		get { return userHighScore; }
		set { userHighScore = value; }
	}
}
