using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
	public class Collectable : MonoBehaviour
	{
		[SerializeField] private int score;
		[SerializeField] private CollectableType type;

		private bool isHit;

		public CollectableType Type { get => type; }

		public UnityAction<int, CollectableType> CollectedEvent;

		public enum CollectableType
		{
			None,
			Sphere,
			Capsule,
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.layer == LayerMask.NameToLayer(GameManager.PlayerLayerName))
			{
				CollectedEvent?.Invoke(score, type);
				Destroy();
			}
		}

		public void SetPosition(float x, float z)
		{
			transform.position = new Vector3(x, transform.position.y, z);
		}

		public void Destroy()
		{
			Destroy(gameObject);
		}
	}
}