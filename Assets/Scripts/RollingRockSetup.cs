using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RollingRockType {
	NORMAL = 125,
	SPEEDY = 150,
	SLOWLY = 100,
}

public enum RollingRockState {
	NONE = 0,
	MOVING = 1,
	IDLING = 2,
}

public class RollingRockSetup : MonoBehaviour {
	
	public Dictionary<int, RollingRockSet> rollingRockSetList = new Dictionary<int, RollingRockSet>();
	public Dictionary<int, int> rollingRockSetConf = new Dictionary<int, int>();
	
	// Use this for initialization
	void Start () {
		RollingRockSet rollingRock1 = new RollingRockSet();
		rollingRock1.index = 1;
		rollingRock1.name = "Standard";
		rollingRock1.idleTime = 4f;
		rollingRock1.position = FloorPosition.TOP;
		rollingRock1.type = RollingRockType.SPEEDY;
		rollingRock1.rollingRocks.Add (new RollingRockDef(0.5f));
		rollingRock1.rollingRocks.Add (new RollingRockDef(1.5f));
		rollingRock1.rollingRocks.Add (new RollingRockDef(1.7f));
		rollingRockSetList.Add(rollingRock1.index, rollingRock1);
		
		RollingRockSet rollingRock2 = new RollingRockSet();
		rollingRock2.index = 2;
		rollingRock2.name = "Standard";
		rollingRock2.idleTime = 5f;
		rollingRock2.position = FloorPosition.TOP;
		rollingRock2.type = RollingRockType.SPEEDY;
		rollingRock2.rollingRocks.Add (new RollingRockDef(1f));
		rollingRock2.rollingRocks.Add (new RollingRockDef(2f));
		rollingRock2.rollingRocks.Add (new RollingRockDef(2.2f));
		rollingRockSetList.Add(rollingRock2.index, rollingRock2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public class RollingRockDef
{
	public float time;
	
	public RollingRockDef(float time)
	{
		this.time = time;
	}
}

[System.Serializable]
public class RollingRockSet
{
	public int index;
	public string name;
	public int floorNum;
	public RollingRockType type;
	public FloorPosition position;
	public float idleTime;
	public List<RollingRockDef> rollingRocks = new List<RollingRockDef>();
}

public class RollingRockManage
{
	public int columnNumber;
	public int rollingRockCount;
	public float idleTimeCount;
	public RollingRockType type;
	public FloorPosition position;
	public RollingRockState movingState;
	public RollingRockSet rollingRockSet;
	public bool rollingRockStartState;
}
