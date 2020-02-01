using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ScenarioButton : MonoBehaviour
{
	public void Setup(Manager manager, Config_Scenario scenario)
	{
		this.manager = manager;
		this.scenario = scenario;
		GetComponentInChildren<Text>().text = scenario.config.name;
	}

    private void Awake()
		=>GetComponent<Button>().onClick.AddListener(OnClicked);

	private void OnClicked()
		=> manager.SelectScenario(scenario);

	private Manager manager;
	private Config_Scenario scenario;
}
