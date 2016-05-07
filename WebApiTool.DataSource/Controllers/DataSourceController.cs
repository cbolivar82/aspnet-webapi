using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiTool.DataSource.Models;
using WebApiTool.DataSource.Service;

namespace WebApiTool.DataSource.Controllers
{
    public class DataSourceController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetData(DataSourceRequest request)
        {
            try
            {
                #region <Validation>

                if (request == null)
                    return BadRequest("The [requestGetData] value to method is NULL");

                if (string.IsNullOrEmpty(request.ResourceType))
                    return BadRequest("The [ResourceType] cannot be null");

                if (string.IsNullOrEmpty(request.ResourceName))
                    return BadRequest("The [ResourceName] cannot be null");

                if (string.IsNullOrEmpty(request.ConnectionStringName))
                    return BadRequest("The [ConnectionStringName] cannot be null");
                #endregion

                return Json(DataSourceService.LoadData(request));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
