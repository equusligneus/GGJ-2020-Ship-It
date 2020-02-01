using System.Collections.Generic;
using UnityEngine;

public class TaskList : MonoBehaviour
{
    public void Tick(Scenario.Status scenarioStatus)
	{
		tasks.Clear();
		bugs.Clear();
		done.Clear();

		foreach (var item in scenarioStatus.tasks)
		{
			var uiItem = new TaskListItem(item);

			if (item.isDone)
			{
				done.Add(uiItem);
			}
			else
			{
				switch (item.type)
				{
					case TaskType.Feature:
						tasks.Add(uiItem);
						break;
					case TaskType.Bug:
						bugs.Add(uiItem);
						break;
					case TaskType.Idle:
						none = uiItem;
						break;
					case TaskType.Relaxing:
						relaxing = uiItem;
						break;
					default:
						break;
				}
			}
		}
	}

	public void OnGUI()
	{
		GUI.Box(area, "");
		GUILayout.BeginArea(area);

		scrollPos = GUILayout.BeginScrollView(scrollPos);

		GUILayout.Label("Tasks");
		foreach(var item in tasks)
			item.Show();

		GUILayout.Label("Bugs");
		foreach (var item in bugs)
			item.Show();

		GUILayout.Label("Others");
		relaxing?.Show();
		none?.Show();

		GUILayout.Label("Done");
		foreach (var item in done)
			item.Show();

		GUILayout.EndScrollView();

		GUILayout.EndArea();
	}

	public Rect area;
	public Vector2 scrollPos;

	public List<TaskListItem> tasks = new List<TaskListItem>();

	public List<TaskListItem> bugs = new List<TaskListItem>();

	public TaskListItem relaxing = null;

	public TaskListItem none = null;

	public List<TaskListItem> done = new List<TaskListItem>();


}
