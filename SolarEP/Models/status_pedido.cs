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
    
    public partial class status_pedido
    {
        public status_pedido()
        {
            this.pedido = new HashSet<pedido>();
        }
    
        public int id { get; set; }
        public string status_nome { get; set; }
    
        public virtual ICollection<pedido> pedido { get; set; }
    }
}
