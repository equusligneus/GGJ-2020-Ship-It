using UnityEngine;

public class DebugInput : MonoBehaviour
{
#if UNITY_EDITOR
	private void OnGUI()
	{
		GUILayout.BeginArea(area);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Set To Work") && manager)
			manager.scenario.SetSteveToFeature();

		if (GUILayout.Button("Set To Fixing") && manager)
			manager.scenario.SetSteveToFixing();

		if (GUILayout.Button("Set to Relax"))
			manager.scenario.SetSteveToRelax();

		if (GUILayout.Button("Set To Idle"))
			manager.scenario.SetSteveToIdle();

		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	public Rect area;
	public Manager manager; 
#endif
}
