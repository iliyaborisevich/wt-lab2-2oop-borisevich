using System;

namespace WtLab2._2oop.Interfaces
{
    public interface IStorable
    {
        int Id { get; set; }
        DateTime CreatedAt { get; set; }
        void PrintInfo();
    }
}