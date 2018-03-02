using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	public enum ButtonType // your custom enumeration
	{
		OneTouch,
		HoldType
	};

	public Sprite enabledSpr;
	public Sprite disabledSpr;

	public ButtonType buttonType;
	public GameObject objectToActive;

	void Start()
	{
		GetComponent<SpriteRenderer>().sprite = disabledSpr;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(buttonType == ButtonType.OneTouch){
			//objectToActive.GetComponent<ActivatableObject>().Active();
			GetComponent<SpriteRenderer>().sprite = enabledSpr;
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if(buttonType == ButtonType.HoldType){
			//objectToActive.GetComponent<ActivatableObject>().Active();
			GetComponent<SpriteRenderer>().sprite = enabledSpr;
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if(buttonType == ButtonType.HoldType){
			//objectToActive.GetComponent<ActivatableObject>().Deactivate();
			GetComponent<SpriteRenderer>().sprite = disabledSpr;
		}
	}
}
