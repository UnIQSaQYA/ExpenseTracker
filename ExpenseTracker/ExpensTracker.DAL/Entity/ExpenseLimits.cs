//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExpensTracker.DAL.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExpenseLimits
    {
        public int Id { get; set; }
        public decimal LimitAmount { get; set; }
        public int CreatedBy { get; set; }
        public bool Status { get; set; }
    
        public virtual Users Users { get; set; }
    }
}
