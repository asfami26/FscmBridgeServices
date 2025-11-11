using System;
using System.Collections.Generic;

namespace FscmBridgeServices.DTOS
{
    public class EditContractParticipantFscm
    {
        public class buyerDto
        {
            public string type { get; set; }
            public string? code { get; set; }
            public string? organizationUuid { get; set; }
            public bool isPrincipal { get; set; }
            public double maximumFundingLimit { get; set; }
            public double overrideLimit { get; set; }
            public bool bundlingAsset { get; set; }
            public bool suspend { get; set; }
            public bool disbursementTransferApprovalEnabled { get; set; }
            public bool settlementTransferApprovalEnabled { get; set; }
            public bool sellerConfigurationEnable { get; set; }
            public bool totalAssetPercentageEnabled { get; set; }
            public double? totalAssetPercentageValue { get; set; }
            public bool payFeesEnabled { get; set; }
            public bool allocation { get; set; }
            public bool holidayList { get; set; }
            public suspension suspension { get; set; }
            public List<stage> stages { get; set; }
            public List<object> approvals { get; set; }
            public List<object> childs { get; set; }
            public List<accounts> accounts { get; set; }
        }

        public class suspension
        {
            public bool isForever { get; set; }
            public bool isSpecificDate { get; set; }
            public string spesificDate { get; set; }
            public int emailNotificationBefore { get; set; }
            public int emailNotificationAfter { get; set; }
            public string overdueDays { get; set; }
            public string overdueAmount { get; set; }
            public int creditRating { get; set; }
            public bool allowFinance { get; set; }
            public bool financingAfterCutOffTime { get; set; }
            public bool isFromInvoice { get; set; }
        }

        public class stage
        {
            public string assetType { get; set; }
            public string percentage { get; set; }
            public bool auto { get; set; }
            public string suspend { get; set; }
            public List<configuration> configurations { get; set; }
            public List<limitSetting> limitSettings { get; set; }
            public List<outstandingParam> outstandingParams { get; set; }
            public List<disbursementPhase> disbursementPhases { get; set; }
            public List<settlementPhase> settlementPhases { get; set; }
            public List<object> insurances { get; set; }
            public List<object> bankCharges { get; set; }
            public referenceRate referenceRate { get; set; }
            public rabate rabate { get; set; }
            public commission commission { get; set; }
            public notificationSettings notificationSettings { get; set; }
            public bool bookingOfficeDisbursement { get; set; }
            public bool bookingOfficeSettlement { get; set; }
            public int amortizationDate { get; set; }
            public int order { get; set; }
            public List<object> generalLedgers { get; set; }
        }

        public class configuration
        {
            public string currency { get; set; }
            public string property { get; set; }
            public string valueType { get; set; }
            public string value { get; set; }
            public string calculationMethod { get; set; }
            public string calculationType { get; set; }
            public bool normalPaymentFlag { get; set; }
            public string normalPaymentValue { get; set; }
            public int calculationStartDate { get; set; }
            public string type { get; set; }
        }

        public class limitSetting
        {
            public string currency { get; set; }
            public double maximumCreditLimit { get; set; }
            public string fundingPercentage { get; set; }
            public string overrideLimitPercentage { get; set; }
            public string overrideAmount { get; set; }
            public string creditType { get; set; }
            public double overridePercentage { get; set; }
        }

        public class outstandingParam
        {
            public string property { get; set; }
        }

        public class disbursementPhase
        {
            public string phaseType { get; set; }
            public int priority { get; set; }
            public List<accountTransfer> accounts { get; set; }
        }

        public class settlementPhase
        {
            public string phaseType { get; set; }
            public int priority { get; set; }
            public List<accountTransfer> accounts { get; set; }
        }

        public class accountTransfer
        {
            public string originAccount { get; set; }
            public string destinationAccount { get; set; }
            public string percentage { get; set; }
        }

        public class referenceRate
        {
            public string calculationMethod { get; set; }
            public List<interest> interests { get; set; }
            public string calculationType { get; set; }
            public int calculationStartDate { get; set; }
            public double sharedRevenue { get; set; }
            public double funderSpread { get; set; }
            public double? minimumRate { get; set; }
        }

        public class interest
        {
            public string currency { get; set; }
            public string period { get; set; }
            public double percentage { get; set; }
        }

        public class rabate
        {
            public string calculationMethod { get; set; }
            public List<object> details { get; set; }
        }

        public class commission
        {
            public string calculationMethod { get; set; }
            public List<commissionConfig> configs { get; set; }
            public string calculationType { get; set; }
        }

        public class commissionConfig
        {
            public string currency { get; set; }
            public string period { get; set; }
            public double percentage { get; set; }
            public double normalPaymentPercentage { get; set; }
        }

        public class notificationSettings
        {
            public string upcomingSettlementNotification { get; set; }
        }

        public class accounts
        {
          
            public string accountType { get; set; }
            public bool defaults { get; set; }
            public accountDetail account { get; set; }
        }

        public class accountDetail
        {
            public string uuid { get; set; }
        }


        
    }

    
}