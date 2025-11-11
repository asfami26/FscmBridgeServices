using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using FscmBridgeServices.DTOS;
using FscmBridgeServices.Models;
using FscmBridgeServices.Services;
using FscmBridgeServices.Util;
using FscmBridgeServices.Services.Interface;
using FscmBridgeServices.Common.DTOS;

namespace FscmBridgeServices.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FscmController : ControllerBase
    {


        private readonly ILogger<FscmController> _logger;
        private readonly IFscm_Service _service;
        public FscmController(ILogger<FscmController> logger, IFscm_Service service)
        {
            _logger = logger;
            _service = service;

        }

        [HttpPost("SendData")]
        public new async Task<MessageProcessingFscm> Request([FromBody] RequestFscmDTOS req)
        {
           return await _service.SendRequest(req);
        }







}
}
