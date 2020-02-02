using System;
using UnityEngine;

[RequireComponent(typeof(Scheduler))]
public class Manager : MonoBehaviour
{
	private void Start()
	{
		scheduler = GetComponent<Scheduler>();
		ui.Setup(this);
	}

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
	private UI ui;

	[SerializeField]
	private Office office;

	public void OnScenarioEnd()
	{
		
	}

	public void ShowStandup(Action onDone)
	{
		MenuPage.SwitchMenu(MenuPage.Type.StandUp, onDone);
	}

	public void ShowLunchBreak(Action onDone)
	{
		MenuPage.SwitchMenu(MenuPage.Type.LunchBreak, onDone);
	}

	public void ShowNight(Action onDone)
	{
		MenuPage.SwitchMenu(MenuPage.Type.Night, onDone);
	}

	public void ShowEnd(Action onDone)
	{
		MenuPage.SwitchMenu(MenuPage.Type.Summary, onDone);
	}

	public void ShowInGameMenu()
	{
		MenuPage.SwitchMenu(MenuPage.Type.InGame);
	}

	public void TickUI()
	{
		ui.Tick(scenario.GetStatus());
	}

	public void CloseApplication()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

}
