using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInput : MonoBehaviour
{
	private void OnGUI()
	{
		GUILayout.BeginArea(area);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Set To Work") && manager)
			manager.GetScenario().SetSteveToFeature();

		if (GUILayout.Button("Set To Fixing") && manager)
			manager.GetScenario().SetSteveToFixing();

		if (GUILayout.Button("Set to Relax"))
			manager.GetScenario().SetSteveToRelax();

		if (GUILayout.Button("Set To Idle"))
			manager.GetScenario().SetSteveToIdle();

		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	private void SetToWork() { }

	private void SetToFixing() { }

	private void SetToRelax() { }

	public Rect area;
	public Manager manager;
}
