using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            DetectSquareClick();
            
        }
    }

    void DetectSquareClick()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit.collider != null)
        {
            GameObject clickedObject = hit.collider.gameObject;
            Debug.Log("Clicked on: " + clickedObject.name);
            HandleClickOnObject(clickedObject);
        }
    }

    void HandleClickOnObject(GameObject obj)
    {
        if(obj.tag == "Square") { 
            
            Square square = obj.GetComponent<Square>();

            if (square == null)
            {
                Debug.Log("Square script is null");
                return;
            }

            square.HandleClickedOn();

        }
    }
}
