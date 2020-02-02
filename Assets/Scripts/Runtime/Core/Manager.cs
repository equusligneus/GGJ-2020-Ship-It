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

	public void OnScenarioEnd()
	{
		
	}

	public void ShowStandup(Action onDone)
	{
		throw new NotImplementedException();
	}

	public void ShowLunchBreak(Action onDone)
	{
		throw new NotImplementedException();
	}

	public void ShowNight(Action onDone)
	{
		throw new NotImplementedException();
	}

	public void ShowEnd(Action onDone)
	{
		throw new NotImplementedException();
	}

	public void TickUI()
	{
		throw new NotImplementedException();
	}
}
