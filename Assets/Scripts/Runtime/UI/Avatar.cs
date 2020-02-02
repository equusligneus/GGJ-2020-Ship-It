using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
	private void Update()
	{
		if (isMoving)
		{
			var position = path.Peek().transform.position;
			transform.position = Vector3.MoveTowards(transform.position, position, 10 * Time.deltaTime);
			if ((position - transform.position).sqrMagnitude < 0.001)
				node =	path.Dequeue();
		}

		if (headAnimator)
			headAnimator.SetBool("isMoving", isMoving);

		if (sprite)
			SetSpriteZ();
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

	public void SetMood()
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

	private void SetSpriteZ()
	{
		Vector3 spritePos = sprite.localPosition;
		spritePos.z = zOffset + (transform.position.y - yZero) * yToZMultiplier;
		sprite.localPosition = spritePos;
	}

	public bool isMoving
		=> path.Count > 0;

	public Dev owner { get; private set; }
	private Office office = null;
	private Node node = null;
	private Queue<Node> path = new Queue<Node>();

	[SerializeField]
	private Transform sprite;

	[SerializeField]
	private SpriteRenderer body;

	[SerializeField]
	private SpriteRenderer head;

	[SerializeField]
	private Animator headAnimator;

	[SerializeField]
	private SpeechBubble speechBubble;

	[SerializeField]
	private float zOffset;
	[SerializeField]
	private float yZero;
	[SerializeField]
	private float yToZMultiplier;
}
