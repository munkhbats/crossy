using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SwipeDirection{
	Up = 1,
	Down = 2,
	Right = 3,
	Left = 4,
	Tap = 5
}

public class UserMovement : MonoBehaviour {
	public static int userHashPosition;
	public bool deathStarPlay = false;
	public bool deathRiverPlay = false;

	private SwipeDirection userDirection;
	private Vector2 firstPressPos;
	private Vector2 secondPressPos;
	private Vector2 currentSwipe;
	private Vector2 fingerStartPos = Vector2.zero;
	private bool isSwipe = false;
	private float minSwipeDist  = 50.0f;
	private float floorDiffY = 7.91376f;
	private float floorDiffX = 1f;
	private float widthSize;
	private float widthDiff;
	private float lengthDiff;
	private bool isJumping = false;
	private Vector3 fromPos;
	private Vector3 toPos;
	private Animator animator;
	private LinkedList<SwipeDirection> movement = new LinkedList<SwipeDirection>();
	private SpriteRenderer spriteRenderer;
	private int userTempHashPosition;
	private FloorAttributes userFloorAtt;
	private float userFlowingSpeed;
	private FloorPosition floorPosition;
	private int userMaxLength;
	private GameObject flowingFloorObject;
	private string getLastObjectInstance;

	[SerializeField]
	private float jumpSpeed = 0f;

	void Awake () {
		widthSize = Floor.instance.widthSize;
		widthDiff = Floor.instance.widthDiff;
		lengthDiff = Floor.instance.lengthDiff;
	}

	// Use this for initialization
	void Start () {
		userHashPosition = 149;
		userMaxLength = 1;
		userDirection = SwipeDirection.Right;
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}

	void FixedUpdate (){

		if(GameManager.I.EndState == true){
			return;
		}

		if(isJumping == true){

			if(flowingFloorObject != null){
				toPos = flowingFloorObject.transform.position;
				toPos.y += 4.72701f;
//				toPos.x -= 1.2712f;
			}

			transform.position = Vector3.Lerp(transform.position, toPos, jumpSpeed * Time.deltaTime);
			if(Vector3.Distance(transform.position, toPos) <= 1.07f){

				if(userFloorAtt == FloorAttributes.WATER || userFloorAtt == FloorAttributes.STICK){
					TestSoundManager.I.PlayDeathRiverSound();
					GameManager.I.DeathType = UserDeathType.WATER;
					GameManager.I.EndState = true;
				}
				transform.position = toPos;
				isJumping = false;
			}
		}else{
			if(movement.Count > 0){
				SwipeDirection direction = movement.First.Value;
				Move(direction);
				movement.RemoveFirst();
			}else{
				if(userFloorAtt == FloorAttributes.BAMBOO){
					Vector3 pos;
					if(floorPosition == FloorPosition.TOP){
						pos = new Vector3(-0.21f, -0.79f, 0f);
					}else{
						pos = new Vector3(0.21f, 0.79f, 0f);
					}
					GetComponent<Rigidbody2D>().velocity = pos.normalized * userFlowingSpeed;

					if(transform.position.y < -96 || transform.position.y > -26){
						GameManager.I.DeathType = UserDeathType.WATER;
						GameManager.I.EndState = true;
					}
				}else{
					GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
				}
			}
		}
	}

