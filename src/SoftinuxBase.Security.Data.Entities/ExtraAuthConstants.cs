#pragma warning disable SA1636

// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.
#pragma warning restore SA1636
namespace SoftinuxBase.Security.Data.Entities
{
    public static class ExtraAuthConstants
    {
        public const int UserIdSize = 36; // This is the size of a GUID when returned as a string

        public const int RoleNameSize = 100;
    }
}