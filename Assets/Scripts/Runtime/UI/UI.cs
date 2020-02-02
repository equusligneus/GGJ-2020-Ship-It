using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
	public void Setup(Manager manager)
	{
		this.manager = manager;
		foreach (var item in pages)
			item.Setup(this);
	}

	public void Tick(Scenario.Status scenarioStatus)
	{ }

	public Manager manager { get; private set; }

	[SerializeField]
	private MenuPage[] pages;

	[SerializeField]
	private TaskList taskList;
}
