using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Windows;
using UnityEngine.UI.Windows.Components;
using UnityEngine.UI.Windows.Components.DragAndDropModules;
using UnityEngine.UI.Windows.WindowTypes;

namespace UI
{
    public class MainScreen : LayoutWindowType
    {
        private ImageComponent _image;

        public override void OnInit()
        {
            this.GetLayoutComponent<ImageComponent>(out _image);
            _image.GetModule<DragComponentModule>().SetBeginDragCallback(this.OnBeginDrag);
            _image.GetModule<DragComponentModule>().SetDragCallback(this.OnDrag);
            _image.GetModule<DragComponentModule>().SetEndDragCallback(this.OnEndDrag);
        }

        public override void OnDeInit()
        {
            _image.GetModule<DragComponentModule>().RemoveAllBeginDragCallbacks();
            _image.GetModule<DragComponentModule>().RemoveAllDragCallbacks();
            _image.GetModule<DragComponentModule>().RemoveAllEndDragCallbacks();
        }

        private void OnBeginDrag(PointerEventData data)
        {
            Debug.Log("OnBeginDrag");
            _image.objectCanvas.sortingOrder = 10;
            _image.objectCanvas.overrideSorting = true;
        }

        private void OnDrag(PointerEventData data)
        {
            _image.transform.localPosition = Vector3.zero;
        }

        private void OnEndDrag(PointerEventData data)
        {
            Debug.Log("OnEndDrag");
            _image.objectCanvas.sortingOrder = 0;
            _image.objectCanvas.overrideSorting = false;
        }
    }
}