using System.Collections;
using UnityEngine;

public enum EdgeSide { Left, Right }

public class CameraPager : MonoBehaviour
{
    public static CameraPager Instance { get; private set; }

    [Header("Scene refs")]
    public Camera mainCam;
    public Transform mom;
    public Transform fella;

    [Header("Pages")]
    public Transform[] pageCenters;   // Page0, Page1, Page2...
    public int currentPage = 0;
    public float pageWidth = 42.67f;
    public float entryInset = 0.6f;
    public float snapTime = 0.15f;

    bool isTransitioning = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void GoToPage(int targetPage, EdgeSide cameFrom)
    {
        if (isTransitioning)
            return;
        if (targetPage < 0 || targetPage >= pageCenters.Length)
            return;
        StartCoroutine(SnapRoutine(targetPage, cameFrom));
    }

    IEnumerator SnapRoutine(int targetPage, EdgeSide cameFrom)
    {
        isTransitioning = true;

        ZeroRB(mom);
        ZeroRB(fella);

        //Count new camera center

        Vector3 start = mainCam.transform.position;
        Vector3 goal = new Vector3(
            pageCenters[targetPage].position.x + 7f,
            pageCenters[targetPage].position.y,
            start.z
        );

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.01f, snapTime);
            mainCam.transform.position = Vector3.Lerp(start, goal, t);
            yield return null;
        }
        mainCam.transform.position = goal;

        float leftEdgeX = pageCenters[targetPage].position.x - pageWidth * 0.5f + entryInset;
        float rightEdgeX = pageCenters[targetPage].position.x + pageWidth * 0.5f - entryInset;

        float spawnX = (cameFrom == EdgeSide.Left) ? leftEdgeX : rightEdgeX;

        //POSITIONS ON THE NEW PAGE
        mom.position = new Vector3(spawnX - 0.25f, mom.position.y, mom.position.z);
        fella.position = new Vector3(spawnX + 0.25f, fella.position.y, fella.position.z);


        currentPage = targetPage;
        isTransitioning = false;
    }

    void ZeroRB(Transform t)
    {
        var rb = t.GetComponent<Rigidbody2D>();
        if (!rb)
            return;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        //if (rb) rb.linearVelocity = Vector2.zero; // Unity 6 API
    }
    
        void OnDrawGizmosSelected()
    {
        if (pageCenters == null)
            return;
        Gizmos.color = Color.cyan;
        foreach (var c in pageCenters)
        {
            if (!c)
                continue;
            //var left  = c.position + Vector3.left  * (pageWidth * 0.5f + 7f);
            var right = c.position + Vector3.right * (pageWidth * 0.5f + 7f);
            //Gizmos.DrawLine(left + Vector3.down * 10f, left + Vector3.up * 10f);
            Gizmos.DrawLine(right + Vector3.down * 10f, right + Vector3.up * 10f);
        }
    }

}
