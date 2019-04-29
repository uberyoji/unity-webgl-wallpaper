using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;

public class DitherController : MonoBehaviour //, IEndDragHandler
{
    public MeshRenderer Plane;

    public Material[] Mats;

    private int Index = 0;

    private SwipeModule SM = new SwipeModule();


    // Start is called before the first frame update
    void Start()
    {
        SM.OnSwipe += OnSwipe;
    }

    // Update is called once per frame
    void Update()
    {
        SM.Update();
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Index = ++Index % Mats.Length;
            Plane.material = Mats[Index];
        }
        */
    }

    void OnSwipe( SwipeModule.Direction Dir )
    {
//        Debug.Log("Swipe detected: " + Dir);

        switch(Dir)
        {
            case SwipeModule.Direction.Right:
                {
                    Index = ++Index % Mats.Length;
                    Plane.material = Mats[Index];
                }
                break;
                case SwipeModule.Direction.Left:
                {
                    Index = (Index + Mats.Length-1) % Mats.Length;
                    Plane.material = Mats[Index];
                }
                break;
        }
    }

    /*
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Press position + " + eventData.pressPosition);
        Debug.Log("End position + " + eventData.position);
        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
        Debug.Log("norm + " + dragVectorDirection);
        GetDragDirection(dragVectorDirection);
    }

    private enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left
    }
    private DraggedDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        Debug.Log(draggedDir);
        return draggedDir;
    }
    */
}
