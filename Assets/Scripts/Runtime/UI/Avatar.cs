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
		node = office.GetStartNode();
		// TODO have them move in?
		transform.position = node.transform.position;
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

	public bool TryMoveTo(TaskType type)
	{
		// if path !empty, remove old path and remove reservation!
		if(office.GetPathToFreeNodeOfType(type, node, ref path, isMoving))
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
	private Animator headAnimator;

	[SerializeField]
	private SpeechBubble speechBubble;
}
