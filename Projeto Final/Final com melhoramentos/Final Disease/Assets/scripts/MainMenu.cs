using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	private float startTime;
	public static float currentTime;
  	private ArrayList ativo;
	public Texture toolbar;
	public Texture biparticao;
	public Texture reproduzida;
	public Texture indicador1;
	public Texture nivel1;
	public Texture nivel2;
	Texture2D textureBarraVida;  
	public GUIStyle barradevida;
    private Rect janelaPopulacaoStats = new Rect(20, 20, 200, 150);
	
	public	GUIStyle skyle;
	GameObject bacteria;
	Rect quadrado;
	Rect rect;
	GUIStyle TextStyle;
	
	// Use this for initialization
	void Start () {
		TextStyle = new GUIStyle();
		TextStyle.normal.textColor = Color.black;
		startTime = Time.time;	
		textureBarraVida = new Texture2D(1, 1);
		textureBarraVida.SetPixel(0,0,Color.green);
		textureBarraVida.Apply();
		quadrado = new Rect(573, 629, biparticao.width-16, biparticao.height-10);
		ativo = new ArrayList();
	}
	void Update(){}
	
	// Update is called once per frame
	void Update2(){
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.y = Screen.height - mousePosition.y;
	 	if (quadrado.Contains(mousePosition))
		{
			if(Input.GetMouseButtonUp(0))
			{
				IEnumerator e = mouseSelection.selectedBacterias.GetEnumerator();
				while (e.MoveNext())
		        {
					bacteria = (GameObject) e.Current;
					if(bacteria.GetComponent<Bacteria>().reproduz){			
						 bacteria.GetComponent<Bacteria>().BiParticao();
		        	}
				}
			}
		}
 	}
	
	void OnGUI() {
		GUI.DrawTexture(new Rect(0, 768-225,1024, 225), toolbar);		
		janelaPopulacaoStats = GUI.Window(0, janelaPopulacaoStats, populacao, "Population Stats");
		itensSelecionados();	
		try{
			int reproduz = 0;
			IEnumerator e = mouseSelection.selectedBacterias.GetEnumerator();
			while (e.MoveNext()){
				bacteria = (GameObject) e.Current;
				if(bacteria.GetComponent<Bacteria>().reproduz)
					reproduz = 1;
			}
			
			if (mouseSelection.selectedBacterias.Count == 1)
				GUI.Label(new Rect(580, 60+575, biparticao.width-16, biparticao.height-10), "" + (int)(30-bacteria.GetComponent<Bacteria>().timerReproduz),TextStyle);
		
		 	if ((mouseSelection.selectedBacterias.Count >= 1)&&(reproduz == 1)){
		  		GUI.DrawTexture(quadrado, biparticao);
		  	}
		}catch{}
		Update2();
	}	
	/**
	 * Função responsável pela apresentação do conteúdo no menu population stats
	 */
    void populacao(int windowID) {	
		GUI.Label(new Rect(10,25,180,30), "Timer: " + timerMethod(),TextStyle);
		GUI.Label(new Rect(10,55,180,200), "Population:\n" + populationStats(),TextStyle);
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }	
	/**
	 * Função responsável pela apresentação do conteúdo no menu selected items
	 */
    void itensSelecionados() {
		try{
			IEnumerator e = mouseSelection.selectedBacterias.GetEnumerator(); 
			int contador = 0;
			GameObject bacteria;
			
			if(mouseSelection.selectedBacterias.Count == 0){
				return;
			}			
			//Mostrar dados da bacteria selecionada
			if (mouseSelection.selectedBacterias.Count == 1){
				contador++;
				e.MoveNext();
				bacteria = (GameObject) e.Current;
				GUI.DrawTexture(new Rect(708, 583+50, 105, 29), indicador1);
				float barradevida = (bacteria.GetComponent<Bacteria>().resistencia1/100.0f)*104.0f;
			
				// Barra de vida
				GUI.skin.box.normal.background = textureBarraVida;
				GUI.Box(new Rect(710, 599+50,barradevida-1, 12), GUIContent.none);
			
				// Informacoes
				if (bacteria.GetComponent<Bacteria>().nivel == 1)
					GUI.DrawTexture(new Rect(265, 630, 68, 67), nivel1);
				else
					GUI.DrawTexture(new Rect(265, 630, 68, 67), nivel2);
				GUI.HorizontalScrollbar(new Rect(265, 700, 68, 20),0, bacteria.GetComponent<Bacteria>().vida,0, 100);
				GUI.Label(new Rect(350, 630, 68, 68), "" + bacteria.name + bacteriaState(bacteria),TextStyle);
			}
			else{
				ativo.Clear();
				// Tem muitas selecionadas
				while (e.MoveNext()){
					bacteria = (GameObject) e.Current;
					ativo.Add(new Rect(265 + (contador%4)*60 , 630 + (contador/4)*60,60,60));
					if (bacteria.GetComponent<Bacteria>().nivel == 1)
						GUI.DrawTexture(new Rect(265 + (contador%4)*60 , 630 + (contador/4)*60, 60, 60), nivel1);
					else
						GUI.DrawTexture(new Rect(265 + (contador%4)*60 , 630 + (contador/4)*60, 60, 60), nivel2);
					
					GUI.HorizontalScrollbar(new Rect(265 + (contador%4)*60, 680 + (contador/4)*60, 60, 20), 0, bacteria.GetComponent<Bacteria>().vida,0, 100);
					contador++;
				}
			}			
		}catch{}
    }
	/**
	 * Função que retorna o texto para o timer
	 */
	private string timerMethod(){
			currentTime = Time.time - startTime;
			int minutes = (int)(currentTime / 60);
			int seconds = (int)(currentTime % 60);
			int fraction = (int)((currentTime * 100) % 100);
			return "" + minutes + ":" +  seconds + ":" + fraction;
	}
	/** 
	 * Função que retorna o texto sobre a população de bactérias
	 */
	private string populationStats(){
		GameObject[] bacterias = GameObject.FindGameObjectsWithTag("bacteria");
		string result = "Total bacteria: " + bacterias.Length + "\n";
		int totalVida = 0;
		int totalResistencia = 0;
		for(int i = 0; i < bacterias.Length; i++){
			totalVida += bacterias[i].GetComponent<Bacteria>().vida;
			totalResistencia += bacterias[i].GetComponent<Bacteria>().resistencia;
		}
		result += "Total health: " + totalVida + "\nTotal resistance: " + totalResistencia;
		return result;
	}
	private string bacteriaState(GameObject obj){
		return " life: " + obj.GetComponent<Bacteria>().vida + " resistence: " + obj.GetComponent<Bacteria>().resistencia1;
	}
}