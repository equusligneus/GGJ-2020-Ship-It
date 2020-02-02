using UnityEngine;
using UnityEngine.UI;

public class TaskListItem : MonoBehaviour
{
	public void Tick(Scenario.TaskInfo info)
	{
		var taskStatus = info.task.GetStatus();

		name.text = taskStatus.name;
		progress.value = taskStatus.workPercentage;
		for(int i = 0; i < programmers.Length; ++i)
		{
			bool show = i < info.devs.Count;
			programmers[i].enabled = show;
			if(show)		
				programmers[i].text = info.devs[i].GetStatus().name;
		}

		addButton.onClick.RemoveAllListeners();
		addButton.interactable = !taskStatus.isDone;
		addButton.onClick.AddListener(() => { info.onAddToTask?.Invoke(info.task); });
	}

	[SerializeField]
	private Text name;
	[SerializeField]
	private Slider progress;
	[SerializeField]
	private Text[] programmers;
	[SerializeField]
	private Button addButton;
}
