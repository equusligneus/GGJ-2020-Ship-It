using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

	[SerializeField]
	private TaskType type;
	[SerializeField]
	private bool isMultiUser;
	[SerializeField]
	private Node[] children = null;
	private Node parent;

	private bool isReserved;

	private void Start()
	{
		for (int i = 0; i < children.Length; ++i)
			children[i].parent = this;
	}

	public void GetPathToRoot(ref Queue<Node> nodes)
	{
		nodes.Enqueue(this);
		if (parent)
			parent.GetPathToRoot(ref nodes);
	}

	public void GetPathToRootReversed(ref Stack<Node> nodes)
	{
		nodes.Push(this);
		if (parent)
			parent.GetPathToRootReversed(ref nodes);
	}

	public void GetFreeNodesOfType(TaskType type, ref List<Node> nodes)
	{
		if (this.type == type && !isReserved)
			nodes.Add(this);
		foreach (var item in children)
			item.GetFreeNodesOfType(type, ref nodes);
	}

	public bool HasChild(Node node)
	{
		foreach(var item in children)
		{
			if (item == node)
				return true;
		}
		return false;
	}

	public bool Reserve()
	{
		if (isReserved)
			return false;
			
		isReserved = !isMultiUser;		
		return true;
	}

	public void Vacate()
	{
		isReserved = false;
	}

#if UNITY_EDITOR

	void OnDrawGizmos()
	{
		Color color = Gizmos.color;
		Gizmos.color = children != null && children.Length > 0 ? Color.red : Color.green;

		if (children == null)
			return;

		Gizmos.DrawSphere(transform.position, 0.2f);
		for(int i = 0; i < children.Length; ++i)
			Gizmos.DrawLine(transform.position, children[i].transform.position);

		Gizmos.color = color;
	}
#endif
}
