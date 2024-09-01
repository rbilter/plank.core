namespace Plank.Core.Contracts
{
    public class PlankValidationResultCollection : List<PlankValidationResult>
    {
        public bool IsValid { get { return Count == 0; } }
    }
}