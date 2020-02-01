using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Manager.cs))]
public class Scheduler : Monobehaviour
{

    void Start()
    {
        if (!Manager.Scenario)
        {
            debug.Log("Scenario not initialized");
            return;
        }
        scenario = GetComponent<Scenario>();
        // This will not work if there is no devparent
        DevParent = GetComponent<DevParent>();
        taskList.Tick(scenario.GetStatus());
    }

    public void StartDay(){
        scenario.StartScenario();
        foreach (GameObject child in DevParent)
        {
            if (child.GetComponent<Avatar>().isMoving){
                child.GetComponent<Avatar>().TrySetTask();
                child.GetComponent<Avatar>().SetMood();
            } else {
                child.StartTick(true);
            }
        }
    }

    public void EndDay()
	{
        if (Manager.currentTick == 0)
        {
            foreach (GameObject child in DevParent)
            {
                child.EndTick();
            }
        }
	}


void Update()
    {
		currentTime += Time.deltaTime;
		if(currentTime > tickTime)
		{
			currentTime -= tickTime;
			++currentTick;
			bool isNewDay = currentTick >= ticksPerDay;
			if (isNewDay)
				currentTick = 0;

			scenario.Tick(isNewDay);
			taskList.Tick(scenario.GetStatus());
		}
    }

    public float tickTime;
    public int ticksPerDay;
    private GameObject DevParent;
    private Scenario scenario;
    private float currentTime;
    private int currentTick;

}