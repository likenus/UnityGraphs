using UnityEngine;

public class EdgeTip : MonoBehaviour
{
	public Transform body;
	public float offset = 0.5f;

	private void Update()
	{
		Vector3 anchor = body.position + body.up * (body.localScale.y - offset);
		transform.SetPositionAndRotation(anchor, body.rotation);
	}
}
