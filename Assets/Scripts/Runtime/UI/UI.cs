using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
	[SerializeField]
	private MenuPage[] pages;

	[SerializeField]
	private TaskList taskList;
	
	
	public void Setup(Manager manager)
	{
		this.manager = manager;
		foreach (var item in pages)
			item.Setup(this);
	}

	public Manager manager { get; private set; }
}
