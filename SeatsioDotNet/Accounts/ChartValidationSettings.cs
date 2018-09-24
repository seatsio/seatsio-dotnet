using RestSharp.Deserializers;

namespace SeatsioDotNet.Accounts
{
    public class ChartValidationSettings
    {
        [DeserializeAs(Name = "VALIDATE_DUPLICATE_LABELS")]
        public string ValidateDuplicateLabels { get; set; }
        
        [DeserializeAs(Name = "VALIDATE_OBJECTS_WITHOUT_CATEGORIES")]
        public string ValidateObjectsWithoutCategories { get; set; }
        
        [DeserializeAs(Name = "VALIDATE_UNLABELED_OBJECTS")]
        public string ValidateUnlabeledObjects { get; set; }
    }
}