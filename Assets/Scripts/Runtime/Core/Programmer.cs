using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Programmer
{
#if UNITY_EDITOR
	public static Programmer CreateSteve()
	{
		Programmer steve = new Programmer();
		steve.config.name = "Steve";
		steve.config.maxSkill = 15;
		steve.config.maxSpeed = 1;
		steve.config.maxAnnoyance = 250;
		return steve;
	}

#endif


	[System.Serializable]
	public struct Config
	{
		public string name;
		public float maxSkill;
		public float maxSpeed;
		public float maxAnnoyance;
	}

	public struct Status
	{
		public Status(Config config, Task task)
		{
			name = config.name;
			annoyance = 0;
			attention = 0;
			currentTaskType = task.GetTaskType();
		}

		public string name;
		public float annoyance;
		public float attention;
		public TaskType currentTaskType;
	}

	public Programmer() { }

	private Programmer(Programmer programmer)
	{
		config = programmer.config;
	}

	public Programmer Clone()
		=> new Programmer(this);

	public void StartTick(bool isNewDay)
	{
		
	}

	public void EndTick(Scenario scenario)
	{
		currentTask.AddProgress(config.maxSpeed);
		
		// TODO do some logic regarding this!!!
		if (currentTask.IsDone)
			scenario.SetSteveToIdle();
	}

	public Status GetStatus()
	{
		return new Status(config, currentTask);
	}

	[SerializeField]
	private Config config;

	public Task currentTask;
}
