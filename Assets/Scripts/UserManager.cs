using UnityEngine;
using System.Collections;

public class UserManager : MonoBehaviour {
	
	private bool deadAnimation;
	Animator anim;
	
	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(GameManager.I.EndState == true && GameManager.I.DeathType != UserDeathType.NONE && GameManager.I.DeadState == false && GameManager.I.CameraZoomIn == true){
			GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

			if(anim.GetBool("SitIdle")){
				anim.SetBool("SitIdle", false);
			}

			StartDeathAnimation();
			GameManager.I.DeadState = true;
		}
	}

	public void StartDeathAnimation(){

		switch(GameManager.I.DeathType){
		case UserDeathType.BULLET:
			anim.SetBool("Explode", true);
			break;
		case UserDeathType.HUMMER:
			anim.SetBool("Hummer_dead", true);
			break;
		case UserDeathType.WATER:
			anim.SetBool("Prolong", true);
			break;
		case UserDeathType.CAMERA:
			anim.SetBool("Squeeze", true);
			break;
		}
	}

	// Called from death animation last frame event
	public void EndDeathAnimation(){

		switch(GameManager.I.DeathType){
		case UserDeathType.BULLET:
			anim.SetBool("Explode", false);
			anim.SetBool("Dead", true);
			break;
		case UserDeathType.HUMMER:
			break;
		case UserDeathType.WATER:
			anim.SetBool("Prolong", false);
			anim.SetBool("Prolong_dead", true);
			break;
		case UserDeathType.CAMERA:
			anim.SetBool("Squeeze", false);
			anim.SetBool("Squeeze_dead", true);
			break;
		}
	}
}


