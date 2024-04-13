using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSprite : MonoBehaviour
{
	public Sprite pauseSprite;
	public Sprite playSprite;
	public Image image;
	private PlayBackActions inputAction;
	
	private void Awake()
	{
		inputAction = new PlayBackActions();
	}
	
	private void OnEnable()
	{
		inputAction.Player.PausePlay.started += OnSpaceBarClicked;
		inputAction.Enable();
	}

    private void OnDisable()
	{
		inputAction.Player.PausePlay.started -= OnSpaceBarClicked;
	}

    private void OnSpaceBarClicked(InputAction.CallbackContext context)
    {
		SwapSprites();
    }
	
	public void SwapSprites()
	{
		if (PlayBackSystem.Paused)
		{
			image.sprite = playSprite;
		}
		if (!PlayBackSystem.Paused)
		{
			image.sprite = pauseSprite;
		}
	}
}
