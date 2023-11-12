
using System.ComponentModel.DataAnnotations;

namespace masterdetailsCore.Models.ViewModels
{
    public class CandidateVM
    {
        public CandidateVM()
        {
            this.SkillList = new List<int>();
        }
        public int CandidateId { get; set; }
        public string CandidateName { get; set; } = default!;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateofBirth { get; set; }

        //public IFormFile? ImageFile { get; set; } = default!;
        public int Phone { get; set; }
        public IFormFile Image { get; set; } = default!;
        public bool Fresher { get; set; }

        public List<int> SkillList { get; set; } = default!;

    }
}
