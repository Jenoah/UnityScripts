using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FixedDeltaToRefreshRate : MonoBehaviour
{
    void Start()
    {
        float headsetRefreshRate = 0;

        List<XRDisplaySubsystem> subsystems = new List<XRDisplaySubsystem>();

        SubsystemManager.GetInstances(subsystems);

        for (int i = 0; i < subsystems.Count; i++)
        {
            subsystems[i].TryGetDisplayRefreshRate(out headsetRefreshRate);
        }


    Time.fixedDeltaTime = 1f / headsetRefreshRate;

        Debug.Log("FixedDeltaTime set to " + Time.fixedDeltaTime  + " as of " + headsetRefreshRate + "Hz");
    }
}
