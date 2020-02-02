using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Manager))]
public class Scheduler : MonoBehaviour
{
	public enum Phase
	{
		StartOfDay,
		StandUp,
		Morning,
		StartOfLunchBreak,
		LunchBreak,
		EndOfLunchBreak,
		Afternoon,
		EndOfDay,
		Night,
		EndOfScenario
	}

	public interface IRoutine
	{
		bool isActive { get; }
		Phase phase { get; }

		Scheduler scheduler { set; }

		void Enter(Phase phase);
		void Update();
		void Exit();
	}

	[System.Serializable]
	public class MoveDevRoutine : IRoutine
	{
		public bool isActive { get; private set; }

		public Phase phase { get; private set; }

		public Scheduler scheduler { get; set; }

		public void Enter(Phase phase)
		{
			this.phase = phase;
			scenario = scheduler.manager.scenario;
			isActive = true;
			SendCommand();
			time = 0.0f;
		}

		public void Update()
		{
			if(remainingDevs > 0)
			{
				time += Time.deltaTime;
				if(time > commandDelay)
				{
					time -= commandDelay;
					SendCommand();
				}
			}
			isActive = remainingDevs > 0 || scheduler.manager.scenario.areDevsMoving;
		}

		public void Exit() { }

		private void SendCommand()
		{
			switch (phase)
			{
				case Phase.StartOfDay:
				case Phase.EndOfLunchBreak:
					remainingDevs = scheduler.manager.scenario.EnterDev();
					break;
				case Phase.StartOfLunchBreak:
				case Phase.EndOfDay:
					remainingDevs = scheduler.manager.scenario.ExitDev();
					break;
				default:
					break;
			}
		}

		[SerializeField]
		private float commandDelay;

		private int remainingDevs;
		private float time;
		private Scenario scenario;
	}

	[System.Serializable]
	private class TickRoutine : IRoutine
	{
		public float tickTime;
		public int ticksPerPhase;

		private float currentTime;
		private int currentTick;

		public Scheduler scheduler { get; set; }

		public bool isActive
			=> currentTick < ticksPerPhase;

		public Phase phase { get; private set; }

		public void Enter(Phase phase) 
		{
			this.phase = phase;
			currentTime = 0;
			currentTick = 0;
			scheduler.manager.ShowInGameMenu();
		}

		public void Update() 
		{
			currentTime += Time.deltaTime;
			if (currentTime > tickTime)
			{
				currentTime -= tickTime;
				++currentTick;

				scheduler.manager.scenario.Tick();
				scheduler.manager.TickUI();
			}
		}

		public void Exit() { }

	}

	[System.Serializable]
	public class WaitRoutine : IRoutine
	{
		public bool isActive { get; private set; }

		public Phase phase { get; private set; }

		public Scheduler scheduler { get; set; }

		public void Enter(Phase phase)
		{
			this.phase = phase;
			isActive = true;
			switch (this.phase)
			{
				case Phase.StandUp:
					scheduler.manager.ShowStandup(OnDone);
					break;
				case Phase.LunchBreak:
					scheduler.manager.ShowLunchBreak(OnDone);
					break;
				case Phase.Night:
					scheduler.manager.ShowNight(OnDone);
					break;
				case Phase.EndOfScenario:
					scheduler.manager.ShowEnd(OnDone);
					break;
				default:
					break;
			}
		}

		public void Update() { }

		public void Exit() { }

		private void OnDone()
			=> isActive = false;
	}

	public bool StartScenario(Manager manager)
	{
		if(manager == null)
		{
			Debug.LogError("Got null manager!!!");
			return false;
		}

		this.manager = manager;

		manager.scenario.StartScenario();
		currentRoutine = moveRoutine;
		currentRoutine.Enter(Phase.StartOfDay);
		isPaused = false;
		return true;
	}


    void Awake()
    {
		isPaused = true;
		moveRoutine.scheduler = this;
		tickRoutine.scheduler = this;
		waitRoutine.scheduler = this;
    }

	private void Update()
    {
		if (isPaused)
			return;

		currentRoutine.Update();
		if (!currentRoutine.isActive)
			SwitchRoutine();
	}

	private void SwitchRoutine()
	{
		currentRoutine.Exit();
		Debug.LogErrorFormat("End of phase {0}", currentRoutine.phase);
		switch (currentRoutine.phase)
		{
			case Phase.StartOfDay:
			currentRoutine = waitRoutine;
			currentRoutine.Enter(Phase.StandUp);
			break;
		case Phase.StandUp:
				currentRoutine = tickRoutine;
				currentRoutine.Enter(Phase.Morning);
				break;
			case Phase.Morning:
				currentRoutine = moveRoutine;
				currentRoutine.Enter(Phase.StartOfLunchBreak);
				break;
			case Phase.StartOfLunchBreak:
				currentRoutine = waitRoutine;
				currentRoutine.Enter(Phase.LunchBreak);
				break;
			case Phase.LunchBreak:
				currentRoutine = moveRoutine;
				currentRoutine.Enter(Phase.EndOfLunchBreak);
				break;
			case Phase.EndOfLunchBreak:
				currentRoutine = tickRoutine;
				currentRoutine.Enter(Phase.Afternoon);
				break;
			case Phase.Afternoon:
				currentRoutine = moveRoutine;
				currentRoutine.Enter(Phase.EndOfDay);
				break;
			case Phase.EndOfDay:
				//manager.scenario.EndDay();
				currentRoutine = waitRoutine;
				currentRoutine.Enter(manager.scenario.daysLeft > 0 ? Phase.Night : Phase.EndOfScenario);
				break;
			case Phase.Night:
				currentRoutine = moveRoutine;
				currentRoutine.Enter(Phase.StartOfDay);
				break;
			case Phase.EndOfScenario:
				// end everything!!!
				isPaused = true;
				manager.OnScenarioEnd();
				break;
			default:
				break;
		}
	}

    public Manager manager { get; private set; }
	private bool isPaused;

	private IRoutine currentRoutine;
	[SerializeField]
	private MoveDevRoutine moveRoutine;
	[SerializeField]
	private TickRoutine tickRoutine;
	[SerializeField]
	private WaitRoutine waitRoutine;

}