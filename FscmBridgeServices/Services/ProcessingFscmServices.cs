using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FscmBridgeServices.DTOS;
using FscmBridgeServices.Helper;
using FscmBridgeServices.Models;
using FscmBridgeServices.Util;
using Microsoft.EntityFrameworkCore;


namespace FscmBridgeServices.Services
{
    public class ProcessingFscmServices
    {
        
        static DatabaseContext dbelo = new DatabaseContext();

        //public async static Task<FscmLog> GetLastStageByRegno(string ap_regno)
        //{
        //    var maxId = await dbelo.FscmLogs
        //        .Where(x => x.ap_regno == ap_regno && x.UuidResponse != null) 
        //        .MaxAsync(x => (int?)x.Id); 

        //    if (maxId == null) return null; 

        //    return await dbelo.FscmLogs
        //        .FirstOrDefaultAsync(x => x.Id == maxId);
        //}

        //public async static Task<FscmLog> GetStageByRegno(string ap_regno,string stageName)
        //{
        //    var maxId = await dbelo.FscmLogs
        //        .Where(x => x.ap_regno == ap_regno&&x.Type==stageName).FirstOrDefaultAsync();
        //    return maxId;

        //}
        
        //public async static Task<MessageProcessingFscm> ProcessingOrganization(string getUrlOrganization, string? headerFscm,string ap_regno,string user_id)
        //{
        //    var fscmOrganization = await GetDataFscmService.GetOrganizationBuyerAsync(ap_regno);
        //    if (fscmOrganization == null)
        //    {
        //        return new MessageProcessingFscm()
        //        {
        //            status = false,
        //            Message = "Failed to Get Data Organization Buyer."
        //        };
                
        //    }
        //    var checkIfExist = await dbelo.FscmLogs
        //        .FirstOrDefaultAsync(x => x.Type == "OrganizationBuyer" && x.Name == "reference" && x.VALUE == fscmOrganization.reference&&x.ap_regno==ap_regno);

        //    if (checkIfExist != null)
        //    {
        //        return new MessageProcessingFscm()
        //        {
        //            status = false,
        //            Message = "Organization buyer already exists."
        //        };
        //    }
        //    var newLog = new FscmLog()
        //    {
        //        Type = "OrganizationBuyer",
        //        Name = "reference",
        //        ap_regno = ap_regno,
        //        CreatedBy = user_id,
        //        VALUE = fscmOrganization.reference,
        //        UuidResponse = string.Empty, 
        //        RequestJson = JsonHelper.ToJSON(fscmOrganization),
        //        ResponseJson = string.Empty 
        //    };


        //    dbelo.FscmLogs.Add(newLog);
        //    await dbelo.SaveChangesAsync();


        //    var organizationResponse = JsonHelper.SendJsonDataToUrl(getUrlOrganization, fscmOrganization, await JsonHelper.GetTokenSikp(), headerFscm);

        //    if (organizationResponse.StatusCode is < 200 or >= 300)
        //    {
        //        return new MessageProcessingFscm()
        //        {
        //            status = false,
        //            Message = organizationResponse.Message + "OrganizationBuyer"
        //        };
        //    }


        //    var existingLog = await dbelo.FscmLogs
        //        .Where(l => l.Type == "OrganizationBuyer" && l.Name == "reference" && l.CreatedBy == user_id && l.ap_regno == ap_regno)
        //        .FirstOrDefaultAsync();

        //    if (existingLog != null)
        //    {
        //        existingLog.UuidResponse = organizationResponse.data[0];
        //        existingLog.ResponseJson = JsonHelper.ToJSON(organizationResponse);
        //        await dbelo.SaveChangesAsync();
        //    }


        //    return new MessageProcessingFscm()
        //    {
        //        status = true,
        //        Message = organizationResponse.data[0]
        //    };

        //}


