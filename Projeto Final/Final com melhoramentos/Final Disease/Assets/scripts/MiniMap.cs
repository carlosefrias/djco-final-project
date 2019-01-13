using UnityEngine;
using System.Collections;

public class MiniMap : MonoBehaviour {
	
	public Transform target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate() {
		transform.position = new Vector3(target.position.x,transform.position.y,target.position.z);
	}
}
