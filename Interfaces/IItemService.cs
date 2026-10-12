using System.Collections.Generic;
using WtLab2._2oop.Models;

namespace WtLab2._2oop.Interfaces
{
    public interface IItemService<T> where T : BaseEntity
    {
        void Add(T item);
        bool Remove(int id);
        T GetById(int id);
        List<T> GetAll();
        int Count { get; }
    }
}