using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public AudioClip mainTheme;
	public AudioClip menuTheme;

	void Start(){
		AudioManager.instance.PlayMusic (menuTheme, 2);
		//para colocar audio ""public AudioClip"" 
		//y luego colocar AudioManager.instance.playSound/music(""audio"",transform.position)en donde se ejecute;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
