using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SpeechBubble : MonoBehaviour
{
	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void PostMood(Mood mood)
	{
		Mood.Config config = mood.GetConfig();
		if (bubble)
			bubble.sprite = config.bubble;

		if (content)
			content.sprite = config.content;

		animator.SetBool("Show", true);
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	[SerializeField]
	private Image bubble;
	[SerializeField]
	private Image content;

	private Animator animator;
}
