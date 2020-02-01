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
        DevParent = GetComponent<DevParent>();
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

    private GameObject DevParent;
    private Scenario scenario;
}