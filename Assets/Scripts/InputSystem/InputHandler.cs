using GamePlay;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace InputSystem 
{
	public class InputHandler : MonoBehaviour
	{
		private InputProvider _inputProvider;
		[CanBeNull] private InputProfile _currentProfile;

		public void Init(InputProvider inputProvider)
		{
			_inputProvider = inputProvider;

			foreach (InputProfile inputProfile in inputProvider.InputProfiles)
				inputProfile.ChangeProfileEvent += ChangeProfile;

			_currentProfile = _inputProvider.GetProfile(ProfileType.GameProfile);
			_currentProfile.ChangeProfileEvent += ChangeProfile;
		}

		public void InitGameOver(Player player)
		{
			player.GameOverEvent += GameOver;
		}
		
		private void FixedUpdate()
		{
			_currentProfile?.Update();
		}

		private void ChangeProfile(ProfileType newInputProfile)
		{
			_currentProfile.ChangeProfileEvent -= ChangeProfile;
			_currentProfile = _inputProvider.GetProfile(newInputProfile);
			_currentProfile.ChangeProfileEvent += ChangeProfile;
		}

		private void GameOver()
		{
			ChangeProfile(ProfileType.GameOverProfile);
		}
	}
}