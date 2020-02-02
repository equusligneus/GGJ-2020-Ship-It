using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AvatarSelector : MonoBehaviour
{
    private void Awake()
    {
		camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
		// todo check for intersection with screen;
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		var hit = Physics2D.GetRayIntersection(ray);

		Avatar newHovered = hit.collider ? hit.collider.GetComponent<Avatar>() : null;
		if(newHovered != hovered)
		{
			hovered = newHovered;
			onAvatarHovered?.Invoke(hovered);
		}

		// todo remove that check again hovered
		Avatar newSelected = Input.GetMouseButtonDown(0) ? (hovered ? hovered : selected) : (Input.GetMouseButtonDown(1) ? null : selected);
		if(newSelected != selected)
		{
			selected = newSelected;
			onAvatarSelected?.Invoke(selected);
		}
    }

	private new Camera camera;
	public Avatar hovered { get; private set; }
	public Avatar selected { get; private set; }

	public event Action<Avatar> onAvatarSelected;
	public event Action<Avatar> onAvatarHovered;
}
