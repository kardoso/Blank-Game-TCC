using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu 
{
    public GameObject title;

    public GameObject[] languageOptions;
    public GameObject[] menuOptions;
    public GameObject[] settingsOptions;

    int menuMode;       //0 is language, 1 is menu, 2 is settings

    public string sceneToLoad;

	protected override void Start()
    {
        base.Start();

        var list = new List<GameObject>();
        list.AddRange(languageOptions);
        list.AddRange(menuOptions);
        list.AddRange(settingsOptions);

        GameObject[] allOptions = list.ToArray();

        foreach(GameObject g in allOptions){
            g.GetComponent<Text>().color = optionNotSelected;
        }

        if(GameManager.Instance.mainMenuMode == 0){
            GameManager.Instance.mainMenuMode = 1;
            StartCoroutine(SetNewOptions(languageOptions));
            menuMode = 0;
        }
        else{
            StartCoroutine(SetNewOptions(menuOptions));
            menuMode = 1;
        }
	}

    protected override void Update()
    {
        base.Update();
    }

    protected override void CheckSubmit(){
        if(!pressed){
            //Language
            if(menuMode == 0){
                if (Input.GetButtonDown("Submit"))
                {
                    //Portugues
                    if(chooseThisOption == 0){
                        Debug.Log("Portuguese");
                        GameManager.Instance.Language = "Portuguese";
                    }
                    //EngLish
                    else if(chooseThisOption == 1){
                        Debug.Log("English");
                        GameManager.Instance.Language = "English";
                    }

                    FindObjectOfType<Lang>().setLanguage((TextAsset)Resources.Load("menusLang"), GameManager.Instance.Language);
                    LoadLanguage();

                    StartCoroutine(SetNewOptions(menuOptions));
                    StartCoroutine(FadeTitle(true));
                    menuMode = 1;

                    pressed = true;
                }
            }
            //Main Menu
            else if(menuMode == 1){
                if (Input.GetButtonDown("Submit"))
                {
                    //New Game
                    if(chooseThisOption == 0){
                        Debug.Log("New Game");
                        TransitionManager.Instance.LoadLevel(sceneToLoad, 4);
                    }
                    //Options
                    else if(chooseThisOption == 1){
                        Debug.Log("Options");
                        StartCoroutine(SetNewOptions(settingsOptions));
                        StartCoroutine(FadeTitle(false));
                        menuMode = 2;
                    }
                    //Quit
                    else if(chooseThisOption == 1){
                        Application.Quit();
                    }

                    pressed = true;
                }
            }
            //Options
            else if(menuMode == 2){
                if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel"))
                {
                    //New Game
                    if(chooseThisOption == 0){
                        //Debug.Log("Portuguese");
                    }
                    //Options
                    else if(chooseThisOption == 1){
                        //Debug.Log("English");
                    }
                    else if(chooseThisOption == 2){

                    }

                    StartCoroutine(SetNewOptions(menuOptions));
                    StartCoroutine(FadeTitle(true));
                    menuMode = 1;

                    pressed = true;
                }
            }
        }
    }

    protected override void CheckToShow(){
        base.CheckToShow();
        if(menuMode == 2){
            foreach(GameObject g in settingsOptions){
                foreach(Transform t in g.transform){
                    t.gameObject.GetComponent<Text>().color = g.GetComponent<Text>().color;
                }
            }
        }
    }

    IEnumerator FadeTitle(bool toActive){
        if(toActive){
            yield return new WaitForSeconds(.9f);
            title.SetActive(true);
            fade.FadeGameObject(title, 1, 0, 1);
        }
        else{
            fade.FadeGameObject(title, 1, 1, 0);
            yield return new WaitForSeconds(.9f);
            title.SetActive(false);
        }
    }

    void LoadLanguage(){
        var langMan = FindObjectOfType<Lang>();
        menuOptions[0].GetComponent<Text>().text = langMan.getString("start_game");
        menuOptions[1].GetComponent<Text>().text = langMan.getString("settings");
        menuOptions[2].GetComponent<Text>().text = langMan.getString("exit");


    }
}
