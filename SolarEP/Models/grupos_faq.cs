//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolarEP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class grupos_faq
    {
        public grupos_faq()
        {
            this.faq = new HashSet<faq>();
        }
    
        public int id { get; set; }
        public string grupo_faq { get; set; }
    
        public virtual ICollection<faq> faq { get; set; }
    }
}
