using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creatable : MonoBehaviour
{
    //public event System.Action<int, int> OnHealthChanged;

    public Stat health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void TakeDamage(int damageTaken);
}
