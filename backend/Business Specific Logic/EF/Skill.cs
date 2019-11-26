using Reusable;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    [Table("Skill")]
    public class Skill : BaseEntity
    {
        public Skill()
        {
            ///Start:Generated:Constructor<<<
            ///End:Generated:Constructor<<<

        }

        [Key]
        public int SkillKey { get; set; }

        public override int id { get { return SkillKey; } set { SkillKey = value; } }

        ///Start:Generated:Properties<<<
        public string Value { get; set; }
        ///End:Generated:Properties<<<

        public List<Employee> Employees { get; set; }
        public List<JobPosition> JobPositions { get; set; }

    }
}