	void Update() {

		// Screen touch 
		if(GameManager.I.EnvironmentType == Environment.UNITYEDITOR){
			if(Input.GetMouseButtonDown(0))
			{
				if(GameManager.I.EndState == true){
					if(GameManager.I.DeadState == true){
						GameManager.I.GameStartTap = true;
					}
					return;
				}

				firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
				animator.SetBool("SitIdle", true);
			}
			if(Input.GetMouseButtonUp(0) && movement.Count < 1)
			{
				secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
				
				currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
				
				currentSwipe.Normalize();
				
				if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				{
					userDirection = SwipeDirection.Up;
					movement.AddLast(SwipeDirection.Up);
				}else if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				{
					userDirection = SwipeDirection.Down;
					movement.AddLast(SwipeDirection.Down);
				}else if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
					userDirection = SwipeDirection.Left;
					movement.AddLast(SwipeDirection.Left);
				}else if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
					userDirection = SwipeDirection.Right;
					movement.AddLast(SwipeDirection.Right);
				}else{
					userDirection = SwipeDirection.Right;
					movement.AddLast(SwipeDirection.Right);
				}
			}
		} else {
			if (Input.touchCount > 0){
				
				foreach (Touch touch in Input.touches)
				{
					switch (touch.phase)
					{
					case TouchPhase.Began :
						/* this is a new touch */

						if(GameManager.I.EndState == true){
							if(GameManager.I.DeadState == true){
								GameManager.I.GameStartTap = true;
							}
							return;
						}

						animator.SetBool("SitIdle", true);
						isSwipe = true;
						fingerStartPos = touch.position;
						break;
						
					case TouchPhase.Canceled :
						/* The touch is being canceled */
						isSwipe = false;
						break;
						
					case TouchPhase.Ended :
						if(movement.Count < 1){
							float gestureDist = (touch.position - fingerStartPos).magnitude;
							
							if (isSwipe && gestureDist > minSwipeDist){
								Vector2 direction = touch.position - fingerStartPos;
								Vector2 swipeType = Vector2.zero;
								
								if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
									// the swipe is horizontal:
									swipeType = Vector2.right * Mathf.Sign(direction.x);
								}else{
									// the swipe is vertical:
									swipeType = Vector2.up * Mathf.Sign(direction.y);
								}
								
								if(swipeType.x != 0.0f){
									if(swipeType.x > 0.0f){
										userDirection = SwipeDirection.Right;
										movement.AddLast(SwipeDirection.Right);
									}else{
										userDirection = SwipeDirection.Left;
										movement.AddLast(SwipeDirection.Left);
									}
								}
								
								if(swipeType.y != 0.0f ){
									if(swipeType.y > 0.5f){
										userDirection = SwipeDirection.Up;
										movement.AddLast(SwipeDirection.Up);
									}else if(swipeType.y < -0.5f){
										userDirection = SwipeDirection.Down;
										movement.AddLast(SwipeDirection.Down);
									}
								}
								
							}else if(isSwipe){
								userDirection = SwipeDirection.Right;
								movement.AddLast(SwipeDirection.Right);
							}
						}
						
						break;
					}
				}
			}
		}
	}

	FloorAttributes GetUserFloorAttribute(int userPosition){

		FloorCellManage userFloorManage = Floor.instance.GetFloorCellManage(userHashPosition);
		userFlowingSpeed = (int)userFloorManage.flowingfloorType;
		floorPosition = userFloorManage.position;
		return userFloorManage.floorAtt;
	}

	void Move(SwipeDirection direction){

		if(isJumping == true) {
			return;
		}

		//   Game starting
		if(GameManager.I.StartState == false){
			GameManager.I.StartState = true;
		}

		animator.SetBool("SitIdle", false);
			
		Vector3 fromPosition = transform.position; //charaObject.transform.position;
		int result, nextPosition;
		int userPosition = userHashPosition;
		
		if(userFloorAtt == FloorAttributes.BAMBOO){
			int width = Floor.instance.GetWidth(userHashPosition);
			userPosition = Floor.instance.GetNearestFloorCell(width, transform.position, 1);
		}

		nextPosition = userPosition;
		result = (int)FloorAttributes.FLOOR;

		switch(direction){
			case SwipeDirection.Down:
				nextPosition = userPosition + 1;
				result = GetNextWallAtt(SwipeDirection.Down, userPosition, nextPosition);					
				break;
			case SwipeDirection.Up:
				nextPosition = userPosition - 1;
				result = GetNextWallAtt(SwipeDirection.Up, userPosition, nextPosition);
				break;
			case SwipeDirection.Left:
				nextPosition = userPosition - Floor.instance.modelLengthNumOnScreen;
				result = GetNextWallAtt(SwipeDirection.Left, userPosition, nextPosition);
				flowingFloorObject = null;	
				break;
			case SwipeDirection.Right:
			case SwipeDirection.Tap:
				nextPosition = userPosition + Floor.instance.modelLengthNumOnScreen;
				result = GetNextWallAtt(SwipeDirection.Right, userPosition, nextPosition);
				flowingFloorObject = null;
				break;
			default:
				nextPosition = userPosition;
			result = (int)FloorAttributes.FLOOR;
			break;
		}

		if(result != (int)FloorAttributes.WALL){

			int nextWidth = Floor.instance.GetWidth(nextPosition);
			int nextLength = Floor.instance.GetLength(nextPosition);
			bool upDownBamboo = false;
			bool useDefaultMovement = true;

			// Score up
			if(userMaxLength < nextWidth){
				GameManager.I.UserScore += 1; 
				userMaxLength = nextWidth;
			}

			if(direction == SwipeDirection.Up || direction == SwipeDirection.Down){
				upDownBamboo = true;
			}

			if(flowingFloorObject != null && userFloorAtt == FloorAttributes.BAMBOO && upDownBamboo == true){
				Flowingfloor flowingFloor = flowingFloorObject.transform.parent.GetComponent<Flowingfloor>();
				FlowingfloorSizeType sizeType =	flowingFloor.flowingfloorSizeType;
				int currentFloor = int.Parse(flowingFloorObject.gameObject.name);
				GameObject nextGameObject;

				switch(sizeType){
				case FlowingfloorSizeType.LONG:
					if(direction == SwipeDirection.Up){
						if(currentFloor < 3 && currentFloor > 0){
							nextGameObject = flowingFloorObject.transform.parent.transform.Find((currentFloor + 1).ToString()).gameObject;
							if(nextGameObject != null){
								toPos = nextGameObject.transform.position;
								flowingFloorObject = nextGameObject;
								useDefaultMovement = false;
							}
						}
					}else{
						if(currentFloor > 1 && currentFloor < 4){
							nextGameObject = flowingFloorObject.transform.parent.transform.Find((currentFloor - 1).ToString()).gameObject;
							if(nextGameObject != null){
								toPos = nextGameObject.transform.position;
								flowingFloorObject = nextGameObject;
								useDefaultMovement = false;
							}
						}
					}
					break;
				case FlowingfloorSizeType.MEDIUM:
					if(direction == SwipeDirection.Up){
						if(currentFloor < 2 && currentFloor > 0){
							nextGameObject = flowingFloorObject.transform.parent.transform.Find((currentFloor + 1).ToString()).gameObject;
							if(nextGameObject != null){
								toPos = nextGameObject.transform.position;
								flowingFloorObject = nextGameObject;
								useDefaultMovement = false;
							}
						}
					}else{
						if(currentFloor > 1 && currentFloor < 3){
							nextGameObject = flowingFloorObject.transform.parent.transform.Find((currentFloor - 1).ToString()).gameObject;
							if(nextGameObject != null){
								toPos = nextGameObject.transform.position;
								flowingFloorObject = nextGameObject;
								useDefaultMovement = false;
							}
						}
					}
					break;
				case FlowingfloorSizeType.SHORT:
					break;
				}

			}

			if(useDefaultMovement == true){
				flowingFloorObject = null;
				toPos = Floor.instance.GetPosition(nextLength, nextWidth, nextLength);
				spriteRenderer.GetComponent<Renderer>().sortingOrder = nextLength + 1;
				toPos.y += floorDiffY;
				toPos.x += floorDiffX;
			}

//			Floor.refreshFloorBack = nextWidth - 1;
//			Floor.refreshFloorFront = nextWidth + 1;

			userHashPosition = nextPosition;
			userFloorAtt = (FloorAttributes)GetUserFloorAttribute(userHashPosition);

			if(userDirection == SwipeDirection.Left){
				transform.eulerAngles = new Vector2(0, 180); 
			}else{
				transform.eulerAngles = new Vector2(0, 0);
			}

			animator.SetTrigger("Jump");

			fromPos = fromPosition;
			isJumping = true;
			TestSoundManager.I.PlayJumpSound();
		}
	}

	int GetNextWallAtt(SwipeDirection direction, int userPosition, int nextPosition) {
		int upDown;
		bool available = false;
		int result;

		switch(direction){
		case SwipeDirection.Down:
			upDown = (int)userPosition / Floor.instance.modelLengthNumOnScreen;
			if(upDown == (int)nextPosition/ Floor.instance.modelLengthNumOnScreen){
				available = true;
			}
			break;
		case SwipeDirection.Left:
			if(nextPosition > 0){
				available = true;
			}
			break;
		case SwipeDirection.Right:
			available = true;
			break;
		case SwipeDirection.Up:
			upDown = (int)userPosition / Floor.instance.modelLengthNumOnScreen;

			if(upDown == (int)nextPosition/ Floor.instance.modelLengthNumOnScreen){
				available = true;
			}
			break;
		}

		if(available == true){
			result = Floor.instance.GetFloorAttributeFromCell(nextPosition);
		}else{
			result = (int)FloorAttributes.WALL;
		}

		return result;
	}
	
	void OnTriggerEnter2D(Collider2D c)
	{
		if(c.gameObject.tag == "Collider"){

			Flowingfloor floor = c.transform.parent.gameObject.GetComponent<Flowingfloor>();
			string name = c.gameObject.transform.parent.gameObject.name + "_" + c.gameObject.name;

			if(name != getLastObjectInstance){
				floorPosition = floor.floorPosition;
				userFlowingSpeed = (int)floor.floorType;
				userFloorAtt = FloorAttributes.BAMBOO;

				flowingFloorObject = c.gameObject;
				getLastObjectInstance = name;
			}
		}
	}
}
