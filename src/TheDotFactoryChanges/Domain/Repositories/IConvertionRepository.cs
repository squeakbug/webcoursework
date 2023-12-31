﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;

namespace Domain.Repositories
{
    public interface IConvertionRepository : IDisposable
    {
        Task<IEnumerable<Convertion>> GetConvertions();
        Task<Convertion> GetConvertionById(int id);
        Task<int> Create(Convertion cvt);
        Task Update(Convertion cvt);
        Task Delete(int id);
    }
}
