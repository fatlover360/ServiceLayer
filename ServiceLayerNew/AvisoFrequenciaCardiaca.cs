
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
    
public partial class AvisoFrequenciaCardiaca
{

    public int Id { get; set; }

    public int RegistoFinal { get; set; }



    public virtual FrequenciaCardiacaValores FrequenciaCardiacaValorSet { get; set; }

    public virtual TipoAviso TipoAvisoSet { get; set; }

}

}
