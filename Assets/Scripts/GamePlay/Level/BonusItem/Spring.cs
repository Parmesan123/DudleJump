using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay
{
	public class Spring : BonusItem
	{
		[SerializeField] private float _bonusSpeed;
		
		private MoveHandler _moveHandler;

		public void Init(MoveHandler moveHandler)
		{
			_moveHandler = moveHandler;
		}
		
		public override void Interact()
		{
			_moveHandler.SetBonusSpeed(_bonusSpeed);
		}
	}
}