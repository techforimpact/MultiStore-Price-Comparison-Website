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
    
    public partial class Comment
    {
        public int review_id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> product_id { get; set; }
        public string review_message { get; set; }
        public string review_name { get; set; }
        public string review_email { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}