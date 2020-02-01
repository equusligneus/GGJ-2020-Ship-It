using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorkLog
{
	static WorkLog()
		=> s_random = new System.Random();

    public static void Commit(Dev.Config dev, ref Dev.Status status)
	{
		var taskConfig = status.task.GetConfig();
		var abilityFactor = dev.skill / taskConfig.difficulty;

		// add more curves 'n' shit
		float progress = dev.speed * status.motivation * abilityFactor;
		float safety = status.focus * abilityFactor;

		status.task.AddProgress(progress);

		// add mood modifiers here
		status.motivation = Mathf.Clamp(status.motivation + taskConfig.enjoyability, 0, 1);
		status.focus = Mathf.Clamp(status.focus - taskConfig.difficulty, 0, 1);
	}


	private static System.Random s_random;
}
