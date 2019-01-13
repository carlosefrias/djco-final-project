using UnityEngine;
using System.Collections;
using System.IO;

public class restartScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		string txt = "High scores:\n";
		try{
			string[] scoretextlines = System.IO.File.ReadAllLines("../topScores/topScore.txt");
			for(int i = 0; i < scoretextlines.Length; i++){
				string[] pal = scoretextlines[i].Split(' ');
				if(pal.Length == 2) {
					txt += pal[0] + " " + timerMethod(float.Parse(pal[1])) + "\n";
				}
			}
		}catch(FileNotFoundException){}
		this.GetComponent<TextMesh>().fontSize = 14;
		this.GetComponent<TextMesh>().text = txt;
		if (Input.GetKey(KeyCode.Space)) {
			Application.LoadLevel("cena1");
		}
	}	
	private string timerMethod(float currentTime){
		int minutes = (int)(currentTime / 60);
		int seconds = (int)(currentTime % 60);
		int fraction = (int)((currentTime * 100) % 100);
		return "" + minutes + ":" +  seconds + ":" + fraction;
	}
	/*
    private Rect janelaGuardaPlayerResult = new Rect(20, 350, 200, 200);
	void OnGUI() {
        GUI.color = Color.red;
        janelaGuardaPlayerResult = GUI.Window(0, janelaGuardaPlayerResult, guardaScore, "Population Stats");
    }
	void guardaScore(int windowID) {
		GUI.color = Color.green;
		GUI.Label(new Rect(10,15,180,30), "Player name: ");
		string name = GUI.TextField(new Rect(10,45,180, 30), "");
		if(GUI.Button(new Rect(10, 75, 180, 30), "save")){
			motorJogo.storeScore(name);
		}
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }*/
	
}