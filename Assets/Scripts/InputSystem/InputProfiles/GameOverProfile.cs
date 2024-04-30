using System;
using UnityEngine;

namespace InputSystem
{
	public class GameOverProfile: InputProfile
	{
		public event Action RestartEvent;
		
		public override void Update()
		{
			Restart();
		}

		private void Restart()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				RestartEvent.Invoke();

			ChangeProfile(ProfileType.GameProfile);
		}
	}
}