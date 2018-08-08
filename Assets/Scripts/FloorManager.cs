using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class FloorManager : ObjectPool {
	
	DefaultFloor defaultFloor = new DefaultFloor();
	private int floorLengthSize;
	private Vector2 initialPosition = new Vector2(0, 0);
	private float widthSize;
	private float widthDiff;
	private float lengthDiff;
	private int modelWidthNumOnScreen;
	private int modelLengthNumOnScreen;
	private float totalWidth;
	private Vector3 floorPosition;
	private Vector3 cameraPosition;
	private float distance;
	private int startBlockNum;
	private int presentBlockNum = 0;
	private float horzExtent;
	private float autoProcessLimit;
	private Dictionary<int, NinjaStarManage> tempStars = new Dictionary<int, NinjaStarManage>();
	private Dictionary<int, StickfloorManage> tempStickfloors = new Dictionary<int, StickfloorManage>();
	private Dictionary<int, FlowingfloorManage> tempFlowingfloors = new Dictionary<int, FlowingfloorManage>();
	private Dictionary<int, RollingRockManage> tempRollingRocks = new Dictionary<int, RollingRockManage>();
	private Dictionary<int, GameObject> tempObstacles = new Dictionary<int, GameObject>();
	private Dictionary<int, GameObject> tempSoldiers = new Dictionary<int, GameObject>();
	private Dictionary<int, GameObject> tempFloors;
	private NinjastarSetup ninjastarSetup;
	private StickfloorSetup stickfloorSetup;
	private FlowingfloorSetup flowingfloorSetup;
	private FloorSetup floorSetup;
	private RollingRockSetup rollingRockSetup;
	private int currentCollectionIndex = -1;
	private int currentColumnIndex = 1;
	private GUIStyle m_guiStyle;
	private int m_fps;
	private int m_frameCount;
	private float m_nextTime;
	private int flowingFloorCount;

	[SerializeField]
	private List<GameObject> floorObjects = new List<GameObject>();
	[SerializeField]
	private List<Sprite> spriteList = new List<Sprite>();
	[SerializeField]
	private List<Sprite> obstaclesSpriteList = new List<Sprite>();
	[SerializeField]
	private List<GameObject> obstaclesObjectList = new List<GameObject>();
	[SerializeField]
	private List<GameObject> soldiersObjectList = new List<GameObject>();
	[SerializeField]
	private float ninjaStarHeight = 3f;
	[SerializeField]
	private GameObject ninjastarObject;
	[SerializeField]
	private float rollingRockHeight = 3f;
	[SerializeField]
	private GameObject rollingRockObject;
	[SerializeField]
	private GameObject rollingRockWarningObject;
	[SerializeField]
	private List<GameObject> flowingfloorObject;
	
	void Awake () {
		Floor.instance.InitCell();
		widthSize = Floor.instance.widthSize;
		widthDiff = Floor.instance.widthDiff;
		lengthDiff = Floor.instance.lengthDiff;
		modelWidthNumOnScreen = Floor.instance.modelWidthNumOnScreen;
		modelLengthNumOnScreen = Floor.instance.modelLengthNumOnScreen;
	
		floorSetup = this.GetComponent<FloorSetup>();
		ninjastarSetup = this.GetComponent<NinjastarSetup>();
		stickfloorSetup = this.GetComponent<StickfloorSetup>();
		flowingfloorSetup = this.GetComponent<FlowingfloorSetup>();
		rollingRockSetup = this.GetComponent<RollingRockSetup>();
	}

	// Use this for initialization
	void Start () {
		tempFloors = new Dictionary<int, GameObject>();
		floorLengthSize = modelLengthNumOnScreen;
		InitFloors();
		DrawFloors();
		startBlockNum = 0;
		flowingFloorCount = 0;
		totalWidth = modelWidthNumOnScreen * widthSize;
		autoProcessLimit = Screen.width / 2;

		//PlayCitySound

		TestSoundManager.I.PlayCitySound();
		
		// FPS
		m_guiStyle = new GUIStyle();
		m_guiStyle.fontSize = 20;
		GUIStyleState state = new GUIStyleState();
		state.textColor = Color.white;
		m_guiStyle.normal = state;
		
		m_nextTime = Time.time + 1;
	}
	
	// Update is called once per frame
	void Update () {
		cameraPosition = Camera.main.transform.position;
		distance = cameraPosition.x - initialPosition.x;
		int floorNum = Mathf.RoundToInt(distance / widthSize);
		if(floorNum - 9 >= startBlockNum){
			UpdateFloors();
			DrawFloors();
			startBlockNum++;
		}

		ThrowNinjastars();
		RunStickfloors();
		Flowfloors();
		RollingRocks();

		// FPS
		m_frameCount++;
		if(Time.time >= m_nextTime) {
			m_fps = m_frameCount;
			m_frameCount = 0;
			m_nextTime += 1;
		}
	}
	
	void CreateFloor (Sprite floorObject, Vector3 point, int index, int order) {
		Quaternion objRotation = Quaternion.Euler(0, 0, 0);
		GameObject floor = CreateObject(floorObjects[0], point, objRotation, order, index.ToString());
		floor.GetComponent<SpriteRenderer>().sprite = floorObject;
		tempFloors.Add(index, floor);
	}

	void CreateObstacle (Sprite floorObject, Vector3 point, int index, int order) {
		Quaternion objRotation = Quaternion.Euler(0, 0, 0);
		point.x -= 1;
		string obstacleName = "Obstacle_" + index.ToString();
		GameObject floor = CreateObject(floorObjects[1], point, objRotation, order, obstacleName);
		floor.GetComponent<SpriteRenderer>().sprite = floorObject;
		tempObstacles.Add(index, floor);
	}

	void CreateObstacleObject (Vector3 point, int objectType, int index, int order) {
		Quaternion objRotation = Quaternion.Euler(0, 0, 0);
		point.x -= 1;
		string obstacleName = "Obstacle_" + index.ToString();
		GameObject tmpObject = CreateObject(obstaclesObjectList[objectType - 1], point, objRotation, order, obstacleName);
		tempObstacles.Add(index, tmpObject);
	}

	void CreateSoldier (Vector3 point, int index, int order) {
		Quaternion objRotation = Quaternion.Euler(0, 0, 0);
		point.x -= 1;
		string soldierName = "Soldier_" + index.ToString();
		int randNum = Random.Range(0, soldiersObjectList.Count);
		GameObject tmpObject = CreateObject(soldiersObjectList[randNum], point, objRotation, order, soldierName);
		tempSoldiers.Add(index, tmpObject);
	}
	
	GameObject CreateObject (GameObject prefab, Vector3 position, Quaternion rotation, int layerIndex, string name) {
		return GetGameObject (prefab, position, rotation, layerIndex, name);
	}
	
	void InitFloors(){
		SetNewDataToCell(modelWidthNumOnScreen, true);
	}
	
	void DrawFloors(){
		int floorType;
		int length;
		int width;
		Sprite floor;
		ObstacleTypes obstacleType;

		List<int> arrayList = Floor.instance.GetCellKeys();
		for(int index=0; index < arrayList.Count; index++)
		{
			// Create if only not existed
			if(!tempFloors.ContainsKey(arrayList[index])){
				width = Floor.instance.GetWidth(arrayList[index]);
				length = Floor.instance.GetLength(arrayList[index]);
				floorType = Floor.instance.GetFloorTypeFromCell(arrayList[index]);
				obstacleType = Floor.instance.GetObstacleTypeFromCell(arrayList[index]);
				Vector3 objectPoint = Floor.instance.GetPosition(length, width, arrayList[index]);

				//  Creating floors
				CreateFloor(spriteList[floorType], objectPoint, arrayList[index], length);
				// Creating obstacles on floor
				if(obstacleType != ObstacleTypes.NONE){
					objectPoint.y -= 0.6f;
					objectPoint.x += 0.7f;

					if(obstacleType == ObstacleTypes.SOLDIER){
						CreateSoldier(objectPoint, arrayList[index], length);
					}else if(obstacleType == ObstacleTypes.POLICECAR || obstacleType == ObstacleTypes.TAXICAB){
						CreateObstacleObject(objectPoint, (int)obstacleType, arrayList[index], length);
					}else{
						CreateObstacle(obstaclesSpriteList[(int)obstacleType - 1], objectPoint, arrayList[index], length);
					}
				}
			}
		}
	}

	int GetName(int length, int width){
		int widthSize = GetWidthSize(width);
		int floorSize = Floor.instance.ConvertToFloorSize(widthSize);

		return length + floorSize;
	}

	int GetWidthSize(int width){
		return width + startBlockNum;
	}

	void SetNewDataToCell(int setCount, bool startState){
		int collectionId = GetCollectionId();
		if(startState == true){
			collectionId = defaultFloor.defaultFloorCollection;
		}

		int columnNo;
		int columnCount;

		for(int width=0; width<setCount; width++){
			columnCount = floorSetup.floorSetCollection[collectionId].floorSetConf.Count;
			columnNo = GetColumnId(columnCount);

			if(columnNo == 1){
				collectionId = GetCollectionId();
			}

			FloorColumnDef floorColumnDef = floorSetup.floorSetCollection[collectionId].floorSetConf[columnNo];
			FloorColumnSet floorList = floorSetup.floorColumnSetList[floorColumnDef.columnIndex];
			int floorSetupListCount = floorList.floorSetList.Count;
			int floorObjectIndex = 0;
			int obstacleObjectIndex = 0;
			int index;

			for(int length=0; length<modelLengthNumOnScreen; length++){

				bool listDataMatchState = false;
				int name = GetName(length, presentBlockNum);
				FloorSet tempFloorSet = new FloorSet();
				ObstacleTypes obstacleType = ObstacleTypes.NONE;
				FloorAttributes tempFloorAtt = FloorAttributes.FLOOR;
				
				if(floorObjectIndex < floorSetupListCount){
					tempFloorSet = floorList.floorSetList[floorObjectIndex];
					if(tempFloorSet.index == length){
						tempFloorAtt = tempFloorSet.floorAttribute;
						if(tempFloorSet.obstacleType != ObstacleTypes.NONE && tempFloorSet.obstacleType != null){
							obstacleType = tempFloorSet.obstacleType;
						}

						listDataMatchState = true;
						Floor.instance.SetToCell(tempFloorSet.floorType, name, obstacleType, tempFloorAtt);
						floorObjectIndex++;
					}
				}
				
				if(listDataMatchState == false){
					tempFloorSet = defaultFloor.initFloor;
					Floor.instance.SetToCell(tempFloorSet.floorType, name, obstacleType, tempFloorAtt);
					listDataMatchState = true;
				}
			}

			int widthSize = GetWidthSize(presentBlockNum);
			// If ninjastar has existing
			if (floorColumnDef.ninjastarIndex != null && floorColumnDef.ninjastarIndex > 0){
				if(tempStars.ContainsKey(widthSize) == false) {
					index = floorColumnDef.ninjastarIndex;
					NinjaStarManage tempNinjastar = new NinjaStarManage();
					tempNinjastar.columnNumber = widthSize;
					tempNinjastar.ninjaStarSet = ninjastarSetup.ninjaStarSetList[index];
					tempNinjastar.movingState = NinjastarState.NONE;
					tempNinjastar.starsCount = 0;
					tempStars.Add (widthSize, tempNinjastar);

					if(ninjastarSetup.ninjaStarSetList[index].position == FloorPosition.TOP){
						int enemyLocation = GetName(3, presentBlockNum);
						Floor.instance.SetObstacleType(ObstacleTypes.SOLDIER, enemyLocation);
					}				
				}
			}

			if (floorColumnDef.stickfloorIndex != null && floorColumnDef.stickfloorIndex > 0){
				if(tempStickfloors.ContainsKey(widthSize) == false) {
					index = floorColumnDef.stickfloorIndex;
					StickfloorManage tempStickfloor = new StickfloorManage();
					tempStickfloor.columnNumber = widthSize;
					tempStickfloor.idleTimeCount = 0f;
					tempStickfloor.stickfloorSet = stickfloorSetup.stickfloorSetList[index];
					tempStickfloor.movingState = StickfloorState.NONE;
					tempStickfloor.leftStickfloor = 0;
					tempStickfloor.speedTimeCount = 0;
					tempStickfloors.Add (widthSize, tempStickfloor);
				}
			}

			if (floorColumnDef.flowingfloorId != null && floorColumnDef.flowingfloorId > 0){
				if(tempFlowingfloors.ContainsKey(widthSize) == false) {
					index = floorColumnDef.flowingfloorId;
					FlowingfloorManage tempFlowingfloor = new FlowingfloorManage();
					tempFlowingfloor.columnNumber = widthSize;
					tempFlowingfloor.flowingfloorSet = flowingfloorSetup.flowingfloorSetList[index];
					tempFlowingfloor.movingState = FlowingfloorState.NONE;
					tempFlowingfloor.count = 0;
					tempFlowingfloors.Add (widthSize, tempFlowingfloor);
				}
			}

			if (floorColumnDef.rollingRockId != null && floorColumnDef.rollingRockId > 0){
				if(tempRollingRocks.ContainsKey(widthSize) == false){
					index = floorColumnDef.rollingRockId;
					RollingRockManage tempRollingRock = new RollingRockManage();
					tempRollingRock.columnNumber = widthSize;
					tempRollingRock.rollingRockSet = rollingRockSetup.rollingRockSetList[index];
					tempRollingRock.movingState = RollingRockState.NONE;
					tempRollingRock.rollingRockCount = 0;
					tempRollingRock.rollingRockStartState = true;
					tempRollingRocks.Add (widthSize, tempRollingRock);
				}
			}

			// Floor local position increase
			if(presentBlockNum < modelWidthNumOnScreen){
				presentBlockNum++;
			}
		}
	}

	void RemoveFromCell(int length, int width){
		if(length >= 0 && width >= 0){
			int name = GetName(length, width);
			Floor.instance.RemoveFromCell(name);

			// Remove floor not needed anymore
			if(tempFloors[name] != null){
				this.ReleaseGameObject (tempFloors[name]);
				tempFloors.Remove(name);
			}

			// Delete if ninjastar is existed 
			int widthSize = GetWidthSize(width);
			if(tempStars.ContainsKey(widthSize)){
				tempStars.Remove(widthSize);
			}

			// Delete if Flowingfloors is existed 
			if(tempFlowingfloors.ContainsKey(widthSize)){
				tempFlowingfloors.Remove(widthSize);
			}

			// Delete if Stickfloors is existed 
			if(tempStickfloors.ContainsKey(widthSize)){
				tempStickfloors.Remove(widthSize);
			}

			// Delete if Rollingrock is existed 
			if(tempRollingRocks.ContainsKey(widthSize)){	
				tempRollingRocks.Remove(widthSize);
			}

			// Delete if obstacle is existed 
			if(tempObstacles.ContainsKey(name)){
				this.ReleaseGameObject (tempObstacles[name]);
				tempObstacles.Remove(name);
			}
			
			// Delete if Soldier is existed 
			if(tempSoldiers.ContainsKey(name)){
				this.ReleaseGameObject (tempSoldiers[name]);
				tempSoldiers.Remove(name);
			}
		}
	}

	void UpdateFloors(){

		int width = 0;
		int length;
		int value;
		int name;

		for(length=0; length < modelLengthNumOnScreen; length++){
			RemoveFromCell(length, width);
		}

		SetNewDataToCell(1, false);
	}

	// Ninjastar motion control function
	void ThrowNinjastars(){
		if(tempStars.Count > 0){
			int nsPos;
			List<int> arrayList = new List<int>(tempStars.Keys);
			for(int index=0; index < arrayList.Count; index++){
				NinjaStarManage ninjastars = tempStars[arrayList[index]];
				switch(ninjastars.movingState){
					case NinjastarState.NONE:
					TestSoundManager.I.PlayStarSound();
					ninjastars.movingState = NinjastarState.MOVING;
					break;
					case NinjastarState.MOVING:
						if(ninjastars.ninjaStarSet.ninjastars.Count <= ninjastars.starsCount){
							ninjastars.idleTimeCount = 0;
							ninjastars.movingState = NinjastarState.IDLING;
						}else{
							int starIndex = ninjastars.starsCount;
							NinjaStarDef ninjastarDef = ninjastars.ninjaStarSet.ninjastars[starIndex];
							int widthSize = ninjastars.columnNumber;

							// Setup position through ninja star position
							if(ninjastars.ninjaStarSet.position == FloorPosition.TOP){
								nsPos = 0;
							}else{
								nsPos = modelLengthNumOnScreen;
							}

							Vector3 position = Floor.instance.GetPosition(nsPos, widthSize, 0);
							int floorIndex = Floor.instance.GetOriginalName(3, widthSize);
							position.y += ninjaStarHeight - 30.3463f;
							position.x -= 8.81896f;

							Quaternion objRotation = Quaternion.Euler(0, 0, 0);
							string name = "Ninjastar_" + ninjastars.starsCount;
							ninjastars.starsCount++;

							StartCoroutine(ThrowStar(ninjastarDef.time, position, objRotation, ninjastars.starsCount, name, ninjastars.ninjaStarSet.position, ninjastars.ninjaStarSet.type, floorIndex)); 
						}
					break;
					case NinjastarState.IDLING:
						if(ninjastars.idleTimeCount >= ninjastars.ninjaStarSet.idleTime){
							ninjastars.starsCount = 0;
							ninjastars.movingState = NinjastarState.NONE;
						}else{
							ninjastars.idleTimeCount += Time.deltaTime;
						}
					break;
				}
			}
		}
	}

	// Stickfloor control function
	void RunStickfloors(){
		if(tempStickfloors.Count > 0){
			int sfPos;

			List<int> arrayList = new List<int>(tempStickfloors.Keys);
			for(int index=0; index < arrayList.Count; index++){
				StickfloorManage stickfloors = tempStickfloors[arrayList[index]];
				switch(stickfloors.movingState){
				case StickfloorState.NONE:
					stickfloors.speedTimeCount = 0;
					stickfloors.movingState = StickfloorState.MOVING;
					stickfloors.leftStickfloor = stickfloors.stickfloorSet.floorCount;
					stickfloors.existingStickfloorCount = 0;
					stickfloors.stickfloorRoot = new int[modelLengthNumOnScreen];

					break;
				case StickfloorState.MOVING:
					if(stickfloors.leftStickfloor == 0 && stickfloors.existingStickfloorCount == stickfloors.stickfloorSet.floorCount){
						stickfloors.idleTimeCount = 0;
						stickfloors.movingState = StickfloorState.IDLING;
					}else{
						float speedTime = stickfloorSetup.GetTypeTime(stickfloors.stickfloorSet.type);
						// Ready for stickfloor move
						if(stickfloors.speedTimeCount >= speedTime ){

							int nextKey = 0;
							int prevKey = 0;
							int maxVal = modelLengthNumOnScreen - 1;
							int minVal = 0;
							int calculation = -1;

							for(int stickFloor = maxVal; stickFloor >= minVal; stickFloor += calculation){

								int name = Floor.instance.GetOriginalName(stickFloor, stickfloors.columnNumber);

								if(stickfloors.stickfloorRoot[stickFloor] == null){
									stickfloors.stickfloorRoot[stickFloor] = 0;
								}

								// If in the first grid
								if(stickFloor == minVal && stickfloors.stickfloorRoot[stickFloor] == 0 && stickfloors.existingStickfloorCount == 0){
									stickfloors.stickfloorRoot[stickFloor] = 1;
									stickfloors.existingStickfloorCount++;
									ChangeFloorToStickfloor(name, StickfloorChangeProgress.HEAD);

								}else if(stickfloors.stickfloorRoot[stickFloor] == 1){

									nextKey = stickFloor + 1;
									prevKey = stickFloor - 1;

									// if head is the last
									if(stickFloor == maxVal && stickfloors.stickfloorRoot[prevKey] == 1){
										stickfloors.leftStickfloor--;
									}else 
									//  last stickfloor
									if(stickFloor == maxVal && stickfloors.stickfloorRoot[prevKey] == 0){
										stickfloors.stickfloorRoot[stickFloor] = 0;
										ChangeFloorToStickfloor(name, StickfloorChangeProgress.TAIL);
									}else

									// If head is found and not in the last grid
									if(nextKey <= maxVal && stickfloors.stickfloorRoot[nextKey] == 0 ){
										name = Floor.instance.GetOriginalName(nextKey, stickfloors.columnNumber);
										stickfloors.stickfloorRoot[nextKey] = 1;
										ChangeFloorToStickfloor(name, StickfloorChangeProgress.HEAD);
									}else

									//  if tail has been found
									if( prevKey >= minVal && stickfloors.stickfloorRoot[prevKey] == 0 && stickfloors.existingStickfloorCount >= stickfloors.stickfloorSet.floorCount){
										stickfloors.stickfloorRoot[stickFloor] = 0;
										ChangeFloorToStickfloor(name, StickfloorChangeProgress.TAIL);
									}else

									//  if head has found and block count has not fulfilled yet
									if(stickfloors.stickfloorRoot[nextKey] == 1 && stickFloor == minVal && stickfloors.existingStickfloorCount < stickfloors.stickfloorSet.floorCount){
										stickfloors.existingStickfloorCount++;
									}else 
									// if tail with block count has fulfilled 
									if(stickfloors.stickfloorRoot[nextKey] == 1 && stickFloor == minVal && stickfloors.existingStickfloorCount >= stickfloors.stickfloorSet.floorCount){
										stickfloors.stickfloorRoot[stickFloor] = 0;
										ChangeFloorToStickfloor(name, StickfloorChangeProgress.TAIL);
									}
								}
							}

							stickfloors.speedTimeCount = 0;

						}else{
							stickfloors.speedTimeCount += Time.deltaTime;
						}
					}
					break;
				case StickfloorState.IDLING:
					if(stickfloors.idleTimeCount >= stickfloors.stickfloorSet.idleTime){
						stickfloors.movingState = StickfloorState.NONE;
					}else{
						stickfloors.idleTimeCount += Time.deltaTime;
					}
					break;
				}
			}
		}
	}

	// Flowfloor control function
	void Flowfloors(){
		if(tempFlowingfloors.Count > 0){
			int sfPos;

			List<int> arrayList = new List<int>(tempFlowingfloors.Keys);
			for(int index=0; index < arrayList.Count; index++){
				FlowingfloorManage flowingfloors = tempFlowingfloors[arrayList[index]];
				switch(flowingfloors.movingState){
				case FlowingfloorState.NONE:
					flowingfloors.movingState = FlowingfloorState.MOVING;
					break;
				case FlowingfloorState.MOVING:
					int ffPos;
					int widthSize = flowingfloors.columnNumber;

					if(flowingfloors.flowingfloorSet.position == FloorPosition.TOP){
						ffPos = 0;
					}else{
						ffPos = modelLengthNumOnScreen;
					}

					int originalName = Floor.instance.GetOriginalName(ffPos, flowingfloors.columnNumber);
					Vector3 position = Floor.instance.GetPosition(ffPos, widthSize, 0);
					position.x -= 2.2f;
					
					Quaternion objRotation = Quaternion.Euler(0, 0, 0);
					string name = "Flowingfloor_" + flowingFloorCount;
					flowingFloorCount++;
					flowingfloors.count++;

					GameObject floor = this.GetGameObject (flowingfloorObject[(int)flowingfloors.flowingfloorSet.flowingfloorSizeType], position, objRotation, flowingfloors.count, name);
					Flowingfloor tempFlowingfloor = floor.GetComponent<Flowingfloor>();
					tempFlowingfloor.ResetParameter();
					tempFlowingfloor.floorPosition = flowingfloors.flowingfloorSet.position;
					tempFlowingfloor.floorType = flowingfloors.flowingfloorSet.type;	
					tempFlowingfloor.width = flowingfloors.columnNumber;
					tempFlowingfloor.index = originalName;
					tempFlowingfloor.ChangeSpeed = (int)flowingfloors.flowingfloorSet.type;
					tempFlowingfloor.flowingfloorSizeType = flowingfloors.flowingfloorSet.flowingfloorSizeType;
					flowingfloors.idleTimeCount = 0;
					flowingfloors.movingState = FlowingfloorState.IDLING;

					break;
				case FlowingfloorState.IDLING:
					if(flowingfloors.idleTimeCount >= flowingfloors.flowingfloorSet.idleTime){
						flowingfloors.movingState = FlowingfloorState.NONE;
					}else{
						flowingfloors.idleTimeCount += Time.deltaTime;
					}
					break;
				}
			}
		}
	}

	// Ninjastar motion control function
	void RollingRocks(){
		if(tempRollingRocks.Count > 0){
			int rrPos;

			List<int> arrayList = new List<int>(tempRollingRocks.Keys);
			for(int index=0; index < arrayList.Count; index++){
				RollingRockManage rollingRocks = tempRollingRocks[arrayList[index]];
				switch(rollingRocks.movingState){
				case RollingRockState.NONE:
					TestSoundManager.I.PlayCarSound();
					rollingRocks.movingState = RollingRockState.MOVING;
					break;
				case RollingRockState.MOVING:
					if(rollingRocks.rollingRockSet.rollingRocks.Count <= rollingRocks.rollingRockCount){
						rollingRocks.idleTimeCount = 0;
						rollingRocks.movingState = RollingRockState.IDLING;
					}else{
						int rollingRockIndex = rollingRocks.rollingRockCount;
						RollingRockDef rollingRockDef = rollingRocks.rollingRockSet.rollingRocks[rollingRockIndex];
						int widthSize = rollingRocks.columnNumber;
						
						// Setup position through ninja star position
						if(rollingRocks.rollingRockSet.position == FloorPosition.TOP){
							rrPos = 0;
						}else{
							rrPos = modelLengthNumOnScreen;
						}
						
						Vector3 position = Floor.instance.GetPosition(rrPos, widthSize, 0);
						position.y += rollingRockHeight;
						
						Quaternion objRotation = Quaternion.Euler(0, 0, 0);
						string name = "RollingStone_" + rollingRocks.rollingRockCount;
						rollingRocks.rollingRockCount++;
						
						StartCoroutine(RollRock(rollingRockDef.time, position, objRotation, rollingRocks.rollingRockCount, name, rollingRocks.rollingRockSet.position, rollingRocks.rollingRockSet.type, rollingRocks.rollingRockStartState)); 

						if(rollingRocks.rollingRockStartState == true){
							rollingRocks.rollingRockStartState = false;
						}
					}
					break;
				case RollingRockState.IDLING:
					if(rollingRocks.idleTimeCount >= rollingRocks.rollingRockSet.idleTime){
						rollingRocks.rollingRockCount = 0;
						rollingRocks.movingState = RollingRockState.NONE;
						rollingRocks.rollingRockStartState = true;
					}else{
						rollingRocks.idleTimeCount += Time.deltaTime;
					}
					break;
				}
			}
		}
	}

	int GetColumnId(int columnCount){

		int result = 1;

		if(columnCount >= currentColumnIndex + 1 && currentColumnIndex > 0){
			result = currentColumnIndex + 1;
		}else if(columnCount < currentColumnIndex + 1){
			currentCollectionIndex = -1;
		}

		currentColumnIndex = result;

		return result;
	}

	int GetCollectionId(){
		
		int result;

		if(currentCollectionIndex == -1){
			int count = floorSetup.floorSetCollection.Count;
			result = floorSetup.floorSetCollection.ElementAt(Random.Range(1, count - 1)).Key;
			currentCollectionIndex = result;
		}else{
			result = currentCollectionIndex;
		}
		
		return result;
	}

	void ChangeFloorToStickfloor(int name, StickfloorChangeProgress progress){

		if(tempFloors.ContainsKey(name)){

			GameObject tempFloor = tempFloors[name];

			switch(progress){
				case StickfloorChangeProgress.HEAD:
				tempFloor.GetComponent<SpriteRenderer>().sprite = spriteList[2];
				Floor.instance.ChangeCellData(FloorTypes.CONCRETE1A, name);

				if(UserMovement.userHashPosition == name){
					Debug.Log("Dead by stick");
					GameManager.I.EndState = true;
				}

				break;
				case StickfloorChangeProgress.MIDDLE:
				// Change on GameObject
				// Change on floorCell
				break;
				case StickfloorChangeProgress.TAIL:
				tempFloor.GetComponent<SpriteRenderer>().sprite = spriteList[1];
				Floor.instance.ChangeCellData(FloorTypes.CONCRETE1A, name);
				break;
			}		
		}
	}

	int ConvertToReverse(int index, FloorPosition position){
		int result = index;

		if(position == FloorPosition.TOP){
			result = modelLengthNumOnScreen - index;
		}

		return result;
	}

	int GetNextKey(int index, FloorPosition position){
		int result = index + 1;
		
		if(position == FloorPosition.TOP){
			result = index - 1;
		}
		
		return result;
	}

	int GetPrevKey(int index, FloorPosition position){
		int result = index - 1;
		
		if(position == FloorPosition.TOP){
			result = index + 1;
		}
		
		return result;
	}

	IEnumerator ThrowStar(float waitTime, Vector3 position, Quaternion objRotation, int ninjaStarCount, string name, FloorPosition starPosition, NinjaStarType type, int floorIndex) {
		yield return new WaitForSeconds(waitTime);

		if(tempSoldiers.ContainsKey(floorIndex)){
			Soldier soldierTmp = tempSoldiers[floorIndex].GetComponent<Soldier>();
			soldierTmp.Shoot();
		}

		GameObject floor = this.GetGameObject (ninjastarObject, position, objRotation, ninjaStarCount, name);
		Ninjastar tempNinjastar = floor.GetComponent<Ninjastar>();
		tempNinjastar.starPosition = starPosition;
		tempNinjastar.starType = type;
		tempNinjastar.ChangeSpeed = (int)type;
	}

	IEnumerator RollRock(float waitTime, Vector3 position, Quaternion objRotation, int rollingRockCount, string name, FloorPosition rollingRockPosition, RollingRockType type, bool rollingRockStartState) {
		yield return new WaitForSeconds(waitTime);
		GameObject floor;
		RollingRock tempRollingRock; 

		if(rollingRockStartState == true){
			//  Warning object
			Vector3 warningObjectPosition = position;
			warningObjectPosition.x -= 3;
			floor = this.GetGameObject (rollingRockWarningObject, warningObjectPosition, objRotation, rollingRockCount, "WarningObject");
			tempRollingRock = floor.GetComponent<RollingRock>();
			tempRollingRock.rollingRockPosition = rollingRockPosition;
			tempRollingRock.rollingRockType = type;

			position.x += 33f;
			position.y += 123.4f;
		}

		floor = this.GetGameObject (rollingRockObject, position, objRotation, rollingRockCount, name);
		tempRollingRock = floor.GetComponent<RollingRock>();
		tempRollingRock.rollingRockPosition = rollingRockPosition;
		tempRollingRock.rollingRockType = type;
	}

	void OnGUI() {
		if(GameManager.I.EnvironmentType == Environment.UNITYEDITOR){
			GUI.Label(new Rect(20, 25, 100, 20), "fpb：" + m_fps, m_guiStyle);
		}
	}
}

public class FlowingManage{
	public Vector2 position;
	public FlowingfloorType flowingfloorType;
}

