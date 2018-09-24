using RestSharp.Deserializers;

namespace SeatsioDotNet.Accounts
{
    public class ChartValidationSettings
    {
        [DeserializeAs(Name = "VALIDATE_DUPLICATE_LABELS")]
        public ChartValidationLevel ValidateDuplicateLabels { get; set; }
        
        [DeserializeAs(Name = "VALIDATE_OBJECTS_WITHOUT_CATEGORIES")]
        public ChartValidationLevel ValidateObjectsWithoutCategories { get; set; }
        
        [DeserializeAs(Name = "VALIDATE_UNLABELED_OBJECTS")]
        public ChartValidationLevel ValidateUnlabeledObjects { get; set; }
    }
}