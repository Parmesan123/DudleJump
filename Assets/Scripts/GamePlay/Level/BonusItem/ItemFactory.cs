using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GamePlay
{
	public class ItemFactory
	{
		private const string SPRING_PATH = "GamePlay/Level/Spring";

		private readonly Spring _springPrefab;
		
		private readonly MoveHandler _moveHandler;
		private readonly Player _player;

		public ItemFactory(MoveHandler moveHandler, Player player)
		{
			_moveHandler = moveHandler;
			_player = player;

			_springPrefab = Resources.Load<Spring>(SPRING_PATH);

			if (_springPrefab is null)
				throw new NullReferenceException("Spring prefab is not found");
		}

		public BonusItem GetItem(BonusItemType itemType)
		{
			switch (itemType)
			{
				case BonusItemType.Spring:
					Spring item = Object.Instantiate(_springPrefab);
					item.Init(_moveHandler);
					return item;
				default:
					throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null);
			}
		}
	}
}