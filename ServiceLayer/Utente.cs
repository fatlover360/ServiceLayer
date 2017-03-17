//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Utente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Utente()
        {
            this.FrequenciaCardiacaValores = new HashSet<FrequenciaCardiacaValores>();
            this.SaturacaoValores = new HashSet<SaturacaoValores>();
            this.PressaoSanguineaValores = new HashSet<PressaoSanguineaValores>();
        }
    
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Apelido { get; set; }
        public int NIF { get; set; }
        public Nullable<int> Telefone { get; set; }
        public string Email { get; set; }
        public int NumeroEmergencia { get; set; }
        public string NomeEmergencia { get; set; }
        public string Morada { get; set; }
        public string Sexo { get; set; }
        public string Alergias { get; set; }
        public Nullable<double> Peso { get; set; }
        public Nullable<int> Altura { get; set; }
        public int SNS { get; set; }
        public System.DateTime DataNascimento { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FrequenciaCardiacaValores> FrequenciaCardiacaValores { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaturacaoValores> SaturacaoValores { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PressaoSanguineaValores> PressaoSanguineaValores { get; set; }
    }
}
