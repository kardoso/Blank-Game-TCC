using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    public bool isInteractable;
    public GameObject enemyToSpawn;
    public Transform point1;
    public Transform point2;
    public Transform point3;
    
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;

    void Start()
    {
        p1 = point1.position;
        p2 = point2.position;
        p3 = point3.position;
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Player")){
            if(isInteractable){
                //input - ainda vou fazer o input
                //isInteractable geralmente vai ser uma porta
                GameObject.Instantiate(enemyToSpawn, p1, Quaternion.identity);
                GameObject.Instantiate(enemyToSpawn, p2, Quaternion.identity);
                GameObject.Instantiate(enemyToSpawn, p3, Quaternion.identity);
                Destroy(this.gameObject);
            }
            else{
                GameObject.Instantiate(enemyToSpawn, p1, Quaternion.identity);
                GameObject.Instantiate(enemyToSpawn, p2, Quaternion.identity);
                GameObject.Instantiate(enemyToSpawn, p3, Quaternion.identity);
                Destroy(this.gameObject);
            }
		}
	}
}
