using UnityEngine;
using UnityEngine.UI;

namespace Blocks.Mechanics
{
    public class ExpandableDragObject : MonoBehaviour
    {
        private float addHeight;

        private RectTransform rt, middle, bottom, snapPoint;
  
        private Vector2 rtInitialSize, middleInitialSize, bottomInitialLocation,snapPointInitialSize;

        public void Awake()
        {
            rt = gameObject.GetComponent<RectTransform>();
            middle = transform.Find("Middle") as RectTransform;
            bottom = transform.Find("Bottom") as RectTransform;
            snapPoint= transform.Find("SnapPoint") as RectTransform;

            rtInitialSize=rt.sizeDelta;
            middleInitialSize=middle.sizeDelta;
            bottomInitialLocation=bottom.localPosition;
            snapPointInitialSize=snapPoint.sizeDelta;
        }

        public void Expand(RectTransform draggingObject)
        {
            addHeight = draggingObject.GetComponent<RectTransform>().sizeDelta.y+
                        snapPoint.GetComponent<VerticalLayoutGroup>().spacing;
            //GridLayoutGroup component of CodeStorage automatically decides pivot position of the contained objects
            //We need the pivot present at the middle top so that the expandable object only expands downwards
            gameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);

            ChangeBlock(-addHeight);
        }
        public void Unexpand(RectTransform draggingObject)
        {
            addHeight = draggingObject.GetComponent<RectTransform>().sizeDelta.y+snapPoint.GetComponent<VerticalLayoutGroup>().spacing;

            ChangeBlock(addHeight);
        }

        public void ChangeBlock(float addHeight)
        {
            middle.sizeDelta = new Vector2(middle.sizeDelta.x, middle.sizeDelta.y - addHeight);
            bottom.localPosition = new Vector2(bottom.localPosition.x, bottom.localPosition.y + addHeight);
            snapPoint.sizeDelta = new Vector2(snapPoint.sizeDelta.x, snapPoint.sizeDelta.y - addHeight);
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y-addHeight);
        }
        public void MoveNestedBlocksToCodeStorage(ExpandableDragObject block, RectTransform codeStorage)
        {
            Transform sp = block.transform.Find("SnapPoint");
        
            for(int i = sp.childCount-1; i>=0; i-- )
            {
                ExpandableDragObject nestedExpandBlock = sp.GetChild(i).GetComponent<ExpandableDragObject>();
            
                if (nestedExpandBlock is not null)
                {
                    MoveNestedBlocksToCodeStorage(nestedExpandBlock, codeStorage);
                    // block.Unexpand(nestedExpandBlock.GetComponent<RectTransform>());
                    // nestedExpandBlock.Reset();
                }
                else
                {
                    sp.GetChild(i).transform.SetParent(codeStorage);
                }
            }
        
            block.Reset();
            block.transform.SetParent(codeStorage);
        }
    
        public void ExpandParentBlocks(ExpandableDragObject block, RectTransform draggingObjectRT)
        {
            block.Expand(draggingObjectRT);
            Transform sp = block.transform.parent;
           
            if (sp is not null)
            {
                ExpandableDragObject nestedExpandBlock = sp.transform.parent.GetComponent<ExpandableDragObject>();
               
                if (nestedExpandBlock is not null)
                {
                    ExpandParentBlocks(nestedExpandBlock, draggingObjectRT);
                }
            }
        }
        
        public void UnexpandParentBlocks(ExpandableDragObject block, RectTransform draggingObjectRT)
        {
            block.Unexpand(draggingObjectRT);
            Transform sp = block.transform.parent;
          
            if (sp is not null)
            {
                ExpandableDragObject nestedExpandBlock = sp.transform.parent.GetComponent<ExpandableDragObject>();
               
                if (nestedExpandBlock is not null)
                {
                    UnexpandParentBlocks(nestedExpandBlock, draggingObjectRT);
                }
            }
        }

        
        public void Reset()
        {
            middle.sizeDelta = middleInitialSize;
            bottom.localPosition = bottomInitialLocation;

            snapPoint.sizeDelta = snapPointInitialSize;
            rt.sizeDelta = rtInitialSize;
        }
    }
}
