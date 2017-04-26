using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class GameOver : MonoBehaviour {

	//public Texture gameOverTexture;

	void OnGUI()
	{
		//GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),gameOverTexture);
		if (GUI.Button(new Rect(Screen.width / 2 - 200, Screen.height /2 - 200, 400, 200),"Try Again")) 
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("WitchRunner");
			//Application.LoadLevel("WitchRunner");
		}
		if (GUI.Button(new Rect(Screen.width / 2 - 200, Screen.height /2, 400, 200),"Quit")) 
		{
			Application.Quit();
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
