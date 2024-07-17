using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Models;
using Services.Interface;

namespace DentalClinic_BE_OData.Controllers
{
    public class MedicalRecordController : ODataController
    {
        private readonly IMedicalRecordService _medicalRecordService;
        public MedicalRecordController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }
        [EnableQuery]
        public async Task<ActionResult<MedicalRecord>> Get(int RecordId)
        {
            var medicalrecord = await _medicalRecordService.ViewMedicalRecord(RecordId);

            if (medicalrecord != null)
            {
                return Ok(medicalrecord);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
