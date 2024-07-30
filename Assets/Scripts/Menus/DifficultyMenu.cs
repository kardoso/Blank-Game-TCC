using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyMenu : Menu 
{
    public GameObject[] menuOptions;
    public GameObject menuTitle;

    public AudioClip bgm;

    public string sceneToLoad;
    public Color mainTransitionColor;

	protected override void Start()
    {
        base.Start();
        LoadLanguage();

        menuOptions[0].GetComponent<Text>().color = optionSelected;
        menuOptions[1].GetComponent<Text>().color = optionSelected;
        SetNewOptions(menuOptions);
        availableOptions = menuOptions;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void CheckSubmit(){
        if (Input.GetButtonDown("Submit") || (Input.GetButtonDown("A") || Input.GetButtonDown("Menu")))
        {
            //easy
            if(chooseThisOption == 0){
                GameManager.Instance.difficulty = 0;
                FindObjectOfType<TransitionManager>().SetColor(mainTransitionColor);
                TransitionManager.Instance.LoadLevel(sceneToLoad, 1);
            }
            //hard
            else if(chooseThisOption == 1)
            {
                GameManager.Instance.difficulty = 1;
                FindObjectOfType<TransitionManager>().SetColor(mainTransitionColor);
                TransitionManager.Instance.LoadLevel(sceneToLoad, 1);
            }
        }
    }

    void LoadLanguage(){
        Lang.Instance.setLanguage(0, GameManager.Instance.Language);

        menuTitle.GetComponent<Text>().text = Lang.Instance.getString("select_difficulty");

        menuOptions[0].GetComponent<Text>().text = Lang.Instance.getString("easy_difficulty");
        menuOptions[1].GetComponent<Text>().text = Lang.Instance.getString("normal_difficulty");
    }
}
