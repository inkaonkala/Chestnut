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
    public float pageWidth = 16.0f;
    public float entryInset = 0.6f;
    public float snapTime = 0.15f;

    bool isTransitioning = false;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void GoToPage(int targetPage, EdgeSide cameFrom)
    {
        if (isTransitioning) return;
        if (targetPage < 0 || targetPage >= pageCenters.Length) return;
        StartCoroutine(SnapRoutine(targetPage, cameFrom));
    }

    IEnumerator SnapRoutine(int targetPage, EdgeSide cameFrom)
    {
        isTransitioning = true;

        ZeroRB(mom);
        ZeroRB(fella);

        Vector3 start = mainCam.transform.position;
        Vector3 goal  = new Vector3(
            pageCenters[targetPage].position.x,
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

        float leftEdgeX  = pageCenters[targetPage].position.x - pageWidth * 0.5f + entryInset;
        float rightEdgeX = pageCenters[targetPage].position.x + pageWidth * 0.5f - entryInset;

        float spawnX = (cameFrom == EdgeSide.Right) ? leftEdgeX : rightEdgeX;
        Vector3 baseSpawn = new Vector3(spawnX, mom.position.y, mom.position.z);

        mom.position   = baseSpawn + Vector3.left * 0.25f;
        fella.position = baseSpawn + Vector3.right * 0.25f;

        currentPage = targetPage;
        isTransitioning = false;
    }

    void ZeroRB(Transform t)
    {
        var rb = t.GetComponent<Rigidbody2D>();
        if (rb) rb.linearVelocity = Vector2.zero; // Unity 6 API
    }
}
