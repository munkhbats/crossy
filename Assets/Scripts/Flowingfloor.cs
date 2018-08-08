using UnityEngine;
using System.Collections;

public class Flowingfloor : MonoBehaviour {
	[SerializeField]
	private int speed;
	[SerializeField]
	public int width;
	[SerializeField]
	public int index;
	[SerializeField]
	public FlowingfloorType floorType;
	[SerializeField]
	public FloorPosition floorPosition;
	[SerializeField]
	public FlowingfloorSizeType flowingfloorSizeType;

	void Start () {
		speed = (int)floorType;
	}
/*	
	void OnTriggerEnter2D(Collider2D c)
	{
//		GameOver.endState = true;
	}
*/	
	void Update () {
		if(gameObject.name != "Deactivated"){
			if(transform.position.y < -140 || transform.position.y > 20){
				gameObject.name = "Deactivated";
			}else{
				Activate ();
			}
		}
	}

	void FixedUpdate () {
/*
		if(gameObject.name != "Deactivated"){
			// Refresh cell only when floor width is close to the player
			if(Floor.refreshFloorBack == width || Floor.refreshFloorFront == width){
				FloorCellManage fromManage = new FloorCellManage();
				FloorCellManage toManage = new FloorCellManage();

				fromManage.floorType = FloorTypes.WATERFLOWINGBLOCK;
				fromManage.flowingfloorType = floorType;
				fromManage.position = floorPosition;
				toManage.floorType = FloorTypes.WATERTYPE;

				int nextIndex = Floor.instance.ChangeToNearestFloorCell(width, transform.position, index, fromManage, toManage);

				if(nextIndex > -1){
					index = nextIndex;
				}
			}
		}
*/		
	}
	
	void Activate ()
	{
		Vector3 pos;
		
		if(floorPosition == FloorPosition.TOP){
			pos = new Vector3(-0.21f, -0.79f, 0f);
		}else{
			pos = new Vector3(0.21f, 0.79f, 0f);
		}
		
		GetComponent<Rigidbody2D>().velocity = pos.normalized * speed;
	}

	public void ResetParameter ()
	{
		speed = 0;
		width = 0;
		index = 0; 
		floorType = FlowingfloorType.SLOWLY;
	}

	public int ChangeSpeed {
		get { return speed; }
		set { speed = value; }
	}
}
