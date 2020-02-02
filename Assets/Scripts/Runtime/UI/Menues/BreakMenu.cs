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

	[SerializeField]
	private Type breakType;
	[SerializeField]
	private Text caption;
	[SerializeField]
	private Button continueButton;
}
