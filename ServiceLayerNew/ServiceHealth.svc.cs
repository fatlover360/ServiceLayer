using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;

namespace ServiceLayerNew
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceHealth" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServiceHealth.svc or ServiceHealth.svc.cs at the Solution Explorer and start debugging.
    public class ServiceHealth : IServiceHealth, IServiceHealthAlert
    {
        #region IServiceHealth

        public bool TestConnection()
        {
            return true;
        }

        public bool ValidatePatient(int sns)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    Utente ut = context.UtenteSet.FirstOrDefault(i => i.SNS == sns);

                    return ut != null;
                }
                catch (ArgumentNullException e)
                {
                    return false;
                }
            }
        }

        public bool ValidatePatientState(int sns)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    Utente ut = context.UtenteSet.FirstOrDefault(i => i.SNS == sns);

                    return ut.Ativo;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool InsertOxygenSaturationRecord(OxygenSaturation saturation)
        {
            try
            {
                using (ModelMyHealth context = new ModelMyHealth())
                {
                    SaturacaoValores saturacao = new SaturacaoValores();
                    Utente ut = context.UtenteSet.FirstOrDefault(i => i.SNS == saturation.PatientSNS);

                    saturacao.Data = saturation.Date;
                    saturacao.Hora = saturation.Time;
                    saturacao.Saturacao = saturation.Saturation;
                    saturacao.Utentes = ut;

                    context.SaturacaoValoresSet.Add(saturacao);
                    context.SaveChanges();

                    ConfiguracoesLimites configuracao = context.ConfiguracoesLimitesSet.FirstOrDefault(i => i.Nome.Equals("SPO2"));

                    int satValue = saturacao.Saturacao;
                    int minimum = configuracao.ValorMinimo;
                    int maximum = configuracao.ValorMaximo;
                    int criticalMinimum = configuracao.ValorCriticoMinimo;
                    int criticalMaximum = configuracao.ValorCriticoMaximo;

                    #region ECA - Evento Critico Anytime

                    if (satValue < criticalMinimum) // < 80%
                    {
                        AvisoSaturacao avisoSaturacaoECA = new AvisoSaturacao();
                        avisoSaturacaoECA.SaturacaoValorSet = saturacao;
                        avisoSaturacaoECA.TipoAvisoSet = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("ECA"));
                        context.AvisoSaturacaoSet.Add(avisoSaturacaoECA);
                        context.SaveChanges();
                        return true;
                    }
                    #endregion ECA


                    #region ECC - Evento Critico Continuo

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo minimo definido para o ECC
                     */

                    TipoAviso ecc = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("ECC"));
                    int minimumTimeECC = ecc.TempoMinimo;
                    DateTime dateForECC = saturacao.Data.AddMinutes(-minimumTimeECC); // Tempo compreendido entre Record e Record-TempoMinimo

                    List<SaturacaoValores> valuesForECC = context.SaturacaoValoresSet
                        .Where(i => i.Data >= dateForECC && i.Data <= saturacao.Data).ToList();

                    if (VerifyTimeOut(minimum, valuesForECC))
                    {
                        SaturacaoValores verificationRecordECC = valuesForECC.FirstOrDefault(i => i.Saturacao < minimum);

                        if (verificationRecordECC == null)
                        {
                            AvisoSaturacao avisoSaturacaoECC = new AvisoSaturacao();
                            avisoSaturacaoECC.SaturacaoValorSet = valuesForECC.Last();
                            avisoSaturacaoECC.TipoAvisoSet = ecc;
                            context.AvisoSaturacaoSet.Add(avisoSaturacaoECC);
                            context.SaveChanges();
                            return true;
                        }
                    }

                    #endregion ECC


                    #region ECI - Evento Critico Intermitente

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo compreendido (tempo minimo e tempo maximo) definido para o ECI
                     */

                    TipoAviso eci = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("ECI"));
                    int minimumTimeECI = eci.TempoMinimo;
                    int maximumTimeECI = eci.TempoMaximo;
                    DateTime dateForECI = saturacao.Data.AddMinutes(-maximumTimeECI);

                    List<SaturacaoValores> valuesForECI = context.SaturacaoValoresSet
                        .Where(i => i.Data >= dateForECI && i.Data <= saturacao.Data)
                        .ToList();

                    IEnumerable<IGrouping<bool, SaturacaoValores>> hashValuesForECI = valuesForECI
                        .GroupBy(i => i.Saturacao < minimum).ToList();

                    if (VerifyTimeOut(minimumTimeECI, hashValuesForECI))
                    {
                        AvisoSaturacao avisoSaturacaoECI = new AvisoSaturacao();
                        avisoSaturacaoECI.SaturacaoValorSet = valuesForECI.Last();
                        avisoSaturacaoECI.TipoAvisoSet = eci;
                        context.AvisoSaturacaoSet.Add(avisoSaturacaoECI);
                        context.SaveChanges();
                        return true;

                    }

                    #endregion ECI


                    #region EAC - Evento Aviso Continuo 

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo minimo definido para o EAC
                     */

                    TipoAviso eac = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("EAC"));
                    int minimumTimeEAC = eac.TempoMinimo;
                    DateTime dateForEAC = saturacao.Data.AddMinutes(-minimumTimeEAC); // Tempo compreendido entre Record e Record-TempoMinimo

                    List<SaturacaoValores> valuesForEAC =
                        context.SaturacaoValoresSet.Where(i => i.Data >= dateForEAC && i.Data <= saturacao.Data).ToList();

                    if (VerifyTimeOut(minimumTimeEAC, valuesForEAC))
                    {
                        SaturacaoValores verificationRecordEAC = valuesForEAC.FirstOrDefault(i => i.Saturacao >= minimum);

                        if (verificationRecordEAC == null)
                        {
                            AvisoSaturacao avisoSaturacaoEAC = new AvisoSaturacao();
                            avisoSaturacaoEAC.SaturacaoValorSet = valuesForEAC.Last();
                            avisoSaturacaoEAC.TipoAvisoSet = eac;
                            context.AvisoSaturacaoSet.Add(avisoSaturacaoEAC);
                            context.SaveChanges();
                            return true;

                        }
                    }

                    #endregion EAC 


                    #region EAI - Evento Aviso Intermitente

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo compreendido (tempo minimo e tempo maximo) definido para o EAI
                     */

                    TipoAviso eai = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("EAI"));
                    int minimumTimeEAI = eai.TempoMinimo;
                    int maximumTimeEAI = eai.TempoMaximo;
                    DateTime dateForEAI = saturacao.Data.AddMinutes(-maximumTimeEAI);

                    List<SaturacaoValores> valuesForEAI = context.SaturacaoValoresSet
                        .Where(i => i.Data >= dateForEAI && i.Data <= saturacao.Data)
                        .ToList();

                    IEnumerable<IGrouping<bool, SaturacaoValores>> hashValuesForEAI = valuesForEAI
                        .GroupBy(i => i.Saturacao < minimum).ToList();

                    if (VerifyTimeOut(minimumTimeEAI, hashValuesForEAI))
                    {
                        AvisoSaturacao avisoSaturacaoEAI = new AvisoSaturacao();
                        avisoSaturacaoEAI.SaturacaoValorSet = valuesForEAI.Last();
                        avisoSaturacaoEAI.TipoAvisoSet = eai;
                        context.AvisoSaturacaoSet.Add(avisoSaturacaoEAI);
                        context.SaveChanges();
                        return true;

                    }

                    #endregion EAI

                }

                return true;
            }
            catch (ArgumentNullException e)
            {
                return false;
            }
            catch (DbUpdateException e)
            {
                return false;
            }
            catch (DbEntityValidationException e)
            {
                return false;
            }
            catch (NotSupportedException e)
            {
                return false;
            }
            catch (ObjectDisposedException e)
            {
                return false;
            }
            catch (InvalidOperationException e)
            {
                return false;
            }

        }

        public bool InsertBloodPressureRecord(BloodPressure bloodPressure)
        {
            try
            {
                PressaoSanguineaValores pressao = new PressaoSanguineaValores();

                using (ModelMyHealth context = new ModelMyHealth())
                {
                    Utente ut = context.UtenteSet.FirstOrDefault(i => i.SNS == bloodPressure.PatientSNS);

                    pressao.Data = bloodPressure.Date;
                    pressao.Hora = bloodPressure.Time;
                    pressao.Distolica = bloodPressure.Diastolic;
                    pressao.Sistolica = bloodPressure.Systolic;
                    pressao.Utentes = ut;

                    context.PressaoSanguineaValoresSet.Add(pressao);
                    context.SaveChanges();

                    ConfiguracoesLimites configuracao = context.ConfiguracoesLimitesSet.FirstOrDefault(i => i.Nome.Equals("BP"));

                    int systolic = pressao.Sistolica;
                    int diastolic = pressao.Distolica;
                    int minimum = configuracao.ValorMinimo;
                    int maximum = configuracao.ValorMaximo;
                    int criticalMinimum = configuracao.ValorCriticoMinimo;
                    int criticalMaximum = configuracao.ValorCriticoMaximo;

                    #region ECA - Evento Critico Anytime

                    if (diastolic < criticalMinimum || systolic > criticalMaximum)
                    {
                        AvisoPressaoSanguinea avisoPressaoECA = new AvisoPressaoSanguinea();
                        avisoPressaoECA.PressaoSanguineaValorSet = pressao;
                        avisoPressaoECA.TipoAvisoSet = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("ECA"));
                        context.AvisoPressaoSanguineaSet.Add(avisoPressaoECA);
                        context.SaveChanges();
                        return true;
                    }
                    #endregion ECA


                    #region ECC - Evento Critico Continuo

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo minimo definido para o ECC
                     */

                    TipoAviso ecc = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("ECC"));
                    int minimumTimeECC = ecc.TempoMinimo;
                    DateTime dateForECC = pressao.Data.AddMinutes(-minimumTimeECC); // Tempo compreendido entre Record e Record-TempoMinimo

                    List<PressaoSanguineaValores> valuesForECC = context.PressaoSanguineaValoresSet
                        .Where(i => i.Data >= dateForECC && i.Data <= pressao.Data)
                        .ToList();

                    if (VerifyTimeOut(minimumTimeECC, valuesForECC))
                    {
                        PressaoSanguineaValores verificationRecordECC = valuesForECC.FirstOrDefault(i => i.Distolica < minimum);

                        if (verificationRecordECC == null)
                        {
                            AvisoPressaoSanguinea avisoPressaoECC = new AvisoPressaoSanguinea();
                            avisoPressaoECC.PressaoSanguineaValorSet = valuesForECC.Last();
                            avisoPressaoECC.TipoAvisoSet = ecc;
                            context.AvisoPressaoSanguineaSet.Add(avisoPressaoECC);
                            context.SaveChanges();
                            return true;
                        }
                    }

                    #endregion ECC


                    #region ECI - Evento Critico Intermitente

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo compreendido (tempo minimo e tempo maximo) definido para o ECI
                     */

                    TipoAviso eci = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("ECI"));
                    int minimumTimeECI = eci.TempoMinimo;
                    int maximumTimeECI = eci.TempoMaximo;
                    DateTime dateForECI = pressao.Data.AddMinutes(-maximumTimeECI);

                    List<PressaoSanguineaValores> valuesForECI = context.PressaoSanguineaValoresSet
                        .Where(i => i.Data >= dateForECI && i.Data <= pressao.Data)
                        .ToList();

                    IEnumerable<IGrouping<bool, PressaoSanguineaValores>> hashValuesForECI = valuesForECI
                        .GroupBy(i => i.Distolica < minimum).ToList();

                    if (VerifyTimeOut(minimumTimeECI, hashValuesForECI))
                    {
                        AvisoPressaoSanguinea avisoPressaoECI = new AvisoPressaoSanguinea();
                        avisoPressaoECI.PressaoSanguineaValorSet = valuesForECI.Last();
                        avisoPressaoECI.TipoAvisoSet = eci;
                        context.AvisoPressaoSanguineaSet.Add(avisoPressaoECI);
                        context.SaveChanges();
                        return true;
                    }

                    #endregion ECI


                    #region EAC - Evento Aviso Continuo 

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo minimo definido para o EAC
                     */

                    TipoAviso eac = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("EAC"));
                    int minimumTimeEAC = eac.TempoMinimo;
                    DateTime dateForEAC = pressao.Data.AddMinutes(-minimumTimeEAC);// Tempo compreendido entre Record e Record-TempoMinimo

                    List<PressaoSanguineaValores> valuesForEAC =
                        context.PressaoSanguineaValoresSet.Where(i => i.Data >= dateForEAC && i.Data <= pressao.Data)
                            .ToList();

                    if (VerifyTimeOut(minimumTimeEAC, valuesForEAC))
                    {
                        PressaoSanguineaValores verificationRecordEAC = valuesForEAC.FirstOrDefault(i => i.Distolica < minimum);

                        if (verificationRecordEAC == null)
                        {
                            AvisoPressaoSanguinea avisoPressaoEAC = new AvisoPressaoSanguinea();
                            avisoPressaoEAC.PressaoSanguineaValorSet = valuesForEAC.Last();
                            avisoPressaoEAC.TipoAvisoSet = eac;
                            context.AvisoPressaoSanguineaSet.Add(avisoPressaoEAC);
                            context.SaveChanges();
                            return true;
                        }
                    }

                    #endregion EAC 


                    #region EAI - Evento Aviso Intermitente

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo compreendido (tempo minimo e tempo maximo) definido para o EAI
                     */

                    TipoAviso eai = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("EAI"));
                    int minimumTimeEAI = eai.TempoMinimo;
                    int maximumTimeEAI = eai.TempoMaximo;
                    DateTime dateForEAI = pressao.Data.AddMinutes(-maximumTimeEAI);

                    List<PressaoSanguineaValores> valuesForEAI = context.PressaoSanguineaValoresSet
                        .Where(i => i.Data >= dateForEAI && i.Data <= pressao.Data)
                        .ToList();

                    IEnumerable<IGrouping<bool, PressaoSanguineaValores>> hashValuesForEAI = valuesForEAI
                        .GroupBy(i => i.Distolica < minimum).ToList();

                    if (VerifyTimeOut(minimumTimeEAI, hashValuesForEAI))
                    {
                        AvisoPressaoSanguinea avisoPressaoEAI = new AvisoPressaoSanguinea();
                        avisoPressaoEAI.PressaoSanguineaValorSet = valuesForEAI.Last();
                        avisoPressaoEAI.TipoAvisoSet = eai;
                        context.AvisoPressaoSanguineaSet.Add(avisoPressaoEAI);
                        context.SaveChanges();
                        return true;
                    }

                    #endregion EAI

                }

                return true;
            }
            catch (ArgumentNullException e)
            {
                return false;
            }
            catch (DbUpdateException e)
            {
                return false;
            }
            catch (DbEntityValidationException e)
            {
                return false;
            }
            catch (NotSupportedException e)
            {
                return false;
            }
            catch (ObjectDisposedException e)
            {
                return false;
            }
            catch (InvalidOperationException e)
            {
                return false;
            }
        }

        public bool InsertHeartRateRecord(HeartRate heartRate)
        {
            try
            {
                FrequenciaCardiacaValores frequencia = new FrequenciaCardiacaValores();

                using (ModelMyHealth context = new ModelMyHealth())
                {
                    Utente ut = context.UtenteSet.FirstOrDefault(i => i.SNS == heartRate.PatientSNS);

                    frequencia.Data = heartRate.Date;
                    frequencia.Hora = heartRate.Time;
                    frequencia.Frequencia = heartRate.Rate;
                    frequencia.Utentes = ut;

                    context.FrequenciaCardiacaValoresSet.Add(frequencia);
                    context.SaveChanges();

                    ConfiguracoesLimites configuracao = context.ConfiguracoesLimitesSet.FirstOrDefault(i => i.Nome.Equals("HR"));

                    int rate = frequencia.Frequencia;
                    int minimum = configuracao.ValorMinimo;
                    int maximum = configuracao.ValorMaximo;
                    int criticalMinimum = configuracao.ValorCriticoMinimo;
                    int criticalMaximum = configuracao.ValorCriticoMaximo;

                    #region ECA - Evento Critico Anytime

                    if (rate < criticalMinimum || rate > criticalMaximum) // < 30bpm ou > 180bpm
                    {
                        AvisoFrequenciaCardiaca avisoFrequenciaECA = new AvisoFrequenciaCardiaca();
                        avisoFrequenciaECA.FrequenciaCardiacaValorSet = frequencia;
                        avisoFrequenciaECA.TipoAvisoSet = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("ECA"));
                        context.AvisoFrequenciaCardiacaSet.Add(avisoFrequenciaECA);
                        context.SaveChanges();
                        return true;
                    }
                    #endregion ECA


                    #region ECC - Evento Critico Continuo

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo minimo definido para o ECC
                     */

                    TipoAviso ecc = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("ECC"));
                    int minimumTimeECC = ecc.TempoMinimo;
                    DateTime dateForECC = frequencia.Data.AddMinutes(-minimumTimeECC);// Tempo compreendido entre Record e Record-TempoMinimo

                    List<FrequenciaCardiacaValores> valuesForECC = context.FrequenciaCardiacaValoresSet.
                        Where(i => i.Data <= frequencia.Data && i.Data >= dateForECC)
                        .ToList();

                    if (VerifyTimeOut(minimumTimeECC, valuesForECC))
                    {
                        FrequenciaCardiacaValores verificationRecordECC = valuesForECC.FirstOrDefault(i => i.Frequencia < minimum || i.Frequencia > maximum);

                        if (verificationRecordECC == null)
                        {
                            AvisoFrequenciaCardiaca avisoFrequenciaECC = new AvisoFrequenciaCardiaca();
                            avisoFrequenciaECC.FrequenciaCardiacaValorSet = valuesForECC.Last();
                            avisoFrequenciaECC.TipoAvisoSet = ecc;
                            context.AvisoFrequenciaCardiacaSet.Add(avisoFrequenciaECC);
                            context.SaveChanges();
                            return true;
                        }
                    }

                    #endregion ECC


                    #region ECI - Evento Critico Intermitente

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo compreendido (tempo minimo e tempo maximo) definido para o ECI
                     */

                    TipoAviso eci = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("ECI"));
                    int minimumTimeECI = eci.TempoMinimo;
                    int maximumTimeECI = eci.TempoMaximo;
                    DateTime dateForECI = frequencia.Data.AddMinutes(-maximumTimeECI);

                    List<FrequenciaCardiacaValores> valuesForECI = context.FrequenciaCardiacaValoresSet
                        .Where(i => i.Data >= dateForECI && i.Data <= frequencia.Data)
                        .ToList();

                    IEnumerable<IGrouping<bool, FrequenciaCardiacaValores>> hashValuesForECI = valuesForECI
                        .GroupBy(i => i.Frequencia < minimum || i.Frequencia > maximum).ToList();

                    if (VerifyTimeOut(minimumTimeECI, hashValuesForECI))
                    {
                        AvisoFrequenciaCardiaca avisoFrequenciaECI = new AvisoFrequenciaCardiaca();
                        avisoFrequenciaECI.FrequenciaCardiacaValorSet = valuesForECI.Last();
                        avisoFrequenciaECI.TipoAvisoSet = eci;
                        context.AvisoFrequenciaCardiacaSet.Add(avisoFrequenciaECI);
                        context.SaveChanges();
                        return true;
                    }

                    #endregion ECI


                    #region EAC - Evento Aviso Continuo 

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo minimo definido para o EAC
                     */

                    TipoAviso eac = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("EAC"));
                    int minimumTimeEAC = eac.TempoMinimo;
                    DateTime dateForEAC = frequencia.Data.AddMinutes(-minimumTimeEAC); // Tempo compreendido entre Record e Record-TempoMinimo

                    List<FrequenciaCardiacaValores> valuesForEAC = context.FrequenciaCardiacaValoresSet.
                        Where(i => i.Data <= frequencia.Data && i.Data >= dateForEAC)
                        .ToList();

                    if (VerifyTimeOut(minimumTimeEAC, valuesForEAC))
                    {
                        FrequenciaCardiacaValores verificationRecordEAC = valuesForEAC.FirstOrDefault(i => i.Frequencia < minimum || i.Frequencia > maximum);

                        if (verificationRecordEAC == null)
                        {
                            AvisoFrequenciaCardiaca avisoFrequenciaEAC = new AvisoFrequenciaCardiaca();
                            avisoFrequenciaEAC.FrequenciaCardiacaValorSet = valuesForEAC.First();
                            avisoFrequenciaEAC.TipoAvisoSet = eac;
                            context.AvisoFrequenciaCardiacaSet.Add(avisoFrequenciaEAC);
                            context.SaveChanges();
                            return true;
                        }

                    }

                    #endregion EAC 


                    #region EAI - Evento Aviso Intermitente

                    /* 
                     * Os parametros recebidos no metodo, 
                     * tem de ter o seu valor fora dos limites definido para o mesmo,
                     * durante o tempo compreendido (tempo minimo e tempo maximo) definido para o EAI
                     */

                    TipoAviso eai = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals("EAI"));
                    int minimumTimeEAI = eai.TempoMinimo;
                    int maximumTimeEAI = eai.TempoMaximo;
                    DateTime dateForEAI = frequencia.Data.AddMinutes(-maximumTimeEAI);

                    List<FrequenciaCardiacaValores> valuesForEAI = context.FrequenciaCardiacaValoresSet
                        .Where(i => i.Data >= dateForEAI && i.Data <= frequencia.Data)
                        .ToList();

                    IEnumerable<IGrouping<bool, FrequenciaCardiacaValores>> hashValuesForEAI = valuesForEAI
                        .GroupBy(i => i.Frequencia < minimum || i.Frequencia > maximum).ToList();

                    if (VerifyTimeOut(minimumTimeEAI, hashValuesForEAI))
                    {
                        AvisoFrequenciaCardiaca avisoFrequenciaEAI = new AvisoFrequenciaCardiaca();
                        avisoFrequenciaEAI.FrequenciaCardiacaValorSet = valuesForEAI.Last();
                        avisoFrequenciaEAI.TipoAvisoSet = eai;
                        context.AvisoFrequenciaCardiacaSet.Add(avisoFrequenciaEAI);
                        context.SaveChanges();
                        return true;
                    }

                    #endregion EAI

                }

                return true;
            }
            catch (ArgumentNullException e)
            {
                return false;
            }
            catch (DbUpdateException e)
            {
                return false;
            }
            catch (DbEntityValidationException e)
            {
                return false;
            }
            catch (NotSupportedException e)
            {
                return false;
            }
            catch (ObjectDisposedException e)
            {
                return false;
            }
            catch (InvalidOperationException e)
            {
                return false;
            }
        }

        #endregion IServiceHealth

        #region IServiceHealthAlert

        public bool InsertPatient(Patient patient)
        {
            try
            {
                Utente ut = new Utente();
                ut.Nome = patient.Name;
                ut.Apelido = patient.Surname;
                ut.NIF = patient.Nif;
                ut.DataNascimento = patient.BirthDate;
                ut.Telefone = patient.Phone;
                ut.CodigoPaisTelefone = patient.PhoneCountryCode;
                ut.Email = patient.Email;
                ut.NumeroEmergencia = patient.EmergencyNumber;
                ut.CodigoPaisNumeroEmergencia = patient.EmergencyNumberCountryCode;
                ut.NomeEmergencia = patient.EmergencyName;
                ut.Sexo = patient.Gender;
                ut.Morada = patient.Adress;
                ut.Peso = patient.Weight;
                ut.Altura = patient.Height;
                ut.Alergias = patient.Alergies;
                ut.SNS = patient.Sns;
                ut.Ativo = patient.Ativo;

                using (ModelMyHealth context = new ModelMyHealth())
                {
                    context.UtenteSet.Add(ut);
                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message + "  " + x.GetBaseException());
                return false;
            }
        }

        public bool UpdateStatePatient(Patient patient)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                Utente ut = context.UtenteSet.FirstOrDefault(i => i.SNS == patient.Sns);

                if (ut == null)
                    return false;

                ut.Ativo = patient.Ativo;
                context.SaveChanges();

                return true;
            }

        }

        public bool UpdatePatient(Patient patient, int sns)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    Utente ut = context.UtenteSet.FirstOrDefault(i => i.SNS == sns);

                    if (ut == null)
                        return false;

                    ut.Nome = patient.Name;
                    ut.Apelido = patient.Surname;
                    ut.NIF = patient.Nif;
                    ut.DataNascimento = patient.BirthDate;
                    ut.Telefone = patient.Phone;
                    ut.Email = patient.Email;
                    ut.NumeroEmergencia = patient.EmergencyNumber;
                    ut.NomeEmergencia = patient.EmergencyName;
                    ut.Sexo = patient.Gender;
                    ut.Morada = patient.Adress;
                    ut.Peso = patient.Weight;
                    ut.Altura = patient.Height;
                    ut.Alergias = patient.Alergies;
                    ut.SNS = patient.Sns;
                    ut.CodigoPaisNumeroEmergencia = patient.EmergencyNumberCountryCode;
                    ut.CodigoPaisTelefone = patient.PhoneCountryCode;

                    context.SaveChanges();

                    return true;
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
                catch (DbEntityValidationException)
                {
                    return false;
                }
                catch (NotSupportedException)
                {
                    return false;
                }
                catch (ObjectDisposedException)
                {
                    return false;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }

        public Patient GetPatient(int sns)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    Utente ut = context.UtenteSet.FirstOrDefault(i => i.SNS == sns);

                    Patient patient = new Patient();
                    patient.Name = ut.Nome;
                    patient.Surname = ut.Apelido;
                    patient.Nif = ut.NIF;
                    patient.BirthDate = ut.DataNascimento;
                    patient.Phone = Convert.ToInt32(ut.Telefone);
                    patient.Email = ut.Email;
                    patient.EmergencyNumber = ut.NumeroEmergencia;
                    patient.EmergencyName = ut.NomeEmergencia;
                    patient.Gender = ut.Sexo;
                    patient.Adress = ut.Morada;
                    patient.Weight = Convert.ToDouble(ut.Peso);
                    patient.Height = Convert.ToInt32(ut.Altura);
                    patient.Alergies = ut.Alergias;
                    patient.Sns = ut.SNS;
                    patient.Ativo = ut.Ativo;
                    patient.EmergencyNumberCountryCode = ut.CodigoPaisNumeroEmergencia;
                    patient.PhoneCountryCode = ut.CodigoPaisTelefone;

                    return patient;
                }
                catch (NullReferenceException e)
                {
                    return null;
                }
                catch (ArgumentNullException e)
                {
                    return null;
                }
            }
        }

        public List<Patient> GetPatientList()
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    List<Patient> patientList = new List<Patient>();

                    var utentes = context.UtenteSet;

                    foreach (Utente ut in utentes)
                    {
                        Patient patient = new Patient();
                        patient.Name = ut.Nome;
                        patient.Surname = ut.Apelido;
                        patient.Nif = ut.NIF;
                        patient.BirthDate = ut.DataNascimento;
                        patient.Phone = Convert.ToInt32(ut.Telefone);
                        patient.Email = ut.Email;
                        patient.EmergencyNumber = ut.NumeroEmergencia;
                        patient.EmergencyName = ut.NomeEmergencia;
                        patient.Gender = ut.Sexo;
                        patient.Adress = ut.Morada;
                        patient.Weight = Convert.ToDouble(ut.Peso);
                        patient.Height = Convert.ToInt32(ut.Altura);
                        patient.Alergies = ut.Alergias;
                        patient.Sns = ut.SNS;
                        patient.Ativo = ut.Ativo;
                        patient.EmergencyNumberCountryCode = ut.CodigoPaisNumeroEmergencia;
                        patient.PhoneCountryCode = ut.CodigoPaisTelefone;

                        patientList.Add(patient);
                    }

                    return patientList;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public List<HeartRate> HeartRateList(int sns)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    List<HeartRate> heartRateList = new List<HeartRate>();

                    var frequencias = context.FrequenciaCardiacaValoresSet.Where(i => i.Utentes.SNS == sns);

                    foreach (FrequenciaCardiacaValores freq in frequencias)
                    {
                        HeartRate heartRate = new HeartRate();
                        heartRate.PatientSNS = freq.Utentes.SNS;
                        heartRate.Date = freq.Data;
                        heartRate.Time = freq.Hora;
                        heartRate.Rate = freq.Frequencia;

                        heartRateList.Add(heartRate);
                    }

                    return heartRateList;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public List<OxygenSaturation> OxygenSaturationList(int sns)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    List<OxygenSaturation> oxygenSaturationsList = new List<OxygenSaturation>();

                    var saturacoes = context.SaturacaoValoresSet.Where(i => i.Utentes.SNS == sns);

                    foreach (SaturacaoValores sat in saturacoes)
                    {
                        OxygenSaturation saturation = new OxygenSaturation();
                        saturation.PatientSNS = sat.Utentes.SNS;
                        saturation.Date = sat.Data;
                        saturation.Time = sat.Hora;
                        saturation.Saturation = sat.Saturacao;

                        oxygenSaturationsList.Add(saturation);
                    }

                    return oxygenSaturationsList;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public List<BloodPressure> BloodPressureList(int sns)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    List<BloodPressure> bloodPressureList = new List<BloodPressure>();

                    var pressoes = context.PressaoSanguineaValoresSet.Where(i => i.Utentes.SNS == sns);

                    foreach (PressaoSanguineaValores pss in pressoes)
                    {
                        BloodPressure bp = new BloodPressure();
                        bp.PatientSNS = pss.Utentes.SNS;
                        bp.Date = pss.Data;
                        bp.Time = pss.Hora;
                        bp.Systolic = pss.Sistolica;
                        bp.Diastolic = pss.Distolica;

                        bloodPressureList.Add(bp);
                    }

                    return bloodPressureList;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public ConfigurationLimitType GetConfigurationLimit(string type)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    ConfiguracoesLimites configuracao = context.ConfiguracoesLimitesSet.FirstOrDefault(i => i.Nome.Equals(type));

                    ConfigurationLimitType conf = new ConfigurationLimitType();
                    conf.Type = configuracao.Nome;
                    conf.MinimumValue = configuracao.ValorMinimo;
                    conf.MaximumValue = configuracao.ValorMaximo;
                    conf.MinimumCriticalValue = configuracao.ValorCriticoMinimo;
                    conf.MaximumCriticalValue = configuracao.ValorCriticoMaximo;

                    return conf;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public List<ConfigurationLimitType> GetConfigurationLimitList()
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    List<ConfigurationLimitType> configurationList = new List<ConfigurationLimitType>();

                    var configuracoes = context.ConfiguracoesLimitesSet;

                    foreach (ConfiguracoesLimites configuracao in configuracoes)
                    {
                        ConfigurationLimitType conf = new ConfigurationLimitType();
                        conf.Type = configuracao.Nome;
                        conf.MinimumValue = configuracao.ValorMinimo;
                        conf.MaximumValue = configuracao.ValorMaximo;
                        conf.MinimumCriticalValue = configuracao.ValorCriticoMinimo;
                        conf.MaximumCriticalValue = configuracao.ValorCriticoMaximo;

                        configurationList.Add(conf);
                    }

                    return configurationList;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public bool InsertConfigurationLimit(ConfigurationLimitType configurationLimitType)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    ConfiguracoesLimites configuracaoVerificao = context.ConfiguracoesLimitesSet.FirstOrDefault(i => i.Nome.Equals(configurationLimitType.Type));

                    if (configuracaoVerificao != null)
                        return false;

                    ConfiguracoesLimites configuracaoLimite = new ConfiguracoesLimites();
                    configuracaoLimite.Nome = configurationLimitType.Type;
                    configuracaoLimite.ValorMaximo = configurationLimitType.MaximumValue;
                    configuracaoLimite.ValorMinimo = configurationLimitType.MinimumValue;
                    configuracaoLimite.ValorCriticoMaximo = configurationLimitType.MaximumCriticalValue;
                    configuracaoLimite.ValorCriticoMinimo = configurationLimitType.MinimumCriticalValue;

                    context.ConfiguracoesLimitesSet.Add(configuracaoLimite);
                    context.SaveChanges();

                    return true;
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
                catch (DbUpdateException e)
                {
                    return false;
                }
                catch (DbEntityValidationException)
                {
                    return false;
                }
                catch (NotSupportedException)
                {
                    return false;
                }
                catch (ObjectDisposedException)
                {
                    return false;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }

        public bool UpdateConfigurationLimit(ConfigurationLimitType configurationLimitType)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    ConfiguracoesLimites confifuracaoLimite = context.ConfiguracoesLimitesSet.FirstOrDefault(i => i.Nome.Equals(configurationLimitType.Type));

                    if (confifuracaoLimite == null)
                    {
                        return false;
                    }
                    else
                    {
                        confifuracaoLimite.ValorMinimo = configurationLimitType.MinimumValue;
                        confifuracaoLimite.ValorMaximo = configurationLimitType.MaximumValue;
                        confifuracaoLimite.ValorCriticoMinimo = configurationLimitType.MinimumCriticalValue;
                        confifuracaoLimite.ValorCriticoMaximo = configurationLimitType.MaximumCriticalValue;
                    }

                    context.SaveChanges();

                    return true;
                }
                catch (NullReferenceException)
                {
                    return false;
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
                catch (DbEntityValidationException)
                {
                    return false;
                }
                catch (NotSupportedException)
                {
                    return false;
                }
                catch (ObjectDisposedException)
                {
                    return false;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }

        public bool DeleteConfigurationLimit(ConfigurationLimitType configurationLimitType)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    ConfiguracoesLimites configuracaoLimite = context.ConfiguracoesLimitesSet.FirstOrDefault(i => i.Nome.Equals(configurationLimitType.Type));

                    if (configuracaoLimite == null)
                    {
                        return false;
                    }
                    else
                    {
                        context.ConfiguracoesLimitesSet.Remove(configuracaoLimite);
                        context.SaveChanges();
                    }

                    return true;
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
                catch (DbEntityValidationException)
                {
                    return false;
                }
                catch (NotSupportedException)
                {
                    return false;
                }
                catch (ObjectDisposedException)
                {
                    return false;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }

        public bool InsertEvent(EventType eventType)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    TipoAviso avisoVarificao = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals(eventType.Name));

                    if (avisoVarificao != null)
                        return false;

                    TipoAviso tipoAviso = new TipoAviso();
                    tipoAviso.Nome = eventType.Name;
                    tipoAviso.TempoMinimo = eventType.MinimumTime;
                    tipoAviso.TempoMaximo = eventType.MaximumTime;

                    context.TipoAvisoSet.Add(tipoAviso);
                    context.SaveChanges();

                    return true;
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
                catch (DbUpdateException e)
                {
                    return false;
                }
                catch (DbEntityValidationException)
                {
                    return false;
                }
                catch (NotSupportedException)
                {
                    return false;
                }
                catch (ObjectDisposedException)
                {
                    return false;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }

        public bool UpdateEvent(EventType eventType)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    TipoAviso tipoAvisoVerificacao = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals(eventType.Name));

                    if (tipoAvisoVerificacao == null)
                        return false;

                    TipoAviso tipoAviso = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals(eventType.Name));
                    tipoAviso.Nome = eventType.Name;
                    tipoAviso.TempoMinimo = eventType.MinimumTime;
                    tipoAviso.TempoMaximo = eventType.MaximumTime;

                    context.SaveChanges();

                    return true;
                }
                catch (NullReferenceException)
                {
                    return false;
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
                catch (DbEntityValidationException)
                {
                    return false;
                }
                catch (NotSupportedException)
                {
                    return false;
                }
                catch (ObjectDisposedException)
                {
                    return false;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }

        public bool DeleteEvent(EventType eventType)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    TipoAviso tipoAviso = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals(eventType.Name));

                    if (tipoAviso == null)
                        return false;

                    TipoAviso aviso = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals(eventType.Name));

                    context.TipoAvisoSet.Remove(aviso);
                    context.SaveChanges();

                    return true;
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
                catch (DbEntityValidationException)
                {
                    return false;
                }
                catch (NotSupportedException)
                {
                    return false;
                }
                catch (ObjectDisposedException)
                {
                    return false;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }

        #endregion IServiceHealthAlert

        #region TimeOuts

        private bool VerifyTimeOut(int range, IEnumerable<IGrouping<bool, SaturacaoValores>> hash)
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

        private bool VerifyTimeOut(int range, IEnumerable<IGrouping<bool, PressaoSanguineaValores>> hash)
        {
            List<PressaoSanguineaValores> valuesBelowMinimumEAI = hash
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

        private bool VerifyTimeOut(int range, IEnumerable<IGrouping<bool, FrequenciaCardiacaValores>> hash)
        {
            List<FrequenciaCardiacaValores> valuesBelowMinimumEAI = hash
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

        private bool VerifyTimeOut(int range, List<SaturacaoValores> list)
        {
            int timespan = 0;

            for (int index = 0; index < list.Count; index++)
            {
                if (index + 1 == list.Count)
                    break;

                timespan += list[index + 1].Data.Minute -
                            list[index].Data.Minute;
            }

            return timespan >= range;
        }

        private bool VerifyTimeOut(int range, List<PressaoSanguineaValores> list)
        {
            int timespan = 0;

            for (int index = 0; index < list.Count; index++)
            {
                if (index + 1 == list.Count)
                    break;

                timespan += list[index + 1].Data.Minute -
                            list[index].Data.Minute;
            }

            return timespan >= range;
        }

        private bool VerifyTimeOut(int range, List<FrequenciaCardiacaValores> list)
        {
            int timespan = 0;

            for (int index = 0; index < list.Count; index++)
            {
                if (index + 1 == list.Count)
                    break;

                timespan += list[index + 1].Data.Minute -
                            list[index].Data.Minute;
            }

            return timespan >= range;
        }

        #endregion TimeOuts
    }
}
