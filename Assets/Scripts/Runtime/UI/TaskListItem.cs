using UnityEngine;

public class TaskListItem
{
	public TaskListItem(Task.Status task)
	{
		this.task = task;
	}

	public void Show()
	{
		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal();
		GUILayout.Label(task.name);
		GUILayout.Label("" + (task.workPercentage * 100) + "%");
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		foreach (var item in task.programmers)
			GUILayout.Label(item.name);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	private readonly Task.Status task;
}
