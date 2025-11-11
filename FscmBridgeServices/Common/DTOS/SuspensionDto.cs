namespace FscmBridgeServices.Common.DTOS
{
    public class SuspensionDto
    {
        public bool IsForever { get; set; }
        public bool IsSpesificDate { get; set; }
        public string? OverdueDays { get; set; }
        public string? OverdueAmount { get; set; }
        public bool AllowFinance { get; set; }
        public bool FromInvoice { get; set; }
        public string? SpesificDateTime { get; set; }
        public int EmailNotificationBefore { get; set; }
        public int EmailNotificationAfter { get; set; }
        public int ParticipantsOverdueAmount { get; set; }
        public string? CreditRating { get; set; }
        public int QuoteRestrictionDays { get; set; }

    }
}
