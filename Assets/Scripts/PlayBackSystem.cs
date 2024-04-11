using UnityEngine;

public class PlayBackSystem : MonoBehaviour
{
	public static bool Paused { get; private set; }
	
	private void Start()
	{
		Paused = false;
	}
	
	public void TogglePause()
	{
		Paused = !Paused;
	}
	
	public void DoPhysicsStep()
	{
		// How tf do I do this?
	}
}
