using System;
using UnityEngine;

namespace Models.Results
{
    [Serializable]
    public class ResultFieldModel
    {
        public string groupName;
        public ResultFieldLayerModel[] layerModels;
    }
}