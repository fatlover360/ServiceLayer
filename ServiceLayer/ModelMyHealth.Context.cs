﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceLayer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ModelMyHealthContainer : DbContext
    {
        public ModelMyHealthContainer()
            : base("name=ModelMyHealthContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Utente> Utente { get; set; }
        public virtual DbSet<Alerta> Alerta { get; set; }
        public virtual DbSet<FrequenciaCardiacaValores> FrequenciaCardiacaValores { get; set; }
        public virtual DbSet<SaturacaoValores> SaturacaoValores { get; set; }
        public virtual DbSet<PressaoSanguineaValores> PressaoSanguineaValores { get; set; }
    }
}
