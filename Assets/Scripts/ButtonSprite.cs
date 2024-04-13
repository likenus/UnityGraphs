using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSprite : MonoBehaviour
{
	public Sprite pauseSprite;
	public Sprite playSprite;
	public Image image;
	
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
