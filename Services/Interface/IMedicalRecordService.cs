using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.Interface
{
    public interface IMedicalRecordService
    {
        Task<MedicalRecord> ViewMedicalRecord(int id);
    }
}
