using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;

namespace ServiceLayerNew.Warnings
{
    public static class SaturationWarnings
    {
        private static SaturacaoValores satRecord;

        public static void Verify(SaturacaoValores _sat)
        {
            satRecord = _sat;

            Thread th = new Thread(new ThreadStart(Run));
            th.Name = "THREAD SAT WARNING";
            th.Start();
        }

        private static void Run()
        {
            AvisoSaturacao avSaturacao = new AvisoSaturacao();
            int saturation = satRecord.Saturacao;
            int minimum = satRecord.AlertaSet.ValorMinimo;
            int maximum = satRecord.AlertaSet.ValorMaximo;
            int criticalMinimum = satRecord.AlertaSet.ValorCriticoMinimo;
            int criticalMaximum = satRecord.AlertaSet.ValorCriticoMaximo;

            using (ModelMyHealth context = new ModelMyHealth())
            {

                #region ECA - Evento Critico Anytime

                if (saturation < criticalMinimum) // < 80%
                {
                    avSaturacao.SaturacaoValorSet = satRecord;
                    avSaturacao.RegistoFinal = satRecord.Id;
                    avSaturacao.TipoAvisoSet = Warning.Get(Warning.Types.ECA);

                    context.AvisoSaturacaoSet.Add(avSaturacao);
                    context.SaveChanges();
                }
                #endregion ECA
                else
                {

                    #region ECC - Evento Critico Continuo

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo minimo definido para o ECC
                     */

                    TipoAviso ecc = Warning.Get(Warning.Types.ECC);
                    int minimumTimeECC = ecc.TempoMinimo;
                    DateTime dateForECC = satRecord.Data.AddMinutes(-minimumTimeECC); // Tempo compreendido entre Record e Record-TempoMinimo

                    List<SaturacaoValores> valuesForECC = context.SaturacaoValoresSet
                        .Where(i => i.Data >= dateForECC && i.Data <= satRecord.Data)
                        .OrderByDescending(i => i.Data).ToList();

                    if (valuesForECC.Any())
                        return;

                    SaturacaoValores verificationRecordECC = valuesForECC.FirstOrDefault(i => i.Saturacao < minimum);

                    if (verificationRecordECC == null)
                    {
                        avSaturacao.SaturacaoValorSet = valuesForECC.First();
                        avSaturacao.RegistoFinal = valuesForECC.Last().Id;
                        avSaturacao.TipoAvisoSet = ecc;
                        context.AvisoSaturacaoSet.Add(avSaturacao);
                        context.SaveChanges();

                        return;
                    }

                    #endregion ECC


                    #region ECI - Evento Critico Intermitente

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo compreendido (tempo minimo e tempo maximo) definido para o ECI
                     */

                    TipoAviso eci = Warning.Get(Warning.Types.ECI);
                    int minimumTimeECI = eci.TempoMinimo;
                    int maximumTimeECI = eci.TempoMaximo;
                    DateTime dateForECI = satRecord.Data.AddMinutes(-maximumTimeECI);

                    List<SaturacaoValores> valuesForECI = context.SaturacaoValoresSet
                        .Where(i => i.Data >= dateForECI && i.Data <= satRecord.Data)
                        .OrderByDescending(i => i.Data).ToList();

                    if (!valuesForECI.Any())
                        return;

                    IEnumerable<IGrouping<bool, SaturacaoValores>> hashValuesForECI = valuesForECI
                        .GroupBy(i => i.Saturacao < minimum).ToList();
                    
                    if (VerifyTimeOut(minimumTimeECI, hashValuesForECI))
                    {
                        avSaturacao.SaturacaoValorSet = valuesForECI.First();
                        avSaturacao.RegistoFinal = valuesForECI.Last().Id;
                        avSaturacao.TipoAvisoSet = eci;
                        context.AvisoSaturacaoSet.Add(avSaturacao);
                        context.SaveChanges();

                        return;
                    }

                    #endregion ECI


                    #region EAC - Evento Aviso Continuo 

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo minimo definido para o EAC
                     */

                    TipoAviso eac = Warning.Get(Warning.Types.EAC);
                    int minimumTimeEAC = eac.TempoMinimo;
                    DateTime dateForEAC = satRecord.Data.AddMinutes(-minimumTimeEAC); // Tempo compreendido entre Record e Record-TempoMinimo

                    List<SaturacaoValores> valuesForEAC =
                        context.SaturacaoValoresSet.Where(i => i.Data >= dateForEAC && i.Data <= satRecord.Data)
                            .OrderByDescending(i => i.Data).ToList();

                    if (!valuesForEAC.Any())
                        return;

                    SaturacaoValores verificationRecordEAC = valuesForEAC.FirstOrDefault(i => i.Saturacao < minimum);

                    if (verificationRecordEAC == null)
                    {
                        avSaturacao.SaturacaoValorSet = valuesForEAC.First();
                        avSaturacao.RegistoFinal = valuesForEAC.Last().Id;
                        avSaturacao.TipoAvisoSet = eac;
                        context.AvisoSaturacaoSet.Add(avSaturacao);
                        context.SaveChanges();

                        return;
                    }

                    #endregion EAC 


                    #region EAI - Evento Aviso Intermitente
                    
                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo compreendido (tempo minimo e tempo maximo) definido para o EAI
                     */

                    TipoAviso eai = Warning.Get(Warning.Types.EAI);
                    int minimumTimeEAI = eai.TempoMinimo;
                    int maximumTimeEAI = eai.TempoMaximo;
                    DateTime dateForEAI = satRecord.Data.AddMinutes(-maximumTimeEAI);

                    List<SaturacaoValores> valuesForEAI = context.SaturacaoValoresSet
                        .Where(i => i.Data >= dateForEAI && i.Data <= satRecord.Data)
                        .OrderByDescending(i => i.Data).ToList();

                    if (!valuesForEAI.Any())
                        return;

                    IEnumerable<IGrouping<bool, SaturacaoValores>> hashValuesForEAI = valuesForEAI
                        .GroupBy(i => i.Saturacao < minimum).ToList();
                    
                    if (VerifyTimeOut(minimumTimeEAI, hashValuesForEAI))
                    {
                        avSaturacao.SaturacaoValorSet = valuesForEAI.First();
                        avSaturacao.RegistoFinal = valuesForEAI.Last().Id;
                        avSaturacao.TipoAvisoSet = eai;
                        context.AvisoSaturacaoSet.Add(avSaturacao);
                        context.SaveChanges();

                        return;
                    }

                    #endregion EAI

                }
            }
        }

        private static bool VerifyTimeOut(int range, IEnumerable<IGrouping<bool, SaturacaoValores>> hash)
        {
            List<SaturacaoValores> valuesBelowMinimumEAI = hash
                        .Where(i => i.Key)
                        .SelectMany(sat => sat.ToList()).ToList();

            int timespan = 0;

            for (int index = 0; index < valuesBelowMinimumEAI.Count; index++)
            {
                if (index + 1 == valuesBelowMinimumEAI.Count)
                    break;

                timespan += valuesBelowMinimumEAI[index + 1].Data.Minute -
                            valuesBelowMinimumEAI[index].Data.Minute;
            }

            return timespan >= range;
        }

    }
}