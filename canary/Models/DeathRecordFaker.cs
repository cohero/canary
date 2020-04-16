using System;
using System.Collections.Generic;
using Bogus.Extensions.UnitedStates;
using System.Linq;
using VRDR;

namespace canary.Models
{
    /// <summary>Class <c>Faker</c> can be used to generate synthetic <c>DeathRecord</c>s. Various
    /// options are available to tailoring the records generated to specific use case by the class.
    /// </summary>
    public class DeathRecordFaker
    {
        /// <summary>Mortality data for code translations.</summary>
        private MortalityData MortalityData = MortalityData.Instance;

        /// <summary>State to use when generating records.</summary>
        private string state;

        /// <summary>Type of Cause of Death; Natural or Injury.</summary>
        private string type;

        /// <summary>Decedent Sex to use.</summary>
        private string sex;

        /// <summary>Constructor with optional arguments to customize the records generated.</summary>
        public DeathRecordFaker(string state = "MA", string type = "Natural", string sex = "Male")
        {
            this.state = state;
            this.type = type;
            this.sex = sex;
        }

        /// <summary>Return a new record populated with fake data.</summary>
        public DeathRecord Generate(bool simple = false)
        {
            DeathRecord record = new DeathRecord();

            Random random = new Random();
            Bogus.Faker faker = new Bogus.Faker("en");
            MortalityData dataHelper = MortalityData;

            // Grab Gender enum value
            Bogus.DataSets.Name.Gender gender = sex == "Male" ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female;

            // Was married?
            bool wasMarried = faker.Random.Bool();

            // Full state name for reference
            string stateName = dataHelper.StateCodeToStateName(state);

            record.Identifier = Convert.ToString(faker.Random.Number(999999));
            record.BundleIdentifier = Convert.ToString(faker.Random.Number(999999));
            DateTime date = faker.Date.Recent();
            record.CertifiedTime = date.ToString("s");
            record.RegisteredTime = new DateTimeOffset(date.AddDays(-1).Year, date.AddDays(-1).Month, date.AddDays(-1).Day, 0, 0, 0, TimeSpan.Zero).ToString("s");

            // Basic Decedent information

            record.GivenNames = new string[] { faker.Name.FirstName(gender), faker.Name.FirstName(gender) };
            record.FamilyName = faker.Name.LastName(gender);
            record.Suffix = faker.Name.Suffix();
            if (gender == Bogus.DataSets.Name.Gender.Female && wasMarried)
            {
                record.MaidenName = faker.Name.LastName(gender);
            }

            record.AliasGivenNames = new string[] { faker.Name.FirstName(gender) };
            record.AliasFamilyName = faker.Name.LastName(gender);

            record.FatherFamilyName = record.FamilyName;
            record.FatherGivenNames = new string[] { faker.Name.FirstName(Bogus.DataSets.Name.Gender.Male), faker.Name.FirstName(Bogus.DataSets.Name.Gender.Male) };
            record.FatherSuffix = faker.Name.Suffix();

            record.MotherGivenNames = new string[] { faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female), faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female) };
            record.MotherMaidenName = faker.Name.LastName(Bogus.DataSets.Name.Gender.Female);
            record.MotherSuffix = faker.Name.Suffix();

            record.SpouseGivenNames = new string[] { faker.Name.FirstName(), faker.Name.FirstName() };
            record.SpouseFamilyName = record.FamilyName;
            record.SpouseSuffix = faker.Name.Suffix();

            record.BirthRecordId = Convert.ToString(faker.Random.Number(999999));

            record.Gender = gender.ToString().ToLower();
            record.SSN = faker.Person.Ssn().Replace("-", string.Empty);
            DateTime birth = faker.Date.Past(123, DateTime.Today.AddYears(-18));
            DateTime death = faker.Date.Recent();
            DateTimeOffset birthUtc = new DateTimeOffset(birth.Year, birth.Month, birth.Day, 0, 0, 0, TimeSpan.Zero);
            DateTimeOffset deathUtc = new DateTimeOffset(death.Year, death.Month, death.Day, 0, 0, 0, TimeSpan.Zero);
            record.DateOfBirth = birthUtc.ToString("yyyy-MM-dd");
            record.DateOfDeath = deathUtc.ToString("yyyy-MM-dd");
            int age = death.Year - birth.Year;
            if (birthUtc > deathUtc.AddYears(-age)) age--;
            record.AgeAtDeath = new Dictionary<string, string>() { { "value", age.ToString() }, { "unit", "a" } };

