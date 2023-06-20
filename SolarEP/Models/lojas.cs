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
    
    public partial class lojas
    {
        public lojas()
        {
            this.campanha_foto = new HashSet<campanha_foto>();
            this.campanha_loja = new HashSet<campanha_loja>();
            this.equipamento_loja = new HashSet<equipamento_loja>();
            this.listacompra_loja = new HashSet<listacompra_loja>();
            this.log_acesso = new HashSet<log_acesso>();
            this.solicitacao = new HashSet<solicitacao>();
            this.pedido = new HashSet<pedido>();
            this.visita = new HashSet<visita>();
        }
    
        public string cnpj_loja { get; set; }
        public string razao_social_loja { get; set; }
        public string loja_ativa { get; set; }
        public string nome_contato { get; set; }
        public string email_contato { get; set; }
        public int id { get; set; }
        public string lattext { get; set; }
        public string longtext { get; set; }
        public string senha { get; set; }
        public Nullable<int> status_loja { get; set; }
        public string email_codigo { get; set; }
        public string kunnr { get; set; }
        public Nullable<System.DateTime> created { get; set; }
    
        public virtual ICollection<campanha_foto> campanha_foto { get; set; }
        public virtual ICollection<campanha_loja> campanha_loja { get; set; }
        public virtual ICollection<equipamento_loja> equipamento_loja { get; set; }
        public virtual ICollection<listacompra_loja> listacompra_loja { get; set; }
        public virtual ICollection<log_acesso> log_acesso { get; set; }
        public virtual loja_status loja_status { get; set; }
        public virtual ICollection<solicitacao> solicitacao { get; set; }
        public virtual ICollection<pedido> pedido { get; set; }
        public virtual ICollection<visita> visita { get; set; }
    }
}