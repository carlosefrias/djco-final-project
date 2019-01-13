using UnityEngine;
using System.Collections;

public class Nutriente : MonoBehaviour {
	public int alimento;
	// Use this for initialization
	void Start () {
		alimento = 25;
		this.gameObject.tag = "nutriente";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
