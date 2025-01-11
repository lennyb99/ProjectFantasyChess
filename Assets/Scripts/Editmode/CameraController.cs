using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float scrollSpeed = 10f;
    public float minZoom = 2f;
    public float maxZoom = 20f;

    public float dragSpeed = 0.5f;  

    public Vector2 boundsMin = new Vector2(-10f, -10f);
    public Vector2 boundsMax = new Vector2(10f, 10f);

    private Vector3 lastMousePosition;

    private void Update()
    {
        HandleZoom();
        HandleDrag();
        ClampCameraPosition();
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            Camera.main.orthographicSize -= scrollInput * scrollSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(
                Camera.main.orthographicSize,
                minZoom,
                maxZoom
            );
        }
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(2))
        {
            // Erste Position merken, sobald mittlere Maustaste gedrückt wird
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(2))
        {
            // Differenz der Mausbewegung berechnen
            Vector3 delta = Input.mousePosition - lastMousePosition;

            // Richtung invertieren, damit es sich anfühlt, als ob man die Welt „zieht“
            // Anpassen durch dragSpeed
            transform.position -= delta * Time.deltaTime * dragSpeed;

            // Aktuelle Mausposition als letzte Mausposition speichern
            lastMousePosition = Input.mousePosition;
        }
    }

    private void ClampCameraPosition()
    {
        // Aktuelle Kameraposition
        Vector3 pos = transform.position;

        // Für eine Orthographic-Kamera sollte man berücksichtigen, 
        // dass der sichtbare Bereich von der Orthographic Size abhängt.
        // Je nach gewünschtem Verhalten kann man hier entweder nur 
        // die Kameramitte einschränken ODER sicherstellen, dass die Kamera 
        // nicht „über den Rand hinausschaut“.

        float camSize = Camera.main.orthographicSize;
        float aspectRatio = (float)Screen.width / Screen.height;
        float camHalfWidth = aspectRatio * camSize;

        // Falls du sicherstellen möchtest, dass die Ränder nicht aus den Bounds rausgehen:
        pos.x = Mathf.Clamp(pos.x, boundsMin.x + camHalfWidth, boundsMax.x - camHalfWidth);
        pos.y = Mathf.Clamp(pos.y, boundsMin.y + camSize, boundsMax.y - camSize);

        // Position zuweisen
        transform.position = pos;
    }
}
