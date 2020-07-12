using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// singleton that manage the game
/// </summary>
public class GameManager : MonoBehaviour
{
    static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
