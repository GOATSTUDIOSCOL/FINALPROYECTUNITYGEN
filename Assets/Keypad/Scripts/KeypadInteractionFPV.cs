using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NavKeypad
{
    public class KeypadInteractionFPV : MonoBehaviour
    {
        public Camera cam;

        private void Awake() => cam = GetComponent<Camera>();
        private void Update()
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.TryGetComponent(out KeypadButton keypadButton))
                    {
                        keypadButton.PressButton();
                    }
                }
            }
        }



    }
}