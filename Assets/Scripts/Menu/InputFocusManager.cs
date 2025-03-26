using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InputFocusManager : MonoBehaviour
{
    public TMP_InputField firstInput;
    public TMP_InputField secondInput;

    public MenuManager menuManager;
    public bool registerForm;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstInput.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;

            if (current == firstInput.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(secondInput.gameObject);
            }
            else if (current == secondInput.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(firstInput.gameObject);
            }
            EventSystem.current.currentSelectedGameObject
                ?.GetComponent<InputField>()?.DeactivateInputField();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!registerForm)
            {
                menuManager.SelectLoginAccount();
            }
            else
            {
                menuManager.SelectRegisterAccount();
            }

          
        }

    }
}