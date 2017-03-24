using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ServiceLayerNew.Warnings
{
    public static class HeartRateWarnings
    {
        private static FrequenciaCardiacaValores fcRecord;

        public static void Verify(FrequenciaCardiacaValores _fc)
        {
            fcRecord = _fc;
            
            Thread th = new Thread(new ThreadStart(Run));
            th.Name = "THREAD HR WARNING";
            th.Start();
        }

        private static void Run()
        {
            AvisoFrequenciaCardiaca avFrequencia = new AvisoFrequenciaCardiaca();
            int rate = fcRecord.Frequencia;
            int minimum = fcRecord.AlertaSet.ValorMinimo;
            int maximum = fcRecord.AlertaSet.ValorMaximo;
            int criticalMinimum = fcRecord.AlertaSet.ValorCriticoMinimo;
            int criticalMaximum = fcRecord.AlertaSet.ValorCriticoMaximo;

            using (ModelMyHealth context = new ModelMyHealth())
            {

                #region ECA - Evento Critico Anytime

                if (rate < criticalMinimum || rate > criticalMaximum) // < 30bpm ou > 180bpm
                {
                    avFrequencia.FrequenciaCardiacaValorSet = fcRecord;
                    avFrequencia.RegistoFinal = fcRecord.Id;
                    avFrequencia.TipoAvisoSet = context.TipoAvisoSet.FirstOrDefault(i => i.Nome == "ECA");
                        //Warning.Get(Warning.Types.ECA);

                    context.AvisoFrequenciaCardiacaSet.Add(avFrequencia);
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
                    DateTime dateForECC = fcRecord.Data.AddMinutes(-minimumTimeECC);// Tempo compreendido entre Record e Record-TempoMinimo

                    List<FrequenciaCardiacaValores> valuesForECC = context.FrequenciaCardiacaValoresSet.
                        Where(i => i.Data <= fcRecord.Data && i.Data >= dateForECC).
                        OrderByDescending(i => i.Data).ToList();

                    if (!valuesForECC.Any())
                        return;

                    FrequenciaCardiacaValores verificationRecordECC = valuesForECC.FirstOrDefault(i => i.Frequencia < minimum || i.Frequencia > maximum);

                    if (verificationRecordECC == null)
                    {
                        AvisoFrequenciaCardiaca avisoFrequenciaECC = new AvisoFrequenciaCardiaca();
                        avisoFrequenciaECC.FrequenciaCardiacaValorSet = valuesForECC.First();
                        avisoFrequenciaECC.RegistoFinal = valuesForECC.Last().Id;
                        avisoFrequenciaECC.TipoAvisoSet = ecc;

                        context.AvisoFrequenciaCardiacaSet.Add(avisoFrequenciaECC);
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
                    DateTime dateForECI = fcRecord.Data.AddMinutes(-maximumTimeECI);

                    List<FrequenciaCardiacaValores> valuesForECI = context.FrequenciaCardiacaValoresSet
                        .Where(i => i.Data >= dateForECI && i.Data <= fcRecord.Data)
                        .OrderByDescending(i => i.Data).ToList();

                    if (!valuesForECI.Any())
                        return;

                    IEnumerable<IGrouping<bool, FrequenciaCardiacaValores>> hashValuesForECI = valuesForECI
                        .GroupBy(i => i.Frequencia < minimum || i.Frequencia > maximum).ToList();

                    if (VerifyTimeOut(minimumTimeECI, hashValuesForECI))
                    {
                        avFrequencia.FrequenciaCardiacaValorSet = valuesForECI.First();
                        avFrequencia.RegistoFinal = valuesForECI.Last().Id;
                        avFrequencia.TipoAvisoSet = eci;
                        context.AvisoFrequenciaCardiacaSet.Add(avFrequencia);
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
                    DateTime dateForEAC = fcRecord.Data.AddMinutes(-minimumTimeEAC); // Tempo compreendido entre Record e Record-TempoMinimo

                    List<FrequenciaCardiacaValores> valuesForEAC = context.FrequenciaCardiacaValoresSet.
                        Where(i => i.Data <= fcRecord.Data && i.Data >= dateForEAC).
                        OrderByDescending(i => i.Data).ToList();

                    if (!valuesForEAC.Any())
                        return;

                    FrequenciaCardiacaValores verificationRecordEAC = valuesForEAC.FirstOrDefault(i => i.Frequencia < minimum || i.Frequencia > maximum);

                    if (verificationRecordEAC == null)
                    {
                        AvisoFrequenciaCardiaca avisoFrequenciaEAC = new AvisoFrequenciaCardiaca();
                        avisoFrequenciaEAC.FrequenciaCardiacaValorSet = valuesForEAC.First();
                        avisoFrequenciaEAC.RegistoFinal = valuesForEAC.Last().Id;
                        avisoFrequenciaEAC.TipoAvisoSet = eac;

                        context.AvisoFrequenciaCardiacaSet.Add(avisoFrequenciaEAC);
                        context.SaveChanges();
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
                    DateTime dateForEAI = fcRecord.Data.AddMinutes(-maximumTimeEAI);

                    List<FrequenciaCardiacaValores> valuesForEAI = context.FrequenciaCardiacaValoresSet
                        .Where(i => i.Data >= dateForEAI && i.Data <= fcRecord.Data)
                        .OrderByDescending(i => i.Data).ToList();

                    if (!valuesForEAI.Any())
                        return;

                    IEnumerable<IGrouping<bool, FrequenciaCardiacaValores>> hashValuesForEAI = valuesForEAI
                        .GroupBy(i => i.Frequencia < minimum || i.Frequencia > maximum).ToList();

                    if (VerifyTimeOut(minimumTimeEAI, hashValuesForEAI))
                    {
                        avFrequencia.FrequenciaCardiacaValorSet = valuesForEAI.First();
                        avFrequencia.RegistoFinal = valuesForEAI.Last().Id;
                        avFrequencia.TipoAvisoSet = eai;
                        context.AvisoFrequenciaCardiacaSet.Add(avFrequencia);
                        context.SaveChanges();
                    }

                    #endregion EAI
                }
            }
        }

        private static bool VerifyTimeOut(int range, IEnumerable<IGrouping<bool, FrequenciaCardiacaValores>> hash)
        {
            List<FrequenciaCardiacaValores> valuesBelowMinimumEAI = hash
                        .Where(i => i.Key)
                        .SelectMany(fc => fc.ToList()).ToList();

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