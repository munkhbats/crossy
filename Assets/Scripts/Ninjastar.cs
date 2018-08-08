using UnityEngine;
using System.Collections;

public class Ninjastar : MonoBehaviour {
	private int speed;
	public NinjaStarType starType;
	public FloorPosition starPosition;
	public bool deathStarPlay = false;

	
	void Start () {
		speed = (int)starType;
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if(c.gameObject.name == "Ninja"){
			SpriteRenderer spRenderer = c.gameObject.GetComponent<SpriteRenderer>();
			spRenderer.sortingLayerName = "DeadNinja";
			GameManager.I.DeathType = UserDeathType.BULLET;
			TestSoundManager.I.PlayDeathSound();
			GameManager.I.EndState = true;
		}
	}

	void Update () {

		if(gameObject.name != "Deactivated"){
			if(transform.position.y < -170 || transform.position.y > 10){
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
		Vector3 pos;

		if(starPosition == FloorPosition.TOP){
			pos = new Vector3(-0.21f, -0.79f, 0f);
		}else{
			pos = new Vector3(0.21f, 0.79f, 0f);
		}

		GetComponent<Rigidbody2D>().velocity = pos.normalized * speed;
	}

	public int ChangeSpeed {
		get { return speed; }
		set { speed = value; }
	}
}
