using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable
{
    public abstract class BaseDocument : BaseEntity, Trackable
    {
        public BaseDocument()
        {
            CreatedAt = DateTimeOffset.Now;
            sys_active = true;
        }

        //[NotMapped]
        public int? TrackKey { get; set; }
        [ForeignKey("TrackKey")]
        public Track InfoTrack { get; set; }

        virtual public bool sys_active { get; set; }

        virtual public bool is_locked { get; set; }

        virtual public string document_status { get; set; }

        virtual public DateTimeOffset CreatedAt { get; set; }

        public int? CheckedoutByKey { get; set; }
        [ForeignKey("CheckedoutByKey")]
        public User CheckedoutBy { get; set; }

        [NotMapped]
        public ICollection<Revision> Revisions { get; set; }

        public string RevisionMessage { get; set; }
    }
}
