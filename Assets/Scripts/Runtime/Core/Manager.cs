using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scheduler))]
public class Manager : MonoBehaviour
{
	private void Awake()
		=> scheduler = GetComponent<Scheduler>();

	public void SelectScenario(Config_Scenario scenario)
	{
		this.scenario = new Scenario(scenario.config, this);
		scheduler.StartScenario(this);
	}

	public void OnScenarioEnd()
	{
		throw new NotImplementedException();
	}

	public Office GetOffice()
		=> office;

	public TaskList GetTaskList()
		=> taskList;

	//public DevList GetDevList()
	//	=> devList;

	public Scenario scenario { get; private set; }

	private Scheduler scheduler;

	[SerializeField]	
	private TaskList taskList;

	[SerializeField]
	private Office office;
}
