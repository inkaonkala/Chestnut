using UnityEngine;
using System.Collections.Generic;

public enum PageDirection { Previous = -1, Next = 1 }

public class EdgeTrigger : MonoBehaviour
{
    public PageDirection direction = PageDirection.Next;

    private readonly HashSet<GameObject> inside = new HashSet<GameObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        inside.Add(other.gameObject);
        if (inside.Count >= 2)
        {
            var pager = CameraPager.Instance;
            if (pager == null) return;

            int dest = Mathf.Clamp(
                pager.currentPage + (int)direction,
                0,
                pager.pageCenters.Length - 1
            );

            // We enter the destination page from this side:
            // moving Next (→) means we enter the next page from its Left edge.
            // moving Previous (←) means we enter the previous page from its Right edge.
            EdgeSide cameFrom = (direction == PageDirection.Next) ? EdgeSide.Left : EdgeSide.Right;

            pager.GoToPage(dest, cameFrom);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        inside.Remove(other.gameObject);
    }
}
