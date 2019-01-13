using UnityEngine;
using System.Collections;

public class Bacteria : MonoBehaviour{
	// Status da Bacteria
	public int resistencia;
	public int resistencia1;
	public int nivel;
	public int vida;
	public bool reproduz;
	public float timer, timerReproduz;
	public int secondsToReproduce;
	//Tratamento do score do jogador..
	public GameObject barradevida;	
	public GameObject selectedbase;
	public static int count = 1;	
	public bool stop = true;	
	
	// Use this for initialization
	void Start (){ 
		vida = 100;
		reproduz = false;
		resistencia = 0;
		resistencia1 = 0;
		timer = 0.0f;
		secondsToReproduce = 30;
		barradevida = Instantiate(GameObject.Find("Lifebar"), this.transform.position, this.transform.rotation) as GameObject;
		barradevida.transform.parent = this.transform;
		barradevida.transform.position += new Vector3(0.0f,2.5f,0.0f);
		barradevida.transform.renderer.enabled = true;
		selectedbase = Instantiate(GameObject.Find("SelectedBase"), this.transform.position, this.transform.rotation) as GameObject;
		selectedbase.transform.parent = this.transform;
		selectedbase.transform.position += new Vector3(0.0f,-2.5f,0.0f);
	 }
	 
	public void BiParticao () 
	{	
		reproduz = false;
		Vector3 newposition = this.transform.position;
	 	newposition += new Vector3(5f,0f,0f);
		try{
			GameObject Filho;
			
		 	if (nivel == 1)
				Filho = Instantiate(GameObject.Find("ModelBac1"), newposition,  this.transform.rotation )  as GameObject;
			 else
			{   
				Filho = Instantiate(GameObject.Find("ModelBac2"), newposition, this.transform.rotation )  as GameObject;
				Filho.GetComponent<Bacteria>().resistencia1 = 100;
			}
			count++;
			Filho.name = "bacteria " + count;
			Filho.transform.name = "bacteria " + count;
			Filho.tag = "bacteria";
		 	Filho.renderer.enabled = true;
			Filho.GetComponent<Bacteria>().enabled = true;
			this.vida /= 2;
			Filho.GetComponent<Bacteria>().vida = this.vida/2;
			Filho.GetComponent<Bacteria>().nivel = this.nivel;
			Filho.GetComponent<Bacteria>().selectedbase.renderer.enabled = false;
			Filho.GetComponent<Bacteria>().secondsToReproduce = 30;			
			Filho.GetComponent<Bacteria>().reproduz = false;
			
			timer = 0.0f;
			timerReproduz = 0.0f;
		}
		catch(MissingReferenceException){}
		catch(MissingComponentException){}
		catch(System.NullReferenceException){}
	}
	
	// Update is called once per frame
	public void Update () 
	{
		timer += Time.deltaTime;
		timerReproduz += Time.deltaTime;
		if(timer >= 5){
			vida -= 5;
			timer = 0.0f;
		}
		if(!reproduz && timerReproduz > secondsToReproduce){
			timerReproduz = 0.0f;
			reproduz = true;
		}
		if (vida <= 0)
		{
			mouseSelection.selectedBacterias.Remove(this.gameObject);
			Destroy(this.gameObject);
		}
		if (resistencia1 >= 100)
			nivel = 2;
		Vector3 newtam = new Vector3(vida/100.0f,0.1f,0.1f);
		barradevida.transform.localScale = newtam;
		
	}
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("nutriente")){
		if (other.gameObject.renderer.isVisible) {
			this.vida += other.GetComponent<Nutriente>().alimento;
			Destroy(other.gameObject);
			if (this.vida > 100)
				this.vida = 100;
			}
		}
	}
}
