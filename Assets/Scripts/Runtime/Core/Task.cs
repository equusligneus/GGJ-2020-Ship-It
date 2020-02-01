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
	[System.Serializable]
	public struct Config
	{
		public string name;
		public TaskType type;
		public int importance;
		public float difficulty;
		public float work;
		public float annoyance;
		public int maxProgrammerNum;
	}

	public struct Status
	{
		public Status(Config config, float currentWork, Dev.Status[] programmers)
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
		public Dev.Status[] programmers;
	}

	public Task(Config config)
	{
		this.config = config;
	}

	public TaskType GetTaskType()
		=> config.type;

	[SerializeField]
	private Config config;

	public float CurrentWork { get; private set; }

	public bool IsDone
		=> CurrentWork >= config.work;

	float bugPotential;

// moving to Scheduler
/*
	public void StartDay()
	{

	}
*/
	public void AddProgress(float progress)
	{
		if (config.type == TaskType.Idle || config.type == TaskType.Relaxing)
			return;

		CurrentWork = Mathf.Min(CurrentWork + progress, config.work);
	}
/*
	public void EndDay()
	{

	}
*/
	public Status GetStatus(Dev[] Devs)
	{
		List<Dev.Status> involvedDevs = new List<Dev.Status>();
		if(!IsDone)
		{
			foreach(var item in Devs)
			{
				if (item.currentTask == this)
					involvedDevs.Add(item.GetStatus());
			}
		}

		return new Status(config, CurrentWork, involvedDevs.ToArray());
	}
}
