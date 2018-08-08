using UnityEngine;
using System.Collections;

public class Logo : MonoBehaviour {

	Vector2 slidePosition = new Vector2(500f, -40.3f);
	Vector2 clearPosition = new Vector2(400f, 400f);
	private float cameraFollowSpeed = 1f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if(gameObject.transform.position.x > 400f){
			gameObject.transform.position = clearPosition;
		}

		if(GameManager.I.StartState == true){
			gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, slidePosition, cameraFollowSpeed * Time.deltaTime);
		}
	}
}
