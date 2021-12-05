using System;

namespace TaxiCompany.Models
{
    public class DriverValidation : BaseModel
    {
        private string idError;
        private string nameError;
        private string drivingLicenceValidToError;
        private string workExperienceError;
        private string nationalityError;

        public string IdError
        {
            get { return idError; }
            set { idError = value; OnPropertyChanged(nameof(IdError)); }
        }

        public string NameError
        {
            get { return nameError; }
            set { nameError = value; OnPropertyChanged(nameof(NameError)); }
        }

        public string DrivingLicenceValidToError
        {
            get { return drivingLicenceValidToError; }
            set { drivingLicenceValidToError = value; OnPropertyChanged(nameof(DrivingLicenceValidToError)); }
        }

        public string WorkExperienceError
        {
            get { return workExperienceError; }
            set { workExperienceError = value; OnPropertyChanged(nameof(WorkExperienceError)); }
        }

        public string NationalityError
        {
            get { return nationalityError; }
            set { nationalityError = value; OnPropertyChanged(nameof(NationalityError)); }
        }

        public bool ValidateDriver(Driver driver)
        {
            bool hasErrors = false;

            //TODO: check if Id exists in db
            if (driver.Id == null || driver.Id.Length != 10)
            {
                IdError = "Задължително дължината на ЕГН трябва да бъде 10 символа.";
                hasErrors = true;
            }
            else
            {
                IdError = string.Empty;
            }

            if (driver.Name == null || driver.Name.Length <= 5)
            {
                NameError = "Името трябва да бъде поне 6 символа.";
                hasErrors = true;
            }
            else
            {
                NameError = string.Empty;
            }

            if (driver.DrivingLicenceValidTo == null)
            {
                DrivingLicenceValidToError = "Датата на валидност на свидетелство за правоспособност е задължително.";
                hasErrors = true;
            }
            else
            {
                DrivingLicenceValidToError = string.Empty;
            }

            if (driver.WorkExperience < 0)
            {
                WorkExperienceError = "Професионалният опит не може да бъде отрицателно число.";
                hasErrors = true;
            }
            else
            {
                WorkExperienceError = string.Empty;
            }

            if (driver.Nationality == null || driver.Nationality.Length <= 2)
            {
                NationalityError = "Националността трябва да бъде поне 3 символа.";
                hasErrors = true;
            }
            else
            {
                NationalityError = string.Empty;
            }

            return hasErrors;
        }
    }
}
