using InputSystem;
using System;
using UnityEngine;

namespace GamePlay
{
	public class MoveHandler : MonoBehaviour
	{
		private const float VELOCITY = 9.8f;
		private readonly Vector2 _velocityDirection = Vector2.down;

		public event Action<float> MoveEvent;
		
		private Player _player;

		private PlatformsHandler _platformsHandler;
		
		private Vector2 _moveDirection;
		private float _currentSpeed;

		private bool _isInit = false;
		
		public void Init(Player player, PlatformsHandler platformsHandler, InputProvider inputProvider)
		{
			_player = player;
			_platformsHandler = platformsHandler;

			GameOverProfile profile = inputProvider.GetProfile(ProfileType.GameOverProfile) as GameOverProfile;
			profile.RestartEvent += Restart;
			
			_player.TouchPlatformEvent += PlayerTouchPlatform;
			_player.GameOverEvent += GameOver;
			
			_currentSpeed = _player.JumpSpeed;

			_isInit = true;
		}
		
		private void FixedUpdate()
		{
			if(!_isInit)
				return;
			
			CalculateSpeed();
			MoveObject();
		}

		public void SetBonusSpeed(float speed)
		{
			_currentSpeed = speed;
		}

		private void MoveObject()
		{
			if (Camera.main.WorldToScreenPoint(_player.Position).y >= (int)(Screen.height / 2) && _currentSpeed < 0)
			{
				_platformsHandler.AddMarkedPlatform();
				_platformsHandler.RemoveMarkedPlatform();
				
				foreach (Platform platform in _platformsHandler.ActivePlatforms)
					platform.Move(_velocityDirection, -_currentSpeed);
				
				MoveEvent?.Invoke(_currentSpeed * Time.fixedDeltaTime);
				
				return;
			}

			_player.Move(_velocityDirection, _currentSpeed);
		}

		private void CalculateSpeed()
		{
			_currentSpeed -= VELOCITY * _velocityDirection.y * Time.fixedDeltaTime;
		}
		
		private void PlayerTouchPlatform()
		{
			_currentSpeed = _player.JumpSpeed;
		}

		private void GameOver()
		{
			_isInit = false;
		}
		
		private void Restart()
		{
			_currentSpeed = _player.JumpSpeed;
			
			_isInit = true;
		}
	}
}