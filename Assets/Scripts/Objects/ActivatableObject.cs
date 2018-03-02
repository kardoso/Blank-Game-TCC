using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivatableObject {
	protected bool isActive;

	public void Active(){
		isActive = true;
	}

	public void Deactivate(){
		isActive = false;
	}
}
