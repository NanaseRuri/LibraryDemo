using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace LibraryDemo.CustomConvention
{
    public class AutoValidationAntiForgeryTokenActionModelConvention:IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            if (IsConventionApplicationOnApplicable(action))
            {
                action.Filters.Add(new ValidateAntiForgeryTokenAttribute());
            }
        }

        public bool IsConventionApplicationOnApplicable(ActionModel action)
        {
            if (action.Attributes.Any(a=>a.GetType()==typeof(HttpPostAttribute))&&
                action.Attributes.All(a => a.GetType() != typeof(ValidateAntiForgeryTokenAttribute)))
            {
                return true;
            }

            return false;
        }
    }
}
