using System;
using UnityEngine;

public class UserBalanceSystem : MonoBehaviour
{
    public static UserBalanceSystem Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
    }
}
