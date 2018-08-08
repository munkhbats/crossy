using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour {

	private static Floor _instance;

	public static Floor instance {
		get 
		{
			if (_instance == null)
			{
				_instance = new Floor();
			}
			return _instance;
		}
	}

	public float widthSize = 8.2f;
	public float widthDiff = 1.81f;
	public float lengthDiff = 6.69f;
	public int modelWidthNumOnScreen = 20;
	public int modelLengthNumOnScreen = 20;
	private static Dictionary<int, FloorCellManage> floorCell;
	
	public static int refreshFloorFront{
		get;
		set;
	}
	public static int refreshFloorBack{
		get;
		set;
	}

	public void InitCell(){
		floorCell = new Dictionary<int, FloorCellManage>();
	}

	public int GetFloorTypeFromCell(int name){
		return (int)floorCell[name].floorType;
	}

	public int GetFloorAttributeFromCell(int name){
		return (int)floorCell[name].floorAtt;
	}

	public ObstacleTypes GetObstacleTypeFromCell(int name){
		return floorCell[name].obstacleType;
	}

	public List<int> GetCellKeys(){
		return new List<int>(floorCell.Keys);
	}

	public Dictionary<int, FloorCellManage> GetCells(){
		return floorCell;
	}

	public void RemoveFromCell(int name){
		floorCell.Remove(name);
	}
	
	public void SetToCell(FloorTypes value, int name, ObstacleTypes obstacleType, FloorAttributes floorAtt){
		FloorCellManage cellManage = new FloorCellManage();
		cellManage.floorType = value;
		cellManage.obstacleType = obstacleType;
		cellManage.floorAtt = floorAtt;
		floorCell.Add(name, cellManage);
	}
	
	public void ChangeCellData(FloorTypes value, int name){
		if(floorCell.ContainsKey(name)){
			floorCell[name].floorType = value;
		}
	}

	public void SetObstacleType(ObstacleTypes type, int name){
		if(floorCell.ContainsKey(name)){
			floorCell[name].obstacleType = type;
		}
	}

	public void ChangeCellDataManage(int index, FloorCellManage floorCellManage){
		if(floorCell.ContainsKey(index)){
			floorCell[index] = floorCellManage;
		}
	}

	public void ChangeCellDataFloorPosition(int index, FloorPosition floorPosition){
		if(floorCell.ContainsKey(index)){
			floorCell[index].position = floorPosition;
		}
	}

	public void ChangeCellDataFloorType(int index, FlowingfloorType floorType){
		if(floorCell.ContainsKey(index)){
			floorCell[index].flowingfloorType = floorType;
		}
	}
	
	public Vector3 GetPosition(int length, int width, int order){
		Vector3 objectPoint = new Vector3(0, 0, 0);
		objectPoint.x = widthSize * width - length * widthDiff;
		objectPoint.y -= lengthDiff * length;
		objectPoint.z = (order / 1000f) * -1;

		return objectPoint;
	}

	public int GetName(int length, int width){
		//int widthSize = GetWidthSize(width);
		//int floorSize = ConvertToFloorSize(widthSize);
		
		return 0;// length + floorSize;
	}

	public int GetOriginalName(int length, int width){
		int floorSize = ConvertToFloorSize(width);
		return length + floorSize;
	}

	public int GetWidth(int index){
		return (int)index / modelLengthNumOnScreen;
	}

	public int GetLength(int index){
		return index % modelLengthNumOnScreen;
	}

	public int ConvertToFloorSize(int widthSize){
		return widthSize * modelLengthNumOnScreen;
	}
	
	public int ChangeToNearestFloorCell(int width, Vector3 position, int fromIndex, FloorCellManage fromManage, FloorCellManage toManage){

		int index = GetNearestFloorCell(width, position, 0);

		if(index > -1 && fromIndex != index){
			ChangeCellDataManage(index, fromManage);
			ChangeCellDataManage(fromIndex, toManage);
		}

		return index;
	}

	public int GetNearestFloorCell(int width, Vector3 position, int extraLength){
		
		Vector3 cellPos;
		float distance = 0;
		float beforeDistance = 100;
		int index = -1;
		
		for(int length = 0; length < modelLengthNumOnScreen; length++){
			cellPos = GetPosition(length, width, 0);
			distance = Vector3.Distance(position, cellPos);
			
			if(beforeDistance > distance){
				beforeDistance = distance;
				index = GetOriginalName(length + extraLength, width);
			}else{
				break;
			}
		}

		return index;
	}

	public FloorCellManage GetFloorCellManage(int index){
		return floorCell[index];
	}
}

public class FloorCellManage
{
	public FloorTypes floorType;
	public FloorAttributes floorAtt;
	public FloorPosition position;
	public FlowingfloorType flowingfloorType;
	public ObstacleTypes obstacleType;
}