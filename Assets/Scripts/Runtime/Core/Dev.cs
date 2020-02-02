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
		avatar = Object.Instantiate(config.avatar, Vector3.zero, Quaternion.identity);
		avatar.Setup(this, manager.GetOffice());
	}

	public void Tick()
	{
		if (avatar.isMoving)
			return;

		if (status.task == null)
			return;

		WorkLog.Commit(config, ref status);

		//if (currentMood == null)
		//	currentMood = scenario.GetNeutralMood();

		//Mood newMood = currentMood.Tick(status.motivation);
		//if (newMood != currentMood)
		//{
		//	currentMood = newMood;
		//	avatar.PostMood(currentMood);
		//}
	}

	public void Enter()
		=> avatar.TryMoveTo(Node.Type.WorkStation);

	public void Exit()
		=> avatar.TryMoveTo(Node.Type.Portal);

	public bool TrySetTask(Task task)
	{
		if (status.task == null)
		{
			if(avatar.TryMoveTo(task.GetConfig().location))
			{
				status.task = task;
				return true;
			}
		}

		if (status.task == task)
			return false;

		// works on same workstation type or has open workstations
		if (status.task.GetConfig().location == task.GetConfig().location || avatar.TryMoveTo(task.GetConfig().location))
		{
			status.task = task;
			return true;
		}

		return false;
	}

	public Status GetStatus()
		=> status;

	public Config GetConfig()
		=> config;

	public Avatar avatar { get; private set; }

	private Config config;
	private Scenario scenario;
	private Manager manager;

	public Status status;

	public Mood currentMood;
	public Transform DevParent;
}
