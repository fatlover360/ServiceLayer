using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ServiceLayerNew.Warnings
{
    public static class BloodPressureWarnings
    {
        private static PressaoSanguineaValores psRecord;

        public static void Verify(PressaoSanguineaValores _ps)
        {
            psRecord = _ps;

            Thread th = new Thread(new ThreadStart(Run));
            th.Name = "THREAD BP WARNING";
            th.Start();
        }

        private static void Run()
        {
            AvisoPressaoSanguinea avPressao = new AvisoPressaoSanguinea();
            int systolic = psRecord.Sistolica;
            int diastolic = psRecord.Distolica;
            int minimum = psRecord.AlertaSet.ValorMinimo;
            int maximum = psRecord.AlertaSet.ValorMaximo;
            int criticalMinimum = psRecord.AlertaSet.ValorCriticoMinimo;
            int criticalMaximum = psRecord.AlertaSet.ValorCriticoMaximo;

            using (ModelMyHealth context = new ModelMyHealth())
            {
                #region ECA - Evento Critico Anytime

                if (diastolic < criticalMinimum || systolic > criticalMaximum)
                {
                    avPressao.PressaoSanguineaValorSet = psRecord;
                    avPressao.RegistoFinal = psRecord.Id;
                    avPressao.TipoAvisoSet = Warning.Get(Warning.Types.ECA);

                    context.AvisoPressaoSanguineaSet.Add(avPressao);
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
                    DateTime dateForECC = psRecord.Data.AddMinutes(-minimumTimeECC); // Tempo compreendido entre Record e Record-TempoMinimo

                    List<PressaoSanguineaValores> valuesForECC = context.PressaoSanguineaValoresSet
                        .Where(i => i.Data >= dateForECC && i.Data <= psRecord.Data)
                        .OrderByDescending(i => i.Data).ToList();

                    if (valuesForECC.Any())
                        return;

                    PressaoSanguineaValores verificationRecordECC = valuesForECC.FirstOrDefault(i => i.Distolica < minimum);

                    if (verificationRecordECC == null)
                    {
                        avPressao.PressaoSanguineaValorSet = valuesForECC.First();
                        avPressao.RegistoFinal = valuesForECC.Last().Id;
                        avPressao.TipoAvisoSet = ecc;
                        context.AvisoPressaoSanguineaSet.Add(avPressao);
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
                    DateTime dateForECI = psRecord.Data.AddMinutes(-maximumTimeECI);

                    List<PressaoSanguineaValores> valuesForECI = context.PressaoSanguineaValoresSet
                        .Where(i => i.Data >= dateForECI && i.Data <= psRecord.Data)
                        .OrderByDescending(i => i.Data).ToList();

                    if (!valuesForECI.Any())
                        return;

                    IEnumerable<IGrouping<bool, PressaoSanguineaValores>> hashValuesForECI = valuesForECI
                        .GroupBy(i => i.Distolica < minimum).ToList();

                    if (VerifyTimeOut(minimumTimeECI, hashValuesForECI))
                    {
                        avPressao.PressaoSanguineaValorSet = valuesForECI.First();
                        avPressao.RegistoFinal = valuesForECI.Last().Id;
                        avPressao.TipoAvisoSet = eci;
                        context.AvisoPressaoSanguineaSet.Add(avPressao);
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
                    DateTime dateForEAC = psRecord.Data.AddMinutes(-minimumTimeEAC);// Tempo compreendido entre Record e Record-TempoMinimo


                    List<PressaoSanguineaValores> valuesForEAC =
                        context.PressaoSanguineaValoresSet.Where(i => i.Data >= dateForEAC && i.Data <= psRecord.Data)
                            .OrderByDescending(i => i.Data).ToList();

                    if (!valuesForEAC.Any())
                        return;

                    PressaoSanguineaValores verificationRecordEAC = valuesForEAC.FirstOrDefault(i => i.Distolica < minimum);

                    if (verificationRecordEAC == null)
                    {
                        avPressao.PressaoSanguineaValorSet = valuesForEAC.First();
                        avPressao.RegistoFinal = valuesForEAC.Last().Id;
                        avPressao.TipoAvisoSet = eac;

                        context.AvisoPressaoSanguineaSet.Add(avPressao);
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
                    DateTime dateForEAI = psRecord.Data.AddMinutes(-maximumTimeEAI);

                    List<PressaoSanguineaValores> valuesForEAI = context.PressaoSanguineaValoresSet
                        .Where(i => i.Data >= dateForEAI && i.Data <= psRecord.Data)
                        .OrderByDescending(i => i.Data).ToList();

                    if (!valuesForEAI.Any())
                        return;

                    IEnumerable<IGrouping<bool, PressaoSanguineaValores>> hashValuesForEAI = valuesForEAI
                        .GroupBy(i => i.Distolica < minimum).ToList();

                    if (VerifyTimeOut(minimumTimeEAI, hashValuesForEAI))
                    {
                        avPressao.PressaoSanguineaValorSet = valuesForEAI.First();
                        avPressao.RegistoFinal = valuesForEAI.Last().Id;
                        avPressao.TipoAvisoSet = eai;
                        context.AvisoPressaoSanguineaSet.Add(avPressao);
                        context.SaveChanges();

                        return;
                    }

                    #endregion EAI

                }
            }
        }

        private static bool VerifyTimeOut(int range, IEnumerable<IGrouping<bool, PressaoSanguineaValores>> hash)
        {
            List<PressaoSanguineaValores> valuesBelowMinimumEAI = hash
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