/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using UnityEngine.Events;
using Vuforia;

/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class videoplayback : MonoBehaviour
{
	public enum TrackingStatusFilter
    {
        Tracked,
        Tracked_ExtendedTracked,
        Tracked_ExtendedTracked_Limited
    }
	public TrackingStatusFilter StatusFilter = TrackingStatusFilter.Tracked_ExtendedTracked_Limited;
    public UnityEvent OnTargetFound;
    public UnityEvent OnTargetLost;
	
	protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;
    protected TrackableBehaviour.StatusInfo m_PreviousStatusInfo;
    protected TrackableBehaviour.StatusInfo m_NewStatusInfo;
    protected bool m_CallbackReceivedOnce = false;
	
    public UnityEngine.Video.VideoPlayer videoPlayer;
    
    
    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();

        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterOnTrackableStatusChanged(OnTrackableStatusChanged);
            mTrackableBehaviour.RegisterOnTrackableStatusInfoChanged(OnTrackableStatusInfoChanged);
        }
    }
	
	protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.UnregisterOnTrackableStatusInfoChanged(OnTrackableStatusInfoChanged);
            mTrackableBehaviour.UnregisterOnTrackableStatusChanged(OnTrackableStatusChanged);
        }
    }
	
	void OnTrackableStatusChanged(TrackableBehaviour.StatusChangeResult statusChangeResult)
    {
        m_PreviousStatus = statusChangeResult.PreviousStatus;
        m_NewStatus = statusChangeResult.NewStatus;

        Debug.LogFormat("Trackable {0} {1} -- {2}",
            mTrackableBehaviour.TrackableName,
            mTrackableBehaviour.CurrentStatus,
            mTrackableBehaviour.CurrentStatusInfo);

        HandleTrackableStatusChanged();
    }

    void OnTrackableStatusInfoChanged(TrackableBehaviour.StatusInfoChangeResult statusInfoChangeResult)
    {
        m_PreviousStatusInfo = statusInfoChangeResult.PreviousStatusInfo;
        m_NewStatusInfo = statusInfoChangeResult.NewStatusInfo;
        
        HandleTrackableStatusInfoChanged();
    }

    protected virtual void HandleTrackableStatusChanged()
    {
        if (!ShouldBeRendered(m_PreviousStatus) &&
            ShouldBeRendered(m_NewStatus))
        {
            OnTrackingFound();
        }
        else if (ShouldBeRendered(m_PreviousStatus) &&
                 !ShouldBeRendered(m_NewStatus))
        {
            OnTrackingLost();
        }
        else
        {
            if (!m_CallbackReceivedOnce && !ShouldBeRendered(m_NewStatus))
            {
                // This is the first time we are receiving this callback, and the target is not visible yet.
                // --> Hide the augmentation.
                OnTrackingLost();
            }
        }

        m_CallbackReceivedOnce = true;
    }

    protected virtual void HandleTrackableStatusInfoChanged()
    {
        if (m_NewStatusInfo == TrackableBehaviour.StatusInfo.WRONG_SCALE)
        {
            Debug.LogErrorFormat("The target {0} appears to be scaled incorrectly. " + 
                                 "This might result in tracking issues. " + 
                                 "Please make sure that the target size corresponds to the size of the " + 
                                 "physical object in meters and regenerate the target or set the correct " +
                                 "size in the target's inspector.", mTrackableBehaviour.TrackableName);
        }
    }

    protected bool ShouldBeRendered(TrackableBehaviour.Status status)
    {
        if (status == TrackableBehaviour.Status.DETECTED ||
            status == TrackableBehaviour.Status.TRACKED)
        {
            // always render the augmentation when status is DETECTED or TRACKED, regardless of filter
            return true;
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked)
        {
            if (status == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                // also return true if the target is extended tracked
                return true;
            }
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked_Limited)
        {
            if (status == TrackableBehaviour.Status.EXTENDED_TRACKED ||
                status == TrackableBehaviour.Status.LIMITED)
            {
                // in this mode, render the augmentation even if the target's tracking status is LIMITED.
                // this is mainly recommended for Anchors.
                return true;
            }
        }

        return false;
    }

    
	
	 
    

    protected virtual void OnTrackingFound()
    {
        videoPlayer.Play();
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
    }


    protected virtual void OnTrackingLost()
    {
        videoPlayer.Stop();
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

   
}