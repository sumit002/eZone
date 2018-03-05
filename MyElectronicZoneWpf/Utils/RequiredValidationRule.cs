using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Globalization;

namespace MyElectronicZoneWpf.Utils
{
    class RequiredValidationRule : ValidationRule    
    {
        public RequiredValidationRule()
        {

        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)    
        {    
            if (value.ToString().Length > 0)    
            {    
                return new ValidationResult(true, null);    
            }
            else    
            {    
                return new ValidationResult(false, "Required !");    
            }
        }    
    }
}
