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
    
    public partial class CategoryImage
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string imageurl { get; set; }
    
        public virtual Category Category { get; set; }
    }
}
