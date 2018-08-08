using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum FlowingfloorType {
	NORMAL = 15,
	SPEEDY = 20,
	SLOWLY = 10,
	NONE = 0,
}

public enum FlowingfloorState {
	NONE = 0,
	MOVING = 1,
	IDLING = 2,
}

public class FlowingfloorSetup : MonoBehaviour {

	public Dictionary<int, FlowingfloorSet> flowingfloorSetList = new Dictionary<int, FlowingfloorSet>();

	// Use this for initialization
	void Start () {
		FlowingfloorSet flowingfloor1 = new FlowingfloorSet();
		flowingfloor1.index = 1;
		flowingfloor1.name = "Standard";
		flowingfloor1.idleTime = 3f;
		flowingfloor1.position = FloorPosition.TOP;
		flowingfloor1.type = FlowingfloorType.SLOWLY;
		flowingfloor1.flowingfloorSizeType = FlowingfloorSizeType.LONG;
		flowingfloorSetList.Add(flowingfloor1.index, flowingfloor1);
		
		FlowingfloorSet flowingfloor2 = new FlowingfloorSet();
		flowingfloor2.index = 2;
		flowingfloor2.name = "Standard";
		flowingfloor2.idleTime = 3f;
		flowingfloor2.position = FloorPosition.BOTTOM;
		flowingfloor2.type = FlowingfloorType.NORMAL;
		flowingfloor2.flowingfloorSizeType = FlowingfloorSizeType.LONG;
		flowingfloorSetList.Add(flowingfloor2.index, flowingfloor2);
		
		FlowingfloorSet flowingfloor3 = new FlowingfloorSet();
		flowingfloor3.index = 3;
		flowingfloor3.name = "Standard";
		flowingfloor3.idleTime = 2f;
		flowingfloor3.position = FloorPosition.TOP;
		flowingfloor3.type = FlowingfloorType.SLOWLY;
		flowingfloor3.flowingfloorSizeType = FlowingfloorSizeType.SHORT;
		flowingfloorSetList.Add(flowingfloor3.index, flowingfloor3);
		
		FlowingfloorSet flowingfloor4 = new FlowingfloorSet();
		flowingfloor4.index = 4;
		flowingfloor4.name = "Standard";
		flowingfloor4.idleTime = 2f;
		flowingfloor4.position = FloorPosition.BOTTOM;
		flowingfloor4.type = FlowingfloorType.NORMAL;
		flowingfloor4.flowingfloorSizeType = FlowingfloorSizeType.MEDIUM;
		flowingfloorSetList.Add(flowingfloor4.index, flowingfloor4);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public class FlowingfloorSet
{
	public int index;
	public string name;
	public int floorCount;
	public FlowingfloorType type;
	public FloorPosition position;
	public float idleTime;
	public FlowingfloorSizeType flowingfloorSizeType;
}

public class FlowingfloorManage
{
	public int columnNumber;
	public float speedTimeCount;
	public float idleTimeCount;
	public int count;
	public FlowingfloorState movingState;
	public FlowingfloorSet flowingfloorSet;
}
