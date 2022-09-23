using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private int targetScore;
		[SerializeField] private int scorePerLevel;
		[SerializeField] private GameUIManager uiManager;
		[SerializeField] private Collectable[] collectablePrefabs;
		[SerializeField] private Transform floor;
		[SerializeField] private LayerMask overlapCheck;

		private int currentLevel;
		private float startTime;
		private int currentScore;
		private Collectable.CollectableType lastCollectedItem;

		public static string PlayerLayerName = "Player";

		private const int SEC_IN_MIN = 60;
		private const int LEVEL_COUNT = 3;

		private void Start()
		{
			startTime = Time.time;
			uiManager.SetLevel(currentLevel + 1);
			uiManager.SetScore(currentScore);
			SpawnItem();
			SpawnItem();
			SpawnItem();
		}

		private void Update()
		{
			int elapsedSec = Mathf.FloorToInt(Time.time - startTime);
			uiManager.SetTime(elapsedSec / SEC_IN_MIN, elapsedSec % SEC_IN_MIN);
		}

		private void SpawnItem()
		{
			Collectable newItem = Instantiate(RandomItem());
			Vector2 pos = GetEmptyPos(1f);
			newItem.SetPosition(pos.x, pos.y);
			newItem.DestroyEvent += OnCollectedItem;
		}

		private Vector2 GetEmptyPos(float radius)
		{
			float boundX = floor.localScale.x / 2f;
			float boundZ = floor.localScale.z / 2f;
			Vector2 pos;
			do
			{
				pos = new Vector2(Random.Range(-boundX, boundX), Random.Range(-boundZ, boundZ));
			}
			while (Physics.OverlapSphere(new Vector3(pos.x, 0.5f, pos.y), radius, overlapCheck).Length > 0);
			return pos;
		}

		private void OnCollectedItem(int score, Collectable.CollectableType type)
		{
			if (type == lastCollectedItem)
			{
				score *= -2;
			}
			lastCollectedItem = type;
			currentScore += score;
			uiManager.SetScore(currentScore);
			if (currentScore >= targetScore)
			{
				EndGame();
			}
			else
			{
				if (currentScore / scorePerLevel > currentLevel && currentLevel < LEVEL_COUNT - 1)
				{
					GoToNextLevel();
				}
				SpawnItem();
			}
		}

		private void GoToNextLevel()
		{
			uiManager.SetLevel(currentLevel + 1);
		}

		private void EndGame()
		{

		}

		public void QuitGame()
		{
			Application.Quit();
		}

		private Collectable RandomItem()
		{
			int level = Random.Range(0, currentLevel);
			int type = Random.Range(0f, 1f) > 0.5f ? 0 : 1;
			return collectablePrefabs[type * LEVEL_COUNT + level];
		}
	}
}