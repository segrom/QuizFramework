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

        private Material _mat;
        
        public Color FirstColor
        {
            set => _image.materialForRendering.SetColor("_FirstColor", value);
            get => _image.materialForRendering.GetColor("_FirstColor");
        }

        public Color SecondColor
        {
            set => _image.materialForRendering.SetColor("_SecondColor", value);
            get => _image.materialForRendering.GetColor("_SecondColor");
        }

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _mat = new Material(Shader.Find("Unlit/GradientShader"));
            _image.material = _mat;
            FirstColor = firstColor;
            SecondColor = secondColor;
        }
        
    }
}