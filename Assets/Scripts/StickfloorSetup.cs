using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum StickfloorType {
	NORMAL = 0,
	SPEEDY = 1,
	SLOWLY = 2,
}

public enum StickfloorState {
	NONE = 0,
	MOVING = 1,
	IDLING = 2,
}

public enum StickfloorChangeProgress {
	HEAD = 0,
	MIDDLE = 1,
	TAIL = 2,
}

public class StickfloorSetup : MonoBehaviour {
	
	public Dictionary<int, StickfloorSet> stickfloorSetList = new Dictionary<int, StickfloorSet>();

	// Use this for initialization
	void Start () {
		StickfloorSet stickfloor1 = new StickfloorSet();
		stickfloor1.index = 1;
		stickfloor1.floorCount = 18;
		stickfloor1.name = "Standard";
		stickfloor1.idleTime = 4f;
		stickfloor1.position = FloorPosition.TOP;
		stickfloor1.type = StickfloorType.NORMAL;
		stickfloorSetList.Add(stickfloor1.index, stickfloor1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public float GetTypeTime(StickfloorType type){
		
		float speedTime = 1f;
		
		switch(type){
		case StickfloorType.SLOWLY:
			speedTime = 2f;
			break;
		case StickfloorType.NORMAL:
			speedTime = 0.05f;
			break;
		case StickfloorType.SPEEDY:
			speedTime = 0.5f;
			break;
		}
		
		return speedTime;
	}
}

[System.Serializable]
public class StickfloorSet
{
	public int index;
	public string name;
	public int floorCount;
	public StickfloorType type;
	public FloorPosition position;
	public float idleTime;
}

public class StickfloorManage
{
	public int columnNumber;
	public int[] stickfloorRoot;
	public int existingStickfloorCount;
	public int leftStickfloor;
	public float speedTimeCount;
	public float idleTimeCount;
	public StickfloorState movingState;
	public StickfloorSet stickfloorSet;
}

