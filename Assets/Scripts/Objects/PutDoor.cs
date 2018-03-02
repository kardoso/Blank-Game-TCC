using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PutDoor : MonoBehaviour {

	public int sceneToLoad;

	void OnTriggerStay2D(Collider2D other){
		if(other.gameObject.tag.Equals("Player")){
			if(other.gameObject.GetComponent<Inventory>().HasDoor()){
				if(Input.GetKeyDown(KeyCode.E)){
					other.gameObject.GetComponent<Inventory>().RemoveDoor();
					SceneManager.LoadScene(sceneToLoad);
				}
			}
		}
	}
}
