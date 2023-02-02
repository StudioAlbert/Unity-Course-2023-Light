using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerInventory/Inventory", fileName = "new inventory")]
public class Inventory : ScriptableObject, IDeserializationCallback
{
    private int _coins;
    public int Coins => _coins;
    public void AddCoins(int coins)
    {
        _coins += coins;
    }
    public void RemoveCoins(int coins)
    {
        _coins -= coins;
        if (_coins < 0)
            _coins = 0;
    }

    public void OnDeserialization(object sender)
    {
        _coins = 0;
    }
}
