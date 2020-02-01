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

	public static Scenario CreateScenario()
	{
		Scenario scenario = new Scenario();
		scenario.config.name = "Work work";
		scenario.config.programmers = new Programmer[1] { Programmer.CreateSteve() };
		scenario.config.tasks = new Task[4] { Task.CreateUIFeature(), Task.CreateUIBug(), Task.CreateRelax(), Task.CreateIdle() };

		return scenario;
	}

	public void SetSteveToFeature()
		=> SetToTask(activeProgrammers[0], activeTasks.FirstOrDefault(t => t.GetTaskType() == TaskType.Feature));

	public void SetSteveToFixing()
		=> SetToTask(activeProgrammers[0], activeTasks.FirstOrDefault(t => t.GetTaskType() == TaskType.Bug));

	public void SetSteveToRelax()
		=> SetToTask(activeProgrammers[0], activeTasks.FirstOrDefault(t => t.GetTaskType() == TaskType.Relaxing));

	public void SetSteveToIdle()
		=> SetToTask(activeProgrammers[0], activeTasks.FirstOrDefault(t => t.GetTaskType() == TaskType.Idle));
#endif

	[System.Serializable]
	public struct Config
	{
		public string name;
		[SerializeReference]
		public Programmer[] programmers;
		[SerializeReference]	
		public Task[] tasks;
	}

	public struct Status
	{
		public Status(string name, List<Task> tasks, Programmer[] programmers)
		{
			this.name = name;
			this.tasks = new Task.Status[tasks.Count];
			for (int i = 0; i < this.tasks.Length; ++i)
				this.tasks[i] = tasks[i].GetStatus(programmers);

			this.programmers = new Programmer.Status[programmers.Length];
			for (int i = 0; i < this.programmers.Length; ++i)
				this.programmers[i] = programmers[i].GetStatus();
		}

		public string name;
		public Task.Status[] tasks;
		public Programmer.Status[] programmers;
	}

	public void StartScenario()
	{
		activeProgrammers = new Programmer[config.programmers.Length];
		for (int i = 0; i < config.programmers.Length; ++i)
			activeProgrammers[i] = config.programmers[i].Clone();

		activeTasks = new List<Task>();
		foreach (var item in config.tasks)
			activeTasks.Add(item.Clone());

		// TODO FIX!!!
		SetSteveToIdle();
	}

	public void SetToTask(Programmer programmer, Task target)
	{
		if(programmer == null)
		{
			Debug.LogError("Programmer not found!");
			return;
		}

		if(target == null)
		{
			Debug.LogError("Task not found!");
			return;
		}

		programmer.currentTask = target;
	}

	public void Tick(bool isNewDay)
	{
		foreach (var item in activeProgrammers)
		{
			item.EndTick(this);
			item.StartTick(isNewDay);
		}

		// check win?
	}

	public Status GetStatus()
		=> new Status(config.name, activeTasks, activeProgrammers);

	[SerializeField]
	private Config config;

	private Programmer[] activeProgrammers;
	//TODO change
	private List<Task> activeTasks;

}
