using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace ApiControleMedico.Services
{
    public class FuncionarioService
    {
        protected readonly DbContexto<Funcionario> ContextoFuncionarios;
        protected readonly EntidadeNegocio<Funcionario> FuncionarioNegocio = new EntidadeNegocio<Funcionario>();

        public FuncionarioService()
        {
            ContextoFuncionarios = new DbContexto<Funcionario>("funcionario");
        }

        public IEnumerable<Funcionario> GetAll()
        {
            var funcionarios = FuncionarioNegocio.GetAll(ContextoFuncionarios.Collection);
            return funcionarios;
        }

        public Funcionario GetOne(string id)
        {
            return FuncionarioNegocio.GetOne(ContextoFuncionarios.Collection, id);
        }

        public Funcionario SaveOne(Funcionario funcionario)
        {
            FuncionarioNegocio.SaveOne(ContextoFuncionarios.Collection, funcionario);
            if (funcionario.UsuarioId.IsNullOrWhiteSpace())
            {
                var usuario = new UsuarioService().CriarNovoUsuarioFuncionario(funcionario);
                funcionario.UsuarioId = usuario.Id;
            }

            FuncionarioNegocio.SaveOne(ContextoFuncionarios.Collection, funcionario);
            return funcionario;
        }

        public bool RemoveOne(string id)
        {
            var usuarioService = new UsuarioService();
            var func = FuncionarioNegocio.GetOne(ContextoFuncionarios.Collection, id);
            usuarioService.RemoveOne(func.UsuarioId);

            return FuncionarioNegocio.RemoveOne(ContextoFuncionarios.Collection, id);
        }

        public Funcionario BuscarComMedicos(string funcionarioId)
        {
            var funcionario = FuncionarioNegocio.GetOne(ContextoFuncionarios.Collection, funcionarioId);

            if (funcionario.MedicosId.HasItems())
            {
                var medicoService = new MedicoService();
                funcionario.Medicos = new List<Medico>();

                foreach (var medicoId in funcionario.MedicosId)
                    funcionario.Medicos.Add(medicoService.GetOne(medicoId));
            }

            return funcionario;
        }

        public List<Funcionario> BuscarPorOficio(string oficioId)
        {
            return ContextoFuncionarios.Collection.Find(c => c.OficioId == oficioId).ToList();
        }

        public void SalvarFoto(string funcionarioId, string nomeArquivo, string caminhoArquivo)
        {
            var gridFs = new GridFSBucket(ContextoFuncionarios.Database);
            string idFoto = "";

            var funcionario = ContextoFuncionarios.Collection.Find(c => c.Id == funcionarioId).FirstOrDefault();
            if (funcionario != null)
            {
                if (!string.IsNullOrEmpty(funcionario.FotoId))
                    gridFs.DeleteAsync(new ObjectId(funcionario.FotoId));

                using (var foto = File.OpenRead(caminhoArquivo))
                {
                    var task = Task.Run(() =>
                    {
                        return gridFs.UploadFromStreamAsync(nomeArquivo, foto);
                    });
                    idFoto = task.Result.ToString();
                }

                funcionario.FotoId = idFoto;
                FuncionarioNegocio.SaveOne(ContextoFuncionarios.Collection, funcionario);

            }
        }

        public byte[] DownloadFoto(string funcionarioId)
        {
            var gridFs = new GridFSBucket(ContextoFuncionarios.Database);
            try
            {
                var task = gridFs.DownloadAsBytesByNameAsync($"funcionario-{funcionarioId}.jpeg");
                Task.WaitAll(task);
                var bytes = task.Result;
                return bytes;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}