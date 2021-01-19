using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.WebApi.Utilities.WebApi
{
    public static class StatusCodeExtension
    {
        /// <summary>
        /// Return an ObjectResult with a StatusCode = 500
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="ex">Exception thrown. The Exception.Message will be returned as part of the response</param>
        /// <returns></returns>
        public static ObjectResult InternalServerError(this ControllerBase controller, Exception ex)
        {
            return controller.StatusCode(500, ex.Message);
        }

        /// <summary>
        /// Return an ObjectResult with a StatusCode = 500
        /// </summary>
        /// <returns></returns>
        public static StatusCodeResult InternalServerError(this ControllerBase controller)
        {
            return controller.StatusCode(500);
        }
    }
}
