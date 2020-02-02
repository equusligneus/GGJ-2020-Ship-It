using System.Collections.Generic;
using UnityEngine;

public class TaskList : MonoBehaviour
{
	public void Clear()
	{
		foreach (var item in items)
			item.gameObject.SetActive(false);
		elementIndex = 0;	
	}
	
	public void Add(Scenario.TaskInfo taskInfo)
	{
		if (elementIndex >= items.Count)
			items.Add(Instantiate(prefab, listRoot, false));

		items[elementIndex].Tick(taskInfo);
		items[elementIndex].gameObject.SetActive(true);
		++elementIndex;
	}

	[SerializeField]
	private TaskListItem prefab;
	[SerializeField]
	private RectTransform listRoot;

	private int elementIndex;
	private List<TaskListItem> items = new List<TaskListItem>();
}
