using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ServiceLayerNew
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceHealth" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServiceHealth.svc or ServiceHealth.svc.cs at the Solution Explorer and start debugging.
    public class ServiceHealth : IServiceHealth, IServiceHealthAlert
    {
        #region IServiceHealth

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
                    Alerta al = context.AlertaSet.FirstOrDefault(i => i.TipoAlertaSet.Nome.Equals("SPO2"));

                    SaturacaoValores saturacao = new SaturacaoValores();
                    saturacao.Data = saturation.Date;
                    saturacao.Hora = saturation.Time;
                    saturacao.Saturacao = saturation.Saturation;
                    saturacao.UtenteSet = ut;
                    saturacao.AlertaSet = al;

                    context.SaturacaoValoresSet.Add(saturacao);
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

        public bool InsertBloodPressureRecord(BloodPressure bloodPressure)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    Utente ut = context.UtenteSet.FirstOrDefault(i => i.SNS == bloodPressure.PatientSNS);
                    Alerta al = context.AlertaSet.FirstOrDefault(i => i.TipoAlertaSet.Nome.Equals("BP"));

                    PressaoSanguineaValores pressao = new PressaoSanguineaValores();
                    pressao.Data = bloodPressure.Date;
                    pressao.Hora = bloodPressure.Time;
                    pressao.Distolica = bloodPressure.Diastolic;
                    pressao.Sistolica = bloodPressure.Systolic;
                    pressao.UtenteSet = ut;
                    pressao.AlertaSet = al;

                    context.PressaoSanguineaValoresSet.Add(pressao);
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

        public bool InsertHeartRateRecord(HeartRate heartRate)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    Utente ut = context.UtenteSet.FirstOrDefault(i => i.SNS == heartRate.PatientSNS);
                    Alerta al = context.AlertaSet.FirstOrDefault(i => i.TipoAlertaSet.Nome.Equals("HR"));

                    FrequenciaCardiacaValores frequencia = new FrequenciaCardiacaValores();
                    frequencia.Data = heartRate.Date;
                    frequencia.Hora = heartRate.Time;
                    frequencia.Frequencia = heartRate.Rate;
                    frequencia.UtenteSet = ut;
                    frequencia.AlertaSet = al;

                    context.FrequenciaCardiacaValoresSet.Add(frequencia);
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

        #endregion

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
                catch (DbUpdateException)
                {
                    return false;
                }
                catch (DbEntityValidationException e)
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

        public Alert GetAlert(string type)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    Alerta alerta = context.AlertaSet.FirstOrDefault(i => i.TipoAlertaSet.Nome.Equals(type));

                    Alert alert = new Alert();
                    alert.Type = alerta.TipoAlertaSet.Nome;
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

        public bool InsertAlert(Alert _alert)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    TipoAlerta alertaVarificao = context.TipoAlertaSet.FirstOrDefault(i => i.Nome.Equals(_alert.Type));

                    if (alertaVarificao != null)
                        return false;

                    TipoAlerta tipoAlerta = new TipoAlerta();
                    tipoAlerta.Nome = _alert.Type;

                    context.TipoAlertaSet.Add(tipoAlerta);
                    context.SaveChanges();

                    Alerta alerta = new Alerta();
                    alerta.ValorMinimo = _alert.MinimumValue;
                    alerta.ValorMaximo = _alert.MaximumValue;
                    alerta.ValorCriticoMinimo = _alert.MinimumCriticalValue;
                    alerta.ValorCriticoMaximo = _alert.MaximumCriticalValue;
                    alerta.TipoAlertaSet = tipoAlerta;

                    context.AlertaSet.Add(alerta);
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

        public bool UpdateAlert(Alert _alert)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    TipoAlerta tipoAlerta = context.TipoAlertaSet.FirstOrDefault(i => i.Nome.Equals(_alert.Type));

                    if (tipoAlerta == null)
                        return false;

                    Alerta alerta = context.AlertaSet.FirstOrDefault(i => i.TipoAlertaSet.Nome.Equals(_alert.Type));

                    alerta.ValorMinimo = _alert.MinimumValue;
                    alerta.ValorMaximo = _alert.MaximumValue;
                    alerta.ValorCriticoMinimo = _alert.MinimumCriticalValue;
                    alerta.ValorCriticoMaximo = _alert.MaximumCriticalValue;

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

        public bool DeleteAlert(Alert _alert)
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    TipoAlerta tipoAlerta = context.TipoAlertaSet.FirstOrDefault(i => i.Nome.Equals(_alert.Type));

                    if (tipoAlerta == null)
                        return false;

                    Alerta alerta = context.AlertaSet.FirstOrDefault(i => i.TipoAlertaSet.Nome.Equals(_alert.Type));

                    context.AlertaSet.Remove(alerta);
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

        public List<Alert> GetAlertList()
        {
            using (ModelMyHealth context = new ModelMyHealth())
            {
                try
                {
                    List<Alert> alertList = new List<Alert>();

                    var alertas = context.AlertaSet;

                    foreach (Alerta al in alertas)
                    {
                        Alert alert = new Alert();
                        alert.Type = al.TipoAlertaSet.Nome;
                        alert.MinimumValue = al.ValorMinimo;
                        alert.MaximumValue = al.ValorMaximo;
                        alert.MinimumCriticalValue = al.ValorCriticoMinimo;
                        alert.MaximumCriticalValue = al.ValorCriticoMaximo;

                        alertList.Add(alert);
                    }

                    return alertList;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        #endregion
    }
}
