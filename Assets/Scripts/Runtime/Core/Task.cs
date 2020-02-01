using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
	Idle,
	Feature,
	Bug,
	Relaxing,
}

[System.Serializable]
public class Task
{
#if UNITY_EDITOR

	public static Task CreateUIFeature()
	{
		Task feature = new Task();
		feature.config.name = "Awesome UI";
		feature.config.type = TaskType.Feature;
		feature.config.difficulty = 6;
		feature.config.work = 42;
		feature.config.annoyance = 9;
		feature.config.maxProgrammerNum = 1;
		return feature;
	}

	public static Task CreateUIBug()
	{
		Task bug = new Task();
		bug.config.name = "Awesome UI no worky";
		bug.config.type = TaskType.Bug;
		bug.config.difficulty = 0;
		bug.config.work = 42;
		bug.config.annoyance = 9000;
		bug.config.maxProgrammerNum = 1;
		return bug;
	}

	public static Task CreateRelax()
	{
		Task relax = new Task();
		relax.config.name = "Play Mario Kart";
		relax.config.type = TaskType.Relaxing;
		relax.config.difficulty = 0;
		relax.config.work = 9000;
		relax.config.annoyance = -25;
		relax.config.maxProgrammerNum = -1;
		return relax;
	}

	public static Task CreateIdle()
	{
		Task idle = new Task();
		idle.config.name = "Idle";
		idle.config.type = TaskType.Idle;
		idle.config.difficulty = 0;
		idle.config.work = 9000;
		idle.config.annoyance = 0;
		idle.config.maxProgrammerNum = -1;
		return idle;
	}


#endif

	[System.Serializable]
	public struct Config
	{
		public string name;
		public TaskType type;
		public float work;
		public float difficulty;
		public float annoyance;
		public int maxProgrammerNum;
	}

	public struct Status
	{
		public Status(Config config, float currentWork, Programmer.Status[] programmers)
		{
			name = config.name;
			type = config.type;
			workPercentage = currentWork / config.work;
			isDone = workPercentage >= 1.0f;
			this.programmers = programmers;
		}

		public readonly string name;
		public readonly TaskType type;
		public float workPercentage;
		public bool isDone;
		public Programmer.Status[] programmers;
	}


	public Task() { }

	private Task(Task task)
	{
		config = task.config;
	}

	public Task Clone()
		=> new Task(this);

	public TaskType GetTaskType()
		=> config.type;

	[SerializeField]
	private Config config;

	public float CurrentWork { get; private set; }

	public bool IsDone
		=> CurrentWork >= config.work;

	float bugPotential;

	public void StartDay()
	{

	}

	public void AddProgress(float progress)
	{
		if (config.type == TaskType.Idle || config.type == TaskType.Relaxing)
			return;

		CurrentWork += progress;
	}

	public void EndDay()
	{

	}

	public Status GetStatus(Programmer[] programmers)
	{
		List<Programmer.Status> involvedProgrammers = new List<Programmer.Status>();
		if(!IsDone)
		{
			foreach(var item in programmers)
			{
				if (item.currentTask == this)
					involvedProgrammers.Add(item.GetStatus());
			}
		}

		return new Status(config, CurrentWork, involvedProgrammers.ToArray());
	}
}
