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
            if (instance == null)
                instance = GetInstance();
            return instance;
        }
    }
    public char splitChar;
    private int lastIndex = 0;
    private Dictionary<int, MainEnemy> EnemyList = new Dictionary<int, MainEnemy>();

    public override void Start()
    {
        base.Start();
        if (instance == null)
            instance = GetInstance();

        if (char.IsWhiteSpace(splitChar))
        {
            Debug.LogError("Split char is not set!!!");
            Debug.Break();
        }
    }

    private static EnemyManager GetInstance()
    {
        if (instance != null)
        {
            Debug.LogWarning("Enemymanager already exists. Old manager will be overwritten...");
        }

        EnemyManager temp = GameObject.FindObjectOfType<EnemyManager>();
        if (temp == null)
        {
            Debug.LogWarning("No Enemymanager found.");
        }



        return temp;
    }

    /// <summary>
    /// Add Ememy to List
    /// </summary>
    /// <param name="_enemy">Enemy to add</param>
    /// <returns>Index of Enemy</returns>
    public int AddEnemy(MainEnemy _enemy)
    {
        _enemy.gameObject.name += splitChar + lastIndex.ToString();
        EnemyList.Add(lastIndex++, _enemy);
        return lastIndex - 1;

    }
    
    /// <summary>
    /// Remove Enemy by Index
    /// </summary>
    /// <param name="_index">Index of Enemy</param>
    /// <returns>return True if Enemy could be removes successfully</returns>
    public bool RemoveEnemy(int _index)
    {
        return EnemyList.Remove(_index);
    }


    #region Get Enemy Methods

    /// <summary>
    /// Get Enemy by Name
    /// </summary>
    /// <param name="_name">Name of Gameobject</param>
    /// <returns>If no Enemy could be found return null, else return an Enemy</returns>
    public MainEnemy GetEnemy(string _name)
    {
        int index = GetIndex(_name);
        if (index < 0) return null;
        return EnemyList[index];
    }

    /// <summary>
    /// Get Enemy by GameObject
    /// </summary>
    /// <param name="_gameObject">Enemy</param>
    /// <returns>If no Enemy could be found return null, else return an Enemy</returns>
    public MainEnemy GetEnemy(GameObject _gameObject)
    {
        return GetEnemy(_gameObject.name);
    }
    #endregion


    #region Get Index

    /// <summary>
    /// Get Index by Name
    /// </summary>
    /// <param name="_name">Name of Gameobject</param>
    /// <returns>return Index of <see cref="EnemyList"/>. If no Enemy could be found return -1</returns>
    public int GetIndex(string _name)
    {
        string[] splitName = _name.Split(splitChar);
        if (splitName.Length < 1) return -1;
        string keyString = splitName[splitName.Length - 1];

        bool isNumber = int.TryParse(keyString, out int index);

        if (!isNumber) return -1;

        return index;
    }

    /// <summary>
    /// Get Index by GameObject
    /// </summary>
    /// <param name="_gameObject">GameObject of Enemy</param>
    /// <returns>return Index of <see cref="EnemyList"/>. If no Enemy could be found return -1</returns>
    public int GetIndex(GameObject _gameObject)
    {
        return GetIndex(_gameObject.name);
    }

    /// <summary>
    /// Get Index by Enemy
    /// </summary>
    /// <param name="_enemy">Enemy class</param>
    /// <returns>return Index of <see cref="EnemyList"/>. If no Enemy could be found or if enemy is null return -1</returns>
    public int GetIndex(MainEnemy _enemy)
    {
        if (_enemy == null) return -1;
        foreach (KeyValuePair<int, MainEnemy> item in EnemyList)
        {
            if (item.Value == _enemy)
                return item.Key;
        }

        return -1;
    }
    #endregion


}
