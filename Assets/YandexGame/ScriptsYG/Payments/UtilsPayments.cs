using System;

namespace YandexGame.ScriptsYG.Payments
{
    [Serializable]
    public class Purchase
    {
        public string id;
        public string title;
        public string description;
        public string imageURI;
        public string priceValue;
        public bool consumed;
    }
}