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
    }


	public Scenario GetScenario()
		=> scenario;

	public Office GetOffice()
		=> office;

	private Scenario scenario;


	//UI
	[SerializeField]
	private Config_Scenario scenarioConfig;
	[SerializeField]	
	private TaskList taskList;

	[SerializeField]
	private Office office; 

}