        // public static async Task<MessageProcessingFscm> ProcessingFinancialOrganization(
        //    string organizationUuid, string getUrlFinance, string? headerFscm, string apRegNo, string userId)
        //{
        //    var fscmOrganization = await GetDataFscmService.GetOrganizationFinanceBuyerFscmAsync(organizationUuid, apRegNo);
        //    var requiredTypes = new[]
        //    {
        //        "OrganizationFinancialBuyerLOAN",
        //        "OrganizationFinancialBuyerGIRO",
        //        "OrganizationFinancialBuyerTRANSACTION",
        //        "OrganizationFinancialBuyerINTEREST"
        //    };
        //    bool allTypesAlreadySent = requiredTypes.All(type =>
        //        dbelo.FscmLogs.Any(x => x.Type == type && x.ap_regno == apRegNo)
        //    );
        //    if (fscmOrganization.Count == 0)
        //    {
        //        return new MessageProcessingFscm
        //        {
        //            status = allTypesAlreadySent,
        //            Message = allTypesAlreadySent 
        //                ? "All Stage Finance Already Sent" 
        //                : "Failed to Get Data Financial Buyer"
        //        };
        //    }


           
        //    var accountNumbers = fscmOrganization.Select(org => org.accountNumber).ToList();
        //    bool isAccountNumberSent = await dbelo.FscmLogs.AnyAsync(x =>
        //        requiredTypes.Contains(x.Type) &&
        //        x.Name == "AccountNumber" &&
        //        accountNumbers.Contains(x.VALUE)
        //        &&x.ap_regno==apRegNo
        //    );

        //    if (isAccountNumberSent)
        //    {
        //        return new MessageProcessingFscm
        //        {
        //            status = false,
        //            Message = "FinancialOrganization AccountNumber Already Sent"
        //        };
        //    }

        //    var newLogs = fscmOrganization.Select(org => new FscmLog
        //    {
        //        Type = "OrganizationFinancialBuyer" + org.accountName.Split(" ")[0],
        //        Name = "AccountNumber",
        //        ap_regno = apRegNo,
        //        CreatedBy = userId,
        //        VALUE = org.accountNumber,
        //        RequestJson = JsonHelper.ToJSON(org),
        //        ResponseJson = null, // Kosong dulu
        //        UuidResponse = null  // Kosong dulu
        //    }).ToList();

        //    dbelo.FscmLogs.AddRange(newLogs);
        //    await dbelo.SaveChangesAsync();


        //    var organizationResponse = JsonHelper.SendJsonDataToUrl(
        //        getUrlFinance,
        //        fscmOrganization,
        //        await JsonHelper.GetTokenSikp(),
        //        headerFscm
        //    );

        //    if (organizationResponse.StatusCode is < 200 or >= 300)
        //    {
        //        return new MessageProcessingFscm
        //        {
        //            status = false,
        //            Message = organizationResponse.Message + " finansial"
        //        };
        //    }

        //    var responseDataList = organizationResponse?.data ?? new List<string>();

        //    if (responseDataList.Count != fscmOrganization.Count)
        //    {
        //        return new MessageProcessingFscm
        //        {
        //            status = false,
        //            Message = "Mismatch between organization request and response count!"
        //        };
        //    }

        //    var existingLogs = await dbelo.FscmLogs
        //        .Where(l => l.Type.StartsWith("OrganizationFinancialBuyer") && l.Name == "AccountNumber" && l.CreatedBy == userId && l.ap_regno == apRegNo)
        //        .ToListAsync();

        //    for (int i = 0; i < existingLogs.Count; i++)
        //    {
        //        existingLogs[i].ResponseJson = JsonHelper.ToJSON(organizationResponse.data[i]);
        //        existingLogs[i].UuidResponse = organizationResponse.data[i];
        //    }

        //    await dbelo.SaveChangesAsync();

        //    return new MessageProcessingFscm
        //    {
        //        status = true,
        //        Message = "Financial Organization data successfully processed"
        //    };
        //}
        //public static async  Task<MessageProcessingFscm> ProcessingContract(string programUuid,
        //    string getUrlContract, string? headerFscm,string ap_regno,string user_id)
        //{
            
            
        //    var fscmContract = await GetDataFscmService.GetDataContractAsync(ap_regno);
        //    if (fscmContract == null || 
        //        string.IsNullOrEmpty(fscmContract.name) || 
        //        fscmContract.buyer == null || fscmContract.buyer.Count == 0 || 
        //        fscmContract.seller == null || fscmContract.seller.Count == 0)
        //    {
        //        return new MessageProcessingFscm()
        //        {
        //            status = false,
        //            Message = "Failed to Get Data Create Contract"
        //        };
        //    }

