using System.Linq;

namespace FscmBridgeServices.Util
{
    public class Constant
    {  
        static DatabaseContext dbelo = new DatabaseContext();
        //
        public static string SP_GETPARAMORGANIZATIONFSCM = "SP_GETPARAMORGANIZATIONFSCM  ";
        public static string SP_GET_FINANCE_ORGANIZATION = "SP_GET_FINANCEORGANIZATION ";
        public static string SP_GET_CONTRACT = "SP_GET_CONTRACT ";
        public static string SP_GET_CONTRACT_FUNDER = "SP_GET_CONTRACT_FUNDER ";
        public static string SP_GET_CONTRACT_BUYER = "SP_GET_CONTRACT_BUYER ";
        public static string SP_GET_CONTRACT_SELLER = "SP_GET_CONTRACT_SELLER ";
        public static string SP_GET_OPTION_RATES = "SP_GET_OPTION_RATES ";
        public static string SP_GET_SUSPENSIONS = "SP_GET_SUSPENSIONS ";
        public static string GET_EDIT_PARTICIPANT_BUYER = "SP_EDIT_CONTRACT_PARTICIPANT_GetBuyer";
            public static string GET_EDIT_PARTICIPANT_SUSPENSION = "SP_EDIT_CONTRACT_PARTICIPANT_GetSuspension";
        public static string GET_EDIT_PARTICIPANT_CONFIGURATIONS = "SP_EDIT_CONTRACT_PARTICIPANT_GetConfigurations";
        public static string GET_EDIT_PARTICIPANT_LIMIT_SETTINGS = "SP_EDIT_CONTRACT_PARTICIPANT_GetLimitSettings";
        public static string GET_EDIT_PARTICIPANT_OUTSTANDING_PARAMS = "SP_EDIT_CONTRACT_PARTICIPANT_GetOutstandingParams";
        public static string GET_EDIT_PARTICIPANT_SETTLEMENT_PHASES = "SP_EDIT_CONTRACT_PARTICIPANT_DEFAULT_SETTLEMENT_PHASES";
        public static string GET_EDIT_PARTICIPANT_ACCOUNTS = "SP_EDITCONTRACT_PARTICIPANT_GET_ACCOUNTS";
        public static string GET_EDIT_PARTICIPANT_ACCOUNT_DETAILS = "SP_EDITCONTRACT_PARTICIPANT_GET_ACCOUNTS"; 
        public static string GET_EDIT_PARTICIPANT_RABATE = "SP_EDIT_PARTICIPANT_GET_RABATE"; 
        public static string GET_EDIT_PARTICIPANT_COMMISSION = "SP_EDIT_PARTICIPANT_GET_COMMISSION";
        public static string GET_EDIT_PARTICIPANT_COMMISSION_CONFIG = "SP_EDIT_PARTICIPANT_GET_COMMISSION_CONFIG";
        public static string GET_EDIT_PARTICIPANT_SETTLEMENT_PHASE_ACCOUNTS = "SP_GET_SETTLEMENT_PHASE_ACCOUNTS";
        public static string GET_EDIT_PARTICIPANT_DISBURSEMENT_PHASES = "SP_EDIT_PARTICIPANT_GET_DISBURSEMENT_PHASES";
        public static string GET_EDIT_PARTICIPANT_DISBURSEMENT_ACCOUNTS = "SP_EDIT_PARTICIPANT_GET_DISBURSEMENT_ACCOUNTS";
        public static string GET_EDIT_PARTICIPANT_REFERENCE_RATE = "SP_EDIT_PARTICIPANT_GET_REFERENCE_RATE";
        public static string GET_EDIT_PARTICIPANT_INTERESTS = "SP_EDIT_PARTICIPANT_GET_INTERESTS";
        public static string GET_EDIT_PARTICIPANT_STAGES = "SP_EDIT_PARTICIPANT_GET_STAGES";

     
        public static string GetUrlUser { get; private set; } = string.Empty;
        public static string GetUrlOrganization { get; private set; } = string.Empty;
        public static string GetUrlContract { get; private set; } = string.Empty;
        public static string GetUrlParticipant { get; private set; } = string.Empty;
        public static string GetUrlFinanceOrganization { get; private set; } = string.Empty;
        public static string? GetHeaderFscm { get; private set; } = string.Empty;
        public static string getUrlEditParticipant { get; set; } = string.Empty;
        public static string ProgramUuidContract { get; set; } = string.Empty;
        public static void InitializeUrls()    
        {
                GetHeaderFscm=dbelo.Enummoduleparams
                    .Where(a => a.MKey == "URL_FSCM_HEADER")
                    .Select(a => a.MValue)
                    .FirstOrDefault()!;
                GetUrlUser = dbelo.Enummoduleparams
                    .Where(a => a.MKey == "URL_USER_FSCM")
                    .Select(a => a.MValue)
                    .FirstOrDefault()!;

                GetUrlOrganization = dbelo.Enummoduleparams
                    .Where(a => a.MKey == "URL_ORGANIZATION_FSCM")
                    .Select(a => a.MValue)
                    .FirstOrDefault()!;

                GetUrlContract = dbelo.Enummoduleparams
                    .Where(a => a.MKey == "URL_CONTRACT_FSCM")
                    .Select(a => a.MValue)
                    .FirstOrDefault()!;

                GetUrlParticipant = dbelo.Enummoduleparams
                    .Where(a => a.MKey == "URL_GET_CONTRACT_PARTICIPANT")
                    .Select(a => a.MValue)
                    .FirstOrDefault()!;

                GetUrlFinanceOrganization = dbelo.Enummoduleparams
                    .Where(a => a.MKey == "URL_FINANCE_ORGANIZATION_FSCM")
                    .Select(a => a.MValue)
                    .FirstOrDefault()!;
                getUrlEditParticipant = dbelo.Enummoduleparams
                    .Where(a => a.MKey == "URL_EDIT_CONTRACT_PARTICIPANT")
                    .Select(a => a.MValue)
                    .FirstOrDefault()!;
                ProgramUuidContract=dbelo.Enummoduleparams
                    .Where(a => a.MKey == "CREATE_CONTRACT_PROGRAMUUID")
                    .Select(a => a.MValue)
                    .FirstOrDefault()!;
           
        }
      




    }
}