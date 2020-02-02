using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MenuPage
{
	public override Type type 
		=> Type.InGame;

	protected override void Tick_Internal(Scenario.Status status)
	{
		base.Tick_Internal(status);
		if (tasks)
			tasks.Tick(status);
	}

	[SerializeField]
	private TaskDisplay tasks;
}
