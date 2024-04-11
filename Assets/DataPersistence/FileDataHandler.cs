using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FileDataHandler
{
	private readonly string dataDirPath;
	public string DataFileName { get; set; }

	public FileDataHandler(string dataDirPath, string dataFileName)
	{
		this.dataDirPath = dataDirPath;
		this.DataFileName = dataFileName;
	}

	public GameData LoadGameData()
	{
		string fullPath = Path.Combine(dataDirPath, DataFileName);
		GameData loadedData = null;
		if (File.Exists(fullPath))
		{
			try
			{
				string dataToLoad = "";
				using (FileStream stream = new(fullPath, FileMode.Open))
				{
					using (StreamReader reader = new(stream))
					{
						dataToLoad = reader.ReadToEnd();
					}
				}

				loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
			}
			catch (Exception e)
			{
				Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
			}
		}
		return loadedData;
	}
	
	public ProgrammData LoadSettings()
	{
		string fullPath = Path.Combine(dataDirPath, DataFileName);
		ProgrammData loadedData = null;
		if (File.Exists(fullPath))
		{
			try
			{
				string dataToLoad = "";
				using (FileStream stream = new(fullPath, FileMode.Open))
				{
					using (StreamReader reader = new(stream))
					{
						dataToLoad = reader.ReadToEnd();
					}
				}

				loadedData = JsonUtility.FromJson<ProgrammData>(dataToLoad);
			}
			catch (Exception e)
			{
				Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
			}
		}
		return loadedData;
	}

	public void Save(GameData data)
	{
		string fullPath = Path.Combine(dataDirPath, DataFileName);
		try
		{
			// create the directory if it doesnt exist
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

			// serialize C# game data object to Json
			string dataToStore = JsonUtility.ToJson(data, true);

			// write serialized data to file
			using (FileStream stream = new(fullPath, FileMode.Create))
			{
				using (StreamWriter writer = new(stream))
				{
					writer.Write(dataToStore);
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogError("Error occured while trying to save data to file: " + fullPath + "\n" + e);
		}
	}
	
	public void Save(ProgrammData data)
	{
		string fullPath = Path.Combine(dataDirPath, DataFileName);
		try
		{
			// create the directory if it doesnt exist
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

			// serialize C# game data object to Json
			string dataToStore = JsonUtility.ToJson(data, true);

			// write serialized data to file
			using (FileStream stream = new(fullPath, FileMode.Create))
			{
				using (StreamWriter writer = new(stream))
				{
					writer.Write(dataToStore);
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogError("Error occured while trying to save data to file: " + fullPath + "\n" + e);
		}
	}
}
