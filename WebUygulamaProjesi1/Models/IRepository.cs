﻿using System.Linq.Expressions;

namespace WebUygulamaProjesi1.Models
{
    public interface IRepository<T> where T : class
    {
        // Generic Interface:  T --> Kitap Türü.
        IEnumerable<T> GetAll(string? includeProps = null); //Tümünü Getir.
        T Get(Expression<Func<T, bool>> filtre, string? includeProps = null);

        void Ekle(T entity);
        void Sil(T entity);
        void SilAralik(IEnumerable<T> entities);
    }
}

