
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
    
public partial class SaturacaoValores
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public SaturacaoValores()
    {

        this.AvisoSaturacaoSet = new HashSet<AvisoSaturacao>();

    }


    public int Id { get; set; }

    public System.DateTime Data { get; set; }

    public System.TimeSpan Hora { get; set; }

    public int Saturacao { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AvisoSaturacao> AvisoSaturacaoSet { get; set; }

    public virtual Utente Utentes { get; set; }

}

}
