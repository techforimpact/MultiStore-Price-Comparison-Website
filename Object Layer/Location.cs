//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Object_Layer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Location
    {
        public int store_id { get; set; }
        public decimal longitude { get; set; }
        public decimal latitude { get; set; }
    
        public virtual Store Store { get; set; }
    }
}