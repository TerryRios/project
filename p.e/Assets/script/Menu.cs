using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public GameObject mainMenuHolder;
	public GameObject optionsMenuHolder;
	public Dropdown resolutionDropdown;

	public Slider[] volumeSliders;
	public Resolution[] resolutions;
	public Toggle[] resolutionToggles;
	public int[] screenWidths;
	int activeScreenResIndex;

	void Start(){
		resolutions = Screen.resolutions;
		resolutionDropdown.ClearOptions ();
		List<string> options = new List<string> ();
		int currentResolutionIndex = 0;
		for (int i = 0; i < resolutions.Length; i++) {
			string option = resolutions [i].width + " x " + resolutions[i].height;
			options.Add (option);
			if (resolutions[i].width == Screen.currentResolution.width&&
				resolutions[i].height == Screen.currentResolution.height) {
				currentResolutionIndex = i;
			}
		}

		resolutionDropdown.AddOptions (options);
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue ();

		activeScreenResIndex = PlayerPrefs.GetInt("screen res index");
		bool isFullscreen = (PlayerPrefs.GetInt("fullscreen") == 1)?true:false;

		volumeSliders [0].value = AudioManager.instance.masterVolumePercent;
		volumeSliders [1].value = AudioManager.instance.musicVolumePercent;
		volumeSliders [2].value = AudioManager.instance.sfxVolumePercent;

		for (int i = 0; i < resolutionToggles.Length; i++) {
			resolutionToggles [i].isOn = i == activeScreenResIndex;
		}
		SetFullscreen (isFullscreen);
	}

	public void Play(){
		SceneManager.LoadScene ("1");
	}

	public void Quit(){
		Application.Quit ();
	}

	public void OptionsMenu(){
		mainMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (true);
	}
	public void MainMenu(){
		mainMenuHolder.SetActive (true);
		optionsMenuHolder.SetActive (false);
	}
	public void SetScreenResolution(int i){
		if (resolutionToggles[i].isOn) {
			activeScreenResIndex = i;
			float aspectRatio = 16 / 9f;
			Screen.SetResolution(screenWidths[i],(int)(screenWidths[i]/aspectRatio),false);
			PlayerPrefs.SetInt ("screen res index", activeScreenResIndex);
			PlayerPrefs.Save ();
		}		
	}
	public void SetFullscreen(bool isFullscreen){
		for (int i = 0; i < resolutionToggles.Length; i++) {
			resolutionToggles [i].interactable = !isFullscreen;
		}
		if (isFullscreen) {
			Resolution[] allResolutions = Screen.resolutions;
			Resolution maxResolution = allResolutions [allResolutions.Length - 1];
			Screen.SetResolution (maxResolution.width, maxResolution.height, true);
		} else {
			SetScreenResolution (activeScreenResIndex);
		}
		PlayerPrefs.SetInt ("fullscreen", ((isFullscreen) ? 1 : 0));
		PlayerPrefs.Save ();
	}
	public void SetMasterVolume(float value){
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Master);		
	}
	public void SetMusicVolume(float value){
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Music);	
	}
	public void SetSfxVolume(float value){
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Sfx);	
	}
	public void SetResolution(int resolutionIndex){
		Resolution resolution = resolutions [resolutionIndex];
		Screen.SetResolution (resolution.width, resolution.height, Screen.fullScreen);
	}

}
