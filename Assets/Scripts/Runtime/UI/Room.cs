using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	[SerializeField]
	private Node[] nodes;

	[SerializeField]
	private TaskType type;


	public TaskType GetTaskType()
		=> type;

	public Node ReserveNode()
	{
		for(int i = 0; i < nodes.Length; ++i)
		{
			if(nodes[i].Reserve())
				return nodes[i];
		}

		return null;
	}
}
