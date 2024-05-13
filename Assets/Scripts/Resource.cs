using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool IsBusy { get; private set; } = false;

    public void SetBusy()
    {
        IsBusy = true;
    }

    public void SetFree()
    {
        IsBusy = false;
    }
}
