using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Security.Common.Attributes
{
    public class PasswordAttributes
    {
        public static ValidationResult PasswordStrongTest(string value_, ValidationContext validationContext_)
        {
            if (value_ == null)
                return ValidationResult.Success;

            if (!Regex.IsMatch(value_, @"(?=^.{6,10}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$"))
                return new ValidationResult("It expects at least 1 small-case letter, 1 capital letter,1 digit, 1 special character and the length should be between 6-10 characters. ", new List<string> {"Password"});

            return ValidationResult.Success;
        }
    }
}