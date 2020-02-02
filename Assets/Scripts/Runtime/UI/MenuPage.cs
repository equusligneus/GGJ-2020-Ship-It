using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public abstract class MenuPage : MonoBehaviour
{
	public enum Type
	{
		None,
		ScenarioSelect,
		StandUp,
		InGame,
		LunchBreak,
		Night,
		Summary
	}

	static MenuPage()
	{
		s_typeToMenu = new Dictionary<Type, MenuPage>();
		s_currentMenu = Type.None;
	}

	public static void SwitchMenu(Type type)
	{
		if (type == s_currentMenu)
			return;

		if (s_typeToMenu.ContainsKey(s_currentMenu))
			s_typeToMenu[s_currentMenu].Close();

		s_currentMenu = type;

		if (s_typeToMenu.ContainsKey(s_currentMenu))
			s_typeToMenu[s_currentMenu].Open();

	}

	protected virtual void Awake()
	{
		canvas = GetComponent<Canvas>();
		s_typeToMenu.Add(type, this);
	}

	public void Setup(UI ui)
	{
		this.ui = ui;
		endGameButton.onClick.AddListener(ui.manager.CloseApplication);
		Construct();
	}

    public virtual void Open()
		=> gameObject.SetActive(true);

	public virtual void Close()
		=> gameObject.SetActive(false);

	protected virtual void Construct() { }

	public abstract Type type { get; }

	protected static Dictionary<Type, MenuPage> s_typeToMenu;
	public static Type s_currentMenu;

	[SerializeField]
	protected Button endGameButton;

	protected Canvas canvas;
	protected UI ui;

}
