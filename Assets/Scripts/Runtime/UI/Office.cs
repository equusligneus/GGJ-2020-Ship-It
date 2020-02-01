using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class Office : MonoBehaviour
{
	[SerializeField]
	private Node root = null;
	[SerializeField]
	private Node start = null;

	public bool GetPathToFreeNodeOfType(TaskType type, Node currentNode, ref Queue<Node> path, bool isMoving)
	{
		bool removeFirstElement = !isMoving;
		if(isMoving)
		{
			Node node;
			do
			{
				node = path.Dequeue();
				// TODO remove "turn at next node" behaviour in case of 180° turn!
				removeFirstElement |= node.HasChild(currentNode);
			}
			while (path.Count > 0);
			node.Vacate();
		}
		else
		{
			path.Clear();
		}

		// makes most sense to measure from the midpoint or there will be weird moves
		Node endPoint = GetNearestFreeNodeOfType(type, root.transform.position);

		Debug.LogErrorFormat("Endpoint is {0}", (endPoint ? "available" : "null"));

		if (!endPoint)
			return false;

		currentNode.GetPathToRoot(ref path);
		// remove start node if not moving
		if(removeFirstElement)	
			path.Dequeue();

		Stack<Node> fromRoot = new Stack<Node>();
		endPoint.GetPathToRootReversed(ref fromRoot);
		fromRoot.Pop();
		while (fromRoot.Count > 0)
			path.Enqueue(fromRoot.Pop());

		Debug.LogErrorFormat("Has {0} nodes!", path.Count);

		return true;
	}

	public Node GetRootNode()
		=> root;

	public Node GetStartNode()
		=> start;

	public Node GetNearestFreeNodeOfType(TaskType type, Vector3 position)
	{
		List<Node> nodes = new List<Node>();
		root.GetFreeNodesOfType(type, ref nodes);

		if (nodes.Count == 0)
			return null;

		return nodes.OrderBy(n => (n.transform.position - position).sqrMagnitude).First();
	}
}