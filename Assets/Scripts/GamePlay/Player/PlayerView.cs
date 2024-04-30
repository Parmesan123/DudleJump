using InputSystem;
using System;
using UnityEngine;

namespace GamePlay
{
	public class PlayerView
	{
		private const string PLAYER_SPRITE = "GamePlay/Player/PlayerSprite";
		private const string JUMP_SPRITE = "GamePlay/Player/PlayerJumpSprite";

		private readonly Sprite _playerSprite;
		private readonly Sprite _jumpsSprite;
		
		private readonly SpriteRenderer _spriteRenderer;

		private bool _playerTurnedLeft = true;
		private bool _playerIsJump;
		
		public PlayerView(SpriteRenderer spriteRenderer, GameProfile gameProfile)
		{
			_spriteRenderer = spriteRenderer;
			
			_playerSprite = _spriteRenderer.sprite;

			_jumpsSprite = Resources.Load<Sprite>("GamePlay/Player/PlayerJumpSprite");

			if (_jumpsSprite is null)
				throw new NullReferenceException("Jump sprite is not found");

			gameProfile.PlayerMovementEvent += FlipSprite;
		}

		public void SetIsJump(float speed)
		{
			if (speed < 0 && !_playerIsJump || 
				speed > 0 && _playerIsJump)
				return;
			
			ChangeSprite();
				
		}

		private void FlipSprite(Vector2 moveVector)
		{
			if (_playerTurnedLeft && moveVector.x < 0 ||
				!_playerTurnedLeft && moveVector.x > 0)
				return;

			_playerTurnedLeft = !_playerTurnedLeft;

			_spriteRenderer.flipX = !_playerTurnedLeft;
		}
		
		private void ChangeSprite()
		{
			if (_playerIsJump)
			{
				_spriteRenderer.sprite = _jumpsSprite;
				_playerIsJump = false;
				return;
			}

			_spriteRenderer.sprite = _playerSprite;
			_playerIsJump = true;
		}
	}
}