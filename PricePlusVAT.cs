//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoursesCompanyApp4438
{
    using System;
    using System.Collections.Generic;
    
    public partial class PricePlusVAT
    {
        public int Id { get; set; }
        public int Price_Id { get; set; }
        public int PriceVAT { get; set; }
    
        public virtual Price Price { get; set; }
    }
}