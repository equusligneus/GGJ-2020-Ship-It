using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
	public void Setup(Manager manager)
	{
		this.manager = manager;
		pages = GetComponentsInChildren<MenuPage>(true);
		foreach (var item in pages)
			item.Setup(this);
	}

	public void Tick(Scenario.Status scenarioStatus)
		=> MenuPage.Tick(scenarioStatus);

	public Manager manager { get; private set; }

	private MenuPage[] pages;
}
