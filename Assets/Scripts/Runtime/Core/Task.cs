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

public class Task
{
	[System.Serializable]
	public struct Config
	{
		public string name;
		public TaskType type;
		public Node.Type location;
		public int importance;
		public float difficulty;
		public float work;
		public float enjoyability;
		public int maxProgrammerNum;
	}

	public struct Status
	{
		public Status(Config config)
		{
			work = 0;
			this.config = config;
		}

		public float workPercentage
			=> work / config.work;
		public bool isDone
			=> workPercentage >= 1.0f;

		public string name
			=> config.name;

		public TaskType type
			=> config.type;

		public float work;
		private Config config;
	}

	public Task(Config config)
	{
		this.config = config;
		status = new Status(config);
	}

	public TaskType GetTaskType()
		=> config.type;

	public Config GetConfig()
		=> config;

	public Status GetStatus()
		=> status;

	public bool IsDone
		=> status.isDone;

	private Config config;

	private Status status;

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

		status.work = Mathf.Min(status.work + progress, config.work);
	}
}
