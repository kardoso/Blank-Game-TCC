using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour {
	
	[SerializeField]
    private string sceneToLoadName;

	private bool keyPressed = false;

	public GameObject buttonImage;
	private bool isInside;

	void Start()
	{
		buttonImage.SetActive(false);
	}

	void Update()
	{
		if(isInside){
			buttonImage.SetActive(keyPressed?false:true);
			if((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("B")) && !keyPressed){
				FindObjectOfType<Inventory>().RemoveKey();
				keyPressed = true;
				TransitionManager.Instance.LoadLevel(sceneToLoadName, 1.0f);
			}
		}
		else{
			buttonImage.SetActive(false);
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if(other.gameObject.tag.Equals("Player")){
			if(other.gameObject.GetComponent<Inventory>().HasKey()){
				isInside = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Player")){
			isInside = false;
		}
	}
}
