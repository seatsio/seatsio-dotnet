namespace SeatsioDotNet
{
    public class SeatsioApiError
    {
        public readonly string Code;
        public readonly string Message;

        public SeatsioApiError(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is SeatsioApiError p))
            {
                return false;
            }Ì

            return (Code.Equals(p.Code)) && (Message.Equals(p.Message));
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode() ^ Message.GetHashCode();
        }
    }
}