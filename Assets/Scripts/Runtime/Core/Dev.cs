using UnityEngine;


[System.Serializable]
public class Dev
{
	[System.Serializable]
	public struct Config
	{
		public string name;
		public float maxSkill;
		public float maxSpeed;
		public float maxAnnoyance;
		public Avatar avatar;
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

	public Dev(Config config)
	{
		this.config = config;
		avatar = Object.Instantiate(config.avatar, Vector3.zero, Quaternion.identity);
	}

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

	private Avatar avatar;

	public Task currentTask;
}
