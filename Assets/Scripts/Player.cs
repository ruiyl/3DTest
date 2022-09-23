using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private float speed;

		private Rigidbody rb;
		private Vector2 inputDir;

		private void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			rb.velocity = speed * ((transform.forward * inputDir.y) + (transform.right * inputDir.x));
		}

		public void Move(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
			{
				Vector2 value = context.ReadValue<Vector2>();
				inputDir = value;
			}
		}
	}
}