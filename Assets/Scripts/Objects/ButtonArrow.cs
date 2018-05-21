using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonArrow : MonoBehaviour {

	public Transform[] objBlocos;
	public AudioClip buttonFX;

	public void EnableButton(){
		foreach(Transform t in objBlocos){
			t.GetComponent<BlocoMovelAtivavel>().EnableBlock();
		}
		GetComponent<Animator>().SetBool("Active", true);
		SoundManager.PlaySFX(buttonFX);
		
	}

	public void DisableButton(){
		GetComponent<Animator>().SetBool("Active", false);
		foreach(Transform t in objBlocos){
			t.GetComponent<BlocoMovelAtivavel>().DisableBlock();
		}
	}

}
