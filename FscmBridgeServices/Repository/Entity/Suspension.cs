namespace FscmBridgeServices.Repository.Entity
{
    public class Suspension
    {
        public bool isForever { get; set; }
        public bool isSpesificDate { get; set; }
        public string? overdueDays { get; set; }
        public string? overdueAmount { get; set; }
        public bool allowFinance { get; set; }
        public bool fromInvoice { get; set; }
        public string? spesificDateTime { get; set; }
        public int emailNotificationBefore { get; set; }
        public int emailNotificationAfter { get; set; }
        public int participantsOverdueAmount { get; set; }
        public string? creditRating { get; set; }
        public int quoteRestrictionDays { get; set; }
    }
}
