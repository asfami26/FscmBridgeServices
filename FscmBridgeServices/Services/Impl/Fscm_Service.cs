
using AutoMapper;
using FscmBridgeServices.Common.DTOS;
using FscmBridgeServices.DTOS;
using FscmBridgeServices.Helper;
using FscmBridgeServices.Repository.DataContext;
using FscmBridgeServices.Repository.Entity;
using FscmBridgeServices.Services.Interface;
using FscmBridgeServices.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FscmBridgeServices.Services.Impl
{
    public class Fscm_Service : IFscm_Service
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public Fscm_Service(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;

        }

        #region GetSp Dynamic Function
        private async Task<TDto?> GetSPSingle<TEntity, TDto>(FormattableString sql)
           where TEntity : class
           where TDto : class
        {
            var entity = await _dataContext.Set<TEntity>()
                .FromSqlInterpolated(sql)
                .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            return _mapper.Map<TDto>(entity);
        }

        public async Task<List<TDto>> GetSPList<TEntity, TDto>(FormattableString sql)
            where TEntity : class
            where TDto : class
        {
            var entities = await _dataContext.Set<TEntity>()
                .FromSqlInterpolated(sql)
                .ToListAsync();

            if (entities.Count == 0)
                return new List<TDto>();

            return _mapper.Map<List<TDto>>(entities);
        }
        #endregion

        #region Get Service

        private async Task<FscmLogDto?> GetLastStageByRegno(string apRegno)
        {
            var maxId = await _dataContext.fscmLogs
                .Where(x => x.ap_regno == apRegno && x.UuidResponse != null)
                .MaxAsync(x => (int?)x.Id);

            if (maxId == null) return null;

            var entity = await _dataContext.fscmLogs
                .FirstOrDefaultAsync(x => x.Id == maxId.Value);

            if (entity == null) return null;

            return _mapper.Map<FscmLogDto>(entity);

        }

        private async Task<OrganizationBuyerDto?> GetOrganizationBuyer(string apRegno)
        {
            var entity = await _dataContext.organizationBuyer
                 .FromSqlInterpolated($"EXEC {Constant.SP_GETPARAMORGANIZATIONFSCM} @ap_regno = {apRegno}")
                 .FirstOrDefaultAsync();

            if (entity == null) return null;
            var dto = _mapper.Map<OrganizationBuyerDto>(entity);

            return dto;
        }

        private async Task<FscmLogDto?> GetStageByRegno(string ap_regno, string stageName)
        {
            var entity = await _dataContext.fscmLogs.FirstOrDefaultAsync(x => x.ap_regno == ap_regno && x.Type == stageName);
            if (entity == null) return null;
            return _mapper.Map<FscmLogDto>(entity);
        }

        private async Task<List<FinanceOrganizationDto>> GetOrganizationFinanceBuyerFscm(string param, string regno)
        {
            var entities = await _dataContext.financeOrganization
               .FromSqlInterpolated($"EXEC {Constant.SP_GET_FINANCE_ORGANIZATION} @ap_regno = {regno}")
               .ToListAsync();

            if (entities == null || entities.Count == 0)
                return new List<FinanceOrganizationDto>();

            entities.ForEach(e => e.organizationUuid = param);

            var dtoList = _mapper.Map<List<FinanceOrganizationDto>>(entities);

            return dtoList;
        }

        private async Task<FscmContractDto?> GetDataContract(string apRegno)
        {
            var contract = await GetSPSingle<Repository.Entity.FscmContract, FscmContractDto>($"EXEC {Constant.SP_GET_CONTRACT} @ap_regno={apRegno}");

            if (contract == null) return null;

            var funders = await GetSPList<Repository.Entity.Funder, FunderDto>($"EXEC {Constant.SP_GET_CONTRACT_FUNDER}");

            var buyers = await GetSPList<Repository.Entity.Buyer, BuyerDto>($"EXEC {Constant.SP_GET_CONTRACT_BUYER} @ap_regno={apRegno}");

            var sellers = await GetSPList<Repository.Entity.Seller, SellerDto>($"EXEC {Constant.SP_GET_CONTRACT_SELLER} @ap_regno={apRegno}");

            var optionRates = await GetSPList<Repository.Entity.OptionRate, OptionRateDto>($"EXEC {Constant.SP_GET_OPTION_RATES} @ap_regno={apRegno}");

            var suspension = await GetSPSingle<Repository.Entity.Suspension, SuspensionDto>($"EXEC {Constant.SP_GET_SUSPENSIONS} @ap_regno={apRegno}");

            if (buyers != null)
            {
                foreach (var b in buyers)
                {
                    b.isPrincipal = false;
                }
            }

            if (optionRates != null)
            {
                foreach (var o in optionRates)
                {
                    o.divisor = 360;
                }
            }

            contract.Funder = funders ?? new List<FunderDto>();
            contract.Buyer = buyers ?? new List<BuyerDto>();
            contract.Seller = sellers ?? new List<SellerDto>();
            contract.OptionRates = optionRates ?? new List<OptionRateDto>();
            contract.Suspensions = suspension;

            return contract;
        }

        private Task<EditContractParticipantFscm.buyerDto> EditDatContractParticipant(string ap_regno)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Process Service
        private async Task<MessageProcessingFscm> ProcessingOrganization(string getUrlOrganization, string? headerFscm, string ap_regno, string user_id)
        {
            var fscmOrganization = await GetOrganizationBuyer(ap_regno);
            if (fscmOrganization == null)
            {
                return new MessageProcessingFscm()
                {
                    status = false,
                    Message = "Failed to Get Data Organization Buyer."
                };

            }
            var checkIfExist = await _dataContext.fscmLogs
                .FirstOrDefaultAsync(x => x.Type == "OrganizationBuyer" && x.Name == "reference" && x.VALUE == fscmOrganization.Reference && x.ap_regno == ap_regno);

            if (checkIfExist != null)
            {
                return new MessageProcessingFscm()
                {
                    status = false,
                    Message = "Organization buyer already exists."
                };
            }

            var newLogDto = new FscmLogDto
            {
                Type = "OrganizationBuyer",
                Name = "reference",
                ApRegno = ap_regno,
                CreatedBy = user_id,
                VALUE = fscmOrganization.Reference,
                UuidResponse = string.Empty,
                RequestJson = JsonHelper.ToJSON(fscmOrganization),
                ResponseJson = string.Empty,
            };

            var newLogEntity = _mapper.Map<FscmLog>(newLogDto);
            _dataContext.fscmLogs.Add(newLogEntity);
            await _dataContext.SaveChangesAsync();

            var organizationResponse = JsonHelper.SendJsonDataToUrl(getUrlOrganization, fscmOrganization, await JsonHelper.GetTokenSikp(), headerFscm);

            if (organizationResponse.StatusCode is < 200 or >= 300)
            {
                return new MessageProcessingFscm()
                {
                    status = false,
                    Message = organizationResponse.Message + "OrganizationBuyer"
                };
            }


            var existingLog = await _dataContext.fscmLogs
                .Where(l => l.Type == "OrganizationBuyer" && l.Name == "reference" && l.CreatedBy == user_id && l.ap_regno == ap_regno)
                .FirstOrDefaultAsync();

            if (existingLog != null)
            {
                existingLog.UuidResponse = organizationResponse.data[0];
                existingLog.ResponseJson = JsonHelper.ToJSON(organizationResponse);
                await _dataContext.SaveChangesAsync();
            }


            return new MessageProcessingFscm()
            {
                status = true,
                Message = organizationResponse.data[0]
            };
        }

        private async Task<MessageProcessingFscm> ProcessingFinancialOrganization(string organizationUuid, string getUrlFinance, string? headerFscm, string apRegNo, string userId)
        {
            var fscmOrganization = await GetOrganizationFinanceBuyerFscm(organizationUuid, apRegNo);

            var requiredTypes = new[]
            {
                "OrganizationFinancialBuyerLOAN",
                "OrganizationFinancialBuyerGIRO",
                "OrganizationFinancialBuyerTRANSACTION",
                "OrganizationFinancialBuyerINTEREST"
            };

            if (fscmOrganization == null || fscmOrganization.Count == 0)
            {
                var existingTypes = await _dataContext.fscmLogs
                    .Where(x => x.ap_regno == apRegNo && requiredTypes.Contains(x.Type))
                    .Select(x => x.Type)
                    .Distinct()
                    .ToListAsync();

                bool allTypesAlreadySent = requiredTypes.All(t => existingTypes.Contains(t));

                return new MessageProcessingFscm
                {
                    status = allTypesAlreadySent,
                    Message = allTypesAlreadySent
                        ? "All Stage Finance Already Sent"
                        : "Failed to Get Data Financial Buyer"
                };
            }

            var accountNumbers = fscmOrganization
                .Select(org => org.AccountNumber)
                .Where(an => !string.IsNullOrEmpty(an))
                .ToList();

            bool isAccountNumberSent = await _dataContext.fscmLogs.AnyAsync(x =>
                requiredTypes.Contains(x.Type) &&
                x.Name == "AccountNumber" &&
                accountNumbers.Contains(x.VALUE) &&
                x.ap_regno == apRegNo
            );

            if (isAccountNumberSent)
            {
                return new MessageProcessingFscm
                {
                    status = false,
                    Message = "FinancialOrganization AccountNumber Already Sent"
                };
            }

            var newLogDtos = fscmOrganization.Select(org =>
            {
                var firstWord = (org.AccountName ?? string.Empty)
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault() ?? string.Empty;

                return new FscmLogDto
                {
                    Type = "OrganizationFinancialBuyer" + firstWord,
                    Name = "AccountNumber",
                    ApRegno = apRegNo,
                    CreatedBy = userId,
                    VALUE = org.AccountNumber,
                    RequestJson = JsonHelper.ToJSON(org),
                    ResponseJson = null,
                    UuidResponse = null
                };
            }).ToList();

            var newLogEntities = _mapper.Map<List<FscmLog>>(newLogDtos);

            await _dataContext.fscmLogs.AddRangeAsync(newLogEntities);
            await _dataContext.SaveChangesAsync();

            var organizationResponse = JsonHelper.SendJsonDataToUrl(
                getUrlFinance,
                fscmOrganization,
                await JsonHelper.GetTokenSikp(),
                headerFscm
            );

            if (organizationResponse.StatusCode is < 200 or >= 300)
            {
                return new MessageProcessingFscm
                {
                    status = false,
                    Message = organizationResponse.Message + " finansial"
                };
            }

            var responseDataList = organizationResponse?.data ?? new List<string>();

            if (responseDataList.Count != fscmOrganization.Count)
            {
                return new MessageProcessingFscm
                {
                    status = false,
                    Message = "Mismatch between organization request and response count!"
                };
            }

            var existingLogEntities = await _dataContext.fscmLogs
                .Where(l =>
                    l.Type.StartsWith("OrganizationFinancialBuyer") &&
                    l.Name == "AccountNumber" &&
                    l.CreatedBy == userId &&
                    l.ap_regno == apRegNo)
                .OrderBy(l => l.Id)
                .ToListAsync();

            if (existingLogEntities.Count != responseDataList.Count)
            {
                return new MessageProcessingFscm
                {
                    status = false,
                    Message = "Mismatch between logs and response count!"
                };
            }

            var existingLogDtos = _mapper.Map<List<FscmLogDto>>(existingLogEntities);

            for (int i = 0; i < existingLogDtos.Count; i++)
            {
                var uuid = responseDataList[i];
                existingLogDtos[i].ResponseJson = JsonHelper.ToJSON(uuid);
                existingLogDtos[i].UuidResponse = uuid;
            }

            _mapper.Map(existingLogDtos, existingLogEntities);
            await _dataContext.SaveChangesAsync();

            return new MessageProcessingFscm
            {
                status = true,
                Message = "Financial Organization data successfully processed"
            };
        }

        private async Task<MessageProcessingFscm> ProcessingContract(string programUuid, string getUrlContract, string? headerFscm, string ap_regno, string user_id)
        {
            var fscmContract = await GetDataContract(ap_regno);

            if (fscmContract == null ||
              string.IsNullOrEmpty(fscmContract.Name) ||
              fscmContract.Buyer == null || fscmContract.Buyer.Count == 0 ||
              fscmContract.Seller == null || fscmContract.Seller.Count == 0)
            {
                return new MessageProcessingFscm()
                {
                    status = false,
                    Message = "Failed to Get Data Create Contract"
                };
            }

            fscmContract.ProgramUuid = programUuid;
            var checkIfExist = await _dataContext.fscmLogs.FirstOrDefaultAsync(x => x.Type == "CreateContract" && x.Name == "ContractName" &&
                                x.VALUE == fscmContract.Name && x.ap_regno == ap_regno);
            if (checkIfExist != null)
            {
                return new MessageProcessingFscm()
                {
                    status = false,
                    Message = "contract name Already Sent"
                };
            }
            var newLog = new FscmLogDto()
            {
                Type = "CreateContract",
                Name = "ContractName",
                RequestJson = JsonHelper.ToJSON(fscmContract),
                ResponseJson = null,
                CreatedBy = user_id,
                ApRegno = ap_regno,
                VALUE = fscmContract.Name,
                UuidResponse = null
            };

            var newLogEntity = _mapper.Map<FscmLog>(newLog);
            _dataContext.fscmLogs.Add(newLogEntity);
            await _dataContext.SaveChangesAsync();

            var contractResponse = JsonHelper.SendJsonDataToUrl(getUrlContract, fscmContract, await JsonHelper.GetTokenSikp(), headerFscm);

            if (contractResponse.StatusCode is < 200 or >= 300)
            {
                return new MessageProcessingFscm()
                {
                    status = false,
                    Message = contractResponse.Message
                };
            }

            var existingLog = await _dataContext.fscmLogs
                .Where(l => l.Type == "CreateContract" && l.Name == "ContractName" && l.CreatedBy == user_id && l.ap_regno == ap_regno)
                .FirstOrDefaultAsync();

            if (existingLog != null)
            {
                existingLog.ResponseJson = JsonHelper.ToJSON(contractResponse);
                existingLog.UuidResponse = contractResponse.data[0];
                await _dataContext.SaveChangesAsync();
            }

            return new MessageProcessingFscm()
            {
                status = true,
                Message = contractResponse.data[0]
            };
        }

        private async Task<MessageProcessingFscm> ProcessingGetContract(string type, string getUrlGetcontract,
            string contractUuid)
        {
            string organizationUuid = await JsonHelper.GetOrganizationUuidAsync(contractUuid, type, getUrlGetcontract, await JsonHelper.GetTokenSikp());
            if (organizationUuid == null)
            {
                return new MessageProcessingFscm()
                {
                    status = false,
                    Message = "Failed to Get Data Contract"
                };

            }
            return new MessageProcessingFscm()
            {
                status = true,
                Message = organizationUuid,
            };
        }

        private async Task<MessageProcessingFscm> ProcessingEditContract(string baseUrl, string buyerContractParticipantUuid, string headerfscm, string ap_regno, string user_id)
        {
            var editContract = await EditDatContractParticipant(ap_regno);
            if (editContract == null)
            {
                return new MessageProcessingFscm()
                {
                    status = false,
                    Message = "Failed to Get Data Edit Contract"
                };

            }

            var (status, message) = await JsonHelper.UpdateJsonToUrl(baseUrl, buyerContractParticipantUuid, await JsonHelper.GetTokenSikp(), editContract, headerfscm);
            var newLog = new FscmLogDto()
            {
                Type = "EditContractBuyer",
                Name = "EditContractBuyer",
                RequestJson = JsonHelper.ToJSON(editContract),
                ResponseJson = string.Empty,
                CreatedBy = user_id,
                ApRegno = ap_regno,
                VALUE = message ?? string.Empty,
            };
            var newLogEntity = _mapper.Map<FscmLog>(newLog);
            _dataContext.fscmLogs.Add(newLogEntity);
            await _dataContext.SaveChangesAsync();
            if (Convert.ToInt32(status) is < 200 or >= 300)
            {
                return new MessageProcessingFscm()
                {
                    status = false,
                    Message = message
                };

            }


            return new MessageProcessingFscm()
            {
                status = true,
                Message = $"Status Code: {status}"
            };
        }
        #endregion

        public async Task<MessageProcessingFscm> SendRequest(RequestFscmDTOS req)
        {
            Constant.InitializeUrls();
            var getLastStage = await GetLastStageByRegno(req.ap_regno) ??
                                   new FscmLogDto { Type = "Start" };

            MessageProcessingFscm processingContract = null;
            MessageProcessingFscm processingFinance = null;
            string contractUuid = getLastStage.UuidResponse;
            if (getLastStage.Type == "Start")
            {
                var processingOrganization = await ProcessingOrganization(
                    Constant.GetUrlOrganization, Constant.GetHeaderFscm, req.ap_regno, req.user_id);

                if (!processingOrganization.status)
                {
                    return new MessageProcessingFscm { status = false, Message = processingOrganization.Message };
                }

                processingFinance = await ProcessingFinancialOrganization(
                    processingOrganization.Message, Constant.GetUrlFinanceOrganization, Constant.GetHeaderFscm,
                    req.ap_regno, req.user_id);

                if (!processingFinance.status)
                {
                    return new MessageProcessingFscm { status = false, Message = processingFinance.Message };
                }

                processingContract = await ProcessingContract(
                    Constant.ProgramUuidContract, Constant.GetUrlContract, Constant.GetHeaderFscm, req.ap_regno,
                    req.user_id);

                if (!processingContract.status)
                {
                    return new MessageProcessingFscm { status = false, Message = processingContract.Message };
                }

                contractUuid = processingContract.Message;
            }

            if (getLastStage.Type is "Start" or "OrganizationBuyer" or "OrganizationFinancialBuyerLOAN" or
                "OrganizationFinancialBuyerGIRO" or "OrganizationFinancialBuyerTRANSACTION" or
                "OrganizationFinancialBuyerINTEREST")
            {
                if (processingFinance == null)
                {
                    var getsBuyerUuid = await GetStageByRegno(req.ap_regno, "OrganizationBuyer");
                    processingFinance = await ProcessingFinancialOrganization(
                        getsBuyerUuid.UuidResponse, Constant.GetUrlFinanceOrganization, Constant.GetHeaderFscm,
                        req.ap_regno, req.user_id);

                    if (!processingFinance.status)
                    {
                        return new MessageProcessingFscm { status = false, Message = processingFinance.Message };
                    }
                }

                if (processingContract == null)
                {
                    processingContract = await ProcessingContract(
                        Constant.ProgramUuidContract, Constant.GetUrlContract, Constant.GetHeaderFscm,
                        req.ap_regno, req.user_id);

                    if (!processingContract.status)
                    {
                        return new MessageProcessingFscm { status = false, Message = processingContract.Message };
                    }

                    contractUuid = processingContract.Message;
                }
            }

            if (getLastStage.Type is "Start" or "OrganizationBuyer" or "OrganizationFinancialBuyerLOAN" or
                "OrganizationFinancialBuyerGIRO" or "OrganizationFinancialBuyerTRANSACTION" or
                "OrganizationFinancialBuyerINTEREST" or "CreateContract")
            {
                var getContract = await ProcessingGetContract(
                    "BUYER", Constant.GetUrlParticipant, contractUuid);

                var processingEditContract = await ProcessingEditContract(
                    Constant.getUrlEditParticipant, getContract.Message, Constant.GetHeaderFscm, req.ap_regno, req.user_id);

                if (processingEditContract.status)
                {
                    return new MessageProcessingFscm { status = true, Message = "FSCM SUCCESSFULLY SENT" };
                }
            }

            if (getLastStage.Type == "EditContractBuyer")
            {
                return new MessageProcessingFscm { status = false, Message = "All Stage Already Sent" };
            }

            return new MessageProcessingFscm { status = false, Message = "Unexpected case encountered" };
        }

    }
}
