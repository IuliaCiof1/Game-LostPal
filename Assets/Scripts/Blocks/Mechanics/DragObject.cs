using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Blocks.Mechanics
{
    public class DragObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
    
        private RectTransform draggingObjectRT, codeEditor, codeStorage;
        private Transform parentOfDraggingObject;
        private Image image; //used to enable and disable raycast;

        //Event for when a DragObject is dropped:
        public delegate void OnDropOnObject(int mode);  //mode = 1 if the object is dropped on an expandable block, otherwise it's 0
        public static event OnDropOnObject OnDropOnObjectSound;

        private void Awake()
        {
            draggingObjectRT = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            codeEditor = GameObject.FindGameObjectWithTag("CodeEditor").GetComponent<RectTransform>();
            codeStorage = GameObject.FindGameObjectWithTag("CodeStorage").GetComponent<RectTransform>(); ;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            OnDropOnObjectSound?.Invoke(0);
            parentOfDraggingObject = transform.parent;
        
            //If this block was contained in an Expandable block then unexpand every Expandable block in hierarchy
            Transform parent = transform.parent.parent;
            if (parent.CompareTag("ExpandableCodeBlock"))
            {
                ExpandableDragObject expandableBlock = parent.GetComponent<ExpandableDragObject>();
                expandableBlock.UnexpandParentBlocks(expandableBlock, draggingObjectRT);
            }
              
            //Keep UI Element in front
            transform.SetParent(transform.root); //set parent to transform of the topmost object in hierarcy
            transform.SetAsLastSibling();
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            draggingObjectRT.position = Input.mousePosition;  //Move object where mouse cursor is
           image.raycastTarget = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            int editorOverlap = RectOverlap(draggingObjectRT, codeEditor);
            
            //A block will be parented to CodeEditor only if it fully overlaps with the CodeEditor, otherwise, it will be parented to CodeStorage
            if (editorOverlap >= 99) //the block was dropped on CodeEditor
            {
                OnDropOnObjectSound?.Invoke(0);

                ExpandableDragObject expandableBlock = null;
                
                if ((expandableBlock =
                        ExpandableBlockOverlap(codeEditor)) is not null) //the block was dropped on an Expandable block
                {
                    //blocks can be ordered only if they are in an Expandable block
                
                    List<DragObject>
                        overlappedBlocks =
                            GetOverlapBlocks(
                                codeEditor); //get the blocks under this block, they must be siblings, the parent block is excluded
                
                    Transform snapPoint = expandableBlock.transform.Find("SnapPoint");
                    int newIndex = snapPoint.childCount;
                
                    Debug.Log("ev sys " + eventData.pointerCurrentRaycast.gameObject.name);
                    
                    if (overlappedBlocks.Count == 2) //there are 2 blocks underneath. Move this block between those 2
                    {
                        Debug.Log("2 count");
                        newIndex = overlappedBlocks.ElementAt(1).transform.GetSiblingIndex();
                        parentOfDraggingObject = snapPoint;
                        expandableBlock.ExpandParentBlocks(expandableBlock,draggingObjectRT);
                    }
                    else if (overlappedBlocks.Count == 1) //there is only 1 block underneath
                    {
                        ExpandableDragObject expandableBlockInside;
                
                        if ((expandableBlockInside =
                                overlappedBlocks.ElementAt(0).GetComponent<ExpandableDragObject>()) is not null &&
                            RectOverlap(expandableBlockInside.GetComponent<RectTransform>(), draggingObjectRT) >=
                            65) //check if that 1 block is Expandable and that it overlaps with this block 65% or more
                        {
                            //place this block inside that 1 block
                            OnDropOnObjectSound?.Invoke(1);
                
                            expandableBlock.ExpandParentBlocks(expandableBlockInside,draggingObjectRT);
                            snapPoint = expandableBlockInside.transform.Find("SnapPoint");
                            parentOfDraggingObject = snapPoint;
                        }
                        else //otherwise we want to place this block above or below that block
                        {
                            if (overlappedBlocks[0].transform.position.y <
                                transform.position.y) //place it above the block
                            {
                                newIndex = overlappedBlocks.ElementAt(0).transform.GetSiblingIndex();
                            }
                            else //place it below the block
                                newIndex = overlappedBlocks.ElementAt(0).transform.GetSiblingIndex() + 1;
                
                            parentOfDraggingObject = overlappedBlocks.ElementAt(0).transform.parent;
                            Debug.Log(parentOfDraggingObject.name);
                            expandableBlock = parentOfDraggingObject.parent.GetComponent<ExpandableDragObject>();
                            //Debug.Log(expandableBlock.name);
                            if(expandableBlock is not null)
                                expandableBlock.ExpandParentBlocks(expandableBlock,draggingObjectRT);
                
                        }
                    }
                    else //there are no blocks to order, the expandable block is empty
                    {
                        OnDropOnObjectSound?.Invoke(1);
                
                        expandableBlock.ExpandParentBlocks(expandableBlock,draggingObjectRT);
                
                        parentOfDraggingObject = snapPoint;
                    }
                
                    transform.SetParent(parentOfDraggingObject);
                    transform.SetSiblingIndex(newIndex);
                }
                else //if the this block is not placed in in an expandable block just parent it ti CodeEditor
                {
                    parentOfDraggingObject = codeEditor;
                    transform.SetParent(parentOfDraggingObject);
                }

                // if (eventData.pointerCurrentRaycast.gameObject.GetComponent<ExpandableDragObject>() != null)
                // {
                //     ExpandableDragObject expandableBlock =
                //         eventData.pointerCurrentRaycast.gameObject.GetComponent<ExpandableDragObject>();
                //     Transform sp = eventData.pointerCurrentRaycast.gameObject.GetComponent<ExpandableDragObject>()
                //         .transform.Find("SnapPoint");
                //     parentOfDraggingObject = sp;
                //     int newIndex = sp.childCount;
                //     Debug.Log("name "+ eventData.pointerCurrentRaycast.gameObject.name);
                //     for (int i = 0; i < sp.childCount; i++)
                //     {
                //         if (eventData.position.y > sp.GetChild(i).position.y)
                //         {
                //             
                //             newIndex = sp.GetChild(i).GetSiblingIndex();
                //         }
                //     }
                //         transform.SetParent(parentOfDraggingObject);
                //        transform.SetSiblingIndex(newIndex);
                //        expandableBlock.ExpandParentBlocks(expandableBlock,draggingObjectRT);
                // }
                // else //if the this block is not placed in in an expandable block just parent it ti CodeEditor
                // {
                //      parentOfDraggingObject = codeEditor;
                //      transform.SetParent(parentOfDraggingObject);
                // }
            }
            else if (editorOverlap < 99) //the block was not fully dropped in CodeEditor, the block goes to CodeStorage
            {
                OnDropOnObjectSound?.Invoke(0);
                parentOfDraggingObject = codeStorage;


                //When the expandable block returns to CodeStorage, unparent all the contained blocks from this expandable block
                if (CompareTag("ExpandableCodeBlock"))
                {
                    ExpandableDragObject expandableBlock = GetComponent<ExpandableDragObject>();
                    expandableBlock.MoveNestedBlocksToCodeStorage(expandableBlock, codeStorage);
                    parentOfDraggingObject = codeStorage;
                }

                transform.SetParent(parentOfDraggingObject);
            }

            image.raycastTarget = true;
        }

        void MoveNestedBlocksToCodeStorage(ExpandableDragObject block)
        {
            Transform sp = block.transform.Find("SnapPoint");
        
            for(int i = sp.childCount-1; i>=0; i-- )
            {
                ExpandableDragObject nestedExpandBlock = sp.GetChild(i).GetComponent<ExpandableDragObject>();
            
                if (nestedExpandBlock is not null)
                {
                    MoveNestedBlocksToCodeStorage(nestedExpandBlock);
                    block.Unexpand(nestedExpandBlock.GetComponent<RectTransform>());
                    nestedExpandBlock.Reset();
                }
                else
                {
                    sp.GetChild(i).transform.SetParent(codeStorage);
                }
            }
        
            block.Reset();
            block.transform.SetParent(codeStorage);
        }
    
        void ExpandParentBlocks(ExpandableDragObject block)
        {
            block.Expand(draggingObjectRT);
            Transform sp = block.transform.parent;
           
            if (sp is not null)
            {
                ExpandableDragObject nestedExpandBlock = sp.transform.parent.GetComponent<ExpandableDragObject>();
               
                if (nestedExpandBlock is not null)
                {
                    ExpandParentBlocks(nestedExpandBlock);
                }
            }
        }
        
        void UnexpandParentBlocks(ExpandableDragObject block)
        {
            block.Unexpand(draggingObjectRT);
            Transform sp = block.transform.parent;
          
            if (sp is not null)
            {
                ExpandableDragObject nestedExpandBlock = sp.transform.parent.GetComponent<ExpandableDragObject>();
               
                if (nestedExpandBlock is not null)
                {
                    UnexpandParentBlocks(nestedExpandBlock);
                }
            }
        }
        
        
        //Function takes 2 rects as input and returns the percentage of overlapping
        public int RectOverlap(RectTransform rect1, RectTransform rect2)
        {
            Canvas.ForceUpdateCanvases();
            Vector3[] r1 = new Vector3[4];
            rect1.GetWorldCorners(r1);
            
            Vector3[] r2 = new Vector3[4];
            rect2.GetWorldCorners(r2);
            
            for(int i=0; i<4; i++)
                Debug.Log(r1[i] + " r2: " + r2[i]);
            
            Debug.Log("mouse world position: " + Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
            /*
        r1[1]               r1[2]
            ___________________
           |                  |
           |__________________|
        r1[0]               r1[3]
        */
        

            //Calculate the width and height of the rectangle (intersection) resulted from overlapping rect1 with rect2
            float X = Math.Min(r1[3].x, r2[3].x) - Math.Max(r1[1].x, r2[1].x);

            float Y = Math.Min(r1[1].y, r2[1].y) - Math.Max(r1[3].y, r2[3].y);
            
            //If the rectangles don't overlap, the X or Y above will result in a negative number.
            //Because of this, we need to make sure the area will be equal to 0 in case of no overlap.
            X = Math.Max(0, X);
            Y = Math.Max(0, Y);
        
            float A = X * Y; //calculate the intersecting area

            float smallerRectArea = (r1[1].y - r1[0].y) * (r1[3].x - r1[0].x);
            //Return -1 if the rectangles don't overlap
            if (A == 0)
            {
                return -1;
            }

            Debug.Log("RectOverlap "+A / smallerRectArea * 100);
            return (int)Math.Round(A / smallerRectArea * 100);
        }
        
        //Check if this block overlaps with any expandable block
        ExpandableDragObject ExpandableBlockOverlap(Transform container)
        {
            ExpandableDragObject [] blocks = container.GetComponentsInChildren<ExpandableDragObject>();
            
            for(int i=blocks.Length-1; i>=0; i--){
                if (blocks[i].CompareTag("ExpandableCodeBlock"))
                {
                    if(RectOverlap(draggingObjectRT, blocks[i].GetComponent<RectTransform>())>0){
                        Debug.Log("expandableblockoverlap "+blocks[i].name);
                        return blocks[i].GetComponent<ExpandableDragObject>();
                        }
                }
            }
           
            return null;
        }

        //Get the blocks under this block. Those blocks need to have the same parent
        public List<DragObject> GetOverlapBlocks(Transform container)
        {
            DragObject [] blocks = container.GetComponentsInChildren<DragObject>();
            List<DragObject> overlappedBlocks = new List<DragObject>();

            for(int i=0; i<blocks.Length; i++){

                DragObject child = blocks[i];
                
                if (child.transform!=transform && child.name!="Player" && ((child.CompareTag("CodeBlock") || child.CompareTag("ExpandableCodeBlock")) && RectOverlap(draggingObjectRT, child.GetComponent<RectTransform>())>0))
                {
                    overlappedBlocks.Add(child);
                }
            }

            //remove unwanted blocks (parents)
            for (int i=0; i<overlappedBlocks.Count-1; i++)
            {
                if (overlappedBlocks.ElementAt(i).transform.parent != overlappedBlocks.ElementAt(i + 1).transform.parent)
                {
                    overlappedBlocks.RemoveAt(i);
                    i--;
                }
            }

            for (int i=0; i<overlappedBlocks.Count; i++)
            {
               Debug.Log(overlappedBlocks.ElementAt(i).name);
            }
            
            return overlappedBlocks;
        }
    }
}
