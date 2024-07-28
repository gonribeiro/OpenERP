using Microsoft.AspNetCore.Mvc;
using OpenERP.Enums.Global;
using OpenERP.Enums.HumanResource;
using System.ComponentModel;

namespace OpenERP.Controllers.v1
{
    public class EnumController : ControllerBase
    {
        [HttpGet("v1/enums/company-types")]
        public ActionResult<List<string>> GetCompanyTypes()
        {
            var companyTypes = Enum.GetValues(typeof(CompanyType))
                .Cast<CompanyType>()
                .Select(e => new
                {
                    id = e.ToString(),
                    name = GetEnumDescription(e)
                })
                .ToList();

            return Ok(companyTypes);
        }

        [HttpGet("v1/enums/currency-types")]
        public ActionResult<List<string>> GetCurrencyTypes()
        {
            var currencyTypes = Enum.GetValues(typeof(Currency))
                .Cast<Currency>()
                .Select(e => new
                {
                    id = e.ToString(),
                    name = GetEnumDescription(e)
                })
                .ToList();

            return Ok(currencyTypes);
        }

        [HttpGet("v1/enums/maritalStatus-types")]
        public ActionResult<List<string>> GetMaritalStatusTypes()
        {
            var maritalStatusTypes = Enum.GetValues(typeof(MaritalStatus))
                .Cast<MaritalStatus>()
                .Select(e => new
                {
                    id = e.ToString(),
                    name = GetEnumDescription(e)
                })
                .ToList();

            return Ok(maritalStatusTypes);
        }

        [HttpGet("v1/enums/contact-types")]
        public ActionResult<List<string>> GetContactTypes()
        {
            var contactTypes = Enum.GetValues(typeof(ContactType))
                .Cast<ContactType>()
                .Select(e => new
                {
                    id = e.ToString(),
                    name = GetEnumDescription(e)
                })
                .ToList();

            return Ok(contactTypes);
        }

        [HttpGet("v1/enums/contactRelation-types")]
        public ActionResult<List<string>> GetContactRelationTypes()
        {
            var contactRelationTypes = Enum.GetValues(typeof(ContactRelationType))
                .Cast<ContactRelationType>()
                .Select(e => new
                {
                    id = e.ToString(),
                    name = GetEnumDescription(e)
                })
                .ToList();

            return Ok(contactRelationTypes);
        }

        [HttpGet("v1/enums/employment-types")]
        public ActionResult<List<string>> GetEmploymentTypes()
        {
            var employmentTypes = Enum.GetValues(typeof(EmploymentType))
                .Cast<EmploymentType>()
                .Select(e => new
                {
                    id = e.ToString(),
                    name = GetEnumDescription(e)
                })
                .ToList();

            return Ok(employmentTypes);
        }

        [HttpGet("v1/enums/vacation-types")]
        public ActionResult<List<string>> GetVacationTypes()
        {
            var vacationTypes = Enum.GetValues(typeof(VacationType))
                .Cast<VacationType>()
                .Select(e => new
                {
                    id = e.ToString(),
                    name = GetEnumDescription(e)
                })
                .ToList();

            return Ok(vacationTypes);
        }

        private string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
