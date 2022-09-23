using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
	public class GameUIManager : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI levelT;
		[SerializeField] private TextMeshProUGUI timeT;
		[SerializeField] private TextMeshProUGUI scoreT;

		private string levelTformat;
		private string timeTformat;
		private string scoreTformat;

		private void Awake()
		{
			levelTformat = levelT.text;
			timeTformat = timeT.text;
			scoreTformat = scoreT.text;
		}

		public void SetLevel(int value)
		{
			levelT.text = string.Format(levelTformat, value);
		}

		public void SetTime(int min, int sec)
		{
			timeT.text = string.Format(timeTformat, min, sec);
		}

		public void SetScore(int value)
		{
			scoreT.text = string.Format(scoreTformat, value);
		}
	}
}