using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FscmBridgeServices.DTO;
using FscmBridgeServices.DTOS;
using FscmBridgeServices.Helper;
using FscmBridgeServices.Util;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FscmBridgeServices.Services
{
    
    public class GetDataFscmService
    {
        static DatabaseContext dbelo = new DatabaseContext();
     
        
        public static async Task<FscmOrganizationBuyer> GetOrganizationBuyerAsync(string param)
        {
            await using var dbContext = new DatabaseContext();
            var parameters = new[]
            {
                new SqlParameter("@ap_regno", param)
            };

            var result = await dbContext.ExecuteStoredProcedureFirstOrDefaultAsync<FscmOrganizationBuyer>(
                Constant.SP_GETPARAMORGANIZATIONFSCM, parameters);

            return result;
        }



        
        
        public static async Task<List<FscmFinanceOrganization>> GetOrganizationFinanceBuyerFscmAsync(string param,string regno)
        {
            await using (var dbContext = new DatabaseContext())
            {
                var parameters = new[]
                {
                    new SqlParameter("@ap_regno", regno)
                };
                var result = await dbContext.ExecuteStoredProcedureListAsync<FscmFinanceOrganization>(
                    Constant.SP_GET_FINANCE_ORGANIZATION, parameters);
                result.ForEach(org => org.organizationUuid = param);

                return result;
            }
        }
        public static async Task<FscmContract> GetDataContractAsync(string param)
        {
            var result = new FscmContract();
            SqlParameter CreateSqlParameter() => new SqlParameter("@ap_regno", SqlDbType.NVarChar) { Value = param ?? (object)DBNull.Value };
          
            try
            {
            

             
                var contractDataTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    return await GetContractDataAsync(dbContext,   new[] { CreateSqlParameter() });
                });

                var funderDataTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    return await GetFunderDataAsync(dbContext,  null);
                });

                var buyerDataTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    return await GetBuyerDataAsync(dbContext,  new[] { CreateSqlParameter() });
                });

                var sellerDataTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    return await GetSellerDataAsync(dbContext,  new[] { CreateSqlParameter() });
                });

                var optionRatesTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    return await GetOptionRatesAsync(dbContext,  new[] { CreateSqlParameter() });
                });

                var suspensionsTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    return await GetSuspensionsAsync(dbContext,  new[] { CreateSqlParameter() });
                });

            
                await Task.WhenAll(contractDataTask, funderDataTask, buyerDataTask, sellerDataTask, optionRatesTask, suspensionsTask);

                var contractData = await contractDataTask;
                var funderData = await funderDataTask;
                var buyerData = await buyerDataTask;
                var sellerData = await sellerDataTask;
                var optionRates = await optionRatesTask;
                var suspensions = await suspensionsTask;

                result = MapToContract(contractData, funderData, buyerData, sellerData, optionRates, suspensions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return result;
        }



        private static async Task<FscmContract> GetContractDataAsync(DatabaseContext dbContext, SqlParameter[] parameters)
        {
            return await dbContext.ExecuteStoredProcedureFirstOrDefaultAsync<FscmContract>(Constant.SP_GET_CONTRACT, parameters);
        }

        private static async Task<List<Funder>> GetFunderDataAsync(DatabaseContext dbContext,SqlParameter[] parameters)
        {
            return await dbContext.ExecuteStoredProcedureListAsync<Funder>(Constant.SP_GET_CONTRACT_FUNDER,parameters);
        }

        private static async Task<List<Buyer>> GetBuyerDataAsync(DatabaseContext dbContext,SqlParameter[] parameters)
        {
            return await dbContext.ExecuteStoredProcedureListAsync<Buyer>(Constant.SP_GET_CONTRACT_BUYER,parameters);
        }

        private static async Task<List<Seller>> GetSellerDataAsync(DatabaseContext dbContext,SqlParameter[] parameters)
        {
            return await dbContext.ExecuteStoredProcedureListAsync<Seller>(Constant.SP_GET_CONTRACT_SELLER,parameters);
        }

        private static async Task<List<OptionRate>> GetOptionRatesAsync(DatabaseContext dbContext,SqlParameter[] parameters)
        {
            return await dbContext.ExecuteStoredProcedureListAsync<OptionRate>(Constant.SP_GET_OPTION_RATES,parameters);
        }

        private static async Task<Suspension> GetSuspensionsAsync(DatabaseContext dbContext,SqlParameter[] parameters)
        {
            return await dbContext.ExecuteStoredProcedureFirstOrDefaultAsync<Suspension>(Constant.SP_GET_SUSPENSIONS,parameters);
        }

        private static FscmContract MapToContract(FscmContract contractData, List<Funder> funderData, List<Buyer> buyerData, List<Seller> sellerData, List<OptionRate> optionRates, Suspension suspensions)
        {
            return new FscmContract
            {
                programUuid = contractData.programUuid,
                name = contractData.name,
                description = contractData.description,
                isDeactivated = false,
                financingAfterCutOffTime = false,
                isAutoRetrySettlement = false,
                funder = funderData.Select(f => new Funder
                {
                    type = f.type,
                    organizationUuid = f.organizationUuid,
                    organizationName = f.organizationName,
                    code = f.code
                }).ToList(),
                buyer = buyerData.Select(b => new Buyer
                {
                    type = b.type,
                    organizationUuid = b.organizationUuid,
                    organizationName = b.organizationName,
                    code = b.code,
                    isPrincipal = false
                }).ToList(),
                seller = sellerData.Select(s => new Seller
                {
                    type =s.type,
                    organizationUuid = s.organizationUuid,
                    organizationName = s.organizationName,
                    code = s.code,
                    isPrincipal = s.isPrincipal
                }).ToList(),
                optionRates = optionRates.Select(o => new OptionRate
                {
                    currency = o.currency,
                    divisor = 360
                }).ToList(),
                suspensions = suspensions,
            };
        }

    public static async Task<EditContractParticipantFscm.buyerDto> EditDatContractParticipant(string ap_regno)
     {
            EditContractParticipantFscm.buyerDto buyerDto = new();

            try
            {
            
                var buyerTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    var parameters = new[]
                    {
                        new SqlParameter("@ap_regno", SqlDbType.NVarChar) { Value = ap_regno ?? (object)DBNull.Value }
                    };
                    return await dbContext.ExecuteStoredProcedureListAsync<EditContractParticipantFscm.buyerDto>(
                        Constant.GET_EDIT_PARTICIPANT_BUYER, parameters);
                });

                var suspensionTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    var parameters = new[]
                    {
                        new SqlParameter("@ap_regno", SqlDbType.NVarChar) { Value = ap_regno ?? (object)DBNull.Value }
                    };
                    return await dbContext.ExecuteStoredProcedureListAsync<EditContractParticipantFscm.suspension>(Constant.GET_EDIT_PARTICIPANT_SUSPENSION,parameters);
                });

                //sadasdsa
                var configurationsTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    return await dbContext.ExecuteStoredProcedureListAsync<EditContractParticipantFscm.configuration>(Constant.GET_EDIT_PARTICIPANT_CONFIGURATIONS);
                });

                var limitSettingsTask = Task.Run(async () =>
                {
                    var parameters = new[]
                    {
                        new SqlParameter("@ap_regno", SqlDbType.NVarChar) { Value = ap_regno ?? (object)DBNull.Value }
                    };
                    await using var dbContext = new DatabaseContext();
                    return await dbContext.ExecuteStoredProcedureListAsync<EditContractParticipantFscm.limitSetting>(Constant.GET_EDIT_PARTICIPANT_LIMIT_SETTINGS,parameters);
                });

                var outstandingParamsTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    return await dbContext.ExecuteStoredProcedureListAsync<EditContractParticipantFscm.outstandingParam>(Constant.GET_EDIT_PARTICIPANT_OUTSTANDING_PARAMS);
                });

                var settlementPhasesTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    return await dbContext.ExecuteStoredProcedureListAsync<EditContractParticipantFscm.settlementPhase>(Constant.GET_EDIT_PARTICIPANT_SETTLEMENT_PHASES);
                });

                var disbursementPhasesTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    return await dbContext.ExecuteStoredProcedureListAsync<EditContractParticipantFscm.disbursementPhase>(Constant.GET_EDIT_PARTICIPANT_DISBURSEMENT_PHASES);
                });

                var referenceRateTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    return await dbContext.ExecuteStoredProcedureFirstOrDefaultAsync<EditContractParticipantFscm.referenceRate>(Constant.GET_EDIT_PARTICIPANT_REFERENCE_RATE)
                           ?? new EditContractParticipantFscm.referenceRate();
                });

                var rabateTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    var rebateTemb=await dbContext.ExecuteStoredProcedureFirstOrDefaultAsync<EditContractParticipantFscm.rabate>(Constant.GET_EDIT_PARTICIPANT_RABATE);
                    rebateTemb.details = new List<object>();
                    
                    return rebateTemb;
                   
                });

                var commissionTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    return await dbContext.ExecuteStoredProcedureFirstOrDefaultAsync<EditContractParticipantFscm.commission>(Constant.GET_EDIT_PARTICIPANT_COMMISSION)
                           ?? new EditContractParticipantFscm.commission { configs = new List<EditContractParticipantFscm.commissionConfig>() };
                });

                var commissionConfigTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    return await dbContext.ExecuteStoredProcedureListAsync<EditContractParticipantFscm.commissionConfig>(Constant.GET_EDIT_PARTICIPANT_COMMISSION_CONFIG);
                });

                var stagesTask = Task.Run(async () =>
                {
                    await using var dbContext = new DatabaseContext();
                    return await dbContext.ExecuteStoredProcedureListAsync<EditContractParticipantFscm.stage>(Constant.GET_EDIT_PARTICIPANT_STAGES);
                });

                await Task.WhenAll(
                    buyerTask, suspensionTask, configurationsTask, limitSettingsTask, outstandingParamsTask,
                    settlementPhasesTask, disbursementPhasesTask, referenceRateTask, rabateTask, commissionTask,
                    commissionConfigTask, stagesTask
                );

                var buyer = (await buyerTask).FirstOrDefault();
                var suspension = (await suspensionTask).FirstOrDefault();
                var configurations = await configurationsTask;
                var limitSettings = await limitSettingsTask;
                var outstandingParams = await outstandingParamsTask;
                var settlementPhases = await settlementPhasesTask;
                var disbursementPhases = await disbursementPhasesTask;
                var referenceRate = await referenceRateTask;
                var rabate = await rabateTask;
                var commission = await commissionTask;
                commission.configs = await commissionConfigTask;
              var stagesData = (await stagesTask).Select(stage => new EditContractParticipantFscm.stage
                {
                    assetType = stage.assetType,
                    percentage = stage.percentage,
                    auto = stage.auto,
                    suspend = stage.suspend,
                    configurations = configurations,
                    limitSettings = limitSettings,
                    outstandingParams = outstandingParams,
                    rabate = rabate,  
                    disbursementPhases = disbursementPhases,
                    settlementPhases = settlementPhases,
                    insurances = new List<object>(),
                    bankCharges = new List<object>(),
                    referenceRate = referenceRate,
                    commission = commission,
                    notificationSettings = new EditContractParticipantFscm.notificationSettings() { upcomingSettlementNotification = "7" },
                    bookingOfficeDisbursement = stage?.bookingOfficeDisbursement ?? true,
                    bookingOfficeSettlement = stage?.bookingOfficeSettlement ?? true,
                    amortizationDate = stage?.amortizationDate ?? 25,
                    order = stage.order,
                    generalLedgers = new List<object>()
                }).ToList();

             
                List<EditContractParticipantFscm.accounts> accounts;
                List<EditContractParticipantFscm.accountDetail> accountDetails;

                await using (var dbContext = new DatabaseContext())
                {
                    accounts = await dbContext.ExecuteStoredProcedureListAsync<EditContractParticipantFscm.accounts>(
                        Constant.GET_EDIT_PARTICIPANT_ACCOUNTS,
                        new SqlParameter("@ap_regno", ap_regno)
                    );

                    accountDetails = await dbContext.ExecuteStoredProcedureListAsync<EditContractParticipantFscm.accountDetail>(
                        Constant.GET_EDIT_PARTICIPANT_ACCOUNTS,
                        new SqlParameter("@ap_regno", ap_regno)
                    );
                }

              
                var accountDetailsQueue = new Queue<EditContractParticipantFscm.accountDetail>(accountDetails);
                foreach (var account in accounts)
                {
                    if (accountDetailsQueue.TryDequeue(out var matchingDetail))
                    {
                        account.account = new EditContractParticipantFscm.accountDetail
                        {
                            uuid = matchingDetail.uuid
                        };
                    }
                }

               
                await Task.WhenAll(
                    settlementPhases.Select(async phase =>
                    {
                        await using var dbContext = new DatabaseContext();
                        var parameter = new SqlParameter("@param", SqlDbType.NVarChar) { Value = phase.phaseType };
                        phase.accounts = await dbContext.ExecuteStoredProcedureListAsync<EditContractParticipantFscm.accountTransfer>(
                            Constant.GET_EDIT_PARTICIPANT_SETTLEMENT_PHASE_ACCOUNTS, parameter);
                    }).ToList()
                );

                await Task.WhenAll(
                    disbursementPhases.Select(async phase =>
                    {
                        await using var dbContext = new DatabaseContext();
                        var parameter = new SqlParameter("@ap_regno", SqlDbType.NVarChar) { Value = phase.phaseType };
                        phase.accounts = await dbContext.ExecuteStoredProcedureListAsync<EditContractParticipantFscm.accountTransfer>(
                            Constant.GET_EDIT_PARTICIPANT_DISBURSEMENT_ACCOUNTS, parameter);
                    }).ToList()
                );


                await using (var dbContext = new DatabaseContext())
                {
                    referenceRate.interests = await dbContext.ExecuteStoredProcedureListAsync<EditContractParticipantFscm.interest>(Constant.GET_EDIT_PARTICIPANT_INTERESTS);
                }

                return new EditContractParticipantFscm.buyerDto()
                {
                    type = buyer?.type,
                    code = buyer?.code,
                    organizationUuid = buyer?.organizationUuid,
                    isPrincipal = buyer?.isPrincipal ?? false,
                    maximumFundingLimit = buyer?.maximumFundingLimit ?? 0,
                    overrideLimit = buyer?.overrideLimit ?? 0,
                    bundlingAsset = buyer.bundlingAsset,
                    suspend = buyer.suspend,
                    disbursementTransferApprovalEnabled = buyer.disbursementTransferApprovalEnabled,
                    settlementTransferApprovalEnabled = buyer.settlementTransferApprovalEnabled,
                    sellerConfigurationEnable = buyer.sellerConfigurationEnable,
                    totalAssetPercentageEnabled = buyer.totalAssetPercentageEnabled,
                    totalAssetPercentageValue = buyer.totalAssetPercentageValue,
                    payFeesEnabled = buyer.payFeesEnabled,
                    allocation = buyer.allocation,
                    holidayList = buyer.holidayList,
                    suspension = suspension ?? new EditContractParticipantFscm.suspension(),
                    stages = stagesData,
                    approvals = new List<object>(),
                    childs = new List<object>(),
                    accounts = accounts,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return buyerDto;
            }
      }








        
        
    }
}