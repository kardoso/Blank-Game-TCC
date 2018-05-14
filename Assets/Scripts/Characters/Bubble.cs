using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		anim.updateMode = AnimatorUpdateMode.UnscaledTime;
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BubbleOn(){
		gameObject.SetActive(true);
		anim.SetTrigger("begin");
	}

	public void BubbleOff(){
		anim.SetTrigger("end");
	}

	public void StartMovement(){
		StartCoroutine(transform.parent.GetComponent<Player>().MoveToInitialPosition(0.5f));
	}

	public void StopMovement(){
		transform.parent.GetComponent<Player>().InitialPositionDone();
		gameObject.SetActive(false);
	}
}
