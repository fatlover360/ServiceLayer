using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ServiceLayerNew
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IServiceHealth
    {
        [OperationContract]
        bool TestConnection();

        [OperationContract]
        bool ValidatePatient(int sns);

        [OperationContract]
        bool ValidatePatientState(int sns);

        [OperationContract]
        bool InsertHeartRateRecord(HeartRate heartRate);

        [OperationContract]
        bool InsertOxygenSaturationRecord(OxygenSaturation saturation);

        [OperationContract]
        bool InsertBloodPressureRecord(BloodPressure bloodPressure);
    }

    [ServiceContract]
    public interface IServiceHealthAlert
    {
        [OperationContract]
        bool InsertPatient(Patient patient);

        [OperationContract]
        bool UpdatePatient(Patient patient, int sns);

        [OperationContract]
        bool UpdateStatePatient(Patient patient);

        [OperationContract]
        Patient GetPatient(int sns);

        [OperationContract]
        List<Patient> GetPatientList();

        [OperationContract]
        List<HeartRate> HeartRateList(int sns);

        [OperationContract]
        List<OxygenSaturation> OxygenSaturationList(int sns);

        [OperationContract]
        List<BloodPressure> BloodPressureList(int sns);

        [OperationContract]
        ConfigurationLimitType GetConfigurationLimit(ConfigurationLimitType type);

        [OperationContract]
        List<ConfigurationLimitType> GetConfigurationLimitList();

        [OperationContract]
        bool InsertConfigurationLimit(ConfigurationLimitType configurationLimitType);

        [OperationContract]
        bool UpdateConfigurationLimit(ConfigurationLimitType configurationLimitType);

        [OperationContract]
        bool DeleteConfigurationLimit(ConfigurationLimitType configurationLimitType);

        [OperationContract]
        bool InsertEvent(Event eventType);
        
        [OperationContract]
        bool UpdateEvent(Event eventType);
        
        [OperationContract]
        bool DeleteEvent(Event eventType);

        [OperationContract]
        List<OxygenSaturation> GetWarningListOxygenSaturation(Event type);

        [OperationContract]
        List<BloodPressure> GetWarningListBloodPressure(Event type);

        [OperationContract]
        List<HeartRate> GetWarningListHeartRate(Event type);
    }

    [DataContract]
    public class Patient
    {
        private string name;
        private string surname;
        private DateTime birthDate;
        private Int32 nif;
        private Int32 phone;
        private string phoneCountryCode;
        private string email;
        private Int32 emergencyNumber;
        private string emergencyNumberCountryCode;
        private string emergencyName;
        private string gender;
        private string adress;
        private string alergies;
        private double weight;
        private Int32 height;
        private Int32 sns;
        private bool ativo;

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        [DataMember]
        public DateTime BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; }
        }

        [DataMember]
        public Int32 Nif
        {
            get { return nif; }
            set { nif = value; }
        }

        [DataMember]
        public Int32 Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        [DataMember]
        public string PhoneCountryCode
        {
            get { return phoneCountryCode; }
            set { phoneCountryCode = value; }
        }

        [DataMember]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [DataMember]
        public Int32 EmergencyNumber
        {
            get { return emergencyNumber; }
            set { emergencyNumber = value; }
        }

        [DataMember]
        public string EmergencyNumberCountryCode
        {
            get { return emergencyNumberCountryCode; }
            set { emergencyNumberCountryCode = value; }
        }

        [DataMember]
        public string EmergencyName
        {
            get { return emergencyName; }
            set { emergencyName = value; }
        }

        [DataMember]
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        [DataMember]
        public string Adress
        {
            get { return adress; }
            set { adress = value; }
        }

        [DataMember]
        public string Alergies
        {
            get { return alergies; }
            set { alergies = value; }
        }

        [DataMember]
        public double Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        [DataMember]
        public Int32 Height
        {
            get { return height; }
            set { height = value; }
        }

        [DataMember]
        public Int32 Sns
        {
            get { return sns; }
            set { sns = value; }
        }

        [DataMember]
        public bool Ativo
        {
            get { return ativo; }
            set { ativo = value; }
        }
    }

    [DataContract]
    public class HeartRate
    {
        private int patientSNS;
        private DateTime date;
        private TimeSpan time;
        private int rate;

        [DataMember]
        public int PatientSNS
        {
            get { return patientSNS; }
            set { patientSNS = value; }
        }

        [DataMember]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        [DataMember]
        public TimeSpan Time
        {
            get { return time; }
            set { time = value; }
        }

        [DataMember]
        public int Rate
        {
            get { return rate; }
            set { rate = value; }
        }
    }

    [DataContract]
    public class OxygenSaturation
    {
        private int patientSNS;
        private DateTime date;
        private TimeSpan time;
        private int saturation;

        [DataMember]
        public int PatientSNS
        {
            get { return patientSNS; }
            set { patientSNS = value; }
        }

        [DataMember]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        [DataMember]
        public TimeSpan Time
        {
            get { return time; }
            set { time = value; }
        }

        [DataMember]
        public int Saturation
        {
            get { return saturation; }
            set { saturation = value; }
        }
    }

    [DataContract]
    public class BloodPressure
    {
        private int patientSNS;
        private DateTime date;
        private TimeSpan time;
        private int diastolic;
        private int systolic;

        [DataMember]
        public int PatientSNS
        {
            get { return patientSNS; }
            set { patientSNS = value; }
        }

        [DataMember]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        [DataMember]
        public TimeSpan Time
        {
            get { return time; }
            set { time = value; }
        }

        [DataMember]
        public int Diastolic
        {
            get { return diastolic; }
            set { diastolic = value; }
        }

        [DataMember]
        public int Systolic
        {
            get { return systolic; }
            set { systolic = value; }
        }
    }

    [DataContract]
    public class ConfigurationLimitType
    {
        private Type configurationType;
        private int minimumValue;
        private int maximumValue;
        private int minimumCriticalValue;
        private int maximumCriticalValue;

        public enum Type
        {
            HR,
            SPO2,
            BP
        }

        [DataMember]
        public Type ConfigurationType
        {
            get { return configurationType; }
            set { configurationType = value; }
        }

        [DataMember]
        public int MinimumValue
        {
            get { return minimumValue; }
            set { minimumValue = value; }
        }

        [DataMember]
        public int MaximumValue
        {
            get { return maximumValue; }
            set { maximumValue = value; }
        }

        [DataMember]
        public int MinimumCriticalValue
        {
            get { return minimumCriticalValue; }
            set { minimumCriticalValue = value; }
        }

        [DataMember]
        public int MaximumCriticalValue
        {
            get { return maximumCriticalValue; }
            set { maximumCriticalValue = value; }
        }
    }

    [DataContract]
    public class Event
    {
        private int minimumTime;
        private int maximumTime;
        private Type evenType;

        public enum Type
        {
            ECA,
            ECC,
            ECI,
            EAC,
            EAI
        }

        [DataMember]
        public Type EvenType
        {
            get { return evenType; }
            set { evenType = value; }
        }

        [DataMember]
        public int MinimumTime
        {
            get { return minimumTime; }
            set { minimumTime = value; }
        }

        [DataMember]
        public int MaximumTime
        {
            get { return maximumTime; }
            set { maximumTime = value; }
        }
    }
}
