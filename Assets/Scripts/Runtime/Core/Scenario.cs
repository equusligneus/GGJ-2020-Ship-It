using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
	using System.Linq; 
#endif

[System.Serializable]
public class Scenario
{
#if UNITY_EDITOR
	public void SetSteveToFeature()
		=> SetToTask(activeDevs[0], activeTasks.FirstOrDefault(t => t.GetTaskType() == TaskType.Feature));

	public void SetSteveToFixing()
		=> SetToTask(activeDevs[0], activeTasks.FirstOrDefault(t => t.GetTaskType() == TaskType.Bug));

	public void SetSteveToRelax()
		=> SetToTask(activeDevs[0], activeTasks.FirstOrDefault(t => t.GetTaskType() == TaskType.Relaxing));

	public void SetSteveToIdle()
		=> SetToTask(activeDevs[0], activeTasks.FirstOrDefault(t => t.GetTaskType() == TaskType.Idle));
#endif

	[System.Serializable]
	public struct Config
	{
		public string name;
		public Config_Dev[] devs;
		public Config_Task[] tasks;
	}

	public struct Status
	{
		public Status(string name, List<Task> tasks, Dev[] devs)
		{
			this.name = name;
			this.tasks = new Task.Status[tasks.Count];
			for (int i = 0; i < this.tasks.Length; ++i)
				this.tasks[i] = tasks[i].GetStatus(devs);

			this.devs = new Dev.Status[devs.Length];
			for (int i = 0; i < this.devs.Length; ++i)
				this.devs[i] = devs[i].GetStatus();
		}

		public string name;
		public Task.Status[] tasks;
		public Dev.Status[] devs;
	}

	public Scenario(Config config)
	{
		this.config = config;
	}

	public void StartScenario()
	{
		activeDevs = new Dev[config.devs.Length];
		for (int i = 0; i < config.devs.Length; ++i)
			activeDevs[i] = new Dev(config.devs[i].config);

		activeTasks = new List<Task>();
		foreach (var item in config.tasks)
			activeTasks.Add(new Task(item.config));

		// TODO FIX!!!
		SetSteveToIdle();
	}

	public void SetToTask(Dev Dev, Task target)
	{
		if(Dev == null)
		{
			Debug.LogError("Programmer not found!");
			return;
		}

		if(target == null)
		{
			Debug.LogError("Task not found!");
			return;
		}

		Dev.currentTask = target;
	}

	public void Tick(bool isNewDay)
	{
		foreach (var item in activeDevs)
		{
			item.EndTick(this);
			item.StartTick(isNewDay);
		}

		// check win?
	}

	public Status GetStatus()
		=> new Status(config.name, activeTasks, activeDevs);

	[SerializeField]
	private Config config;

	private Dev[] activeDevs;
	//TODO change
	private List<Task> activeTasks;

}
