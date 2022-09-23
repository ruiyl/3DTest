using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
	public class Collectable : MonoBehaviour
	{
		[SerializeField] private int score;
		[SerializeField] private float lifeTimeAfterHit;
		[SerializeField] CollectableType type;

		private bool isHit;

		public UnityAction<int, CollectableType> DestroyEvent;

		public enum CollectableType
		{
			None,
			Sphere,
			Capsule,
		}

		private void Update()
		{
			if (isHit)
			{
				lifeTimeAfterHit -= Time.deltaTime;
				if (lifeTimeAfterHit <= 0f)
				{
					DestroySelf();
				}
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.layer == LayerMask.NameToLayer(GameManager.PlayerLayerName))
			{
				isHit = true;
			}
		}

		public void SetPosition(float x, float z)
		{
			transform.position = new Vector3(x, transform.position.y, z);
		}

		private void DestroySelf()
		{
			DestroyEvent?.Invoke(score, type);
			Destroy(gameObject);
		}
	}
}