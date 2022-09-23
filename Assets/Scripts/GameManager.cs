using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private int targetScore;
		[SerializeField] private int scorePerLevel;
		[SerializeField] private float cubeSpawnInterval;
		[SerializeField] private GameUIManager uiManager;
		[SerializeField] private Collectable[] spherePrefabs;
		[SerializeField] private Collectable[] capsulePrefabs;
		[SerializeField] private Transform cubePrefab;
		[SerializeField] private Transform floor;
		[SerializeField] private Player player;
		[SerializeField] private LayerMask overlapCheck;

		private int currentLevel;
		private float startTime;
		private int currentScore;
		private Collectable.CollectableType lastCollectedItem;
		private List<Collectable> spawnedItems;
		private bool refreshItemTrigger;
		private float spawnCubeTime;

		public static float[] LevelScales = new float[] { 0.5f, 0.75f, 1f };
		public static float[] FloorLevelScales = new float[] { 5f, 7.5f, 10f };
		public static string PlayerLayerName = "Player";

		private const int SEC_IN_MIN = 60;
		private const int LEVEL_COUNT = 3;

		private void Start()
		{
			spawnedItems = new List<Collectable>();
			startTime = Time.time;
			uiManager.SetLevel(currentLevel + 1);
			uiManager.SetScore(currentScore);
			SpawnItem(GetEmptyPos(1f));
			SpawnItem(GetEmptyPos(1f));
			SpawnItem(GetEmptyPos(1f));

			spawnCubeTime = Time.time + cubeSpawnInterval;
		}

		private void Update()
		{
			int elapsedSec = Mathf.FloorToInt(Time.time - startTime);
			uiManager.SetTime(elapsedSec / SEC_IN_MIN, elapsedSec % SEC_IN_MIN);
			if (refreshItemTrigger)
			{
				refreshItemTrigger = false;
				RefreshItem();
			}
			if (Time.time > spawnCubeTime)
			{
				SpawnCube(GetEmptyPos(1f));
				spawnCubeTime += cubeSpawnInterval;
			}
		}

		private void SpawnItem(Vector2 pos, Collectable prefab = null)
		{
			Collectable newItem = Instantiate(prefab == null ? RandomItem() : prefab);
			newItem.SetPosition(pos.x, pos.y);
			newItem.CollectedEvent += OnCollectedItem;

			spawnedItems.Add(newItem);
		}

		private void SpawnCube(Vector2 pos)
		{
			Transform newCube = Instantiate(cubePrefab);
			newCube.position = new Vector3(pos.x, newCube.position.y, pos.y);
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
				SpawnItem(GetEmptyPos(1f));
			}
		}

		private void GoToNextLevel()
		{
			currentLevel++;
			player.ScaleByLevel(currentLevel);
			floor.localScale = new Vector3(FloorLevelScales[currentLevel], 1f, FloorLevelScales[currentLevel]);
			uiManager.SetLevel(currentLevel + 1);
			refreshItemTrigger = true;
		}

		private void RefreshItem()
		{
			for (int i = spawnedItems.Count - 1; i >= 0; i--)
			{
				Collectable item = spawnedItems[i];
				spawnedItems.RemoveAt(i);
				if (item == null)
				{
					continue;
				}
				switch (item.Type)
				{
					case Collectable.CollectableType.Sphere:
						SpawnItem(item.transform.position, spherePrefabs[currentLevel]);
						break;
					case Collectable.CollectableType.Capsule:
						SpawnItem(item.transform.position, capsulePrefabs[currentLevel]);
						break;
				}
				item.Destroy();
			}
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
			int level = currentLevel;
			if (Random.Range(0f, 1f) > 0.5f)
			{
				return spherePrefabs[level];
			}
			else
			{
				return capsulePrefabs[level];
			}
		}
	}
}