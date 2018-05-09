using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenseMining.Entities.FpTree
{
    public class FpTreeUpdateInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Root))]
        public Guid RootId { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public Node Root { get; set; }

        public FpTreeUpdateInfo()
        {
            
        }

        public FpTreeUpdateInfo(Guid rootId, DateTimeOffset creationTime)
        {
            RootId = rootId;
            CreationTime = creationTime;
        }
    }
}
