using System;
using WtLab2._2oop.Interfaces;

namespace WtLab2._2oop.Models
{
    public abstract class BaseEntity : IStorable
    {
        protected string _entityType;

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        protected BaseEntity()
        {
            CreatedAt = DateTime.Now;
            _entityType = GetType().Name;
        }

        public abstract void PrintInfo();

        public virtual string GetShortInfo()
        {
            return $"[{_entityType}] {Id} — создано {CreatedAt:dd.MM.yyyy HH:mm}";
        }

        protected string GetEntityType()
        {
            return _entityType;
        }
    }
}