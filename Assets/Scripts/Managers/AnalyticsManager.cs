using System.Collections.Generic;
using UnityEngine.Analytics;	// do not remove

public class AnalyticsManager
{
	const string EndlessEventKey = "endless";
	const string EndlessBackToMenuEventKey = "endless_backToMenu";
	const string ChallengePassedEventKey = "challenge_passed";
	const string ChallengeFailedEventKey = "challenge_failed";

	public static void SendEndlessStat(float playTime, float score)
	{
		Dictionary<string, object> data = new Dictionary<string, object>()
		{
			{ "play_time", GetPlayTimeInterval(playTime) },
			{ "score", GetScoreInterval(score) }
		};
		SendAnalytic(EndlessEventKey, data);
	}

	public static void SendEndlessBackToMenuStat(float playTime, float score)
	{
		Dictionary<string, object> data = new Dictionary<string, object>()
		{
			{ "play_time", GetPlayTimeInterval(playTime) },
			{ "score", GetScoreInterval(score) }
		};
		SendAnalytic(EndlessBackToMenuEventKey, data);
	}

	public static void SendChallengeStat(int level, ChallengeMode mode, bool passed)
	{
		string info = level.ToString();
		if (mode == ChallengeMode.Hardcore)
		{
			info += "H";
		}
		Dictionary<string, object> data = new Dictionary<string, object>()
		{
			{ info, 1 }
		};
		if (passed)
		{
			SendAnalytic(ChallengePassedEventKey, data);
		}
		else
		{
			SendAnalytic(ChallengeFailedEventKey, data);
		}
	}

	static void SendAnalytic(string eventKey, Dictionary<string, object> data)
	{
#if !UNITY_EDITOR
		AnalyticsEvent.Custom(eventKey, data);
#endif
	}

	static PlayTimeInterval GetPlayTimeInterval(float time)
	{
		if (time < 10f)
		{
			return PlayTimeInterval.LT10;
		}
		if (time < 15f)
		{
			return PlayTimeInterval.B10A15;
		}
		if (time < 20f)
		{
			return PlayTimeInterval.B15A20;
		}
		if (time < 30f)
		{
			return PlayTimeInterval.B20A30;
		}
		if (time < 40f)
		{
			return PlayTimeInterval.B30A40;
		}
		if (time < 50f)
		{
			return PlayTimeInterval.B40A50;
		}
		if (time < 70f)
		{
			return PlayTimeInterval.B50A70;
		}
		if (time < 100f)
		{
			return PlayTimeInterval.B70A100;
		}
		if (time < 120f)
		{
			return PlayTimeInterval.B100A120;
		}
		if (time < 150f)
		{
			return PlayTimeInterval.B120A150;
		}
		if (time < 200f)
		{
			return PlayTimeInterval.B150A200;
		}
		if (time < 250f)
		{
			return PlayTimeInterval.B200A250;
		}
		if (time < 300f)
		{
			return PlayTimeInterval.B250A300;
		}
		if (time < 400f)
		{
			return PlayTimeInterval.B300A400;
		}
		if (time < 500f)
		{
			return PlayTimeInterval.B400A500;
		}
		if (time < 600f)
		{
			return PlayTimeInterval.B500A600;
		}
		return PlayTimeInterval.MT600;
	}

	static ScoreInterval GetScoreInterval(float score)
	{
		if (score < 10f)
		{
			return ScoreInterval.LT10;
		}
		if (score < 30f)
		{
			return ScoreInterval.B10A30;
		}
		if (score < 50f)
		{
			return ScoreInterval.B30A50;
		}
		if (score < 70f)
		{
			return ScoreInterval.B50A70;
		}
		if (score < 100f)
		{
			return ScoreInterval.B70A100;
		}
		if (score < 150f)
		{
			return ScoreInterval.B100A150;
		}
		if (score < 200f)
		{
			return ScoreInterval.B150A200;
		}
		if (score < 250f)
		{
			return ScoreInterval.B200A250;
		}
		if (score < 300f)
		{
			return ScoreInterval.B250A300;
		}
		if (score < 400f)
		{
			return ScoreInterval.B300A400;
		}
		if (score < 500f)
		{
			return ScoreInterval.B400A500;
		}
		if (score < 700f)
		{
			return ScoreInterval.B500A700;
		}
		if (score < 1000f)
		{
			return ScoreInterval.B700A1000;
		}
		return ScoreInterval.MT1000;
	}

	enum PlayTimeInterval
	{
		LT10,
		B10A15,
		B15A20,
		B20A30,
		B30A40,
		B40A50,
		B50A70,
		B70A100,
		B100A120,
		B120A150,
		B150A200,
		B200A250,
		B250A300,
		B300A400,
		B400A500,
		B500A600,
		MT600
	}

	enum ScoreInterval
	{
		LT10,
		B10A30,
		B30A50,
		B50A70,
		B70A100,
		B100A150,
		B150A200,
		B200A250,
		B250A300,
		B300A400,
		B400A500,
		B500A700,
		B700A1000,
		MT1000
	}
}