            // Birthsex

            record.BirthSex = gender.ToString()[0].ToString();

            // Place of residence

            Dictionary<string, string> residence = new Dictionary<string, string>();
            PlaceCode residencePlace = dataHelper.StateCodeToRandomPlace(state);
            residence.Add("addressLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            residence.Add("addressCity", residencePlace.City);
            residence.Add("addressCounty", residencePlace.County);
            residence.Add("addressState", stateName);
            residence.Add("addressCountry", "United States");
            record.Residence = residence;

            // Residence Within City Limits
            record.ResidenceWithinCityLimits = true;

            // Place of birth

            Dictionary<string, string> placeOfBirth = new Dictionary<string, string>();
            PlaceCode placeOfBirthPlace = dataHelper.StateCodeToRandomPlace(state);
            placeOfBirth.Add("addressCity", placeOfBirthPlace.City);
            placeOfBirth.Add("addressCounty", placeOfBirthPlace.County);
            placeOfBirth.Add("addressState", stateName);
            placeOfBirth.Add("addressCountry", "United States");
            record.PlaceOfBirth = placeOfBirth;

            // Place of death

            PlaceCode placeOfDeathPlace = dataHelper.StateCodeToRandomPlace(state);
            record.DeathLocationName = placeOfDeathPlace.City + " Hospital";

            Dictionary<string, string> placeOfDeath = new Dictionary<string, string>();
            placeOfDeath.Add("addressLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            placeOfDeath.Add("addressCity", placeOfDeathPlace.City);
            placeOfDeath.Add("addressCounty", placeOfDeathPlace.County);
            placeOfDeath.Add("addressState", stateName);
            placeOfDeath.Add("addressCountry", "United States");
            record.DeathLocationAddress = placeOfDeath;

            // Marital status

            Dictionary<string, string> maritalStatus = new Dictionary<string, string>();
            Tuple<string, string>[] maritalStatusCodes =
            {
                Tuple.Create("M", "Married"),
                Tuple.Create("D", "Divorced"),
                Tuple.Create("W", "Widowed"),
            };
            Tuple<string, string> maritalStatusCode = faker.Random.ArrayElement<Tuple<string, string>>(maritalStatusCodes);
            if (!wasMarried)
            {
                maritalStatusCode = Tuple.Create("S", "Never Married");
            }
            maritalStatus.Add("code", maritalStatusCode.Item1);
            maritalStatus.Add("system", "http://terminology.hl7.org/CodeSystem/v3-MaritalStatus");
            maritalStatus.Add("display", maritalStatusCode.Item2);
            record.MaritalStatus = maritalStatus;

            // Ethnicity
            if (faker.Random.Bool() && !simple)
            {
                var ethnicityDetailed = dataHelper.CDCEthnicityCodes.ElementAt(2 + faker.Random.Number(15));
                Tuple<string, string>[] ethnicity = {
                    Tuple.Create("Hispanic or Latino", "2135-2"),
                    Tuple.Create(ethnicityDetailed.Value, ethnicityDetailed.Key)
                };
                record.Ethnicity = ethnicity;
            }
            else
            {
                Tuple<string, string>[] ethnicity = { Tuple.Create("Not Hispanic or Latino", "2186-5") };
                record.Ethnicity = ethnicity;
            }

            // Race

            if (!simple)
            {
                Tuple<string, string>[] ombRaces =
                {
                    Tuple.Create("Black or African American", "2054-5"),
                    Tuple.Create("Asian", "2028-9"),
                    Tuple.Create("American Indian or Alaska Native", "1002-5"),
                    Tuple.Create("Native Hawaiian or Other Pacific Islander", "2076-8")
                };
                Tuple<string, string> ombRace = faker.Random.ArrayElement<Tuple<string, string>>(ombRaces);
                var cdcRaceW = dataHelper.CDCRaceWCodes.ElementAt(1 + faker.Random.Number(10));
                Tuple<string, string>[] race = { Tuple.Create("White", "2106-3"), Tuple.Create(cdcRaceW.Value, cdcRaceW.Key), ombRace };
                record.Race = race;
            }
            else
            {
                record.Race = new Tuple<string, string>[]{ Tuple.Create("White", "2106-3") };
            }

            // Education level

            Dictionary<string, string> education = new Dictionary<string, string>();
            Tuple<string, string>[] educationCodes =
            {
                Tuple.Create("BD", "College or baccalaureate degree complete"),
                Tuple.Create("GD", "Graduate or professional Degree complete"),
                Tuple.Create("SEC", "Some secondary or high school education"),
                Tuple.Create("SCOL", "Some College education"),
            };
            Tuple<string, string> educationCode = faker.Random.ArrayElement<Tuple<string, string>>(educationCodes);
            education.Add("code", educationCode.Item1);
            education.Add("system", "http://terminology.hl7.org/CodeSystem/v3-EducationLevel");
            education.Add("display", educationCode.Item2);
            record.EducationLevel = education;

            Tuple<string, string>[] occupationIndustries =
            {
                Tuple.Create("secretary", "State agency"),
                Tuple.Create("carpenter", "construction"),
                Tuple.Create("Programmer", "Health Insurance"),
            };
            Tuple<string, string> occInd = faker.Random.ArrayElement<Tuple<string, string>>(occupationIndustries);

            // Occupation

            record.UsualOccupation = occInd.Item1;

            // Industry

            record.UsualIndustry = occInd.Item2;

            // Military Service

            Dictionary<string, string> military = new Dictionary<string, string>();
            Tuple<string, string>[] militaryCodes =
            {
                Tuple.Create("Y", "Yes"),
                Tuple.Create("N", "No"),
            };
            Tuple<string, string> militaryCode = faker.Random.ArrayElement<Tuple<string, string>>(militaryCodes);
            military.Add("code", militaryCode.Item1);
            military.Add("system", "http://terminology.hl7.org/CodeSystem/v2-0136");
            military.Add("display", militaryCode.Item2);
            record.MilitaryService = military;

            // Funeral Home Name

            record.FuneralHomeName = faker.Name.LastName() + " Funeral Home";

            // Funeral Home Address

            Dictionary<string, string> fha = new Dictionary<string, string>();
            PlaceCode fhaPlace = dataHelper.StateCodeToRandomPlace(state);
            fha.Add("addressLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            fha.Add("addressCity", fhaPlace.City);
            fha.Add("addressCounty", fhaPlace.County);
            fha.Add("addressState", stateName);
            fha.Add("addressCountry", "United States");
            record.FuneralHomeAddress = fha;

            // Disposition Location Name

            record.DispositionLocationName = faker.Name.LastName() + " Cemetery";

            // Disposition Location Address

            Dictionary<string, string> dispLoc = new Dictionary<string, string>();
            PlaceCode dispLocPlace = dataHelper.StateCodeToRandomPlace(state);
            dispLoc.Add("addressCity", dispLocPlace.City);
            dispLoc.Add("addressCounty", dispLocPlace.County);
            dispLoc.Add("addressState", stateName);
            dispLoc.Add("addressCountry", "United States");
            record.DispositionLocationAddress = dispLoc;

            // Disposition Method

            Dictionary<string, string> disposition = new Dictionary<string, string>();
            Tuple<string, string>[] dispositionTypeCodes =
            {
                Tuple.Create("449971000124106", "Patient status determination, deceased and buried"),
                Tuple.Create("449961000124104", "Patient status determination, deceased and cremated"),
                Tuple.Create("449931000124108", "Patient status determination, deceased and entombed"),
            };
            Tuple<string, string> dispositionTypeCode = faker.Random.ArrayElement<Tuple<string, string>>(dispositionTypeCodes);
            disposition.Add("code", dispositionTypeCode.Item1);
            disposition.Add("system", "http://snomed.info/sct");
            disposition.Add("display", dispositionTypeCode.Item2);
            record.DecedentDispositionMethod = disposition;

            // Mortician

            record.MorticianFamilyName = faker.Name.LastName();
            record.MorticianGivenNames = new string[] { faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female), faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female) };
            record.MorticianSuffix = faker.Name.Suffix();
            record.MorticianIdentifier = Convert.ToString(faker.Random.Number(999999));

            // Basic Certifier information

            record.CertifierFamilyName = faker.Name.LastName();
            record.CertifierGivenNames = new string[] { faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female), faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female) };
            record.CertifierSuffix = "MD";

            Dictionary<string, string> certifierAddress = new Dictionary<string, string>();
            PlaceCode certifierAddressPlace = dataHelper.StateCodeToRandomPlace(state);
            certifierAddress.Add("addressLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            certifierAddress.Add("addressCity", certifierAddressPlace.City);
            certifierAddress.Add("addressCounty", certifierAddressPlace.County);
            certifierAddress.Add("addressState", stateName);
            certifierAddress.Add("addressCountry", "United States");
            record.CertifierAddress = certifierAddress;

            // Certifier type
            Dictionary<string, string> certificationType = new Dictionary<string, string>();
            certificationType.Add("system", "http://snomed.info/sct");
            certificationType.Add("code", "434641000124105");
            certificationType.Add("display", "Death certification and verification by physician");
            record.CertificationRole = certificationType;

            // CertifierLicenseNumber
            record.CertifierLicenseNumber = Convert.ToString(faker.Random.Number(999999));

            // Interested Party
            record.InterestedPartyName = faker.Name.LastName() + " LLC";
            Dictionary<string, string> interestedPartyAddress = new Dictionary<string, string>();
            PlaceCode interestedPartyAddressPlace = dataHelper.StateCodeToRandomPlace(state);
            interestedPartyAddress.Add("addressLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            interestedPartyAddress.Add("addressCity", interestedPartyAddressPlace.City);
            interestedPartyAddress.Add("addressCounty", interestedPartyAddressPlace.County);
            interestedPartyAddress.Add("addressState", stateName);
            interestedPartyAddress.Add("addressCountry", "United States");
            record.InterestedPartyAddress = interestedPartyAddress;
            record.InterestedPartyIdentifier = Convert.ToString(faker.Random.Number(999999));
            record.InterestedPartyType = new Dictionary<string, string>() { { "code", "prov" }, { "system", "http://terminology.hl7.org/CodeSystem/organization-type" }, { "display", "Healthcare Provider" } };

            if (type == "Natural")
            {
                Dictionary<string, string> mannerOfDeath = new Dictionary<string, string>();
                mannerOfDeath.Add("code", "38605008");
                mannerOfDeath.Add("system", "http://snomed.info/sct");
                mannerOfDeath.Add("display", "Natural death");
                record.MannerOfDeathType = mannerOfDeath;

                record.DateOfDeath = deathUtc.ToString("o");
                record.DateOfDeathPronouncement = deathUtc.AddHours(1).ToString("o");

                // Randomly pick one of four possible natural causes
                int choice = faker.Random.Number(3);
                if (choice == 0)
                {
                    Tuple<string, string, Dictionary<string, string>>[] causes =
                    {
                        Tuple.Create("Pulmonary embolism", "30 minutes", new Dictionary<string, string>()),
                        Tuple.Create("Deep venuous thrombosis in left thigh", "3 days", new Dictionary<string, string>()),
                        Tuple.Create("Acute hepatic failure", "3 days", new Dictionary<string, string>()),
                        Tuple.Create("Moderately differentiated hepatocellular carcinoma", "over 3 months", new Dictionary<string, string>())
                    };
                    record.CausesOfDeath = causes;

                    record.AutopsyPerformedIndicator = new Dictionary<string, string>() { { "code", "N" }, { "system", "http://terminology.hl7.org/CodeSystem/v2-0136" }, { "display", "No" } };
                    record.AutopsyResultsAvailable = new Dictionary<string, string>() { { "code", "N" }, { "system", "http://terminology.hl7.org/CodeSystem/v2-0136" }, { "display", "No" } };;
                    record.ExaminerContacted = false;

                    record.TobaccoUse = new Dictionary<string, string>() { { "code", "373067005" }, { "system", "http://snomed.info/sct" }, { "display", "No" } };
                }
                else if (choice == 1)
                {
                    Tuple<string, string, Dictionary<string, string>>[] causes =
                    {
                        Tuple.Create("Acute myocardial infarction", "2 days", new Dictionary<string, string>()),
                        Tuple.Create("Arteriosclerotic heart disease", "10 years", new Dictionary<string, string>())
                    };
                    record.CausesOfDeath = causes;

                    record.ContributingConditions = "Carcinoma of cecum, Congestive heart failure";

                    record.AutopsyPerformedIndicator = new Dictionary<string, string>() { { "code", "N" }, { "system", "http://terminology.hl7.org/CodeSystem/v2-0136" }, { "display", "No" } };
                    record.AutopsyResultsAvailable = new Dictionary<string, string>() { { "code", "N" }, { "system", "http://terminology.hl7.org/CodeSystem/v2-0136" }, { "display", "No" } };;
                    record.ExaminerContacted = false;

                    record.TobaccoUse = new Dictionary<string, string>() { { "code", "373067005" }, { "system", "http://snomed.info/sct" }, { "display", "No" } };
                }
                else if (choice == 2)
                {
                    Tuple<string, string, Dictionary<string, string>>[] causes =
                    {
                        Tuple.Create("Pulmonary embolism", "1 hour", new Dictionary<string, string>()),
                        Tuple.Create("Acute myocardial infarction", "7 days", new Dictionary<string, string>()),
                        Tuple.Create("Chronic ischemic heart disease", "8 years", new Dictionary<string, string>())
                    };
                    record.CausesOfDeath = causes;

                    record.ContributingConditions = "Non-insulin-dependent diabetes mellitus, Obesity, Hypertension, Congestive heart failure";

                    record.AutopsyPerformedIndicator = new Dictionary<string, string>() { { "code", "N" }, { "system", "http://terminology.hl7.org/CodeSystem/v2-0136" }, { "display", "No" } };
                    record.AutopsyResultsAvailable = new Dictionary<string, string>() { { "code", "N" }, { "system", "http://terminology.hl7.org/CodeSystem/v2-0136" }, { "display", "No" } };;
                    record.ExaminerContacted = false;

                    record.TobaccoUse = new Dictionary<string, string>() { { "code", "373066001" }, { "system", "http://snomed.info/sct" }, { "display", "Yes" } };
                }
                else if (choice == 3)
                {
                    Tuple<string, string, Dictionary<string, string>>[] causes =
                    {
                        Tuple.Create("Rupture of left ventricle", "Minutes", new Dictionary<string, string>()),
                        Tuple.Create("Myocardial infarction", "2 Days", new Dictionary<string, string>()),
                        Tuple.Create("Coronary atherosclerosis", "2 Years", new Dictionary<string, string>())
                    };
                    record.CausesOfDeath = causes;

                    record.ContributingConditions = "Non-insulin-dependent diabetes mellitus, Cigarette smoking, Hypertension, Hypercholesterolemia, Coronary bypass surgery";

                    record.AutopsyPerformedIndicator = new Dictionary<string, string>() { { "code", "Y" }, { "system", "http://terminology.hl7.org/CodeSystem/v2-0136" }, { "display", "Yes" } };
                    record.AutopsyResultsAvailable = new Dictionary<string, string>() { { "code", "Y" }, { "system", "http://terminology.hl7.org/CodeSystem/v2-0136" }, { "display", "Yes" } };;
                    record.ExaminerContacted = true;

                    record.TobaccoUse = new Dictionary<string, string>() { { "code", "373066001" }, { "system", "http://snomed.info/sct" }, { "display", "Yes" } };
                }
            }
            if (type == "Injury")
            {
                // Randomly pick one of three possible injury causes
                int choice = faker.Random.Number(2);
                if (choice == 0)
                {
                    Tuple<string, string, Dictionary<string, string>>[] causes =
                    {
                        Tuple.Create("Carbon monoxide poisoning", "Unkown", new Dictionary<string, string>()),
                        Tuple.Create("Inhalation of automobile exhaust fumes", "Unkown", new Dictionary<string, string>())
                    };
                    record.CausesOfDeath = causes;

                    record.ContributingConditions = "Terminal gastric adenocarcinoma, depression";

                    record.AutopsyPerformedIndicator = new Dictionary<string, string>() { { "code", "Y" }, { "system", "http://terminology.hl7.org/CodeSystem/v2-0136" }, { "display", "Yes" } };
                    record.AutopsyResultsAvailable = new Dictionary<string, string>() { { "code", "Y" }, { "system", "http://terminology.hl7.org/CodeSystem/v2-0136" }, { "display", "Yes" } };;
                    record.ExaminerContacted = true;

                    record.TobaccoUse = new Dictionary<string, string>() { { "code", "UNK" }, { "system", "http://hl7.org/fhir/v3/NullFlavor" }, { "display", "unknown" } };

                    Dictionary<string, string> mannerOfDeath = new Dictionary<string, string>();
                    mannerOfDeath.Add("code", "44301001");
                    mannerOfDeath.Add("system", "http://snomed.info/sct");
                    mannerOfDeath.Add("display", "Suicide");
                    record.MannerOfDeathType = mannerOfDeath;

                    Dictionary<string, string> detailsOfInjury = new Dictionary<string, string>();
                    record.InjuryLocationName = "Own home garage";
                    record.InjuryDate = new DateTimeOffset(deathUtc.Year, deathUtc.Month, deathUtc.Day, 0, 0, 0, TimeSpan.Zero).ToString("s");
                    record.InjuryLocationDescription = "Inhaled carbon monoxide from auto exhaust through hose in an enclosed garage";

                    Dictionary<string, string> detailsOfInjuryAddr = new Dictionary<string, string>();
                    detailsOfInjuryAddr.Add("addressLine1", residence["addressLine1"]);
                    detailsOfInjuryAddr.Add("addressCity", residencePlace.City);
                    detailsOfInjuryAddr.Add("addressCounty", residencePlace.County);
                    detailsOfInjuryAddr.Add("addressState", stateName);
                    detailsOfInjuryAddr.Add("addressCountry", "United States");
                    record.InjuryLocationAddress = detailsOfInjuryAddr;

                    record.DateOfDeath = new DateTimeOffset(deathUtc.Year, deathUtc.Month, deathUtc.Day, 0, 0, 0, TimeSpan.Zero).ToString("s");
                }
                else if (choice == 1)
                {
                    Tuple<string, string, Dictionary<string, string>>[] causes =
                    {
                        Tuple.Create("Cardiac tamponade", "15 minutes", new Dictionary<string, string>()),
                        Tuple.Create("Perforation of heart", "20 minutes", new Dictionary<string, string>()),
                        Tuple.Create("Gunshot wound to thorax", "20 minutes", new Dictionary<string, string>())
                    };
                    record.CausesOfDeath = causes;

                    record.AutopsyPerformedIndicator = new Dictionary<string, string>() { { "code", "Y" }, { "system", "http://terminology.hl7.org/CodeSystem/v2-0136" }, { "display", "Yes" } };
                    record.AutopsyResultsAvailable = new Dictionary<string, string>() { { "code", "Y" }, { "system", "http://terminology.hl7.org/CodeSystem/v2-0136" }, { "display", "Yes" } };
                    record.ExaminerContacted = true;

                    record.TobaccoUse = new Dictionary<string, string>() { { "code", "373067005" }, { "system", "http://snomed.info/sct" }, { "display", "No" } };

                    Dictionary<string, string> mannerOfDeath = new Dictionary<string, string>();
                    mannerOfDeath.Add("code", "27935005");
                    mannerOfDeath.Add("system", "http://snomed.info/sct");
                    mannerOfDeath.Add("display", "Homicide");
                    record.MannerOfDeathType = mannerOfDeath;

                    Dictionary<string, string> detailsOfInjury = new Dictionary<string, string>();
                    record.InjuryLocationName = "restaurant";
                    record.InjuryDate = deathUtc.AddMinutes(-20).ToString("s");
                    record.InjuryLocationDescription = "Shot by another person using a handgun";

                    Dictionary<string, string> detailsOfInjuryAddr = new Dictionary<string, string>();
                    PlaceCode detailsOfInjuryPlace = dataHelper.StateCodeToRandomPlace(state);
                    detailsOfInjuryAddr.Add("addressLine1", residence["addressLine1"]);
                    detailsOfInjuryAddr.Add("addressCity", detailsOfInjuryPlace.City);
                    detailsOfInjuryAddr.Add("addressCounty", detailsOfInjuryPlace.County);
                    detailsOfInjuryAddr.Add("addressState", stateName);
                    detailsOfInjuryAddr.Add("addressCountry", "United States");
                    record.InjuryLocationAddress = detailsOfInjuryAddr;
                }
                else if (choice == 2)
                {
                    Tuple<string, string, Dictionary<string, string>>[] causes =
                    {
                        Tuple.Create("Cerebral contusion", "minutes", new Dictionary<string, string>()),
                        Tuple.Create("Fractured skull", "minutes", new Dictionary<string, string>()),
                        Tuple.Create("Blunt impact to head", "minutes", new Dictionary<string, string>())
                    };
                    record.CausesOfDeath = causes;

                    record.AutopsyPerformedIndicator = new Dictionary<string, string>() { { "code", "N" }, { "system", "http://terminology.hl7.org/CodeSystem/v2-0136" }, { "display", "No" } };
                    record.AutopsyResultsAvailable = new Dictionary<string, string>() { { "code", "N" }, { "system", "http://terminology.hl7.org/CodeSystem/v2-0136" }, { "display", "No" } };
                    record.ExaminerContacted = true;

                    record.TobaccoUse = new Dictionary<string, string>() { { "code", "373067005" }, { "system", "http://snomed.info/sct" }, { "display", "No" } };

                    Dictionary<string, string> mannerOfDeath = new Dictionary<string, string>();
                    mannerOfDeath.Add("code", "7878000");
                    mannerOfDeath.Add("system", "http://snomed.info/sct");
                    mannerOfDeath.Add("display", "Accidental death");
                    record.MannerOfDeathType = mannerOfDeath;

                    Dictionary<string, string> detailsOfInjury = new Dictionary<string, string>();
                    record.InjuryLocationName = "Highway";
                    record.InjuryDate = deathUtc.ToString("s");
                    record.InjuryLocationDescription = "Automobile accident. Car slid off wet road and struck tree.";

                    Dictionary<string, string> detailsOfInjuryAddr = new Dictionary<string, string>();
                    PlaceCode detailsOfInjuryPlace = dataHelper.StateCodeToRandomPlace(state);
                    detailsOfInjuryAddr.Add("addressLine1", residence["addressLine1"]);
                    detailsOfInjuryAddr.Add("addressCity", detailsOfInjuryPlace.City);
                    detailsOfInjuryAddr.Add("addressCounty", detailsOfInjuryPlace.County);
                    detailsOfInjuryAddr.Add("addressState", stateName);
                    detailsOfInjuryAddr.Add("addressCountry", "United States");
                    record.InjuryLocationAddress = detailsOfInjuryAddr;
                }
            }

            if (gender == Bogus.DataSets.Name.Gender.Female)
            {
                Dictionary<string, string> pregnanacyStatus = new Dictionary<string, string>();
                pregnanacyStatus.Add("code", "PHC1260");
                pregnanacyStatus.Add("system", "urn:oid:2.16.840.1.114222.4.5.274");
                pregnanacyStatus.Add("display", "Not pregnant within the past year");
                record.PregnancyStatus = pregnanacyStatus;
            }
            else
            {
                Dictionary<string, string> pregnanacyStatus = new Dictionary<string, string>();
                pregnanacyStatus.Add("code", "NA");
                pregnanacyStatus.Add("system", "http://hl7.org/fhir/v3/NullFlavor");
                pregnanacyStatus.Add("display", "not applicable");
                record.PregnancyStatus = pregnanacyStatus;
            }

            return record;
        }
    }
}
