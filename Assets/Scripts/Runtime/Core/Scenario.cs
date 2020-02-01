using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
	using System.Linq; 
#endif

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

	public struct Config
	{
		public string name;
		public Config_Dev[] devs;
		public Config_Task[] tasks;
		public Config_Mood neutralMood;
		public Config_Mood[] moods;
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

	public Scenario(Config config, Manager manager)
	{
		this.config = config;
		this.manager = manager;
	}

	public void StartScenario()
	{
		activeTasks = new List<Task>();
		foreach (var item in config.tasks)
			activeTasks.Add(new Task(item.config));

		activeDevs = new Dev[config.devs.Length];
		for (int i = 0; i < config.devs.Length; ++i)
		{
			activeDevs[i] = new Dev(config.devs[i].config, this, manager);
			SetIdle(activeDevs[i]);
		}
	}

	public void SetIdle(Dev dev)
		=> SetToTask(dev, activeTasks.FirstOrDefault(t => t.GetTaskType() == TaskType.Idle));

	public void SetToTask(Dev dev, Task target)
	{
		if(dev == null)
		{
			Debug.LogError("Programmer not found!");
			return;
		}

		if(target == null)
		{
			Debug.LogError("Task not found!");
			return;
		}

		dev.TrySetTask(target);
	}

	public void Tick(bool isNewDay)
	{
		foreach (var item in activeDevs)
		{
			item.EndTick();
			item.StartTick(isNewDay);
		}

		// check win?
	}

	public Status GetStatus()
		=> new Status(config.name, activeTasks, activeDevs);


	public Mood GetNeutralMood()
		=> new Mood(config.neutralMood.config);


	private Manager manager;
	private Config config;
	private Dev[] activeDevs;
	private List<Task> activeTasks;

}
