using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundManager : ObjectPool {

	private List<GameObject> tempBackgrounds = new List<GameObject>();
	private List<GameObject> tempTopBorder = new List<GameObject>();
	private List<GameObject> tempBottomBorder = new List<GameObject>();
	private int allBackgroundNum = 0;
	private int allTopBorderNum = 0;
	private int allBottomBorderNum = 0;
	private int borderNum = 0;
	private float backgroundWidthSize = 70f;
	private float widthSize = 70f;
	private Vector3 initialPosition = new Vector3(33.4f, 19.1f, 0f);
	private Vector3 topBorderPosition = new Vector3(-0.3f, -13.7f, 0f);
	private Vector3 bottomBorderPosition = new Vector3(-47.4f, -56.4f, 0f);

	[SerializeField]
	private GameObject BackgroundObject;
	[SerializeField]
	private GameObject TopBorderObject;
	[SerializeField]
	private GameObject BottomBorderObject;
	[SerializeField]
	private Camera mainCamera;

	// Use this for initialization
	void Start () {
		AddBackground(3);
	}
	
	// Update is called once per frame
	void Update () {
//		Vector3 cameraPosition = this.mainCamera.transform.position;
//		float distance = cameraPosition.x - initialPosition.x;
//		int floorNum = Mathf.RoundToInt(distance / backgroundWidthSize);
/*
		if(floorNum >= allBackgroundNum - 1){
			AddBackground(1);
			RemoveBackground();
//			AddTopBorder(1);
//			RemoveTopBorder();
//			AddBottomBorder(1);
//			RemoveBottomBorder();
		}
*/		
	}

	void AddBackground (int countNum) {
		for(int count = 0; count < countNum; count++) {
			float x = allBackgroundNum * backgroundWidthSize;
			Vector3 point = new Vector3(x, 0, 0);
			string name = "background_" + allBackgroundNum;
			GameObject tempObject = CreateObject(BackgroundObject, point, name, allBackgroundNum);
			tempBackgrounds.Add(tempObject);
			allBackgroundNum++;
		}
	}

	void AddTopBorder (int countNum) {
		for(int count = 0; count < countNum; count++) {
			float x = (allTopBorderNum * widthSize) + topBorderPosition.x;
			Vector3 point = new Vector3(x, topBorderPosition.y, 0);
			string name = "topBorder_" + allTopBorderNum;
			GameObject tempObject = CreateObject(TopBorderObject, point, name, allTopBorderNum);
			tempTopBorder.Add(tempObject);
			allTopBorderNum++;
		}
	}

	void AddBottomBorder (int countNum) {
		for(int count = 0; count < countNum; count++) {
			float x = (allBottomBorderNum * widthSize) + bottomBorderPosition.x;
			Vector3 point = new Vector3(x, bottomBorderPosition.y, 0);
			string name = "bottomBorder_" + allBottomBorderNum;
			GameObject tempObject = CreateObject(BottomBorderObject, point, name, allBottomBorderNum);
			tempBottomBorder.Add(tempObject);
			allBottomBorderNum++;
		}
	}
	
	GameObject CreateObject (GameObject objectPrefab, Vector3 point, string name, int order) {
		Quaternion objRotation = Quaternion.Euler(0, 0, 0);
		GameObject tempObject = GetGameObject (objectPrefab, point, objRotation, order, name);
		return tempObject;
	}

	void RemoveBackground () {
		ReleaseGameObject (tempBackgrounds[0]);
		tempBackgrounds.RemoveAt(0);
	}

	void RemoveTopBorder () {
		ReleaseGameObject (tempTopBorder[0]);
		tempTopBorder.RemoveAt(0);
	}

	void RemoveBottomBorder () {
		ReleaseGameObject (tempBottomBorder[0]);
		tempBottomBorder.RemoveAt(0);
	}
}
