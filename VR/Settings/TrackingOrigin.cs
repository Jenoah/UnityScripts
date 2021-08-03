using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TrackingOrigin : MonoBehaviour
{
    [SerializeField] private TrackingOriginModeFlags trackingOrigin = TrackingOriginModeFlags.Floor;

    private List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
    // Start is called before the first frame update
    void Start()
    {
        SubsystemManager.GetInstances(subsystems);
        for (int i = 0; i < subsystems.Count; i++)
        {
            subsystems[i].TrySetTrackingOriginMode(trackingOrigin);
        }
    }

}
