using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

    [Header("Attributes")]
    public float viewRadius;
    public float viewAngle;

    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask obstacleMask;

    private float meshResolution = 0.5f;
    private int edgeResolveIteration = 3;
    private float edgeDstThreshold = 0.5f;

    public MeshFilter viewMeshFilter;
    private Mesh viewMesh;

    public List<Transform> visibleTargets = new List<Transform>();

    int stepCount;
    float stepAngleSize;
    bool isShowed;

    private float refreshTime;

    [HideInInspector] public Vector3 startPosition, endPosition;

    private void Awake() {
        isShowed = true;

        viewMesh = new Mesh();
        viewMeshFilter.mesh = viewMesh;
    }

    private void OnEnable() {
        StartCoroutine("FindTargetWithDelay", RefreshTime);
    }

    private void FixedUpdate() {
        if(isShowed) {
            DrawFieldOfView();
        }
    }

    public void Show() {
        isShowed = true;
        viewMeshFilter.gameObject.SetActive(true);
    }
    public void Hide() {
        isShowed = false;
        viewMeshFilter.gameObject.SetActive(false);
    }

    public void FindVisibleTargets() {
        visibleTargets.Clear();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);
        for(int i = 0;i < targetsInViewRadius.Length;i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            float dstToTarget = Vector3.Distance(transform.position, target.position);

            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
                if(!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                    visibleTargets.Add(target);
                }
            }
        }

        AddBorderTargets(transform.eulerAngles.y - viewAngle / 2);
        AddBorderTargets(transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * stepCount);

        void AddBorderTargets(float angle) {
            Vector3 startPosition = transform.position + new Vector3(0, 0.5f, 0);

            RaycastHit[] hits = Physics.RaycastAll(startPosition, DirFromAngle(angle, true), viewRadius, enemyMask);

            for(int y = 0;y < hits.Length;y++) {
                if(!Physics.Raycast(startPosition, DirFromAngle(angle, true), Vector3.Distance(startPosition, hits[y].transform.position), obstacleMask)) {
                    visibleTargets.Add(hits[y].transform);
                }
            }
        }
    }

    public Transform GetClosestEnemy() {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach(Transform potentialTarget in visibleTargets) {
            if(potentialTarget != null) {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;

                if(dSqrToTarget < closestDistanceSqr) {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }
        }

        return bestTarget;
    }

    private void DrawFieldOfView() {
        List<Vector3> viewPoints = new List<Vector3>();
        stepCount = Mathf.RoundToInt(viewAngle * meshResolution);

        ViewCastInfo oldViewCast = new ViewCastInfo();
        for(int i = 0;i <= stepCount;i++) {
            stepAngleSize = viewAngle / stepCount;
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;

            ViewCastInfo newViewCast = ViewCast(angle);

            if(i > 0) {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDstThreshold;
                if(oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded)) {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if(edge.pointA != Vector3.zero) {
                        viewPoints.Add(edge.pointA);
                    }
                    if(edge.pointB != Vector3.zero) {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;

            if(i == 0) {
                startPosition = DirFromAngle(angle, true) * viewRadius;
            }
            if(i == stepCount) {
                endPosition = DirFromAngle(angle, true) * viewRadius;
            }
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for(int i = 0;i < vertexCount - 1;i++) {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if(i < vertexCount - 2) {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateBounds();
    }
    private IEnumerator FindTargetWithDelay(float delay) {
        while(true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;

        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for(int i = 0;i < edgeResolveIteration;i++) {
            float angle = (minAngle + maxAngle) / 2;

            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDstThreshold;
            if(newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded) {
                minAngle = angle;
                minPoint = newViewCast.point;
            } else {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }
    private ViewCastInfo ViewCast(float globalAngle) {
        Vector3 dir = DirFromAngle(globalAngle, true);

        RaycastHit hit;
        if(Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask)) {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        } else {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if(!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private float RefreshTime {
        get {
            return refreshTime < 1f ? 1f : refreshTime;
        }
        set { refreshTime = value; }
    }

    private struct ViewCastInfo {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool hit, Vector3 point, float distance, float angle) {
            this.hit = hit;
            this.point = point;
            this.distance = distance;
            this.angle = angle;
        }
    }
    private struct EdgeInfo {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB) {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }
}