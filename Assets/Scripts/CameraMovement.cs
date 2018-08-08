using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	[SerializeField]
	private Transform target;

	private float speedySmoothing = 3f;
	private float smoothing = 2f;
	private float ultraSmoothing = 1f;
	private float leftClose = 10f;
	private float upDownClose = 3f;
	private float cameraFollowSpeed = 80f;
	private float userDieBuffer = 40f;
	private float size1 = 38f;
	private float size2 = 25f;
	private float smoothZoomIn = 10f;
	private Vector3 camPos;
	Vector3 offset;
	
	// Use this for initialization
	void Start () {
		offset = transform.position - target.position;
		Camera.main.orthographicSize = size1;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 targetPos = transform.position;
		float changeSpeed = smoothing * Time.deltaTime;
		Vector3 targetCamPos = target.position + offset;

		if(GameManager.I.StartState == true && GameManager.I.EndState == false){

			if(targetCamPos.x < transform.position.x - userDieBuffer){
				GameManager.I.EndState = true;
				GameManager.I.DeathType = UserDeathType.CAMERA;
			}

			if(targetCamPos.y - upDownClose > transform.position.y && targetCamPos.x + leftClose > transform.position.x){
				targetPos.x = targetCamPos.x + leftClose; 
				targetPos.y = targetCamPos.y + upDownClose ;
				changeSpeed = Time.deltaTime * ultraSmoothing;
			}else if(targetCamPos.x + leftClose > transform.position.x){
				targetPos.x = targetCamPos.x + leftClose; 
				changeSpeed = Time.deltaTime * smoothing;
			}else if(targetCamPos.x > transform.position.x){
				targetPos.x = targetCamPos.x; 
				changeSpeed = Time.deltaTime * smoothing;
			}else if(targetCamPos.y < transform.position.y){
				targetPos.x += speedySmoothing * Time.deltaTime;
				targetPos.y = targetCamPos.y;
				changeSpeed = Time.deltaTime;
			}else if(targetCamPos.y - upDownClose > transform.position.y){
				targetPos.y = targetCamPos.y + speedySmoothing ;
				changeSpeed = Time.deltaTime;
			}

			targetPos.x += cameraFollowSpeed * Time.deltaTime;
			transform.position = Vector3.Lerp(transform.position, targetPos, changeSpeed);
			camPos = transform.position;

		}else if (GameManager.I.EndState == true) {
			if(GameManager.I.DeathType == UserDeathType.CAMERA){
				changeSpeed = Time.deltaTime * smoothing;
				Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, size2, Time.deltaTime * smoothing);
				transform.position = Vector3.Lerp(transform.position, targetCamPos, changeSpeed);
				GameManager.I.CameraZoomIn = true;
			}else if(GameManager.I.DeadState == false){
				StartCoroutine(Vibrate());
			}
		}
	}

	IEnumerator Vibrate() {
		Vector3 transTo = camPos;
		int rand;
		rand = Random.Range(1, 4);
		
		if(rand == 4){
			transTo.x += 2f;
			transTo.y += 2f;
		}else if(rand == 1){
			transTo.x -= 2f;
			transTo.y += 2f;
		}else if(rand == 2){
			transTo.x += 2f;
			transTo.y -= 2f;
		}else{
			transTo.x -= 2f;
			transTo.y -= 2f;
		}
		
		float changeSpeed = Time.deltaTime * speedySmoothing;
		transform.position = Vector3.Lerp(transform.position, transTo , changeSpeed);
		yield return new WaitForSeconds(0.1f);
		GameManager.I.CameraZoomIn = true;
	}
}
