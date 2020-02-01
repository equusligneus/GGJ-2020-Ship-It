using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//#if UNITY_EDITOR

//using UnityEditor;

//public partial class Config : ScriptableObject
//{
//	[MenuItem("Assets/Create/Create Config")]
//	public static void Create()
//	{
//		Selection.

//	}
//}

//#endif

public partial class Config : /*ScriptableObject*/ MonoBehaviour
{
	[SerializeReference]
	public Programmer[] programmers;

	[SerializeReference]
	public Task[] tasks;

	[SerializeReference]
	public Scenario[] scenarios;
}
