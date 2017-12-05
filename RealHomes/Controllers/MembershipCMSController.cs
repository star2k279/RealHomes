using System;
using System.Web.Mvc;
using System.Collections.Generic;

using System.Configuration;
using Umbraco.Web.Mvc;
using Umbraco.Core.Models;
using RealHomes.Helper;
using umbraco.cms.businesslogic.web;

using System.Threading.Tasks;
using System.Web.Configuration;
using RealHomes.Models;

namespace RealHomes.Controllers
{
    public class MembershipCMSController : SurfaceController
    {
        private const string VIEW_NAME = "Membership/_RegisterMember";

        [HttpGet]
        public ActionResult MemberLogin()
        {
            return PartialView(VIEW_NAME , new MembershipCMSModel());
        }
    }
}