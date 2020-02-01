using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mood
{
	[System.Serializable]
	public struct Config
	{
		public string name;
		public Sprite bubble;
		public Sprite content;

		public float skillMultiplier;
		public float annoyanceIncreaseMultiplier;
		public float annoyanceDecreaseMultiplier;
		public float attentionMultiplier;
		public AnimationCurve MoodChangePropabilityCurve;
	}

	public Mood(Config config)
	{
		this.config = config;
	}

	private Config config;
}
