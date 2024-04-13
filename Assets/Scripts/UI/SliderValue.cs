using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
	public Slider slider;
	public TextMeshProUGUI text;

	public void OnSliderUpdate()
	{
		text.text = string.Format("{0:f2}", slider.value);
	}
}
