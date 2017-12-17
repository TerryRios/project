using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveAndLoad : MonoBehaviour {

	public static PlayerClass playerclass;

	public string SAVE_FILE = "/SAVEGAME";
	public string FILE_EXTENSION = ".TER";

	public string _playername;
	public int _level;

	private void Awake(){
		playerclass = new PlayerClass ();
	}

	public void SaveData(){

		Stream stream = File.Open (Application.dataPath + SAVE_FILE + FILE_EXTENSION, FileMode.OpenOrCreate);
		BinaryFormatter bf = new BinaryFormatter ();

		playerclass.Playername = _playername;
		playerclass.level = _level;

		bf.Serialize (stream, playerclass);
		stream.Close ();
	}

	public void LoadData(){

		Stream stream = File.Open (Application.dataPath + SAVE_FILE + FILE_EXTENSION, FileMode.OpenOrCreate);
		BinaryFormatter bf = new BinaryFormatter ();

//		List<listname> name = (List<listname>)bf.Deserialize (stream);
		playerclass = (PlayerClass)bf.Deserialize (stream);
		stream.Close ();

		_playername = playerclass.Playername;
		_level = playerclass.level;
	}
}
