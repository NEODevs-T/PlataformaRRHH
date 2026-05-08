using Inspecciones.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace Inspecciones.Data{
    public interface IDataPregunta
    {
        Task<List<ImaqPre>> ObtenerLasPreguntasPorMquina(int idMaquina);
    }

    public class DataPregunta : IDataPregunta
    {
        private readonly DbNeoContext _cotext;

        public DataPregunta(DbNeoContext context)
        {
            this._cotext = context;
        }

        public async Task<List<ImaqPre>> ObtenerLasPreguntasPorMquina(int idMaquina)
        {
            return await this._cotext.ImaqPres.Where(mp => mp.IdMaquina == idMaquina).Include(mp => mp.IdPreguntaNavigation).ThenInclude(p => p.IdTipPreNavigation).OrderBy(mp => mp.IdPreguntaNavigation.IdTipPre).AsNoTracking().ToListAsync(); //ThenInclude(p => p.IdTipPreNavigation)
        }
    }

    public interface IDataInspeccion
    {
        Task<bool> InsertarInspeccion(Inspeccion inspeccion,List<InspecDatum> listData);
    }
    public class DataInspeccion : IDataInspeccion
    {
        private readonly DbNeoContext _cotext;

        public DataInspeccion(DbNeoContext context)
        {
            this._cotext = context;
        }
        public async Task<bool> InsertarInspeccion(Inspeccion inspeccion,List<InspecDatum> listData){
            InspecDatum data = new InspecDatum();
            foreach (var item in listData)
            {
                data.IdInspecNavigation = inspeccion;
                data.Idobserv = item.Idobserv;
                data.IdMaqPre = item.IdMaqPreNavigation.IdMaqPre;
                data.Iddata = item.Iddata;
                this._cotext.InspecData.Add(data);
                data = new InspecDatum();
            }
            return await this._cotext.SaveChangesAsync() > 0;
        }
    }

    public interface IDataMaquina
    {
        Task<string> obtenerNombreMaquina(int idMaquina);
    }

    public class DataMaquina : IDataMaquina
    {
        private readonly DbNeoContext _cotext;

        public DataMaquina(DbNeoContext context)
        {
            this._cotext = context;
        }

        public async Task<string> obtenerNombreMaquina(int idMaquina)
        {
            Imaquina? data = await this._cotext.Imaquinas.Where(m => m.IdMaquina == idMaquina).FirstOrDefaultAsync();

            return (data != null) ? data.Mnombre : "";
        }
    }

}