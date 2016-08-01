using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Abp.Web.Api.SwaggerTool.SampleSite.Service
{
    
    public interface ISampleAppService:IApplicationService
    {
       
        Task<string> GetById(int Id);
    }

    public class SampleAppService : ApplicationService, ISampleAppService
    {
        public async Task<string> GetById(int Id)
        {
            return Id.ToString();
        }
    }
}