        //    fscmContract.programUuid = programUuid;
        //    var checkIfExist = await dbelo.FscmLogs.FirstOrDefaultAsync(x => x.Type == "CreateContract" && x.Name == "ContractName" && x.VALUE == fscmContract.name&&x.ap_regno==ap_regno);
        //    if (checkIfExist != null)
        //    {
        //        return new MessageProcessingFscm()
        //        {
        //            status = false,
        //            Message = "contract name Already Sent"
        //        };
        //    }
        //    var newLog = new FscmLog()
        //    {
        //        Type = "CreateContract",
        //        Name = "ContractName",
        //        RequestJson = JsonHelper.ToJSON(fscmContract),
        //        ResponseJson = null,
        //        CreatedBy = user_id,
        //        ap_regno = ap_regno,
        //        VALUE = fscmContract.name,
        //        UuidResponse = null
        //    };

        //    dbelo.FscmLogs.Add(newLog);
        //    await dbelo.SaveChangesAsync();

        //    var contractResponse = JsonHelper.SendJsonDataToUrl(getUrlContract, fscmContract, await JsonHelper.GetTokenSikp(), headerFscm);

        //    if (contractResponse.StatusCode is < 200 or >= 300)
        //    {
        //        return new MessageProcessingFscm()
        //        {
        //            status = false,
        //            Message = contractResponse.Message
        //        };
        //    }

        //    var existingLog = await dbelo.FscmLogs
        //        .Where(l => l.Type == "CreateContract" && l.Name == "ContractName" && l.CreatedBy == user_id && l.ap_regno == ap_regno)
        //        .FirstOrDefaultAsync();

        //    if (existingLog != null)
        //    {
        //        existingLog.ResponseJson = JsonHelper.ToJSON(contractResponse);
        //        existingLog.UuidResponse = contractResponse.data[0];
        //        await dbelo.SaveChangesAsync();
        //    }

        //    return new MessageProcessingFscm()
        //    {
        //        status = true,
        //        Message =contractResponse.data[0]
        //    };
        //}

        //public static async Task<MessageProcessingFscm> ProcessingGetContract(string type, string getUrlGetcontract,
        //    string contractUuid)
        //{
        //    string organizationUuid = await JsonHelper.GetOrganizationUuidAsync(contractUuid, type, getUrlGetcontract, await JsonHelper.GetTokenSikp());
        //    if (organizationUuid == null)
        //    {
        //        return new MessageProcessingFscm()
        //        {
        //            status = false,
        //            Message = "Failed to Get Data Contract"
        //        };
                
        //    }
        //    return new MessageProcessingFscm()
        //    {
        //        status = true,
        //        Message =organizationUuid,
        //    };
        //}

        public static async Task<MessageProcessingFscm> ProcessingEditContract(string baseUrl,string buyerContractParticipantUuid,string headerfscm,string ap_regno,string user_id)
        {
            var editContract=  await GetDataFscmService.EditDatContractParticipant(ap_regno);
            if (editContract == null)
            {
                return new MessageProcessingFscm()
                {
                    status = false,
                    Message = "Failed to Get Data Edit Contract"
                };
                
            }
         
            var (status, message)   = await JsonHelper.UpdateJsonToUrl(baseUrl, buyerContractParticipantUuid, await JsonHelper.GetTokenSikp(), editContract,headerfscm);
            var newLog = new FscmLog()
            {
                Type = "EditContractBuyer",
                Name = "EditContractBuyer",
                RequestJson = JsonHelper.ToJSON(editContract),
                ResponseJson = string.Empty,
                CreatedBy = user_id,
                ap_regno = ap_regno,
                VALUE = message??string.Empty,
            };
            dbelo.FscmLogs.Add(newLog);
            await dbelo.SaveChangesAsync();
            if (Convert.ToInt32(status) is < 200 or >= 300)
            {
                return new MessageProcessingFscm()
                {
                    status = false,
                    Message =message
                };
                
            }
       
        
            return new MessageProcessingFscm()
            {
                status = true,
                Message =$"Status Code: {status}"
            };
        }
    }
}