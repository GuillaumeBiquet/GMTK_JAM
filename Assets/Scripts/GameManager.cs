using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [SerializeField] int maxShips = 100;
    [System.NonSerialized] public int nbShips = 0;


    public bool TooManyShips { get { return nbShips >= maxShips; } }


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }




}
