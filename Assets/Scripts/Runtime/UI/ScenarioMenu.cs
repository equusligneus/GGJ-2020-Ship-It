using UnityEngine;

public class ScenarioMenu : MenuPage
{
	protected override void Awake()
	{
		base.Awake();
		Construct();
		base.Close();
	}

	private void Construct()
	{
		foreach(var item in scenarios)
			Instantiate(prefab, buttonRoot, false).Setup(ui.manager, item);
	}

	public override Type type
		=> Type.ScenarioSelect;

	[SerializeField]
	private ScenarioButton prefab;
	[SerializeField]
	private RectTransform buttonRoot;
	[SerializeField]
	private Config_Scenario[] scenarios;
}
