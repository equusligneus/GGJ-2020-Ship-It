using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
	private void Update()
	{
		if(isMoving)
		{
			var position = path.Peek().transform.position;
			transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime);
			if ((position - transform.position).sqrMagnitude < 0.001)
				node =	path.Dequeue();
		}
		if (headAnimator)
			headAnimator.SetBool("isMoving", isMoving);
	}

	public void Setup(Dev owner, Office office)
	{
		this.owner = owner;
		this.office = office;

		var config = owner.GetConfig();
		if (body)
			body.color = config.avatarColor;

		name = config.name;
		node = office.GetPortalNode();
		// TODO have them move in?
		transform.position = node.transform.position;
	}

	public void Enter()
	{
		office.SetToPortal(transform);
		head.enabled = true;
		body.enabled = true;
		office.PlotPathToNodeType(Node.Type.WorkStation, node, ref path, isMoving);
	}

	public void Exit()
	{
		office.PlotPathToNodeType(Node.Type.Portal, node, ref path, isMoving);
	}

	public void SetMood(Dev dev)
	{
		// set the mood of the dev
	}

	public void PostMood(Mood mood)
	{
		Debug.LogFormat("{0} is {1}", name, mood.Name);
		//speechBubble.PostMood(mood);
	}

	public bool TryMoveTo(Node.Type type)
	{
		if(office.PlotPathToNodeType(type, node, ref path, isMoving))
		{
			// remove 
			node.Vacate();
			return true;
		}
		return false;
	}

	public bool isMoving
		=> path.Count > 0;

	private Dev owner = null;
	private Office office = null;
	private Node node = null;
	private Queue<Node> path = new Queue<Node>();


	[SerializeField]
	private SpriteRenderer body;

	[SerializeField]
	private SpriteRenderer head;

	[SerializeField]
	private Animator headAnimator;

	[SerializeField]
	private SpeechBubble speechBubble;
}
