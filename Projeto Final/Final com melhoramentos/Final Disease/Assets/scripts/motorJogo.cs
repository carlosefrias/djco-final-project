using UnityEngine;
using System.Collections;
using System.IO;


public class motorJogo : MonoBehaviour {
	private bool paused;
	// Use this for initialization
	void Start () {
		paused = false;		
	}
	private bool criaInimigos = false;
	private int passoAnterior = 0;
	// Update is called once per frame
	void Update () {
		float timer = MainMenu.currentTime;
		int passo = (int)(timer / 4);
		if(passo > passoAnterior) criaInimigos = true;
		passoAnterior = passo;
		Vector3 pos = new Vector3(Random.Range(0.0f, 200.0f), 1.0f, Random.Range(0.0f, 200.0f));
		//A criar os inimigos
		GameObject[] enemyes = GameObject.FindGameObjectsWithTag("Enemy");
		for(int i = 0; i < passo && enemyes.Length < 100; i++){
			if(criaInimigos){
				GameObject obj = Instantiate(GameObject.Find("ModelPill"), pos, Random.rotation) as GameObject;
				obj.renderer.enabled = true;
				criaInimigos = false;
			}
		}
		//A criar os nutrientes
		try{
			GameObject[] nut = GameObject.FindGameObjectsWithTag("nutriente");
			for(int i = 0; (i < 5) && (nut.Length < 200); i++){
				pos = new Vector3(Random.Range(0.0f, 200.0f), 1.0f, Random.Range(0.0f, 200.0f));
				GameObject obj = Instantiate(GameObject.Find("Nutriente"), pos, Quaternion.identity) as GameObject;
				obj.renderer.enabled = true;
			}
		}catch{}
		//A verificar se é fim do jogo.
		if(gameOver()){
			storeScore("Player");//para já
			Application.LoadLevel("finalscene");
		}
		//implementação de pausa no jogo
		if(!paused){
			if(Input.GetKeyDown(KeyCode.Escape)){
				Time.timeScale = 0;
				paused = true;
				GameObject.Find("Main Camera").GetComponent<AudioSource>().mute = true;
			}
		}else{
			if(Input.GetKeyDown(KeyCode.Escape)){
				paused = false;
				GameObject.Find("Main Camera").GetComponent<AudioSource>().mute = false;
				Time.timeScale = 1;
			}
		}
	}
	/**
	 * Função utilizada para armazenar num ficheiro os top scores
	 */
	public static void storeScore(string playerName){
		Score score = new Score(playerName, MainMenu.currentTime);
		ArrayList storedScores = new ArrayList();
		//ler o ficheiro do pc, caso exista e criar arraylist contendo os scores
		try{
			string[] scoretextlines = System.IO.File.ReadAllLines("../topScores/topScore.txt");
			for(int i = 0; i < scoretextlines.Length; i++){
				string[] pal = scoretextlines[i].Split(' ');
				if(pal.Length == 2) storedScores.Add(new Score(pal[0], float.Parse(pal[1])));
			}
		}catch{}
		//Adiciona o novo score ao arraylist
		storedScores.Add(score);
		//ordena o arrayList
		ArrayList sortedStoredScores = sortScores(storedScores);
		//preparar as linhas de texto a escrever no ficheiro
		string[] linhas = new string[sortedStoredScores.Count];
		int j = 0;
		IEnumerator e = sortedStoredScores.GetEnumerator();
		while(e.MoveNext()){
			Score scr = (Score) e.Current;
			linhas[j++] = scr.toString();
		}
		//A guardar apenas os top 10 high scores
		string[] lines = new string[10];
		for (int i = 0; i < 10 && i < linhas.Length; i++) lines[i] = linhas[i];
		//a preparar para escrever no ficheiro
	    if (!Directory.Exists ("../topScores")) {
	        Directory.CreateDirectory ("../topScores");
	    }
		//a escrever no ficheiro
		System.IO.File.WriteAllLines("../topScores/topScore.txt", lines);
	}
	
	public static ArrayList sortScores(ArrayList lista){
		ArrayList listaOrdenada = new ArrayList();
		while(lista.Count > 0){
			float max = -1.0f;
			Score smax = new Score("x",0.0f);
			IEnumerator e = lista.GetEnumerator();
			while(e.MoveNext()){
				Score s = (Score) e.Current;
				if(s.getTime() > max){
					max = s.getTime();
					smax = s;
				}
			}
			if(smax.getPlayerName() != "x") listaOrdenada.Add(smax);
			lista.Remove(smax);
		}
		return listaOrdenada;
	}
	private bool gameOver(){
		GameObject[] bacterias = GameObject.FindGameObjectsWithTag("bacteria");
		return (bacterias.Length == 0);
	}
	void OnGUI () {
        if(paused) GUI.Label (new Rect (500, 200, 200, 40), "Paused!\nPress ESC to resume game");
    }
}

public class Score{
	private string playerName;
	private float time;
	
	public Score(string name, float time){
		this.playerName = name;
		this.time = time;
	}
	
	public float getTime(){
		return this.time;
	}
	public void setTime(float time){
		this.time = time;
	}
	public void setPlayerName(string name){
		this.playerName = name;
	}
	public string getPlayerName(){
		return playerName;
	}
	public string toString(){
		return "" + getPlayerName() + " " + getTime();
	}
}
