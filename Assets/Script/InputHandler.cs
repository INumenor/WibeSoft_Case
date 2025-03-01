using UnityEngine;

public class InputHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol týk veya mobil dokunma
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            HandleTouch(touchPosition);
        }
    }

    void HandleTouch(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        if (hit.collider != null)
        {
            Debug.Log("Dokunulan nesne: " + hit.collider.name);
        }
    }
}
