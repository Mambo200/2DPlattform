using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MainScript
{
    private static EnemyManager instance = null;
    public static EnemyManager Get
    {
        get
        {
            return instance;
        }
    }
    public char splitChar;
    private int lastIndex = 0;
    private Dictionary<int, MainEnemy> EnemyList = new Dictionary<int, MainEnemy>();

    public override void Start()
    {
        base.Start();
        if(instance != null)
        {
            Debug.LogWarning("Enemymanager already exists. Old manager will be overwritten...");
        }

        instance = GameObject.FindObjectOfType<EnemyManager>();
        if(instance == null)
        {
            Debug.LogWarning("No Enemymanager found.");
        }

        if (char.IsWhiteSpace(splitChar))
        {
            Debug.LogError("Split char is not set!!!");
            Debug.Break();
        }
    }

    public int AddEnemy(MainEnemy _enemy)
    {
        _enemy.gameObject.name += splitChar + lastIndex.ToString();
        EnemyList.Add(lastIndex++, _enemy);
        return lastIndex - 1;

    }
    
    public bool RemoveEnemy(int _index)
    {
        return EnemyList.Remove(_index);
    }
    

    #region Get Enemy Methods
    public MainEnemy GetEnemy(string _name)
    {
        int index = GetIndex(_name);
        if (index < 0) return null;
        return EnemyList[index];
    }

    public MainEnemy GetEnemy(GameObject _gameObject)
    {
        return GetEnemy(_gameObject.name);
    }
    #endregion


    #region Get Index
    public int GetIndex(string _name)
    {
        string[] splitName = _name.Split(splitChar);
        if (splitName.Length < 1) return -1;
        string keyString = splitName[splitName.Length - 1];

        bool isNumber = int.TryParse(keyString, out int index);

        if (!isNumber) return -1;

        return index;
    }

    public int GetIndex(GameObject _gameObject)
    {
        return GetIndex(_gameObject.name);
    }

    public int GetIndex(MainEnemy _enemy)
    {
        foreach (KeyValuePair<int, MainEnemy> item in EnemyList)
        {
            if (item.Value == _enemy)
                return item.Key;
        }

        return -1;
    }
    #endregion


}
