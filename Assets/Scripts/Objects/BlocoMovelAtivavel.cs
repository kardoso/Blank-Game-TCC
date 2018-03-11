using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocoMovelAtivavel : MonoBehaviour {

	public void EnableBlock(){
		GetComponent<Animator>().SetBool("isOn", false);
	}

	public void DisableBlock(){
		GetComponent<Animator>().SetBool("isOn", true);
	}

	void EnableCollider(){
		GetComponent<BoxCollider2D>().enabled = true;
	}

	void DisableCollider(){
		GetComponent<BoxCollider2D>().enabled = false;
	}
}
