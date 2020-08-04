// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SoftinuxBase.Barebone.Extensions
{
    public static class ModelStateExtensions
    {
        public static IEnumerable<(string, string)> AllErrors(this ModelStateDictionary modelState_)
        {
            var result = new List<(string, string)>();
            var erroneousFields = modelState_.Where(ms_ => ms_.Value.Errors.Any())
                .Select(x_ => new { x_.Key, x_.Value.Errors });

            foreach (var erroneousField in erroneousFields)
            {
                var fieldKey = erroneousField.Key;
                var fieldErrors = erroneousField.Errors
                    .Select(error_ => (fieldKey, error_.ErrorMessage));
                result.AddRange(fieldErrors);
            }

            return result;
        }
    }
}