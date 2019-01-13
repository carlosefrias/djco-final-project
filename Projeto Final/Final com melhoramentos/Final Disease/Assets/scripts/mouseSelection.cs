using UnityEngine;
using System.Collections;

public class mouseSelection : MonoBehaviour {
	private bool flagPontoInicial = false;
	public static ArrayList selectedBacterias;
	

	/**
	  * Use this for initialization
	  */
	void Start () {
		selectedBacterias = new ArrayList();
	}
	void Update () {
	
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    RaycastHit hit;
		
		//clicou no botão esquerdo do rato
		if(Input.GetMouseButtonUp(0)){
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if(Physics.Raycast(ray, out hit, 100)){
				//clicou sobre uma bactéria
				GameObject obj = hit.transform.gameObject;
				//A definir o ponto inicial do percurso das bactérias
				if(!flagPontoInicial){ 
					flagPontoInicial = true;
				}
				if(obj.tag == "object" || obj.tag == "bacteria"){	
					//Se o objecto clicado já estiver selecionado, desseleciona-o
					if(selectedBacterias.Contains(obj)){
						selectedBacterias.Remove(obj);
						adicionaBase(obj, false);
					}
					//Seleciona a bactéria
					else{
						selectedBacterias.Add(obj);
						adicionaBase(obj, true);
					}
				}
				else{
					IEnumerator e = selectedBacterias.GetEnumerator();
			        while (e.MoveNext())
			        {
			            GameObject objecto = (GameObject) e.Current;
						adicionaBase(objecto, false);					
			        }
					selectedBacterias.Clear();
				}
			}
		}
		//clicou no botao direito do rato
		if(Input.GetMouseButtonUp(1) && selectedBacterias.Count > 0){
			if(Physics.Raycast(ray, out hit, 100)){
				Vector3 ponto = hit.point;
				
				Vector3 pontoMedio = new Vector3(0, 0, 0);
				IEnumerator f = selectedBacterias.GetEnumerator();
				while (f.MoveNext()) {
					GameObject obj = (GameObject) f.Current;
					pontoMedio += obj.transform.position;
				}
				pontoMedio /= selectedBacterias.Count;
				if(hit.transform.tag == "Floor"){
					
				}
				else{
					if(selectedBacterias.Count > 0){
						flagPontoInicial = false;
						IEnumerator e = selectedBacterias.GetEnumerator();
				        while (e.MoveNext())
				        {
				            GameObject obj = (GameObject) e.Current;
							Vector3 origem = obj.transform.position;
							Vector3 distanciaPM = origem - pontoMedio;
							try{
								StartCoroutine(moveObject(obj, ponto + distanciaPM));
							}
							catch(MissingReferenceException){	
							}
						}
					}
				}
			}		
		}
	}
	
	/**
	 * Move um objeto, passando o nome, o ponto de origem e o ponto de destino
	 */	
	IEnumerator moveObject(GameObject obj, Vector3 destino) {	
		//pegar o vetor deslocamento
		Vector3 origem = obj.transform.position;
		Vector3 desloc = destino - origem;
		Vector3 passo;
		if (obj.GetComponent<Bacteria>().stop) {
			obj.GetComponent<Bacteria>().stop = false;
			//faz com que ele tenha norma = 1
			passo = desloc / (5*(Mathf.Sqrt(desloc.x*desloc.x + desloc.y*desloc.y + desloc.z*desloc.z)));
			if(obj != null)
				while ((Mathf.Abs(obj.transform.position.x - destino.x) > 0.1) && (!obj.GetComponent<Bacteria>().stop)) {
					Vector3 objPosition = obj.transform.position;
					Vector3 newPosition = new Vector3(objPosition.x + passo.x, objPosition.y, objPosition.z + passo.z);
					obj.transform.position = newPosition;
					yield return new WaitForSeconds(0.01f);
					if (newPosition.x <= 2.5 | newPosition.x >= 197 | newPosition.z <= 2.5 | newPosition.z >= 197)
						obj.GetComponent<Bacteria>().stop = true;
				}
			obj.GetComponent<Bacteria>().stop = true;
		} else {
			obj.GetComponent<Bacteria>().stop = true;
			yield return new WaitForSeconds(0.01f);
			StartCoroutine(moveObject(obj, destino));
		}
	}
	private void adicionaBase(GameObject obj, bool mostra){
		obj.GetComponent<Bacteria>().selectedbase.renderer.enabled = mostra;
	}
}
