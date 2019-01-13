using UnityEngine;
using System.Collections;

public class Limits : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	//Update is called once per frame
	void LateUpdate () {
		Vector3 aux =  new Vector3(transform.position.x, transform.position.y, transform.position.z);
		if (aux.x < 5.0f)
			aux.x = 5.0f;
		if (aux.z < -10.0f)
			aux.z = -10.0f;
		if (aux.x > 200.0f)
			aux.x = 200.0f;
		if (aux.z > 180.0f)
			aux.z = 180.0f;
		transform.position = aux;
	}
}
 

