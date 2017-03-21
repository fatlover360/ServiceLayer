
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
    
public partial class TipoAlerta
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TipoAlerta()
    {

        this.FrequenciaCardiacaValoresSets = new HashSet<FrequenciaCardiacaValores>();

        this.PressaoSanguineaValoresSets = new HashSet<PressaoSanguineaValores>();

        this.SaturacaoValoresSets = new HashSet<SaturacaoValores>();

    }


    public int Id { get; set; }

    public int ValorMinimo { get; set; }

    public int ValorMaximo { get; set; }

    public int ValorCriticoMinimo { get; set; }

    public int ValorCriticoMaximo { get; set; }

    public string Nome { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<FrequenciaCardiacaValores> FrequenciaCardiacaValoresSets { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<PressaoSanguineaValores> PressaoSanguineaValoresSets { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<SaturacaoValores> SaturacaoValoresSets { get; set; }

}

}
