using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpeechBubble : MonoBehaviour
{
	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private Animator animator;
}
