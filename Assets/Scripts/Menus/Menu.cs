using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour{

	protected GameObject[] availableOptions;
    protected int chooseThisOption;
    protected bool pressed;
    private float inptVel;
    protected Fade fade;
    public Color optionSelected;
    public Color optionNotSelected;
    //public Color optionPressed;

    protected virtual void Start()
    {
        fade = FindObjectOfType<Fade>();
        
        chooseThisOption = 0;

		pressed = false;
    }

	protected virtual void Update()
    {
        CheckToShow();
        KeyboardInput();
        CheckSubmit();
        inptVel = Input.GetAxisRaw("Vertical");
    }


    protected virtual void CheckToShow(){
        for(int i = 0; i < availableOptions.Length; i++){
            if(availableOptions[i] == availableOptions[chooseThisOption]){
                availableOptions[i].GetComponent<Text>().color = optionSelected;
            }
            else{
                availableOptions[i].GetComponent<Text>().color = optionNotSelected;
            }
        }
    }

    protected abstract void CheckSubmit();

    protected IEnumerator SetNewOptions(GameObject[] options){
        pressed = true;
        if(availableOptions != null){
            foreach(GameObject g in availableOptions){
                fade.FadeGameObject(g, 1, 1, 0);
                foreach(Transform gg in g.transform){
                    fade.FadeGameObject(gg.gameObject, 1, 1, 0);
                }
            }
            yield return new WaitForSecondsRealtime(0.9f);
            foreach(GameObject g in availableOptions){
                g.SetActive(false);
                foreach(Transform gg in g.transform){
                    gg.gameObject.SetActive(false);
                }
            }
        }
        availableOptions = options;
        chooseThisOption = 0;
        CheckToShow();
        foreach(GameObject g in availableOptions){
            g.SetActive(true);
            fade.FadeGameObject(g, 1, 0, 1);
            foreach(Transform gg in g.transform){
                gg.gameObject.SetActive(true);
                fade.FadeGameObject(gg.gameObject, .1f, 0, 1);
            }
        }
        pressed = false;
    }

	protected void KeyboardInput()
    {
        if (!pressed)
        {
            if (Input.GetAxisRaw("Vertical") > 0 && inptVel < 1)
            {
                PreviousOption();
            }

            if (Input.GetAxisRaw("Vertical") < 0 && inptVel > -1)
            {
                NextOption();
            }
        }
    }

    void PreviousOption()
    {
        if (chooseThisOption == 0)
        {
            chooseThisOption = availableOptions.Length - 1;
        }
        else
        {
            chooseThisOption -= 1;
        }
    }

    void NextOption()
    {
        if (chooseThisOption == availableOptions.Length - 1)
        {
            chooseThisOption = 0;
        }
        else
        {
            chooseThisOption += 1;
        }
    }
}