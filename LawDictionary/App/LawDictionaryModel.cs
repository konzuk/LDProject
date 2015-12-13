using System;

namespace LawDictionary
{
    [Serializable]
    public class LawDictionaryModel
    {
        public LawDictionaryModel()
        {
        }

        public LawDictionaryModel(string code, string document, string type, string year, string nGO, string description)
        {
            Code = code;
            Document = document;
            Type = type;
            Year = year;
            NGO = nGO;
            Description = description;
        }

        public string Code { get; set; }
        public string Document { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }
        public string NGO { get; set; }
        public string Description { get; set; }
    }
}