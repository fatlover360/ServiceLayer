using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ServiceLayer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IServiceHealth
    {
        [OperationContract]
        bool ValidatePatient(int sns);
        
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
        bool UpdatePatient(Patient patient);

        [OperationContract]
        bool DeletePatient(Patient patient);

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
        Alert GetAlert(string type);

        [OperationContract]
        List<Alert> GetAlertList();

        [OperationContract]
        bool InsertAlert(Alert _alert);

        [OperationContract]
        bool UpdateAlert(Alert _alert);

        [OperationContract]
        bool DeleteAlert(Alert _alert);
    }

    [DataContract]
    public class Patient
    {
        private string name;
        private string surname;
        private int nif;
        private string phone;
        private string email;
        private string emergencyNumber;
        private string emergencyName;
        private string sex;
        private string adress;
        private string alergies;
        private double weight;
        private int height;
        private int sns;

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
        public int Nif
        {
            get { return nif; } 
            set { nif = value; }
        }

        [DataMember]
        public string Phone
        {
            get { return phone; } 
            set { phone = value; }
        }

        [DataMember]
        public string Email
        {
            get { return email; } 
            set { email = value; }
        }

        [DataMember]
        public string EmergencyNumber
        {
            get { return emergencyNumber; } 
            set { emergencyNumber = value; }
        }

        [DataMember]
        public string EmergencyName
        {
            get { return emergencyName; } 
            set { emergencyName = value; }
        }

        [DataMember]
        public string Sex
        {
            get { return sex; } 
            set { sex = value; }
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
        public int Height
        {
            get { return height; } 
            set { height = value; }
        }

        [DataMember]
        public int Sns
        {
            get { return sns; } 
            set { sns = value; }
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
    public class Alert
    {
        private string type;
        private int minimumValue;
        private int maximumValue;
        private int minimumCriticalValue;
        private int maximumCriticalValue;

        [DataMember]
        public string Type
        {
            get { return type; }
            set { type = value; }
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
}
