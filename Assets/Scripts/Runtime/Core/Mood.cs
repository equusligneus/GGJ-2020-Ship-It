using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mood
{
	public enum ConditionType
	{
		Inactive,
		ValueGreaterThan,
		ValueLowerThan,
	}

	[System.Serializable]
	public struct FollowUpMood
	{
		public ConditionType annoyanceCondition;
		public float annoyanceValue;
		public Config_Mood mood;

		public bool CanChange(float annoyanceValue)
		{
			switch (annoyanceCondition)
			{
				case ConditionType.ValueGreaterThan:
					return annoyanceValue > this.annoyanceValue;
				case ConditionType.ValueLowerThan:
					return annoyanceValue < this.annoyanceValue;
				default:
					return true;
			}
		}
	}

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
		public float speedMultiplier;	
		public AnimationCurve MoodChangePropabilityCurve;

		public FollowUpMood[] followUps;
	}

	static Mood()
	{
		s_random = new System.Random();
	}

	public Mood(Config config)
	{
		this.config = config;
	}

	public Mood Tick(float annoyanceValue)
	{
		++ticks;

		if (s_random.Next() < config.MoodChangePropabilityCurve.Evaluate(ticks))
			return ChangeMood(annoyanceValue);

		return this;
	}

	private Mood ChangeMood(float annoyanceValue)
	{
		foreach (var item in config.followUps)
		{
			if (item.CanChange(annoyanceValue))
				return new Mood(item.mood.config);
		}
		return this;
	}

	public Config GetConfig()
		=> config;

	public string Name
		=> config.name;

	private static System.Random s_random;

	private Config config;
	private int ticks;


}
