using UnityEngine;
using UnityEngine.EventSystems;

namespace Blocks.Mechanics
{
    public class CopyCodeBlock : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        private Transform CodeStorage;
        private bool copied = false;
        private int siblingIndex;
        private void Awake()
        {
            CodeStorage = GameObject.FindGameObjectWithTag("CodeStorage").GetComponent<RectTransform>();
            // siblingIndex = transform.GetSiblingIndex();
            // Debug.Log(siblingIndex + "index copy and prent is " + transform.parent);

        }
    
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!copied)
            {
               // Debug.Log(siblingIndex + "index copy and prent is " + transform.parent);
                GameObject copy = Instantiate(this, transform.position, Quaternion.identity, CodeStorage).gameObject;
                //copy.transform.SetSiblingIndex(siblingIndex);
                copied = true;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (transform.parent == CodeStorage)
            {
                Destroy(gameObject);
            }
        }
    }
}
