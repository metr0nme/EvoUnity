using System;
using UnityEngine;

public class ClientEventManager : MonoBehaviour
{

    public static ClientEventManager current;

    private void Awake()
    {
        current = this;
    }

    public event Action<float[]> onFireShake;
    public void FireShake(float[] vectorArray)
    {
        if(onFireShake != null)
            onFireShake(vectorArray);   
    }

}
