using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace masterdetailsCore.Models
{
    public class Skill

    {
        public Skill()
        {
            this.CandidateSkills = new List<CandidateSkill>();
        }
        public int SkillId { get; set; }
        public string SkillName { get; set; } = default!;
        public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } 

    }
    public class Candidate
    {
        public Candidate()
        {
            this.CandidateSkills = new List<CandidateSkill>();
        }
        public int CandidateId { get; set; }
        public string CandidateName { get; set; } = default!;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateofBirth { get; set; }
        public int Phone { get; set; }
        public byte[] Image { get; set; } = default!;

       
        public bool Fresher { get; set; }
        public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } 

    }

    public class CandidateSkill
    {
        public int CandidateSkillId { get; set; }
        [ForeignKey("Skill")]
        public int SkillId { get; set; }
        [ForeignKey("Candidate")]
        public int CandidateId { get; set; }

        //nav
        public virtual Skill Skill { get; set; } = default!;
        public virtual Candidate? Candidate { get; set; } = default!;

    }


    public class JobDbContext : DbContext
    {

        public JobDbContext(DbContextOptions<JobDbContext> options) : base(options)
        {

        }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateSkill> CandidateSkills { get; set; }

    }


}
