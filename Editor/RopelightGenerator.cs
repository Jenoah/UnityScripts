using UnityEngine;
using UnityEditor;

public class RopelightGenerator : EditorWindow
{
    [Header("Rope properties")]
    [SerializeField, Tooltip("How many segments should the rope be divided in? More results into more realism at the cost of performance")]
    private int ropeSegments = 5;
    [SerializeField, Tooltip("What material should the rope have?")]
    private Material ropeMaterial = null;
    [SerializeField, Tooltip("How thick should the rope be?")]
    private float ropeThickness = 0.05f;

    [Space(20), Header("Light properties")]
    [SerializeField, Tooltip("How many lights should there be over the entire rope?")]
    private int lightAmount = 10;
    [SerializeField, Tooltip("Which model should the light be made out of? Include the light source in here")]
    private GameObject lightPrefab = null;
    [SerializeField, Tooltip("How far away from the rope segment should the light be placed? Mostly the ropethickness + lightPrefab size")]
    private Vector3 lightOffset = Vector3.zero;

    [Space(20), Header("Attach points")]
    [SerializeField]
    private Transform beginPoint = null;
    [SerializeField]
    private Transform endPoint = null;

    private Rigidbody beginPointRigidbody = null;
    private Rigidbody endPointRigidbody = null;
    private Rigidbody prevRigidbody = null;
    private Joint segmentJoint = null;

    [MenuItem("Tools/Ropelight")]
    public static void ShowRopelightWindow()
    {
        GetWindow(typeof(RopelightGenerator)).minSize = new Vector2(256, 256);
    }

    void OnGUI()
    {
        GUILayout.Label("Rope properties", EditorStyles.boldLabel);
        ropeSegments = EditorGUILayout.IntField("Rope segments", ropeSegments);
        ropeMaterial = (Material)EditorGUILayout.ObjectField("Rope material", ropeMaterial, typeof(Material), false);
        ropeThickness = EditorGUILayout.FloatField("Rope Thickness", ropeThickness);

        GUILayout.FlexibleSpace();
        GUILayout.Space(20);
        GUILayout.Label("Light properties");
        lightAmount = EditorGUILayout.IntField("Light amount", lightAmount);
        lightPrefab = (GameObject)EditorGUILayout.ObjectField("Light prefab", lightPrefab, typeof(GameObject), false);
        lightOffset = EditorGUILayout.Vector3Field("Light offset", lightOffset);

        GUILayout.FlexibleSpace();
        GUILayout.Space(20);
        GUILayout.Label("Attach points");
        beginPoint = (Transform)EditorGUILayout.ObjectField("Begin point", beginPoint, typeof(Transform), true);
        endPoint = (Transform)EditorGUILayout.ObjectField("End point", endPoint, typeof(Transform), true);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Generate light"))
        {
            GenerateLight();
        }
    }

    public void GenerateLight()
    {
        if (beginPoint == null || endPoint == null || ropeSegments < 0 || beginPoint == endPoint)
        {
            Debug.LogError("Not all required fields are filled in or some fields contain values that are not allowed");
            return;
        }

        //Creating anchor points
        GameObject ropeStartPoint = new GameObject("Rope start point");
        GameObject ropeEndPoint = new GameObject("Rope end point");

        ropeStartPoint.transform.position = beginPoint.position;
        ropeEndPoint.transform.position = endPoint.position;

        if (!ropeStartPoint.TryGetComponent(out beginPointRigidbody))
        {
            beginPointRigidbody = ropeStartPoint.gameObject.AddComponent<Rigidbody>();
        }
        if (!ropeEndPoint.TryGetComponent(out endPointRigidbody))
        {
            endPointRigidbody = ropeEndPoint.gameObject.AddComponent<Rigidbody>();
        }

        beginPointRigidbody.isKinematic = true;
        endPointRigidbody.isKinematic = true;

        //Parent setup
        GameObject ropeParent = new GameObject("RopeLight");
        ropeParent.transform.position = Vector3.Lerp(ropeStartPoint.transform.position, ropeEndPoint.transform.position, 0.5f);
        ropeStartPoint.transform.SetParent(ropeParent.transform);
        ropeEndPoint.transform.SetParent(ropeParent.transform);

        //Defining angles, lengths and positions
        float segmentLength = Vector3.Distance(ropeStartPoint.transform.position, ropeEndPoint.transform.position) / ropeSegments;
        Vector3 direction = ropeStartPoint.transform.position - ropeEndPoint.transform.position;
        float segmentAngleY = Vector3.SignedAngle(direction, Vector3.forward, Vector3.up);
        direction = ropeParent.transform.InverseTransformDirection(direction);
        float segmentAngleX = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        int lightsPerSegment = Mathf.CeilToInt(lightAmount / ropeSegments);
        for (int i = 0; i < ropeSegments; i++)
        {
            GameObject ropeSegment = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ropeSegment.transform.localScale = new Vector3(ropeThickness, ropeThickness, segmentLength);
            ropeSegment.transform.eulerAngles = new Vector3(segmentAngleX, -segmentAngleY, 180);
            ropeSegment.transform.position = Vector3.Lerp(ropeStartPoint.transform.position, ropeEndPoint.transform.position, (1f / ropeSegments) * (i + 0.5f));
            if (ropeMaterial != null)
            {
                ropeSegment.GetComponent<Renderer>().material = ropeMaterial;
            }
            segmentJoint = ropeSegment.AddComponent<HingeJoint>();
            segmentJoint.autoConfigureConnectedAnchor = true;
            segmentJoint.anchor = new Vector3(0, 0, -0.5f);
            Rigidbody currentRopeRigidBody = ropeSegment.GetComponent<Rigidbody>();
            if (lightsPerSegment != 0 && lightPrefab != null)
            {
                for (int l = 0; l < lightsPerSegment; l++)
                {
                    GameObject lightSegment = Instantiate(lightPrefab);
                    lightSegment.transform.position = Vector3.Lerp(ropeSegment.transform.position - ropeSegment.transform.TransformDirection(new Vector3(0, 0, segmentLength / 2)), ropeSegment.transform.position + ropeSegment.transform.TransformDirection(new Vector3(0, 0, segmentLength / 2)), (1f / lightsPerSegment) * (l + 0.5f)) - ropeSegment.transform.TransformDirection(lightOffset);
                    Joint lightJoint;
                    if (!lightSegment.TryGetComponent(out lightJoint))
                    {
                        lightJoint = lightSegment.AddComponent<FixedJoint>();
                    }
                    lightJoint.connectedBody = currentRopeRigidBody;
                    lightJoint.transform.SetParent(ropeParent.transform);
                }
            }

            if (i == 0)
            {
                segmentJoint.connectedBody = beginPointRigidbody;
            }
            else if (i == ropeSegments - 1)
            {
                segmentJoint.connectedBody = prevRigidbody;
                Joint endJoint = ropeEndPoint.gameObject.AddComponent<HingeJoint>();
                endJoint.connectedBody = ropeSegment.GetComponent<Rigidbody>();
                endJoint.autoConfigureConnectedAnchor = false;
                endJoint.anchor = new Vector3(0, 0, 0);
            }
            else
            {
                segmentJoint.connectedBody = prevRigidbody;
            }

            prevRigidbody = currentRopeRigidBody;
            ropeSegment.transform.SetParent(ropeParent.transform);
        }
    }
}
