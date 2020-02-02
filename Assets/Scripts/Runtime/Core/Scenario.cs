using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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

	[System.Serializable]
	public struct Config
	{
		public string name;
		public Config_Dev[] devs;
		public Config_Task[] tasks;
		public Config_Mood neutralMood;
		public Config_Mood[] moods;
	}

	public struct TaskInfo
	{
		public Task task;
		public List<Dev> devs;
		public Action<Task> onAddToTask;
	}

	public struct Status
	{
		public Status(string name, List<Task> tasks, Dev[] devs, Action<Task> onAddToTask)
		{
			this.name = name;
			taskInfos = new TaskInfo[tasks.Count];
			for (int i = 0; i < taskInfos.Length; ++i)
			{
				taskInfos[i].task = tasks[i];
				taskInfos[i].devs = new List<Dev>();
				for (int j = 0; j < devs.Length; ++j)
					if (devs[j].GetStatus().task == tasks[i])
						taskInfos[i].devs.Add(devs[j]);
				taskInfos[i].onAddToTask = onAddToTask;
			}

			this.devs = new Dev.Status[devs.Length];
			for (int i = 0; i < this.devs.Length; ++i)
				this.devs[i] = devs[i].GetStatus();
		}

		public string name;
		public TaskInfo[] taskInfos;
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
		}
	}

	public int EnterDev()
	{
		if (devsInGame >= activeDevs.Length)
			return 0;

		activeDevs[devsInGame].Enter();
		++devsInGame;
		return activeDevs.Length - devsInGame;
	}

	public int ExitDev()
	{
		if (devsInGame <= 0)
			return 0;
		--devsInGame; 	
		activeDevs[devsInGame].Exit();
		return devsInGame;
	}


	//internal void EndDay()
	//{
	//	throw new NotImplementedException();
	//}

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

	public void Tick()
	{
		foreach (var item in activeDevs)
			item.Tick();

		// check win?
	}

	public Status GetStatus()
		=> new Status(config.name, activeTasks, activeDevs, AddSelectedToTask);


	public Mood GetNeutralMood()
		=> new Mood(config.neutralMood.config);


	private void AddSelectedToTask(Task task)
	{
		SetToTask(manager.GetSelected(), task);
	}

	private Manager manager;
	private Config config;
	private Dev[] activeDevs;
	private List<Task> activeTasks;
	internal int daysLeft;

	private int devsInGame = 0;
	public bool areDevsMoving 
	{ 
		get 
		{
			foreach(var item in activeDevs)
			{
				if (item.avatar.isMoving)
					return true;
			}
			return false;
		}
	}
}
