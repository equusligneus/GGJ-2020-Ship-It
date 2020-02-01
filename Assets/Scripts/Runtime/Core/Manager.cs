using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
    {
		if(!scenarioConfig)
		{
			enabled = false;
			return;
		}

		scenario = new Scenario(scenarioConfig.config, this);
		scenario.StartScenario();
		taskList.Tick(scenario.GetStatus());
    }

    // Update is called once per frame
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

	public Scenario GetScenario()
		=> scenario;

	public Office GetOffice()
		=> office;

	//Game
	public float tickTime;
	public int ticksPerDay;	
	private float currentTime;
	private int currentTick;

	private Scenario scenario;


	//UI
	[SerializeField]
	private Config_Scenario scenarioConfig;
	[SerializeField]	
	private TaskList taskList;

	[SerializeField]
	private Office office; 

}
