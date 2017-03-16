using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ServiceLayer
{
    //lets go
    public class ServiceHealth : IServiceHealth, IServiceHealthAlert
    {

        #region IServiceHealth

        public bool ValidatePatient(int sns)
        {
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    Utente ut = context.Utente.FirstOrDefault(i => i.SNS == sns);

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
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    Utente ut = context.Utente.FirstOrDefault(i => i.SNS == saturation.PatientSNS);

                    SaturacaoValores saturacao = new SaturacaoValores();
                    saturacao.Data = saturation.Date;
                    saturacao.Hora = saturation.Time;
                    saturacao.Saturacao = saturation.Saturation;
                    saturacao.Utente = ut;

                    context.SaturacaoValores.Add(saturacao);
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
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    Utente ut = context.Utente.FirstOrDefault(i => i.SNS == bloodPressure.PatientSNS);

                    PressaoSanguineaValores pressao = new PressaoSanguineaValores();
                    pressao.Data = bloodPressure.Date;
                    pressao.Hora = bloodPressure.Time;
                    pressao.Distolica = bloodPressure.Diastolic;
                    pressao.Sistolica = bloodPressure.Systolic;
                    pressao.Utente = ut;

                    context.PressaoSanguineaValores.Add(pressao);
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
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    Utente ut = context.Utente.FirstOrDefault(i => i.SNS == heartRate.PatientSNS);

                    FrequenciaCardiacaValores frequencia = new FrequenciaCardiacaValores();
                    frequencia.Data = heartRate.Date;
                    frequencia.Hora = heartRate.Time;
                    frequencia.Frequencia = heartRate.Rate;
                    frequencia.Utente = ut;

                    context.FrequenciaCardiacaValores.Add(frequencia);
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

        #endregion

        #region IServiceHealthAlert

        public bool InsertPatient(Patient patient)
        {
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    Utente ut = new Utente();
                    ut.Nome = patient.Name;
                    ut.Apelido = patient.Surname;
                    ut.NIF = patient.Nif;
                    ut.Telefone = patient.Phone;
                    ut.Email = patient.Email;
                    ut.NumeroEmergencia = patient.EmergencyNumber;
                    ut.NomeEmergencia = patient.EmergencyName;
                    ut.Sexo = patient.Sex;
                    ut.Morada = patient.Adress;
                    ut.Peso = patient.Weight;
                    ut.Altura = patient.Height;
                    ut.Alergias = patient.Alergies;
                    ut.SNS = patient.Sns;

                    context.Utente.Add(ut);
                    context.SaveChanges();

                    return true;
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

        public bool UpdatePatient(Patient patient)
        {
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    Utente ut = context.Utente.FirstOrDefault(i => i.SNS == patient.Sns);

                    if (ut == null)
                        return false;

                    ut.Nome = patient.Name;
                    ut.Apelido = patient.Surname;
                    ut.NIF = patient.Nif;
                    ut.Telefone = patient.Phone;
                    ut.Email = patient.Email;
                    ut.NumeroEmergencia = patient.EmergencyNumber;
                    ut.NomeEmergencia = patient.EmergencyName;
                    ut.Sexo = patient.Sex;
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

        public bool DeletePatient(Patient patient)
        {
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    Utente ut = context.Utente.FirstOrDefault(i => i.SNS == patient.Sns);

                    if (ut == null)
                        return false;

                    context.Utente.Remove(ut);
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
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    Utente ut = context.Utente.FirstOrDefault(i => i.SNS == sns);

                    Patient patient = new Patient();
                    patient.Name = ut.Nome;
                    patient.Surname = ut.Apelido;
                    patient.Nif = ut.NIF;
                    patient.Phone = ut.Telefone;
                    patient.Email = ut.Email;
                    patient.EmergencyNumber = ut.NumeroEmergencia;
                    patient.EmergencyName = ut.NomeEmergencia;
                    patient.Sex = ut.Sexo;
                    patient.Adress = ut.Morada;
                    patient.Weight = Convert.ToDouble(ut.Peso);
                    patient.Height = Convert.ToInt32(ut.Altura);
                    patient.Alergies = ut.Alergias;
                    patient.Sns = ut.SNS;

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
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    List<Patient> patientList = new List<Patient>();

                    var utentes = context.Utente;

                    foreach (Utente ut in utentes)
                    {
                        Patient patient = new Patient();
                        patient.Name = ut.Nome;
                        patient.Surname = ut.Apelido;
                        patient.Nif = ut.NIF;
                        patient.Phone = ut.Telefone;
                        patient.Email = ut.Email;
                        patient.EmergencyNumber = ut.NumeroEmergencia;
                        patient.EmergencyName = ut.NomeEmergencia;
                        patient.Sex = ut.Sexo;
                        patient.Adress = ut.Morada;
                        patient.Weight = Convert.ToDouble(ut.Peso);
                        patient.Height = Convert.ToInt32(ut.Altura);
                        patient.Alergies = ut.Alergias;
                        patient.Sns = ut.SNS;

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
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    List<HeartRate> heartRateList = new List<HeartRate>();

                    var frequencias = context.FrequenciaCardiacaValores.Where(i => i.Utente.SNS == sns);

                    foreach (FrequenciaCardiacaValores freq in frequencias)
                    {
                        HeartRate heartRate = new HeartRate();
                        heartRate.PatientSNS = freq.Utente.SNS;
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
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    List<OxygenSaturation> oxygenSaturationsList = new List<OxygenSaturation>();

                    var saturacoes = context.SaturacaoValores.Where(i => i.Utente.SNS == sns);

                    foreach (SaturacaoValores sat in saturacoes)
                    {
                        OxygenSaturation saturation = new OxygenSaturation();
                        saturation.PatientSNS = sat.Utente.SNS;
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
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    List<BloodPressure> bloodPressureList = new List<BloodPressure>();

                    var pressoes = context.PressaoSanguineaValores.Where(i => i.Utente.SNS == sns);

                    foreach (PressaoSanguineaValores pss in pressoes)
                    {
                        BloodPressure bp = new BloodPressure();
                        bp.PatientSNS = pss.Utente.SNS;
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
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    Alerta alerta = context.Alerta.FirstOrDefault(i => i.Tipo.Equals(type));

                    Alert alert = new Alert();
                    alert.Type = alerta.Tipo;
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
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    Alerta alertaVarificao = context.Alerta.FirstOrDefault(i => i.Tipo.Equals(_alert.Type));

                    if (alertaVarificao != null)
                        return false;

                    Alerta alerta = new Alerta();
                    alerta.Tipo = _alert.Type;
                    alerta.ValorMinimo = _alert.MinimumValue;
                    alerta.ValorMaximo = _alert.MaximumValue;
                    alerta.ValorCriticoMinimo = _alert.MinimumCriticalValue;
                    alerta.ValorCriticoMaximo = _alert.MaximumCriticalValue;

                    context.Alerta.Add(alerta);
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

        public bool UpdateAlert(Alert _alert)
        {
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    Alerta alerta = context.Alerta.FirstOrDefault(i => i.Tipo.Equals(_alert.Type));

                    if (alerta == null)
                        return false;

                    alerta.Tipo = _alert.Type;
                    alerta.ValorMinimo = _alert.MinimumValue;
                    alerta.ValorMaximo = _alert.MaximumValue;
                    alerta.ValorCriticoMinimo = _alert.MinimumCriticalValue;
                    alerta.ValorCriticoMaximo = _alert.MaximumCriticalValue;

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

        public bool DeleteAlert(Alert _alert)
        {
            using (ModelMyHealthContainer context = new ModelMyHealthContainer())
            {
                try
                {
                    Alerta alerta = context.Alerta.FirstOrDefault(i => i.Tipo.Equals(_alert.Type));

                    if (alerta == null)
                        return false;

                    context.Alerta.Remove(alerta);
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

        #endregion

    }
}
