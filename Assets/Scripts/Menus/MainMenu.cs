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
    public GameObject[] creditsOnlyOption;
    public GameObject[] credits;

    public AudioClip bgm;

    int menuMode;       //0 is language, 1 is menu, 2 is settings, 3 is credits

    private int currentResolution;
	private bool isFullScreen;
	private bool vsyncActive;

    public string sceneToLoad;
    public Color mainTransitionColor;

    void Awake()
    {
        if(QualitySettings.vSyncCount == 0){
			vsyncActive = false;
		}
		else{
			vsyncActive = true;
		}
        GetCurrentScreenAtAwake();

        if(FindObjectOfType<SoundManager>() == null){
			//var soundManager= new GameObject().AddComponent<SoundManager>();
            //soundManager.gameObject.AddComponent<AudioSource>();
		    //soundManager.name = "SoundManager";
		}
    }

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
            LoadLanguage();
            StartCoroutine(FadeTitle(true));
            StartCoroutine(SetNewOptions(menuOptions));
            SoundManager.PlayBGM(bgm, true, 1);
            menuMode = 1;
        }
	}

    protected override void Update()
    {
        base.Update();
    }

    protected override void CheckSubmit(){
        //Language
        if(menuMode == 0){
            if (Input.GetButtonDown("Submit")  || (Input.GetButtonDown("A") || Input.GetButtonDown("Menu")))
            {
                //Portugues
                if(chooseThisOption == 0){
                    GameManager.Instance.Language = "Portuguese";
                }
                //EngLish
                else if(chooseThisOption == 1){
                    GameManager.Instance.Language = "English";
                }
                //Japanese
                else if(chooseThisOption == 2){
                    GameManager.Instance.Language = "Japanese";
                }

                LoadLanguage();

                StartCoroutine(SetNewOptions(menuOptions));
                StartCoroutine(FadeTitle(true));
                menuMode = 1;
                SoundManager.PlayBGM(bgm, true, 1);

                pressed = true;
            }
        }
        //Main Menu
        else if(menuMode == 1){
            if (Input.GetButtonDown("Submit") || (Input.GetButtonDown("A") || Input.GetButtonDown("Menu")))
            {
                //New Game
                if(chooseThisOption == 0){
                    Debug.Log("New Game");
                    GameManager.Instance.mainMenuMode = 1;
                    FindObjectOfType<TransitionManager>().SetColor(mainTransitionColor);
                    TransitionManager.Instance.LoadLevel(sceneToLoad, 1);
                }
                //Options
                else if(chooseThisOption == 1){
                    Debug.Log("Options");
                    menuMode = 2;
                    StartCoroutine(SetNewOptions(settingsOptions));
                    StartCoroutine(FadeTitle(false));
                }
                //Creits
                else if(chooseThisOption == 2){
                    menuMode = 3;
                    StartCoroutine(SetNewOptions(creditsOnlyOption));
                    StartCoroutine(ShowCredits(true));
                    StartCoroutine(FadeTitle(false));
                }
                //Quit
                else if(chooseThisOption == 3){
                    Application.Quit();
                }

                pressed = true;
            }
        }
        //Options
        else if(menuMode == 2){
            /********************************************** OPTIONS ******************************************/
            //Music Volume
            if(chooseThisOption == 0){
                if(Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0){
                    SoundManager.increaseBGMVolume();
                }
                else if(Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0){
                    SoundManager.decreaseBGMVolume();
                }
            }
            //FX Volume
            else if(chooseThisOption == 1){
                if(Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0){
                    SoundManager.increaseFXVolume();
                }
                else if(Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0){
                    SoundManager.decreaseFXVolume();
                }
            }
            //Screen Mode
            else if(chooseThisOption == 2){
                ScreenResolution();
                if(Input.GetButtonDown("Horizontal"))
				    isFullScreen = !isFullScreen;
            }
            //Resolution
            else if(chooseThisOption == 3){
                ScreenResolution();
                if(Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0){
                    if (currentResolution == 7)
                    {
                        currentResolution = 0;
                    }
                    else
                    {
                        currentResolution += 1;
                    }
                }
                else if(Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0){
                    if (currentResolution == 0)
                    {
                        currentResolution = 7;
                    }
                    else
                    {
                        currentResolution -= 1;
                    }
                }
            }
            //VSync
            else if(chooseThisOption == 4){
                if(Input.GetButtonDown("Horizontal")){
                    vsyncActive = !vsyncActive;
                    if(vsyncActive){
                        QualitySettings.vSyncCount = 1;
                    }
                    else if(!vsyncActive){
                        QualitySettings.vSyncCount = 0;
                    }
                }
            }
            
            //back to menu
            if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel") || (Input.GetButtonDown("A") || Input.GetButtonDown("B")) || Input.GetButtonDown("Menu"))
            {

                StartCoroutine(SetNewOptions(menuOptions));
                StartCoroutine(FadeTitle(true));
                menuMode = 1;

                pressed = true;
            }

            /********************************************** LABELS ******************************************/
            for (int i = 0; i < settingsOptions.Length; i++)
            {
                //opção do música de fundo
                if(i == 0){
                    int volume = (int)System.Math.Round(SoundManager.GetBGMVolume()*100, 2);
                    settingsOptions[i].transform.Find("Number").GetComponent<Text>().text = volume.ToString();
                }

                //opção de efeitos sonoros
                if(i == 1){
                    int volume = (int)System.Math.Round(SoundManager.GetSFXVolume()*100, 2);
                    settingsOptions[i].transform.Find("Number").GetComponent<Text>().text = volume.ToString();
                }

                //Opção full screen
                if (i == 2)
                {
                    if(isFullScreen){
                        settingsOptions[i].transform.Find("Text").GetComponent<Text>().text = Lang.Instance.getString("full_screen");
                    }
                    else{
                        settingsOptions[i].transform.Find("Text").GetComponent<Text>().text = Lang.Instance.getString("windowned");
                    }
                }

                //opção da resolução
                if(i == 3){
                    switch (currentResolution)
                    {
                        case 0:
                            settingsOptions[i].transform.Find("Text").GetComponent<Text>().text = "640 x 360";
                            break;
                        case 1:
                            settingsOptions[i].transform.Find("Text").GetComponent<Text>().text = "896 x 504";
                            break;
                        case 2:
                            settingsOptions[i].transform.Find("Text").GetComponent<Text>().text = "1024 x 576";
                            break;
                        case 3:
                            settingsOptions[i].transform.Find("Text").GetComponent<Text>().text = "1280 x 720";
                            break;
                        case 4:
                            settingsOptions[i].transform.Find("Text").GetComponent<Text>().text = "1360 x 765";
                            break;
                        case 5:
                            settingsOptions[i].transform.Find("Text").GetComponent<Text>().text = "1408 x 792";
                            break;
                        case 6:
                            settingsOptions[i].transform.Find("Text").GetComponent<Text>().text = "1600 x 900";
                            break;
                        case 7:
                            settingsOptions[i].transform.Find("Text").GetComponent<Text>().text = "1920 x 1080";
                            break;
                        default:
                            break;
                    }
                }

                //opção de vsync
                if(i == 4){
                    if(vsyncActive){
                        settingsOptions[i].transform.Find("Text").GetComponent<Text>().text = Lang.Instance.getString("on");
                    }
                    else if(!vsyncActive){
                        settingsOptions[i].transform.Find("Text").GetComponent<Text>().text = Lang.Instance.getString("off");
                    }
                }

                /*//Método de entrada
                if(i == 5){
                    if(inputMethod){
                        settingsOptions[i].transform.Find("Text").GetComponent<Text>().text = LMan.getString("joystick");
                    }
                    else if(!inputMethod){
                        settingsOptions[i].transform.Find("Text").GetComponent<Text>().text = LMan.getString("keyboard");
                    }
                }*/
            }
        }
        //Credits
        else if(menuMode == 3){
            if((Input.GetButtonDown("Cancel") || Input.GetButtonDown("Submit")) ||(Input.GetButtonDown("A") || Input.GetButtonDown("B")) || Input.GetButtonDown("Menu")){
                menuMode = 1;
                StartCoroutine(SetNewOptions(menuOptions));
                StartCoroutine(ShowCredits(false));
                StartCoroutine(FadeTitle(true));
            }
        }
    }

    IEnumerator ShowCredits(bool show){
        yield return new WaitForSeconds(0.5f);
        if(show){
            foreach(GameObject g in credits){
                g.SetActive(true);
                fade.FadeGameObject(g, 1, 0, 1);
                foreach(Transform t in g.transform){
                    t.gameObject.SetActive(true);
                    fade.FadeGameObject(t.gameObject, 1, 0, 1);
                }
            }
        }
        else{
            foreach(GameObject g in credits){
                fade.FadeGameObject(g, 1, 1, 0);
                foreach(Transform t in g.transform){
                    fade.FadeGameObject(t.gameObject, 1, 1, 0);
                }
            }
            yield return new WaitForSeconds(1);
            foreach(GameObject g in credits){
                g.SetActive(false);
                foreach(Transform t in g.transform){
                    t.gameObject.SetActive(false);
                }
            }
        }
    }

    void GetCurrentScreenAtAwake(){
		int s_width = Screen.width;
		/*
		0 - 640
		1 - 896
		2 - 1024 
		3 - 1280 
		4 - 1360
		5 - 1408
		6 - 1600
		7 - 1920
		*/
		switch (s_width)
		{
			case 640:
				currentResolution = 0;
				break;
			case 896:
				currentResolution = 1;
				break;
			case 1024:
				currentResolution = 2;
				break;
			case 1280:
				currentResolution = 3;
				break;
			case 1360:
				currentResolution = 4;
				break;
			case 1408:
				currentResolution = 5;
				break;
			case 1600:
				currentResolution = 6;
				break;
			case 1920:
				currentResolution = 7;
				break;
			default:
				break;
		}

		if(Screen.fullScreen){
			isFullScreen = true;
		}
		else{
			isFullScreen = false;
		}
	}

    void ScreenResolution(){
        /*
            currentResolution:
            0 - 640 x 360
            1 - 896 x 504
            2 - 1024 x 576
            3 - 1280 x 720
            4 - 1360 x 765
            5 - 1408 x 792
            6 - 1600 x 900
            7 - 1920 x 1080
        */
        /*Vector2[] resolutions = {
                                    new Vector2(640, 360), 
                                    new Vector2(896, 504), 
                                    new Vector2(1024, 576), 
                                    new Vector2(1280, 720), 
                                    new Vector2(1360, 765), 
                                    new Vector2(1408, 792), 
                                    new Vector2(1600, 900), 
                                    new Vector2(1920, 1080)
                                };
        */

        switch (currentResolution)
        {
            case 0:
                if(isFullScreen){
                    Screen.SetResolution(640, 360, true);
                    break;
                }
                else{
                    Screen.SetResolution(640, 360, false);
                    break;
                }
            case 1:
                if(isFullScreen){
                    Screen.SetResolution(896, 504, true);
                    break;
                }
                else{
                    Screen.SetResolution(896, 504, false);
                    break;
                }
            case 2:
                if(isFullScreen){
                    Screen.SetResolution(1024, 576, true);
                    break;
                }
                else{
                    Screen.SetResolution(1024, 576, false);
                    break;
                }
            case 3:
                if(isFullScreen){
                    Screen.SetResolution(1280, 720, true);
                    break;
                }
                else{
                    Screen.SetResolution(1280, 720, false);
                    break;
                }
            case 4:
                if(isFullScreen){
                    Screen.SetResolution(1360, 765, true);
                    break;
                }
                else{
                    Screen.SetResolution(1360, 765, false);
                    break;
                }
            case 5:
                if(isFullScreen){
                    Screen.SetResolution(1408, 792, true);
                    break;
                }
                else{
                    Screen.SetResolution(1408, 792, false);
                    break;
                }
            case 6:
                if(isFullScreen){
                    Screen.SetResolution(1600, 900, true);
                    break;
                }
                else{
                    Screen.SetResolution(1600, 900, false);
                    break;
                }
            case 7:
                if(isFullScreen){
                    Screen.SetResolution(1920, 1080, true);
                    break;
                }
                else{
                    Screen.SetResolution(1920, 1080, false);
                    break;
                }
            default:
                break;
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
        Lang.Instance.setLanguage(0, GameManager.Instance.Language);

        menuOptions[0].GetComponent<Text>().text = Lang.Instance.getString("start_game");
        menuOptions[1].GetComponent<Text>().text = Lang.Instance.getString("settings");
        menuOptions[2].GetComponent<Text>().text = Lang.Instance.getString("credits");
        menuOptions[3].GetComponent<Text>().text = Lang.Instance.getString("exit");

        settingsOptions[0].GetComponent<Text>().text = Lang.Instance.getString("bgm_volume");
        settingsOptions[1].GetComponent<Text>().text = Lang.Instance.getString("fx_volume");
        settingsOptions[2].GetComponent<Text>().text = Lang.Instance.getString("screen_mode");
        settingsOptions[3].GetComponent<Text>().text = Lang.Instance.getString("resolution");
        settingsOptions[4].GetComponent<Text>().text = Lang.Instance.getString("v_sync");

        creditsOnlyOption[0].GetComponent<Text>().text = "";

        credits[0].transform.GetChild(0).GetComponent<Text>().text = Lang.Instance.getString("credit_code");
        credits[1].transform.GetChild(0).GetComponent<Text>().text = Lang.Instance.getString("credit_code");
        credits[2].transform.GetChild(0).GetComponent<Text>().text = Lang.Instance.getString("credit_art") + "  /  " + Lang.Instance.getString("credit_sound");
        credits[3].transform.GetChild(0).GetComponent<Text>().text = Lang.Instance.getString("credit_art");
        credits[4].transform.GetChild(0).GetComponent<Text>().text = Lang.Instance.getString("credit_level");
        credits[5].transform.GetChild(0).GetComponent<Text>().text = Lang.Instance.getString("credit_level");
        credits[6].GetComponent<Text>().text = Lang.Instance.getString("credit_sfx");
    }
}
