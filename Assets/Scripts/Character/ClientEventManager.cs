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

    public event Action<Vector3> onVMFireShake;
    public void VMFireShake(Vector3 push)
    {
        if(onVMFireShake != null)
            onVMFireShake(push);
    }


}
