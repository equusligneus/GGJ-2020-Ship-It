using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mood
{
	public enum ConditionType
	{
		Any,
		ValueGreaterThan,
		ValueLowerThan,
	}

	[System.Serializable]
	public struct FollowUpMood
	{
		public ConditionType motivationCondition;
		public float motivationValue;
		public ConditionType focusCondition;
		public float focusValue;
		public Config_Mood mood;

		public bool CanChange(float motivation, float focus)
		{
			return IsValid(motivationCondition, motivation, motivationValue) &&
				IsValid(focusCondition, focus, focusValue);
		}

		private bool IsValid(ConditionType type, float value1, float value2)
		{
			switch (type)
			{
				case ConditionType.ValueGreaterThan:
					return value1 > value2;
				case ConditionType.ValueLowerThan:
					return value1 < value2;
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

	public Mood Tick(float motivation, float focus)
	{
		++ticks;

		if (s_random.Next() < config.MoodChangePropabilityCurve.Evaluate(ticks))
			return ChangeMood(motivation, focus);

		return this;
	}

	private Mood ChangeMood(float motivation, float focus)
	{
		foreach (var item in config.followUps)
		{
			if (item.CanChange(motivation, focus))
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
