using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BurbBar : MonoBehaviour
{
    public Image burbFill;
    public FellaMove fella;

    void update()
    {
        if (fella != null && burbFill != null)
        {
            burbFill.fillAmount = fella.burbs / 3f;
        }
    }

}
