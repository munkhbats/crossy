using UnityEngine;
using System.Collections;

public class RollingRock : MonoBehaviour {
	private int speed;
	public RollingRockType rollingRockType;
	public FloorPosition rollingRockPosition;
	public bool deathCarPlay = false;
	
	
	void Start () {
		speed = (int)rollingRockType;
	}
	
	void OnTriggerEnter2D(Collider2D c)
	{
		if(c.gameObject.name == "Ninja"){
			SpriteRenderer spRenderer = c.gameObject.GetComponent<SpriteRenderer>();
			spRenderer.sortingLayerName = "DeadNinja";
			Debug.Log("Dead by ninjastar");
			GameManager.I.EndState = true;
			TestSoundManager.I.PlayDeathCarSound();
			GameManager.I.DeathType = UserDeathType.HUMMER;
		}
	}
	
	void Update () {
		
		if(gameObject.name != "Deactivated"){
			if(transform.position.y < -250){
				gameObject.name = "Deactivated";
				Vector3 pos = new Vector3(0, 0, 0);
				GetComponent<Rigidbody2D>().velocity = pos;
			}else{
				Activate ();
			}
		}
	}
	
	void Activate ()
	{
		Vector3 pos = new Vector3(0, 0, 0);
		
		if(rollingRockPosition == FloorPosition.TOP){
			pos = new Vector3(-0.21f, -0.79f, 0f);
		}
		
		GetComponent<Rigidbody2D>().velocity = pos.normalized * speed;
	}
}
