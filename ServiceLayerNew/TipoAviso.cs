
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
    
public partial class TipoAviso
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TipoAviso()
    {

        this.AvisoFrequenciaCardiacaSet = new HashSet<AvisoFrequenciaCardiaca>();

        this.AvisoSaturacaoSet = new HashSet<AvisoSaturacao>();

        this.AvisoPressaoSanguineaSet = new HashSet<AvisoPressaoSanguinea>();

    }


    public int Id { get; set; }

    public string Nome { get; set; }

    public int TempoMinimo { get; set; }

    public int TempoMaximo { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AvisoFrequenciaCardiaca> AvisoFrequenciaCardiacaSet { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AvisoSaturacao> AvisoSaturacaoSet { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AvisoPressaoSanguinea> AvisoPressaoSanguineaSet { get; set; }

}

}
