using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugText : MonoBehaviour {
	
	Text text;
	
	// Use this for initialization
	void Start () {
		text = GetComponent <Text> ();
	}
	
	// Update is called once per frame
	void Update () {

		if(gameObject.name == "Score"){
			text.text = "Score : " + GameManager.I.UserScore.ToString();
			
			if (GameManager.I.DeadState == true){
				text.fontSize = 40;
				text.alignment = TextAnchor.MiddleRight;
				text.text = "Score : " + GameManager.I.UserScore.ToString() + "\n\nTAP TO RETRY";
			}
		}else{
			text.text = "High Score : " + HighScore.I.UserHighScore.ToString();
		}
	}
}
