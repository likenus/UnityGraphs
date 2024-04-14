using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public Canvas gameUI;
	public Canvas pauseUI;
	public Canvas saveUI;
	public Canvas loadUI;
	public Transform content;
	public GameObject scrollPrefab;
	public static bool Paused { get; private set; }
	public TMP_InputField textPrompt;
	public InputController inputManager;
	public CameraController cameraController;
	private PauseMenuActions inputActions;

	private void Start()
	{
		Paused = false;
		gameUI.enabled = true;
		pauseUI.enabled = saveUI.enabled = loadUI.enabled = false;
		InitializeLoadMenuContent();
	}

	private void Awake()
	{
		inputActions = new PauseMenuActions();
	}

	private void OnEnable()
	{
		inputActions.Player.PauseMenu.started += ToggleMenu;
		inputActions.Enable();
	}

	private void ToggleMenu(InputAction.CallbackContext context)
	{
		TogglePauseMenu();
	}

	public void TogglePauseMenu()
	{
		PlayBackSystem.TogglePause();
		Paused = PlayBackSystem.Paused;

		cameraController.enabled = !Paused;
		inputManager.gameObject.SetActive(!Paused);
		gameUI.enabled = !Paused;
		pauseUI.enabled = Paused;
		saveUI.enabled = loadUI.enabled = false;
	}

	public void ToggleSaveMenu()
	{
		pauseUI.enabled = !pauseUI.enabled;
		saveUI.enabled = !saveUI.enabled;
	}

	public void ToggleLoadMenu()
	{
		pauseUI.enabled = !pauseUI.enabled;
		loadUI.enabled = !loadUI.enabled;
	}

	private void OnDisable()
	{
		inputActions.Player.PauseMenu.started -= ToggleMenu;
	}

	public void ResetGraph()
	{
		inputManager.graph.Clear();
		DataPersistenceManager.Instance.NewGame();
		DataPersistenceManager.Instance.FileName = "";
		DataPersistenceManager.Instance.LoadGame();
	}

	public void SaveGraph()
	{
		string fileName = textPrompt.text;
		if (fileName.Length == 0)
		{
			Debug.LogError("Invalid filename.");
			return;
		}

		DataPersistenceManager.Instance.FileName = fileName;
		DataPersistenceManager.Instance.SaveGame();
		ToggleSaveMenu();
		UpdateLoadMenuContent(fileName);
	}

	private List<string> LoadSavedFiles()
	{
		string fullPath = Path.Combine(Path.GetFullPath("."), "Data", "graphs");
		if (Directory.Exists(fullPath))
		{
			try
			{
				FileInfo[] fileInfos = new DirectoryInfo(fullPath).GetFiles();
				return fileInfos.Select(f => f.Name).ToList();
			}
			catch (Exception e)
			{
				Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
			}
		}
		return new List<string>();
	}

	public void OnLoadClick(string fileName)
	{
		DataPersistenceManager.Instance.SaveGame();
		DataPersistenceManager.Instance.FileName = fileName;
		DataPersistenceManager.Instance.LoadGame();
		TogglePauseMenu();
	}

	public void InitializeLoadMenuContent()
	{
		List<string> savedFiles = LoadSavedFiles();
		foreach (string fileName in savedFiles)
		{
			GameObject gameObject = Instantiate(scrollPrefab, content);
			TMP_Text text = gameObject.GetComponentInChildren<TMP_Text>();
			Button button = gameObject.GetComponent<Button>();
			button.onClick.AddListener(delegate { OnLoadClick(fileName); });
			text.text = Path.GetFileNameWithoutExtension(fileName);
		}
	}

	private void UpdateLoadMenuContent(string fileName)
	{
		GameObject gameObject = Instantiate(scrollPrefab, content);
		TMP_Text text = gameObject.GetComponentInChildren<TMP_Text>();
		Button button = gameObject.GetComponent<Button>();
		button.onClick.AddListener(delegate { OnLoadClick(fileName); });
		text.text = Path.GetFileNameWithoutExtension(fileName);
	}
}
