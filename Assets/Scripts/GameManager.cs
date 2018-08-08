using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

	private int userScore;
	private bool endState;
	private bool startState;
	private bool deadState;
	private UserDeathType deathType;
	private Environment env;
	private float restartTimer;
	private bool cameraZoomIn;
	private bool gameStartTap;
	private float size1 = 38f;
	private float size2 = 20f;

	[SerializeField]
	private float restartDelay = 2f;

	protected override void Init() {
		base.Init();
	}

	// Use this for initialization
	void Start () 
	{
		gameStartTap = false;
		restartTimer = 0;
		userScore = 0;
		EndState = false;
		deadState = false;
		StartState = false;
		deathType = UserDeathType.NONE;
		cameraZoomIn = false;

#if UNITY_EDITOR
		env = Environment.UNITYEDITOR;
#else
		env = Environment.DEVICE;
#endif
	}

	void Update()
	{
		if(deadState == true && gameStartTap == true){
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	public bool EndState {
		get { return endState; }
		set { endState = value; }
	}

	public bool StartState {
		get { return startState; }
		set { startState = value; }
	}

	public bool DeadState {
		get { return deadState; }
		set { deadState = value; }
	}

	public int UserScore {
		get { return userScore; }
		set { userScore = value; }
	}

	public Environment EnvironmentType {
		get { return env; }
		set { env = value; }
	}

	public UserDeathType DeathType {
		get { return deathType; }
		set { deathType = value; }
	}

	public bool CameraZoomIn {
		get { return cameraZoomIn; }
		set { cameraZoomIn = value; }
	}

	public bool GameStartTap {
		get { return gameStartTap; }
		set { gameStartTap = value; }
	}
}
