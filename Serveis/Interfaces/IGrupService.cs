using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IGrupService
    {
        public Task<String> calculaKarmaBaseAsync(int idAnyEscolar, String idGrup);
    }

}
