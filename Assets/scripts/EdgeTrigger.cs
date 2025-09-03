using UnityEngine;
using System.Collections.Generic;

public class EdgeTrigger : MonoBehaviour
{
    public int targetPage;
    public EdgeSide cameFrom;

    private readonly HashSet<GameObject> inside = new HashSet<GameObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        inside.Add(other.gameObject);
        if (inside.Count >= 2)
            CameraPager.Instance.GoToPage(targetPage, cameFrom);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        inside.Remove(other.gameObject); 
    }
}
