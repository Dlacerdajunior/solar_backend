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
    
    public partial class equipamentos
    {
        public equipamentos()
        {
            this.equipamento_loja = new HashSet<equipamento_loja>();
        }
    
        public int id { get; set; }
        public Nullable<int> codigo { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public string url_foto { get; set; }
    
        public virtual ICollection<equipamento_loja> equipamento_loja { get; set; }
    }
}
