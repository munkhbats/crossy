using UnityEngine;
using System.Collections;

public class Soldier : MonoBehaviour {

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}

	public void Shoot(){
		animator.SetTrigger("Shoot");
	}
}
