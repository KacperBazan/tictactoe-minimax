using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public GameObject hoverObject;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Space"))
        {
            hoverObject = collision.gameObject;
        }
        else
        {
            hoverObject = null;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        hoverObject = null;
    }
}
