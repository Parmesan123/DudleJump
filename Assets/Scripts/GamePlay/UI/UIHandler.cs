using InputSystem;
using System;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace GamePlay.UI
{
	public class UIHandler : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _scoreUi;
		[SerializeField] private GameObject _gameOverPanel;

		private StringBuilder _stringBuilder;
		private MoveHandler _moveHandler;
		private Player _player;
		private float _currentScore;
		
		public void Init(MoveHandler moveHandler, Player player, InputProvider inputProvider)
		{
			_moveHandler = moveHandler;
			_player = player;
			
			GameOverProfile profile = inputProvider.GetProfile(ProfileType.GameOverProfile) as GameOverProfile;
			profile.RestartEvent += Restart;
			
			_moveHandler.MoveEvent += UpdateScore;
			_player.GameOverEvent += ShowGameOverPanel;

			_scoreUi.text = "Score: " + 0;
		}
		
		private void UpdateScore(float value)
		{
			if(value > 0)
				return;
			
			_currentScore += Mathf.Abs(value);

			_scoreUi.text = "Score: " + (int)(_currentScore + 1);
		}

		private void ShowGameOverPanel()
		{
			_gameOverPanel.gameObject.SetActive(true);
		}
		
		private void Restart()
		{
			_gameOverPanel.gameObject.SetActive(false);
			_currentScore = 0;
		}
	}
}