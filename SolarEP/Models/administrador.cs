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
    
    public partial class administrador
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string nome { get; set; }
        public string login { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public string mandante { get; set; }
        public string email_codigo { get; set; }
        public Nullable<int> tipo_usuario { get; set; }
        public Nullable<int> adm_tipo { get; set; }
    
        public virtual adm_tipo adm_tipo1 { get; set; }
    }
}