using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public GameObject keyIndicatorSphere;

	private int keyQuant;

	void Awake()
	{
		keyIndicatorSphere.SetActive(false);
		keyQuant = 0;
	}

	public bool HasKey(){
		return keyQuant > 0;
	}

	public void AddKey(){
		keyQuant += 1;
		keyIndicatorSphere.SetActive(true);
	}

	public void RemoveKey(){
		keyQuant -= 1;
		keyIndicatorSphere.SetActive(false);
	}
}
