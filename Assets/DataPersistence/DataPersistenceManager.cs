using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
	[Header("File Storage Config")]
	[SerializeField] private string settingFileName;

	public static DataPersistenceManager Instance { get; private set; }
	private const string graphDir = "graphs";
	private const string settingsDir = "settings";
	private readonly Regex regex = new("[a-zA-Z]+\\.json");
	private GameData gameData;
	private ProgrammData programmData;
	private List<IDataPersistence> dataPersistenceObjects;
	private FileDataHandler gameDataHandler;
	private FileDataHandler programmDataHandler;
	private string dirPath;
	private string _fileName;
	public string FileName
	{
		get { return _fileName; }
		set
		{
			if (!regex.IsMatch(value)) { _fileName = value + ".json"; }
			else { _fileName = value; }
			gameDataHandler = new FileDataHandler(Path.Combine(dirPath, graphDir), _fileName);
		}
	}

	private void Awake()
	{
		if (Instance != null)
			Debug.LogError("Found more than one DataPersistenceManager in the scene");
		Instance = this;
	}

	private void Start()
	{
		// First Load Settings
		programmData = new ProgrammData();
		dirPath = Path.Combine(Path.GetFullPath("."), "Data"); // Alternatively use Application.persistentFilePath
		programmDataHandler = new FileDataHandler(Path.Combine(dirPath, settingsDir), settingFileName);
		LoadSettings();
		
		// Then Load Game Data
		gameDataHandler = new FileDataHandler(Path.Combine(dirPath, graphDir), FileName); 
		dataPersistenceObjects = FindAllDataPersistenceObjects();
		LoadGame();
	}

	public void NewGame()
	{
		gameData = new GameData();
		foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
		{
			dataPersistenceObj.LoadData(gameData);
		}
		Debug.Log("Successfully loaded all game data.");
	}

	public void LoadGame()
	{
		// Load any saved data from file using data handler
		gameData = gameDataHandler.LoadGameData();

		// if no data can be loaded, initialize new game
		if (gameData == null)
		{
			Debug.Log("No Data was found. Initializing data to defaults.");
			NewGame();
		}

		// Push the Loaded data to all scripts that need it
		foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
		{
			dataPersistenceObj.LoadData(gameData);
		}
		Debug.Log("Successfully loaded all game data.");
	}
	
	public void LoadSettings()
	{
		programmData = programmDataHandler.LoadSettings();
		
		if (programmData == null)
		{
			FileName = "graph.json";
			return;
		}
		
		Debug.Log("Loaded " + programmData.fileName);
		FileName = programmData.fileName;
		Debug.Log("Successfully loaded all settings.");
	}

	public void SaveGame()
	{
		// Pass data to other scripts so they can update it
		foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
		{
			dataPersistenceObj.SaveData(ref gameData);
		}

		// Save data to file using data handler
		gameDataHandler.Save(gameData);
		Debug.Log("Successfully saved all game data.");
	}
	
	public void SaveSettings()
	{
		programmData ??= new();
		programmData.fileName = FileName;
		// Save data to file using data handler
		programmDataHandler.Save(programmData);
		Debug.Log("Successfully saved all programm data.");
	}

	private void OnApplicationQuit()
	{
		SaveGame();
		SaveSettings();
	}

	private List<IDataPersistence> FindAllDataPersistenceObjects()
	{
		return new List<IDataPersistence>(FindObjectsOfType<MonoBehaviour>()
			.OfType<IDataPersistence>());
	}
}
