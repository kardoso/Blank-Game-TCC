using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocoTemporario : MonoBehaviour
{

    private bool isActive;
    private float timeActive;

    // Use this for initialization
    void Start()
    {
        timeActive = 3;
        isActive = true;
        StartCoroutine("ChangeActivity");
    }

    IEnumerator ChangeActivity()
    {
        while (true)
        {
            if (isActive)
            {
                EnableBlock();
            }
            else
            {
                DisableBlock();
            }
            isActive = !isActive;
            yield return new WaitForSeconds(timeActive);
        }
    }

    public void EnableBlock()
    {
        GetComponent<Animator>().SetBool("isOn", false);
    }

    public void DisableBlock()
    {
        GetComponent<Animator>().SetBool("isOn", true);
    }

    void EnableCollider()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    void DisableCollider()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
