using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDisplay : MonoBehaviour
{

	public void Tick(Scenario.Status scenario)
	{
		if (featureList)
			featureList.Clear();
		if (bugList)
			bugList.Clear();
		if (doneList)
			doneList.Clear();

		foreach(var item in scenario.taskInfos)
		{
			Task.Status task = item.task.GetStatus();
			if(task.isDone)
			{
				if (doneList)
					doneList.Add(item);
			}

			else /*if(task.type == TaskType.Feature)*/
			{
				if (featureList)
					featureList.Add(item);
			}
			//else if(task.type == TaskType.Bug)
			//{

			//}
		}
	}

	[SerializeField]
	private TaskList featureList;
	[SerializeField]
	private TaskList bugList;
	[SerializeField]
	private TaskList doneList;
}
