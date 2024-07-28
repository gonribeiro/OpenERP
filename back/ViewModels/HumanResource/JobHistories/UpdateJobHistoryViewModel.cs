using System.ComponentModel.DataAnnotations.Schema;

namespace OpenERP.ViewModels.HumanResource.JobHistories
{
    public class UpdateJobHistoryViewModel
    {
        public required int JobId { get; set; }
        public required int DepartmentId { get; set; }
        [Column(TypeName = "Date")]
        public required DateTime StartDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }
    }
}