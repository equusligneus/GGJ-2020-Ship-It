using UnityEngine;
using UnityEngine.UI;

public class BreakMenu : MenuPage
{
	public override Type type
		=> breakType;

	protected override void Construct()
	{
		base.Construct();
		caption.text = type.ToString();
		continueButton.onClick.AddListener(() => SwitchMenu(Type.None));
	}

	protected override void Tick_Internal(Scenario.Status status)
	{
		base.Tick_Internal(status);
		if (tasks)
			tasks.Tick(status);
	}

	[SerializeField]
	private Type breakType;
	[SerializeField]
	private Text caption;
	[SerializeField]
	private Button continueButton;
	[SerializeField]
	private TaskDisplay tasks;
}
