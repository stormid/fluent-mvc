using System;
using System.Web.Mvc;

namespace FluentMvc.Spec
{
    public class TestAreaRegistration : AreaRegistration
    {
        public override void RegisterArea(AreaRegistrationContext context)
        {
            throw new NotImplementedException();
        }

        public override string AreaName
        {
            get { return "Test"; }
        }
    }
}