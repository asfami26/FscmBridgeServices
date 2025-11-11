using FscmBridgeServices.Common.DTOS;
using FscmBridgeServices.DTOS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FscmBridgeServices.Services.Interface
{
    public interface IFscm_Service
    {
        Task<MessageProcessingFscm> SendRequest(RequestFscmDTOS req);
    }
}
