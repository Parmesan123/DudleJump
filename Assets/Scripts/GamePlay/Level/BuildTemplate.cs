using GamePlay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "StandardTemplate", menuName = "ScriptableObject/BuildTemplate/StandardTemplate")]
public class BuildTemplate : ScriptableObject
{
    [SerializeField] private PlatformType[] _usingPlatform;
    [SerializeField] private BonusItemData[] _bonusItems;
    
    [Serializable]
    private struct BonusItemData
    {
        [SerializeField] private BonusItemType _bonusItem;
        [SerializeField, Range(0, 100)] private float _chanceSpawn;

        public BonusItemType BonusItemType => _bonusItem;
        public float ChanceSpawn => _chanceSpawn / 100;
    }
    
    [SerializeField] private float _minXRange;
    [SerializeField] private float _maxXRange;

    [SerializeField] private float _minYRange;
    [SerializeField] private float _maxYRange;

    [SerializeField] private int _platformAmount;

    private Vector2 _lastPlatformPos;
    private ItemFactory _itemFactory;
    
    public int CurrentPlatformAmount { get; private set; }

    public void Init(ItemFactory itemFactory)
    {
        _itemFactory = itemFactory;
    }
    
    public void SetStartValue(Vector2 posLastPlatform)
    {
        _lastPlatformPos = posLastPlatform;
        CurrentPlatformAmount = _platformAmount;
    }
    
    public bool TrySetPlatform(Platform platform)
    {
        if (CurrentPlatformAmount <= 0)
            return false;

        Vector2 newPosition = CalculateNewPlatformsPos();

        platform.transform.position = newPosition;

        SetBonus(platform);
        
        _lastPlatformPos = newPosition;

        CurrentPlatformAmount -= 1;
        
        return true;
    }

    public PlatformType GetRandomPlatformType() => _usingPlatform[Random.Range(0, _usingPlatform.Length - 1)];

    private Vector2 CalculateNewPlatformsPos()
    {
        Camera camera = Camera.main;
        
        float yCoordinate = _lastPlatformPos.y + Random.Range(_minYRange, _maxYRange);
        float xCoordinate;
        
        float randomX = Mathf.Pow(-1, Random.Range(1, 3)) * Random.Range(_minXRange, _maxXRange);
        float screenPointX = camera.WorldToScreenPoint(new Vector2(_lastPlatformPos.x + randomX + 2, 0)).x;
        
        if (screenPointX  >= Screen.width)
        {
            xCoordinate = camera.ScreenToWorldPoint(new Vector2(screenPointX - Screen.width, 0)).x;

            return new Vector2(xCoordinate, yCoordinate);
        }

        screenPointX = camera.WorldToScreenPoint(new Vector2(_lastPlatformPos.x + randomX - 2, 0)).x;
        
        if (screenPointX <= 0)
        {
            xCoordinate = camera.ScreenToWorldPoint(new Vector2(screenPointX + Screen.width, 0)).x ;

            return new Vector2(xCoordinate, yCoordinate);
        }
        
        xCoordinate = _lastPlatformPos.x + randomX;
        
        return new Vector2(xCoordinate, yCoordinate);
    }

    private void SetBonus(Platform platform)
    {
        BonusItemData itemData = _bonusItems[Random.Range(0, _bonusItems.Length)];

        if (!(Random.Range(0f, 1f) <= itemData.ChanceSpawn)) 
            return;
        
        BonusItem item = _itemFactory.GetItem(itemData.BonusItemType);
            
        item.transform.SetParent(platform.transform);
        item.transform.localPosition = new Vector2(Random.Range(-1, 1) * 0.8f, 0.3f);
        item.SetPlatform(platform);
    }
}