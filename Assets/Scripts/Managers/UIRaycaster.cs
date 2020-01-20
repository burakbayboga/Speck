using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIRaycaster : MonoBehaviour
{
    public GraphicRaycaster GraphicRaycaster;

    private List<RaycastResult> Hits;
    private SButton LastInteracted;
    
    private void Start()
    {
        Hits = new List<RaycastResult>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        ProcessMouseInput();
#else
        ProcessTouchInput();
#endif

    }

    private void ProcessMouseInput()
    {
        bool mouseDown = Input.GetMouseButtonDown(0);
        bool mouseUp = Input.GetMouseButtonUp(0);

        if (mouseUp || mouseDown)
        {
            Hits.Clear();
            
            PointerEventData ped = new PointerEventData(null)
            {
                position = Input.mousePosition
            };
            
            GraphicRaycaster.Raycast(ped, Hits);
            if (Hits.Count > 0 && Hits[0].gameObject.CompareTag("SUI"))
            {
                SButton uiElement = Hits[0].gameObject.GetComponent<SButton>();
                if (mouseDown)
                {
                    uiElement.OnDown();
                    LastInteracted = uiElement;
                }
                else if (mouseUp && LastInteracted != null)
                {
                    LastInteracted.OnUp(uiElement == LastInteracted);
                    LastInteracted = null;
                }
            }
            else
            {
                if (LastInteracted != null)
                {
                    LastInteracted.OnUp(false);
                }
                LastInteracted = null;
            }
        }
    }

    private void ProcessTouchInput()
    {

        if (Input.touchCount > 1)
        {
            if (LastInteracted != null)
            {
                LastInteracted.OnUp(false);
            }
            LastInteracted = null;
        }
        else if (Input.touchCount == 1)
        {
            Touch currentTouch = Input.GetTouch(0);

            if (currentTouch.phase == TouchPhase.Began)
            {
                Hits.Clear();

                PointerEventData ped = new PointerEventData(null)
                {
                    position = currentTouch.position
                };

                GraphicRaycaster.Raycast(ped, Hits);

                if (Hits.Count > 0 && Hits[0].gameObject.CompareTag("SUI"))
                {
                    SButton uiElement = Hits[0].gameObject.GetComponent<SButton>();

                    uiElement.OnDown();
                    LastInteracted = uiElement;
                }
            }
            else if (currentTouch.phase == TouchPhase.Ended)
            {
                Hits.Clear();

                PointerEventData ped = new PointerEventData(null)
                {
                    position = currentTouch.position
                };

                GraphicRaycaster.Raycast(ped, Hits);

                if (Hits.Count > 0 && Hits[0].gameObject.CompareTag("SUI"))
                {
                    SButton uiElement = Hits[0].gameObject.GetComponent<SButton>();

                    if (LastInteracted != null)
                    {
                        LastInteracted.OnUp(uiElement == LastInteracted);
                        LastInteracted = null;
                    }
                }
                else
                {
                    if (LastInteracted != null)
                    {
                        LastInteracted.OnUp(false);
                    }
                    LastInteracted = null;
                }
            }
        }
    }
}
