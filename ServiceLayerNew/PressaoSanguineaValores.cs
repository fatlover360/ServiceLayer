//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceLayerNew
{
    using System;
    using System.Collections.Generic;
    
    public partial class PressaoSanguineaValores
    {
        public int Id { get; set; }
        public System.DateTime Data { get; set; }
        public System.TimeSpan Hora { get; set; }
        public int Distolica { get; set; }
        public int Sistolica { get; set; }
        public int Utente_Id { get; set; }
        public int Alertas_Id { get; set; }
    
        public virtual Alerta AlertaSet { get; set; }
        public virtual Utente UtenteSet { get; set; }
    }
}
