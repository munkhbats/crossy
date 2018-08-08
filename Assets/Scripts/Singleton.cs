using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
	private static T instance = null;

	public static T I {
		get { return instance; }
	}

	public static T GetInstance() {
		return instance;
	}

	/// <summary>
	/// if Drag And Drop to hierarchy, two instance created
	/// </summary>
	private void Awake() {
		if(instance == null) {
			instance = this as T;	
			instance.Init();
		}
	}

	/// <summary>
	/// Init this Object
	/// </summary>
	protected virtual void Init() {
	}

	private void OnApplicationQuit() {
		instance = null;	
	}
}
