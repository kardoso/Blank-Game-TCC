using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour {
	
	[SerializeField]
    private string sceneToLoadName;

	private bool keyPressed = false;

	void OnTriggerStay2D(Collider2D other){
		if(other.gameObject.tag.Equals("Player")){
			if(other.gameObject.GetComponent<Inventory>().HasKey()){
				if(Input.GetKeyDown(KeyCode.E) && !keyPressed){
					other.gameObject.GetComponent<Inventory>().RemoveKey();
					keyPressed = true;
					TransitionManager.Instance.LoadLevel(sceneToLoadName, 1.0f);
				}
			}
		}
	}
}
