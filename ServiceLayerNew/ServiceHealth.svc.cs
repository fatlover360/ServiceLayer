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
using ServiceLayerNew.Warnings;

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

        public bool InsertOxygenSaturationRecord(OxygenSaturation saturation)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    Utente ut = context.UtenteSet.FirstOrDefault(i => i.SNS == saturation.PatientSNS);
                    TipoAlerta al = context.TipoAlertaSet.FirstOrDefault(i => i.Nome.Equals("SPO2"));

                    SaturacaoValores saturacao = new SaturacaoValores();
                    saturacao.Data = saturation.Date;
                    saturacao.Hora = saturation.Time;
                    saturacao.Saturacao = saturation.Saturation;
                    saturacao.UtenteSet = ut;
                    saturacao.AlertaSet = al;

                    context.SaturacaoValoresSet.Add(saturacao);
                    context.SaveChanges();

                    SaturationWarnings.Verify(saturacao);
                    
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
        }

        public bool InsertBloodPressureRecord(BloodPressure bloodPressure)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    Utente ut = context.UtenteSet.FirstOrDefault(i => i.SNS == bloodPressure.PatientSNS);
                    TipoAlerta al = context.TipoAlertaSet.FirstOrDefault(i => i.Nome.Equals("BP"));

                    PressaoSanguineaValores pressao = new PressaoSanguineaValores();
                    pressao.Data = bloodPressure.Date;
                    pressao.Hora = bloodPressure.Time;
                    pressao.Distolica = bloodPressure.Diastolic;
                    pressao.Sistolica = bloodPressure.Systolic;
                    pressao.UtenteSet = ut;
                    pressao.AlertaSet = al;

                    context.PressaoSanguineaValoresSet.Add(pressao);
                    context.SaveChanges();

                    BloodPressureWarnings.Verify(pressao);

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
        }

        public bool InsertHeartRateRecord(HeartRate heartRate)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    Utente ut = context.UtenteSet.FirstOrDefault(i => i.SNS == heartRate.PatientSNS);
                    TipoAlerta al = context.TipoAlertaSet.FirstOrDefault(i => i.Nome.Equals("HR"));

                    FrequenciaCardiacaValores frequencia = new FrequenciaCardiacaValores();
                    frequencia.Data = heartRate.Date;
                    frequencia.Hora = heartRate.Time;
                    frequencia.Frequencia = heartRate.Rate;
                    frequencia.UtenteSet = ut;
                    frequencia.AlertaSet = al;

                    context.FrequenciaCardiacaValoresSet.Add(frequencia);
                    context.SaveChanges();

                    HeartRateWarnings.Verify(frequencia);

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
        }

        #endregion IServiceHealth

        #region IServiceHealthAlert

        public bool InsertPatient(Patient patient)
        {
            using (ModelMyHealth context = new ModelMyHealth())
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

                    context.UtenteSet.Add(ut);
                    context.SaveChanges();

                    return true;

                }
                catch (Exception x)
                {
                    Console.WriteLine(x.Message + "  " + x.GetBaseException());

                    return false;
                }
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

                    var frequencias = context.FrequenciaCardiacaValoresSet.Where(i => i.UtenteSet.SNS == sns);

                    foreach (FrequenciaCardiacaValores freq in frequencias)
                    {
                        HeartRate heartRate = new HeartRate();
                        heartRate.PatientSNS = freq.UtenteSet.SNS;
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

                    var saturacoes = context.SaturacaoValoresSet.Where(i => i.UtenteSet.SNS == sns);

                    foreach (SaturacaoValores sat in saturacoes)
                    {
                        OxygenSaturation saturation = new OxygenSaturation();
                        saturation.PatientSNS = sat.UtenteSet.SNS;
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

                    var pressoes = context.PressaoSanguineaValoresSet.Where(i => i.UtenteSet.SNS == sns);

                    foreach (PressaoSanguineaValores pss in pressoes)
                    {
                        BloodPressure bp = new BloodPressure();
                        bp.PatientSNS = pss.UtenteSet.SNS;
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

        public AlertType GetAlert(string type)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    TipoAlerta alerta = context.TipoAlertaSet.FirstOrDefault(i => i.Nome.Equals(type));

                    AlertType alert = new AlertType();
                    alert.Type = alerta.Nome;
                    alert.MinimumValue = alerta.ValorMinimo;
                    alert.MaximumValue = alerta.ValorMaximo;
                    alert.MinimumCriticalValue = alerta.ValorCriticoMinimo;
                    alert.MaximumCriticalValue = alerta.ValorCriticoMaximo;

                    return alert;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public bool InsertAlert(AlertType alertType)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    TipoAlerta alertaVarificao = context.TipoAlertaSet.FirstOrDefault(i => i.Nome.Equals(alertType.Type));

                    if (alertaVarificao != null)
                        return false;

                    TipoAlerta tipoAlerta = new TipoAlerta();
                    tipoAlerta.Nome = alertType.Type;
                    tipoAlerta.ValorMaximo = alertType.MaximumValue;
                    tipoAlerta.ValorMinimo = alertType.MinimumValue;
                    tipoAlerta.ValorCriticoMaximo = alertType.MaximumCriticalValue;
                    tipoAlerta.ValorCriticoMinimo = alertType.MinimumCriticalValue;

                    context.TipoAlertaSet.Add(tipoAlerta);
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

        public bool UpdateAlert(AlertType alertType)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    TipoAlerta tipoAlerta = context.TipoAlertaSet.FirstOrDefault(i => i.Nome.Equals(alertType.Type));

                    if (tipoAlerta == null)
                        return false;

                    TipoAlerta alerta = context.TipoAlertaSet.FirstOrDefault(i => i.Nome.Equals(alertType.Type));

                    alerta.ValorMinimo = alertType.MinimumValue;
                    alerta.ValorMaximo = alertType.MaximumValue;
                    alerta.ValorCriticoMinimo = alertType.MinimumCriticalValue;
                    alerta.ValorCriticoMaximo = alertType.MaximumCriticalValue;

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

        public bool DeleteAlert(AlertType alertType)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    TipoAlerta tipoAlerta = context.TipoAlertaSet.FirstOrDefault(i => i.Nome.Equals(alertType.Type));

                    if (tipoAlerta == null)
                        return false;

                    TipoAlerta alerta = context.TipoAlertaSet.FirstOrDefault(i => i.Nome.Equals(alertType.Type));

                    context.TipoAlertaSet.Remove(alerta);
                    context.TipoAlertaSet.Remove(tipoAlerta);
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

        public List<AlertType> GetAlertList()
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    List<AlertType> alertList = new List<AlertType>();

                    var alertas = context.TipoAlertaSet;

                    foreach (TipoAlerta al in alertas)
                    {
                        AlertType alertType = new AlertType();
                        alertType.Type = al.Nome;
                        alertType.MinimumValue = al.ValorMinimo;
                        alertType.MaximumValue = al.ValorMaximo;
                        alertType.MinimumCriticalValue = al.ValorCriticoMinimo;
                        alertType.MaximumCriticalValue = al.ValorCriticoMaximo;

                        alertList.Add(alertType);
                    }

                    return alertList;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        #endregion IServiceHealthAlert
        
    }
}
