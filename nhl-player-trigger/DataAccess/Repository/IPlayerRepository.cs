using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IPlayerRepository
    {
        Task AddUpdatePlayers(List<PlayerValue> playersWithValues);
        Task<int> GetPlayerCountBySeason(int year);
    }
}
