using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SpeechBubble : MonoBehaviour
{
	private void Awake()
	{
		animator = GetComponent<Animator>();
		remainingViewTime = -1;
	}

	public void PostMood(Mood mood)
	{
		Mood.Config config = mood.GetConfig();
		if (bubble)
			bubble.sprite = config.bubble;

		if (content)
			content.sprite = config.content;

		remainingViewTime = viewTime;
		animator.SetBool("Show", true);
	}

    // Update is called once per frame
    void Update()
    {
        if(remainingViewTime > 0)
		{
			remainingViewTime -= Time.deltaTime;
			if (remainingViewTime <= 0)
				animator.SetBool("Show", false);
		}
    }

	[SerializeField]
	private Image bubble;
	[SerializeField]
	private Image content;
	[SerializeField]
	private float viewTime;

	private float remainingViewTime;
	private Animator animator;
}
