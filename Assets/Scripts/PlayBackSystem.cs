using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayBackSystem : MonoBehaviour
{
	public static bool Paused { get; private set; }
	private PlayBackActions inputActions;
	public static bool DoAlgorithmStep { get; set; } = false;
	
	private void Start()
	{
		Paused = false;
	}
	
	private void Awake()
	{
		inputActions = new PlayBackActions();
	}
	
	private void OnEnable()
	{
		inputActions.Player.PausePlay.started += OnSpaceBarClicked;
		inputActions.Player.Step.started += OnDotClicked;
		inputActions.Enable();
	}

	private void OnDisable()
	{
		inputActions.Player.PausePlay.started -= OnSpaceBarClicked;
		inputActions.Player.Step.started -= OnDotClicked;
	}
	
	private void OnDotClicked(InputAction.CallbackContext context)
	{
		DoSingleStep();
	}
	
	private void OnSpaceBarClicked(InputAction.CallbackContext context)
	{
		TogglePause();
	}
	
	public void DoSingleStep()
	{
		DoAlgorithmStep = true;
	}

	public static void TogglePause()
	{
		DoAlgorithmStep = Paused;
		Paused = !Paused;
		if (Paused) { Time.timeScale = 0; }
		else { Time.timeScale = 1; }
	}
}
