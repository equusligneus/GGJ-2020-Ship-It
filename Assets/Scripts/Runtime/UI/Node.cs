using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
	public enum Type
	{
		None,
		Portal,
		WorkStation,
		PlayStation,
		DartsBoard,
		BillardTable,
		BeanBag,
		Bed
	}

	[SerializeField]
	private Type type;
	[SerializeField]
	private bool isMultiUser;
	[SerializeField]
	private Node[] children = null;
	private Node parent;

	private bool isReserved;

	private void Start()
	{
		for (int i = 0; i < children.Length; ++i)
		{
			if (children[i])
				children[i].parent = this;
			else
				Debug.LogErrorFormat(this, "Child node {0} not assigned", i);
		}
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

	public void GetFreeNodesOfType(Type type, ref List<Node> nodes)
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

		switch (type)
		{
			case Type.None:
				Gizmos.color = Color.black;
				break;
			case Type.Portal:
				Gizmos.color = Color.grey;
				break;
			case Type.WorkStation:
				Gizmos.color = Color.green;
				break;
			case Type.PlayStation:
			case Type.DartsBoard:
			case Type.BillardTable:
				Gizmos.color = Color.yellow;
				break;
			case Type.BeanBag:
			case Type.Bed:
				Gizmos.color = Color.red;
				break;
			default:
				break;
		}

		Gizmos.DrawSphere(transform.position, 0.2f);

		if (children == null)
			return;
		
		Gizmos.color = Color.blue;
		for (int i = 0; i < children.Length; ++i)
		{
			if (children[i])
				Gizmos.DrawLine(transform.position, children[i].transform.position);
		}
		Gizmos.color = color;
	}
#endif
}
