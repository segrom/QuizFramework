using System;

namespace Models.Results
{
    [Serializable]
    public class ResultFieldLayerModel
    {
        public float globalRightnessRangeMin = 0;
        public float globalRightnessRangeMax = 1;
        public string resultTitle;
        public string resultDescription;
    }
}