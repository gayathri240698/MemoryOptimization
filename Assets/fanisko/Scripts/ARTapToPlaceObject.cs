using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Video;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]

    public GameObject objectToPlace;
    public GameObject placementIndicator;     private Pose placementPose;     private bool placementPoseIsValid = false;
    private bool indicatorDisplay = true;

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

    private void Start()
    {
        objectToPlace.SetActive(false);
    }

    void Update()
    {
        if (indicatorDisplay)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();
        }          if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)         {             PlaceObject();         }

       
    }

    private void PlaceObject()     {
        objectToPlace.transform.position = placementPose.position;
        if (objectToPlace.GetComponentInChildren<VideoPlayer>() != null)
        {
            objectToPlace.GetComponentInChildren<VideoPlayer>().Play();
        }
        objectToPlace.SetActive(true);
        indicatorDisplay = false;
        placementIndicator.SetActive(false);     } 


    private void UpdatePlacementPose()
    {
     
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));         var hits = new List<ARRaycastHit>();         m_RaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);          placementPoseIsValid = hits.Count > 0;          if (placementPoseIsValid)         {             placementPose = hits[0].pose;              var cameraForward = Camera.main.transform.forward;             var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;             placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        } 
    }

    private void UpdatePlacementIndicator()
    {         if (placementPoseIsValid)         {             placementIndicator.SetActive(true);             placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);         }         else         {
            placementIndicator.SetActive(false);         }     }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}
