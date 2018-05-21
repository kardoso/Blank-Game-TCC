using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour {
	
	[SerializeField]
    private string sceneToLoadName;

	public AudioClip doorFX;

	private bool keyPressed = false;

	private bool isInside;

	private TransitionManager.TransitionType tType;

	void Start()
	{
		foreach(Transform t in transform){
			t.gameObject.SetActive(false);
		}
	}

	void Update()
	{
		if(isInside){
			transform.Find("ButtonKeyboard").gameObject.SetActive(keyPressed?false:true);
			if((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("B")) && !keyPressed){
				FindObjectOfType<Inventory>().RemoveKey();
				FindObjectOfType<Player>().StopMovement();
				FindObjectOfType<Player>().GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
				keyPressed = true;
				SoundManager.PlaySFX(doorFX);
				GetComponent<Animator>().SetTrigger("AbrirPorta");
			}
		}
		else{
			foreach(Transform t in transform){
				t.gameObject.SetActive(false);
			}
		}
	}

	void AbrirPorta(){
		int _value = Random.Range(0,2);
		switch (_value){
			case 0:
				GetComponent<Animator>().SetTrigger("Porta1");
				tType = TransitionManager.TransitionType.Vertical_Bottom;
				break;
			case 1:
				GetComponent<Animator>().SetTrigger("Porta2");
				tType = TransitionManager.TransitionType.Horizontal_Right;
				break;
			default:
				break;
		}
	}

	void LoadLevel(){
		TransitionManager.Instance.SettingTransitionType(tType);
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
