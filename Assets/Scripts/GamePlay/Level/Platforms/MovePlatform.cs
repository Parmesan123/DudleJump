using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay
{
	public class MovePlatform : Platform
	{
		[SerializeField] private float _minSpeedValue;
		[SerializeField] private float _maxSpeedValue;
		
		private float _leftY;
		private float _rightY;
		private float _currentSpeed;
		
		private void FixedUpdate()
		{
			HorizontalMove();
		}

		private void HorizontalMove()
		{
			transform.position = Position + _currentSpeed * Time.fixedDeltaTime * Vector2.right;

			if (Position.x >= _rightY || Position.x <= _leftY)
				_currentSpeed = -_currentSpeed;
		}

		public override void SetPlatformActive(bool value)
		{
			base.SetPlatformActive(value);
			
			if (!value) 
				return;
			
			_currentSpeed = Random.value * (_maxSpeedValue - _maxSpeedValue) + _minSpeedValue;
			_leftY = Random.value * Screen.width;
			_rightY = Random.value * (Screen.width - _leftY) + _leftY;
		}
	}
}