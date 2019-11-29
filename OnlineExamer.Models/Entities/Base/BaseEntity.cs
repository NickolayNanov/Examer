using System;

namespace OnlineExamer.Models.Entities.Base
{
    public abstract class BaseEntity<TKey>
    {
        protected BaseEntity()
        {
            this.CreatedAt = DateTime.Now;
        }

        public TKey Id { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
