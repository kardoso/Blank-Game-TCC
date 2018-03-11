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
				FindObjectOfType<Player>().StopMovement();
				keyPressed = true;
				int _value = Random.Range(0,2);
				switch (_value){
					case 0:
						GetComponent<Animator>().SetTrigger("Porta1");
						break;
					case 1:
						GetComponent<Animator>().SetTrigger("Porta2");
						break;
					default:
						break;
				}
			}
		}
		else{
			buttonImage.SetActive(false);
		}
	}

	void LoadLevel(){
		TransitionManager.Instance.LoadLevel(sceneToLoadName, 1.0f);
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
