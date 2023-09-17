using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tools
{
    [ExecuteInEditMode]
    public class Gradient : MonoBehaviour
    {
        [SerializeField] private Color firstColor;
        [SerializeField] private Color secondColor;

        private Material mat;
        
        public Color FirstColor
        {
            set => image.materialForRendering.SetColor("_FirstColor", value);
            get => image.materialForRendering.GetColor("_FirstColor");
        }

        public Color SecondColor
        {
            set => image.materialForRendering.SetColor("_SecondColor", value);
            get => image.materialForRendering.GetColor("_SecondColor");
        }

        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
            mat = new Material(image.material);
            image.material = mat;
            FirstColor = firstColor;
            SecondColor = secondColor;
        }

        private void OnDestroy()
        {
            Destroy(mat);
        }
    }
}