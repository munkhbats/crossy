using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum NinjaStarType {
	NORMAL = 20,
	SPEEDY = 30,
	SLOWLY = 10,
}

public enum NinjastarState {
	NONE = 0,
	MOVING = 1,
	IDLING = 2,
}

public class NinjastarSetup : MonoBehaviour {
	
	public Dictionary<int, NinjaStarSet> ninjaStarSetList = new Dictionary<int, NinjaStarSet>();
	public Dictionary<int, int> ninjaStarSetConf = new Dictionary<int, int>();

	// Use this for initialization
	void Start () {
		NinjaStarSet ninjaStar1 = new NinjaStarSet();
		ninjaStar1.index = 1;
		ninjaStar1.name = "Standard";
		ninjaStar1.idleTime = 2.1f;
		ninjaStar1.position = FloorPosition.TOP;
		ninjaStar1.type = NinjaStarType.NORMAL;
		ninjaStar1.ninjastars.Add (new NinjaStarDef(0f));
		ninjaStarSetList.Add(ninjaStar1.index, ninjaStar1);
		
		NinjaStarSet ninjaStar2 = new NinjaStarSet();
		ninjaStar2.index = 2;
		ninjaStar2.name = "Standard";
		ninjaStar2.idleTime = 2.2f;
		ninjaStar2.position = FloorPosition.BOTTOM;
		ninjaStar2.type = NinjaStarType.NORMAL;
		ninjaStar2.ninjastars.Add (new NinjaStarDef(0f));
		ninjaStarSetList.Add(ninjaStar2.index, ninjaStar2);
		
		NinjaStarSet ninjaStar3 = new NinjaStarSet();
		ninjaStar3.index = 3;
		ninjaStar3.name = "Standard";
		ninjaStar3.idleTime = 2.6f;
		ninjaStar3.position = FloorPosition.TOP;
		ninjaStar3.type = NinjaStarType.NORMAL;
		ninjaStar3.ninjastars.Add (new NinjaStarDef(2f));
		ninjaStar3.ninjastars.Add (new NinjaStarDef(2.5f));
		ninjaStarSetList.Add(ninjaStar3.index, ninjaStar3);
		
		NinjaStarSet ninjaStar4 = new NinjaStarSet();
		ninjaStar4.index = 4;
		ninjaStar4.name = "Standard";
		ninjaStar4.idleTime = 2.6f;
		ninjaStar4.position = FloorPosition.BOTTOM;
		ninjaStar4.type = NinjaStarType.NORMAL;
		ninjaStar4.ninjastars.Add (new NinjaStarDef(2f));
		ninjaStar4.ninjastars.Add (new NinjaStarDef(2.5f));
		ninjaStarSetList.Add(ninjaStar4.index, ninjaStar4);
		
		NinjaStarSet ninjaStar5 = new NinjaStarSet();
		ninjaStar5.index = 5;
		ninjaStar5.name = "Standard";
		ninjaStar5.idleTime = 4.8f;
		ninjaStar5.position = FloorPosition.TOP;
		ninjaStar5.type = NinjaStarType.NORMAL;
		ninjaStar5.ninjastars.Add (new NinjaStarDef(1f));
		ninjaStar5.ninjastars.Add (new NinjaStarDef(1.5f));
		ninjaStar5.ninjastars.Add (new NinjaStarDef(3f));
		ninjaStar5.ninjastars.Add (new NinjaStarDef(3.5f));
		ninjaStar5.ninjastars.Add (new NinjaStarDef(4f));
		ninjaStarSetList.Add(ninjaStar5.index, ninjaStar5);
		
		NinjaStarSet ninjaStar6 = new NinjaStarSet();
		ninjaStar6.index = 6;
		ninjaStar6.name = "Standard";
		ninjaStar6.idleTime = 5.1f;
		ninjaStar6.position = FloorPosition.BOTTOM;
		ninjaStar6.type = NinjaStarType.NORMAL;
		ninjaStar6.ninjastars.Add (new NinjaStarDef(1f));
		ninjaStar6.ninjastars.Add (new NinjaStarDef(1.5f));
		ninjaStar6.ninjastars.Add (new NinjaStarDef(3f));
		ninjaStar6.ninjastars.Add (new NinjaStarDef(3.5f));
		ninjaStar6.ninjastars.Add (new NinjaStarDef(4f));
		ninjaStarSetList.Add(ninjaStar6.index, ninjaStar6);
		
		NinjaStarSet ninjaStar7 = new NinjaStarSet();
		ninjaStar7.index = 7;
		ninjaStar7.name = "Standard";
		ninjaStar7.idleTime = 2.4f;
		ninjaStar7.position = FloorPosition.TOP;
		ninjaStar7.type = NinjaStarType.SPEEDY;
		ninjaStar7.ninjastars.Add (new NinjaStarDef(0f));
		ninjaStarSetList.Add(ninjaStar7.index, ninjaStar7);
		
		NinjaStarSet ninjaStar8 = new NinjaStarSet();
		ninjaStar8.index = 8;
		ninjaStar8.name = "Standard";
		ninjaStar8.idleTime = 2.5f;
		ninjaStar8.position = FloorPosition.BOTTOM;
		ninjaStar8.type = NinjaStarType.SPEEDY;
		ninjaStar8.ninjastars.Add (new NinjaStarDef(0f));
		ninjaStarSetList.Add(ninjaStar8.index, ninjaStar8);
		
		NinjaStarSet ninjaStar9 = new NinjaStarSet();
		ninjaStar9.index = 9;
		ninjaStar9.name = "Standard";
		ninjaStar9.idleTime = 1.6f;
		ninjaStar9.position = FloorPosition.TOP;
		ninjaStar9.type = NinjaStarType.SLOWLY;
		ninjaStar9.ninjastars.Add (new NinjaStarDef(0f));
		ninjaStarSetList.Add(ninjaStar9.index, ninjaStar9);
		
		NinjaStarSet ninjaStar10 = new NinjaStarSet();
		ninjaStar10.index = 10;
		ninjaStar10.name = "Standard";
		ninjaStar10.idleTime = 1.7f;
		ninjaStar10.position = FloorPosition.BOTTOM;
		ninjaStar10.type = NinjaStarType.SLOWLY;
		ninjaStar10.ninjastars.Add (new NinjaStarDef(0f));
		ninjaStarSetList.Add(ninjaStar10.index, ninjaStar10);
		
		NinjaStarSet ninjaStar11 = new NinjaStarSet();
		ninjaStar11.index = 11;
		ninjaStar11.name = "Standard";
		ninjaStar11.idleTime = 2.2f;
		ninjaStar11.position = FloorPosition.TOP;
		ninjaStar11.type = NinjaStarType.SLOWLY;
		ninjaStar11.ninjastars.Add (new NinjaStarDef(0f));
		ninjaStar11.ninjastars.Add (new NinjaStarDef(0.5f));
		ninjaStarSetList.Add(ninjaStar11.index, ninjaStar11);
		
		NinjaStarSet ninjaStar12 = new NinjaStarSet();
		ninjaStar12.index = 12;
		ninjaStar12.name = "Standard";
		ninjaStar12.idleTime = 2.4f;
		ninjaStar12.position = FloorPosition.BOTTOM;
		ninjaStar12.type = NinjaStarType.SLOWLY;
		ninjaStar12.ninjastars.Add (new NinjaStarDef(0f));
		ninjaStar12.ninjastars.Add (new NinjaStarDef(0.5f));
		ninjaStarSetList.Add(ninjaStar12.index, ninjaStar12);
		
		NinjaStarSet ninjaStar13 = new NinjaStarSet();
		ninjaStar13.index = 13;
		ninjaStar13.name = "Standard";
		ninjaStar13.idleTime = 3.3f;
		ninjaStar13.position = FloorPosition.TOP;
		ninjaStar13.type = NinjaStarType.NORMAL;
		ninjaStar13.ninjastars.Add (new NinjaStarDef(0f));
		ninjaStar13.ninjastars.Add (new NinjaStarDef(0.5f));
		ninjaStar13.ninjastars.Add (new NinjaStarDef(2f));
		ninjaStarSetList.Add(ninjaStar13.index, ninjaStar13);
		
		NinjaStarSet ninjaStar14 = new NinjaStarSet();
		ninjaStar14.index = 14;
		ninjaStar14.name = "Standard";
		ninjaStar14.idleTime = 3.4f;
		ninjaStar14.position = FloorPosition.BOTTOM;
		ninjaStar14.type = NinjaStarType.NORMAL;
		ninjaStar14.ninjastars.Add (new NinjaStarDef(0f));
		ninjaStar14.ninjastars.Add (new NinjaStarDef(0.5f));
		ninjaStar14.ninjastars.Add (new NinjaStarDef(2f));
		ninjaStarSetList.Add(ninjaStar14.index, ninjaStar14);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public class NinjaStarDef
{
	public float time;
	
	public NinjaStarDef(float time)
	{
		this.time = time;
	}
}

[System.Serializable]
public class NinjaStarSet
{
	public int index;
	public string name;
	public int floorNum;
	public NinjaStarType type;
	public FloorPosition position;
	public float idleTime;
	public List<NinjaStarDef> ninjastars = new List<NinjaStarDef>();
}

public class NinjaStarManage
{
	public int columnNumber;
	public int starsCount;
	public float idleTimeCount;
	public NinjaStarType type;
	public FloorPosition position;
	public NinjastarState movingState;
	public NinjaStarSet ninjaStarSet;
}
