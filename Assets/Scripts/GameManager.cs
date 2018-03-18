using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {

	public int mainMenuMode = 0;	//0 if first initialization, 1 if the game is over
	private string language;
    public string Language
    {
        get
        {
            return language;
        }

        set
        {
            language = value;
        }
    }
}
