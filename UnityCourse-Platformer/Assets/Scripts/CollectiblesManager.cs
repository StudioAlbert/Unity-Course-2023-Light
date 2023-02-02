using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CollectiblesManager : MonoBehaviour
{

    private List<Collectible> _coins;

    private int _pickedUpCount;

    [SerializeField] private UnityEvent _allCoinsCollected;
    [SerializeField] private TextMeshProUGUI _coinCounter;

    [SerializeField] private Inventory _inventory;
    
    // Start is called before the first frame update
    void Start()
    {
        _coins = GetComponentsInChildren<Collectible>().ToList();
        foreach (Collectible coin in _coins)
        {
            coin.OnPickup += HandlePickup;
        }
    }

    private void OnDisable()
    {
        foreach (Collectible coin in _coins)
        {
            coin.OnPickup -= HandlePickup;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandlePickup()
    {
        _pickedUpCount++;
        _coinCounter.text = _pickedUpCount.ToString();
        
        if (_pickedUpCount >= _coins.Count)
            _allCoinsCollected.Invoke();

    }

    public void SetScore()
    {
        _inventory.AddCoins(_pickedUpCount);
    }
}
