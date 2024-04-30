using System;
using UnityEngine;

namespace InputSystem
{
	public class GameProfile : InputProfile
	{
		public event Action<Vector2> PlayerMovementEvent;

		public override void Update()
		{
			MovementInput();
		}

		private void MovementInput()
		{
			Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
			
			if (moveVector == Vector2.zero)
				return;
			
			PlayerMovementEvent.Invoke(moveVector);
		}
	}
}