using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public GameObject doorIndicatorSphere;

	private int doorQuant;

	void Awake()
	{
		doorIndicatorSphere.SetActive(false);
		doorQuant = 0;
	}

	public bool HasDoor(){
		return doorQuant > 0;
	}

	public void AddDoor(){
		doorQuant += 1;
		doorIndicatorSphere.SetActive(true);
		FindObjectOfType<Fade>().FadeGameObject(doorIndicatorSphere, true, 1f);
	}

	public void RemoveDoor(){
		doorQuant -= 1;
		doorIndicatorSphere.SetActive(false);
		FindObjectOfType<Fade>().FadeGameObject(doorIndicatorSphere, true, 1f);
	}
}
