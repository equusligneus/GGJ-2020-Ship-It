using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class Office : MonoBehaviour
{
	[SerializeField]
	private Node root = null;
	[SerializeField]
	private Node portal = null;

	private void Awake()
	{
		if(!portal)
		{
			List<Node> portals = new List<Node>();
			root.GetFreeNodesOfType(Node.Type.Portal, ref portals);
			portal = portals[0];
		}
	}

	public bool PlotPathToNodeType(Node.Type type, Node currentNode, ref Queue<Node> path, bool isMoving)
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

		Debug.LogFormat("Endpoint is {0}", (endPoint ? endPoint.name : "null"));

		if (!endPoint)
			return false;
		endPoint.Reserve();
		currentNode.GetPathToRoot(ref path);
		// remove start node if not moving
		if(removeFirstElement)	
			path.Dequeue();

		Stack<Node> fromRoot = new Stack<Node>();
		endPoint.GetPathToRootReversed(ref fromRoot);
		fromRoot.Pop();
		while (fromRoot.Count > 0)
			path.Enqueue(fromRoot.Pop());

		Debug.LogFormat("Path has {0} nodes!", path.Count);

		return true;
	}

	public void SetToPortal(Transform transform)
		=> transform.position = portal.transform.position;

	public Node GetRootNode()
		=> root;

	public Node GetPortalNode()
		=> portal;

	public Node GetNearestFreeNodeOfType(Node.Type type, Vector3 position)
	{
		List<Node> nodes = new List<Node>();
		root.GetFreeNodesOfType(type, ref nodes);

		if (nodes.Count == 0)
			return null;

		return nodes.OrderBy(n => (n.transform.position - position).sqrMagnitude).First();
	}
}