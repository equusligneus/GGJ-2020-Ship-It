using UnityEngine;


[System.Serializable]
public class Dev
{
	[System.Serializable]
	public struct Config
	{
		public string name;
		public float skill;
		public AnimationCurve challengeEnjoymentCurve;
		public float speed;

		public float maxAnnoyance;
		public Avatar avatar;
		public Color avatarColor;
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

	public Dev(Config config, Scenario scenario, Manager manager)
	{
		this.config = config;
		this.scenario = scenario;
		this.manager = manager;
		avatar = Object.Instantiate(config.avatar, Vector3.zero, Quaternion.identity);
		avatar.Setup(this, manager.GetOffice());
	}

	public void StartTick(bool isNewDay)
	{
		
	}

	public void EndTick()
	{
		if (avatar.isMoving)
			return;

		currentTask.AddProgress(config.speed);
		
		if (currentTask.IsDone)
			scenario.SetIdle(this);

		if (currentMood == null)
			currentMood = scenario.GetNeutralMood();

		Mood newMood = currentMood.Tick(status.annoyance);
		if (newMood != currentMood)
		{
			currentMood = newMood;
			avatar.PostMood(currentMood);
		}
	}

	public void TrySetTask(Task task)
	{
		if (currentTask == null)
		{
			avatar.TryMoveTo(task.GetTaskType());
			currentTask = task;
			return;
		}

		if (currentTask == task)
			return;

		// works on same workstation type or has open workstations
		if (currentTask.GetTaskType() == task.GetTaskType() || avatar.TryMoveTo(task.GetTaskType()))
			currentTask = task;
		else
			scenario.SetIdle(this); // TODO This might cause a StackOverflow!!! Fix reservations for idle!
	}

	public Status GetStatus()
		=> status;

	public Config GetConfig()
		=> config;

	private Config config;
	private Scenario scenario;
	private Manager manager;

	private Avatar avatar;
	private Status status;

	public Task currentTask;
	public Mood currentMood;
}
