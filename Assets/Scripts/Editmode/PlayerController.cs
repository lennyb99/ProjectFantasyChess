using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public BoardManager boardManager;

    public bool multiSelect = false;

    public GameObject multiSelectFirstSquare;
    public GameObject multiSelectLastSquare;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            multiSelect = true;
        }else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            multiSelect= false;
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            if (!multiSelect)
            {
                DetectSingleSquareClick();
            }
            else
            {
                multiSelectFirstSquare = DetectMultiSelectSquare();
            }
        }

        if(multiSelect && Input.GetMouseButtonUp(0))
        {
            
            multiSelectLastSquare = DetectMultiSelectSquare();
            if (multiSelectFirstSquare != null && multiSelectLastSquare != null)
            {
                HandleMultiSelect();
            }
        }
    }

    GameObject DetectMultiSelectSquare()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return null;
        }
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit.collider != null)
        {
            GameObject clickedObject = hit.collider.gameObject;
            return clickedObject;
        }
        return null;
    }

    void HandleMultiSelect()
    {
        foreach (GameObject square in boardManager.ReturnMatrixOfSquareObjects(multiSelectFirstSquare.GetComponent<EditSquare>(), multiSelectLastSquare.GetComponent<EditSquare>())) { 
            HandleClickOnObject(square);
        }
        
    }

    void DetectSingleSquareClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit.collider != null)
        {
            GameObject clickedObject = hit.collider.gameObject;
            HandleClickOnObject(clickedObject);
        }
    }

    void HandleClickOnObject(GameObject obj)
    {
        if(obj.tag == "Square") { 
            
            EditSquare square = obj.GetComponent<EditSquare>();

            if (square == null)
            {
                Debug.Log("Square script is null");
                return;
            }

            square.HandleClickedOn();

        }
    }
}
