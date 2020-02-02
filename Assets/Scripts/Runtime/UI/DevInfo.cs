using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class DevInfo : MonoBehaviour
{
	private void Awake()
		=> rectTrans = GetComponent<RectTransform>();

	// Update is called once per frame
	void Update()
    {
		visual.SetActive(selector.hovered);
		if(selector.hovered)
		{
			var status = selector.hovered.owner.GetStatus();
			motivation.value = status.motivation;
			focus.value = status.focus;

			var pos = rectTrans.localPosition;
			pos.x = Input.mousePosition.x - Screen.width / 2;
			pos.y = Input.mousePosition.y - Screen.height / 2;
			rectTrans.localPosition = pos;
		}
    }

	// TODO set reference otherwise
	[SerializeField]
	private AvatarSelector selector;

	[SerializeField]
	private GameObject visual;
	[SerializeField]
	private Slider motivation;
	[SerializeField]
	private Slider focus;

	private RectTransform rectTrans;
}
