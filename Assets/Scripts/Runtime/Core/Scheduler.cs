using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Manager))]
public class Scheduler : MonoBehaviour
{
	public enum ScenarioPhase
	{
		StartOfDay,
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
		ScenarioPhase phase { get; }

		Scheduler scheduler { set; }

		void Enter(ScenarioPhase phase);
		void Update();
		void Exit();
	}

	[System.Serializable]
	public class EnterDevRoutine : IRoutine
	{
		public bool isActive
			=> true;

		public ScenarioPhase phase { get; private set; }

		public Scheduler scheduler { get; set; }

		public void Enter(ScenarioPhase phase)
		{
			this.phase = phase;
		}

		public void Update()
		{
		}

		public void Exit()
		{
			
		}
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

		public ScenarioPhase phase { get; private set; }

		public void Enter(ScenarioPhase phase) 
		{
			this.phase = phase;
			currentTime = 0;
			currentTick = 0;
		}

		public void Update() 
		{
			currentTime += Time.deltaTime;
			if (currentTime > tickTime)
			{
				currentTime -= tickTime;
				++currentTick;

				scheduler.manager.scenario.Tick();
				scheduler.manager.GetTaskList().Tick(scheduler.manager.scenario.GetStatus());
			}
		}

		public void Exit() { }

	}

	[System.Serializable]
	public class ExitDevRoutine : IRoutine
	{
		public bool isActive 
			=> true;

		public ScenarioPhase phase { get; private set; }

		public Scheduler scheduler { get; set; }

		public void Enter(ScenarioPhase phase)
		{
			this.phase = phase;
		}

		public void Update()
		{
		}

		public void Exit()
		{
		}
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
		currentRoutine = enterRoutine;
		currentRoutine.Enter(ScenarioPhase.StartOfDay);
		return true;
	}


    void Awake()
    {
		enterRoutine.scheduler = this;
		tickRoutine.scheduler = this;
		exitRoutine.scheduler = this;
        //if (!Manager.Scenario)
        //{
        //    debug.Log("Scenario not initialized");
        //    return;
        //}
        //scenario = GetComponent<Scenario>();
        //// This will not work if there is no devparent
        //DevParent = GetComponent<DevParent>();
        //taskList.Tick(scenario.GetStatus());
    }

 //   public void StartDay()
	//{
 //       scenario.StartScenario();
 //       foreach (GameObject child in DevParent)
 //       {
 //           if (child.GetComponent<Avatar>().isMoving){
 //               child.GetComponent<Avatar>().TrySetTask();
 //               child.GetComponent<Avatar>().SetMood();
 //           } else {
 //               child.StartTick(true);
 //           }
 //       }
 //   }

 //   public void EndDay()
	//{
 //       if (Manager.currentTick == 0)
 //       {
 //           foreach (GameObject child in DevParent)
 //           {
 //               child.EndTick();
 //           }
 //       }
	//}


	private void Update()
    {
		if (isPaused)
			return;

		currentRoutine.Update();
		if(!currentRoutine.isActive)
		{
			currentRoutine.Exit();
			switch (currentRoutine.phase)
			{
				case ScenarioPhase.StartOfDay:
					currentRoutine = tickRoutine;
					currentRoutine.Enter(ScenarioPhase.Morning);
					break;
				case ScenarioPhase.Morning:
					currentRoutine = exitRoutine;
					currentRoutine.Enter(ScenarioPhase.StartOfLunchBreak);
					break;
				case ScenarioPhase.StartOfLunchBreak:
					// wait phase
					break;
				case ScenarioPhase.LunchBreak:
					currentRoutine = enterRoutine;
					currentRoutine.Enter(ScenarioPhase.EndOfLunchBreak);
					break;
				case ScenarioPhase.EndOfLunchBreak:
					currentRoutine = tickRoutine;
					currentRoutine.Enter(ScenarioPhase.Afternoon);
					break;
				case ScenarioPhase.Afternoon:
					currentRoutine = exitRoutine;
					currentRoutine.Enter(ScenarioPhase.EndOfDay);
					break;
				case ScenarioPhase.EndOfDay:
					// wait phase
					break;
				case ScenarioPhase.Night:
					// if not finished!!!				
					currentRoutine = enterRoutine;
					currentRoutine.Enter(ScenarioPhase.StartOfDay);
					break;
				case ScenarioPhase.EndOfScenario:
					// end everything!!!
					isPaused = true;
					manager.OnScenarioEnd();
					break;
				default:
					break;
			}
		}
    }


    public Manager manager { get; private set; }
	private bool isPaused;

	private IRoutine currentRoutine;
	[SerializeField]
	private EnterDevRoutine enterRoutine;
	[SerializeField]
	private TickRoutine tickRoutine;
	[SerializeField]
	private ExitDevRoutine exitRoutine;

}