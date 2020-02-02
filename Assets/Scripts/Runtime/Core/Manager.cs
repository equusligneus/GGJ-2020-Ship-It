using System;
using UnityEngine;

[RequireComponent(typeof(Scheduler))]
public class Manager : MonoBehaviour
{
	private void Start()
	{
		scheduler = GetComponent<Scheduler>();
		ui.Setup(this);
		selector.onAvatarHovered += (Avatar a) => Debug.LogWarningFormat("Hovered {0}", (a ? a.name : "none"));
		selector.onAvatarSelected += (Avatar a) => Debug.LogWarningFormat("Selected {0}", (a ? a.name : "none"));
	}

	public void SelectScenario(Config_Scenario scenario)
	{
		this.scenario = new Scenario(scenario.config, this);
		scheduler.StartScenario(this);
		MenuPage.SwitchMenu(MenuPage.Type.None);
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
	private AvatarSelector selector;

	[SerializeField]
	private Office office;

	public void OnScenarioEnd()
	{
		
	}

	public void ShowStandup(Action onDone)
	{
		MenuPage.SwitchMenu(MenuPage.Type.StandUp, onDone);
		TickUI();
	}

	public void ShowLunchBreak(Action onDone)
	{
		MenuPage.SwitchMenu(MenuPage.Type.LunchBreak, onDone);
		TickUI();
	}

	public void ShowNight(Action onDone)
	{
		MenuPage.SwitchMenu(MenuPage.Type.Night, onDone);
		TickUI();
	}

	public void ShowEnd(Action onDone)
	{
		MenuPage.SwitchMenu(MenuPage.Type.Summary, onDone);
		TickUI();
	}

	public void ShowInGameMenu()
	{
		MenuPage.SwitchMenu(MenuPage.Type.InGame);
		TickUI();
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

	public Dev GetSelected()
	{
		if (selector.selected)
			return selector.selected.owner;
		Debug.LogError("Nobody selected!!!");
		return null;
	}
}
