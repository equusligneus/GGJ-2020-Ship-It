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
			motivation = 1;
			focus = 1;
			this.task = task;
			mood = null;
		}

		public string name;
		public float motivation;
		public float focus;
		public Task task;
		public Mood mood;
	}

	public Dev(Config config, Scenario scenario, Manager manager)
	{
		this.config = config;
		this.scenario = scenario;
		this.manager = manager;
		avatar = Object.Instantiate(config.avatar, Vector3.zero, Quaternion.identity,DevParent);
		avatar.Setup(this, manager.GetOffice());
	}

	public void StartTick(bool isNewDay)
	{
		
	}

	public void EndTick()
	{
		if (avatar.isMoving)
			return;

		WorkLog.Commit(config, ref status);
		//currentTask.AddProgress(config.speed);
		
		//if (currentTask.IsDone)
		//	scenario.SetIdle(this);

		//if (currentMood == null)
		//	currentMood = scenario.GetNeutralMood();

		//Mood newMood = currentMood.Tick(status.motivation);
		//if (newMood != currentMood)
		//{
		//	currentMood = newMood;
		//	avatar.PostMood(currentMood);
		//}
	}

	public void TrySetTask(Task task)
	{
		if (status.task == null)
		{
			avatar.TryMoveTo(task.GetTaskType());
			status.task = task;
			return;
		}

		if (status.task == task)
			return;

		// works on same workstation type or has open workstations
		if (status.task.GetTaskType() == task.GetTaskType() || avatar.TryMoveTo(task.GetTaskType()))
			status.task = task;
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

	public Mood currentMood;
	public Transform DevParent;
}
