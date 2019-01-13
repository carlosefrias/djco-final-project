using UnityEngine;
using System.Collections;

public class inimigo : MonoBehaviour {
	
	public int ataque;
	public int resistencia;
	public float range;
	private GameObject[] bacterias;
	private bool stop = false;
	
	// Use this for initialization
	void Start () {
		ataque = 30;
		resistencia = 20;
		range = 50.0f;
	}
	
	// Update is called once per frame
	void Update () {
		bacterias = GameObject.FindGameObjectsWithTag("bacteria");
		float min = range + 1;
		if(bacterias.Length > 0){
			int pos = 0;
			for(int i = 0; i < bacterias.Length; i++){
				float dist = distance(this.transform.position, bacterias[i].transform.position);
				if(dist < min && dist <= range){
					pos = i;
					min = dist;
				}
			}
			StartCoroutine(moveObject(bacterias[pos].transform.position));
		}
	}
	private float distance(Vector3 p1, Vector3 p2){
		return Mathf.Sqrt((p1.x-p2.x)*(p1.x-p2.x)+(p1.y-p2.y)*(p1.y-p2.y)+(p1.z-p2.z)*(p1.z-p2.z));
	}
	private IEnumerator moveObject(Vector3 destino) {
		Vector3 origem = this.transform.position;
		Vector3 desloc = destino - origem;
		Vector3 passo;
		if(stop){
			stop = false;
			passo = desloc / (100*(Mathf.Sqrt(desloc.x*desloc.x + desloc.y*desloc.y + desloc.z*desloc.z)));
			while (Mathf.Abs(this.transform.position.x - destino.x) > 0.1 && !stop) {
				Vector3 objPosition = this.transform.position;
				Vector3 newPosition = new Vector3(objPosition.x + passo.x, objPosition.y, objPosition.z + passo.z);
				this.transform.position = newPosition;
				yield return new WaitForSeconds(0.02f);
				if (newPosition.x <= 2.5 | newPosition.x >= 197 | newPosition.z <= 2.5 | newPosition.z >= 197)
					stop = true;
			}
			stop = true;
		}else{
			stop = true;
			yield return new WaitForSeconds(0.02f);
			StartCoroutine(moveObject(destino));
		}
	}
	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag("bacteria")){
			if (other.GetComponent<Bacteria>().nivel <= 1){
				other.GetComponent<Bacteria>().vida -= this.ataque;
				other.GetComponent<Bacteria>().resistencia1 += this.resistencia;
				if (other.GetComponent<Bacteria>().resistencia1 > 100)
					other.GetComponent<Bacteria>().resistencia1 = 100;
			}
			stop = true;
			destroyObject();
		}
	}
	public void destroyObject(){
		var renderers = this.GetComponentsInChildren<Renderer>();
		renderers[0].enabled = false;
		renderers[1].enabled = false;
		renderers[2].enabled = false;
		Destroy(this);
		this.gameObject.renderer.enabled = false;  	 		
	}
}
