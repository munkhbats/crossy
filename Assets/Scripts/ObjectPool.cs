using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{	
	// ゲームオブジェクトのDictionary
	private Dictionary<int, List<GameObject>> pooledGameObjects = new Dictionary<int, List<GameObject>>();
	
	// ゲームオブジェクトをpooledGameObjectsから取得する。必要であれば新たに生成する
	public GameObject GetGameObject (GameObject prefab, Vector3 position, Quaternion rotation, int objOrder, string name)
	{
		// プレハブのインスタンスIDをkeyとする
		int key = prefab.GetInstanceID ();
		
		// Dictionaryにkeyが存在しなければ作成する
		if (pooledGameObjects.ContainsKey (key) == false) {

			pooledGameObjects.Add (key, new List<GameObject> ());
		}
		
		List<GameObject> gameObjects = pooledGameObjects [key];
		
		GameObject go = null;
		
		for (int i = 0; i < gameObjects.Count; i++) {
			
			go = gameObjects [i];
			
			// 現在非アクティブ（未使用）であれば
			if (go.name == "Deactivated") {

				go.transform.localPosition = position;
				go.transform.localRotation = rotation;
				go.GetComponent<Renderer>().sortingOrder = objOrder;
				go.name = name;
				
				return go;
			}
		}
		
		// 使用できるものがないので新たに生成する
		go = Instantiate (prefab) as GameObject;
		// ObjectPoolゲームオブジェクトの子要素にする
		go.transform.parent = transform;
		go.transform.localPosition = position;
		go.transform.localRotation = rotation;
		go.GetComponent<Renderer>().sortingOrder = objOrder;
		go.name = name;
		
		// リストに追加
		gameObjects.Add (go);
		
		return go;
	}
	
	// ゲームオブジェクトを非アクティブにする。こうすることで再利用可能状態にする
	public void ReleaseGameObject (GameObject go)
	{
		// 非アクティブにする
		go.name = "Deactivated";
	}
/*
	public void AddObject (GameObject prefab) {
		// プレハブのインスタンスIDをkeyとする
		int key = prefab.GetInstanceID ();
		// 使用できるものがないので新たに生成する
		GameObject go = (GameObject)Instantiate (prefab);
		// ObjectPoolゲームオブジェクトの子要素にする
		go.transform.parent = transform;
		// リストに追加
		pooledGameObjects[key].Add(go);
	}
*/	
}