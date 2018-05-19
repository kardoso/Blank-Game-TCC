using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Menu 
{
    public GameObject[] menuOptions;
    public GameObject[] settingsOptions;

    int menuMode;       //0 is menu, 1 is settings

    private int currentResolution;
	private bool isFullScreen;
	private bool vsyncActive;

    public string sceneToLoad;

	private float currentTimeScale;

    void Awake()
    {
        if(QualitySettings.vSyncCount == 0){
			vsyncActive = false;
		}
		else{
			vsyncActive = true;
		}
        GetCurrentScreenAtAwake();
    }

	protected override void Start()
    {
        base.Start();

        var list = new List<GameObject>();
        list.AddRange(menuOptions);
        list.AddRange(settingsOptions);

        GameObject[] allOptions = list.ToArray();

        foreach(GameObject g in allOptions){
            g.GetComponent<Text>().color = optionNotSelected;
        }

		StartCoroutine(SetNewOptions(menuOptions));
	}

    protected override void Update()
    {
        base.Update();
    }

    protected override void CheckSubmit(){
        //Menu
        if(menuMode == 0){
            if (Input.GetButtonDown("Submit"))
            {
                //Resume
                if(chooseThisOption == 0){
                    Debug.Log("Resume");
                    this.gameObject.SetActive(false);
                }
                //Options
                else if(chooseThisOption == 1){
                    Debug.Log("Options");
                    StartCoroutine(SetNewOptions(settingsOptions));
                    menuMode = 1;
                }
                //Quit
                else if(chooseThisOption == 2){
					Time.timeScale = 1;
                    TransitionManager.Instance.LoadLevel(sceneToLoad, 1);
                }

                pressed = true;
            }

			if(Input.GetButtonDown("Cancel")){
				Debug.Log("Resume");
				this.gameObject.SetActive(false);
			}
        }
        //Options
        else if(menuMode == 1){
            /********************************************** OPTIONS ******************************************/
            //Music Volume
            if(chooseThisOption == 0){
                if(Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0){
                    SoundManager.Instance.increaseBGMVolume();
                }
                else if(Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0){
                    SoundManager.Instance.decreaseBGMVolume();
                }
            }
            //FX Volume
            else if(chooseThisOption == 1){
                if(Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0){
                    SoundManager.Instance.increaseFXVolume();
                }
                else if(Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0){
                    SoundManager.Instance.decreaseFXVolume();
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
            if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel"))
            {

                StartCoroutine(SetNewOptions(menuOptions));
                menuMode = 0;

                pressed = true;
            }

            /********************************************** LABELS ******************************************/
            for (int i = 0; i < settingsOptions.Length; i++)
            {
                //opção do música de fundo
                if(i == 0){
                    int volume = (int)System.Math.Round(SoundManager.Instance.GetBGMVolume()*100, 2);
                    settingsOptions[i].transform.Find("Number").GetComponent<Text>().text = volume.ToString();
                }

                //opção de efeitos sonoros
                if(i == 1){
                    int volume = (int)System.Math.Round(SoundManager.Instance.GetSFXVolume()*100, 2);
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

    void LoadLanguage(){
        Lang.Instance.setLanguage(0, GameManager.Instance.Language);

        menuOptions[0].GetComponent<Text>().text = Lang.Instance.getString("resume");
        menuOptions[1].GetComponent<Text>().text = Lang.Instance.getString("settings");
        menuOptions[2].GetComponent<Text>().text = Lang.Instance.getString("main_menu");

        settingsOptions[0].GetComponent<Text>().text = Lang.Instance.getString("bgm_volume");
        settingsOptions[1].GetComponent<Text>().text = Lang.Instance.getString("fx_volume");
        settingsOptions[2].GetComponent<Text>().text = Lang.Instance.getString("screen_mode");
        settingsOptions[3].GetComponent<Text>().text = Lang.Instance.getString("resolution");
        settingsOptions[4].GetComponent<Text>().text = Lang.Instance.getString("v_sync");
    }

	void OnEnable()
	{
		FindObjectOfType<LevelManager>().CanPause = false;
		currentTimeScale = Time.timeScale;
		LoadLanguage();
		pressed = false;
		Time.timeScale = 0f;
	}

	void OnDisable()
	{
		pressed = true;
		Time.timeScale = currentTimeScale;
		FindObjectOfType<LevelManager>().CanPause = true;
	}
}
