namespace EmployeePortal.Core.DTOs
{
    public class SalaryDto
    {
        public string EmployeeEmail { get; set; }
        public string PayrollDate { get; set; }
        public long Basic { get; set; }
        public long HRA { get; set; }
        public long ShiftAllowance { get; set; }
        public long TravelAllowance { get; set; }
        public long MiscellaneousCredit { get; set; }
        public long PT { get; set; }
        public long PF { get; set; }
        public long MiscellaneousDebit { get; set; }
        public long TotalEarning { get; set; }
        public long TotalDeduction { get; set; }
        public long NetSalary { get; set; }
        public string ProcessedBy { get; set; }
        public string Remarks { get; set; }
    }
}
