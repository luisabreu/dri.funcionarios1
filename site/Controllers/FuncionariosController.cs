﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using Domain.Handlers;
using Domain.Messages;
using Domain.Messages.Comandos;
using Domain.Messages.Handlers;
using Domain.Messages.Relatorios;
using Domain.Relatorios;
using NHibernate;
using site.Models;
using FuncionarioDto = Domain.Messages.Relatorios.Funcionario;

namespace site.Controllers {
    public class FuncionariosController : Controller {
        private readonly IGestorRelatorios _gestorRelatorios;
        private readonly IProcessador _processador;
        private readonly ISession _session;

        public FuncionariosController(ISession session, IGestorRelatorios gestorRelatorios, IProcessador processador) {
            Contract.Requires(session != null);
            Contract.Requires(gestorRelatorios != null);
            Contract.Requires(processador != null);
            Contract.Ensures(_session != null);
            Contract.Ensures(_gestorRelatorios != null);
            Contract.Ensures(_processador != null);
            _session = session;
            _gestorRelatorios = gestorRelatorios;
            _processador = processador;
        }

        public ActionResult Index() {
            return View(new DadosPesquisa {NifOuNome = "", Funcionarios = Enumerable.Empty<ResumoFuncionario>(), PesquisaEfetuada = false});
        }

        public ActionResult Pesquisa(string nifOuNome) {
            Contract.Requires(!string.IsNullOrEmpty(nifOuNome), Msg.String_vazia);
            using (var tran = _session.BeginTransaction()) {
                var funcionarios = _gestorRelatorios.PesquisaFuncionarios(nifOuNome);
                return View("Index", new DadosPesquisa {NifOuNome = nifOuNome, Funcionarios = funcionarios, PesquisaEfetuada = true});
            }
        }

        public ActionResult Funcionario(int? id) {
            using (var tran = _session.BeginTransaction()) {
                var tipos = _gestorRelatorios.ObtemTodosTiposFuncionarios();
                var func = id.HasValue ?
                    _gestorRelatorios.ObtemFuncionario(id.Value) :
                    CriaFuncionarioDtoVazio(tipos);


                return View(new DadosFormularioFuncionario {Funcionario = func, TiposFuncionario = tipos, Novo = func == null || Novo(func)});
            }
        }

        private static FuncionarioDto CriaFuncionarioDtoVazio(IEnumerable<TipoFuncionario> tipos) {
            return new Funcionario {
                                       Contactos = new List<Contacto>(),
                                       Nif = "",
                                       Nome = "",
                                       TipoFuncionario = tipos.First()
                                   };
        }

        [HttpPost]
        public ActionResult DadosGerais(int id, int versao, string nome, string nif, int tipoFuncionario) {
            var criarNovoFuncionario = id == 0 && versao == 0;
            IEnumerable<TipoFuncionario> tipos = null;
            FuncionarioDto funcionario = null;
            MsgGravacao msg = null;
            var novo = true;

            using (var tran = _session.BeginTransaction()) {
                try {
                    tipos = _session.QueryOver<TipoFuncionario>().List<TipoFuncionario>();
                    var tipo = tipos.FirstOrDefault(t => t.Id == tipoFuncionario);
                    Contract.Assert(tipo != null, Msg.Tipo_funcionario_inexistente);

                    if (!criarNovoFuncionario) {
                        var comando = new ModificaDadosGeraisFuncionario(id, versao, nome, nif, tipo);
                        msg = _processador.Trata(comando);
                    }
                    else {
                        var comando = new CriaFuncionario(nome, nif, tipo);
                        msg = _processador.Trata(comando);
                        novo = !msg.GravadaComSucesso();
                        id = msg.Id;
                    }

                    tran.Commit();
                }
                catch (Exception ex) {
                    ModelState.AddModelError("total", ex.Message);
                }
            }
            return View("Funcionario", new DadosFormularioFuncionario {
                Funcionario = !criarNovoFuncionario || !novo ? _session.Load<FuncionarioDto>(id) : CriaFuncionarioDtoVazio(tipos),
                                                                          Novo = criarNovoFuncionario && novo,
                                                                          TiposFuncionario = tipos
                                                                      });
        }


        [HttpPost]
        public ActionResult EliminaContacto(int id, int versao, string contacto) {
            IEnumerable<TipoFuncionario> tipos = null;
            MsgGravacao msg = null;
            using (var tran = _session.BeginTransaction()) {
                try {
                    tipos = _session.QueryOver<TipoFuncionario>().List<TipoFuncionario>();
                    var ct = Contacto.Parses(contacto);
                    Contract.Assert(ct != null, Msg.Contacto_invalido);
                    var cmd = new ModificaContactosFuncionario(id, versao, new[] {ct}, null);

                    msg = _processador.Trata(cmd);
                    tran.Commit();
                }
                catch (Exception ex) {
                    ModelState.AddModelError("total", ex.Message);
                }
            }
            return View("Funcionario", new DadosFormularioFuncionario {
                                                                          Funcionario = _session.Load<FuncionarioDto>(id),
                                                                          Novo = false,
                                                                          TiposFuncionario = tipos
                                                                      });
        }

        [HttpPost]
        public ActionResult AdicionaContacto(int id, int versao, string contacto) {
            IEnumerable<TipoFuncionario> tipos = null;
            MsgGravacao msg = null;
            using (var tran = _session.BeginTransaction()) {
                try {
                    tipos = _session.QueryOver<TipoFuncionario>().List<TipoFuncionario>();
                    var ct = Contacto.Parses(contacto);
                    Contract.Assert(ct != null, Msg.Contacto_invalido);
                    var cmd = new ModificaContactosFuncionario(id, versao, null, new[] {ct});

                    msg = _processador.Trata(cmd);
                    tran.Commit();
                }
                catch (Exception ex) {
                    ModelState.AddModelError("total", ex.Message);
                }
            }
            return View("Funcionario", new DadosFormularioFuncionario {
                                                                          Funcionario = _session.Load<FuncionarioDto>(id),
                                                                          Novo = false,
                                                                          TiposFuncionario = tipos
                                                                      });
        }

        private static bool Novo(FuncionarioDto funcionario) {
            return funcionario != null && funcionario.Id == 0 && funcionario.Versao == 0;
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant() {
            Contract.Invariant(_session != null);
            Contract.Invariant(_gestorRelatorios != null);
            Contract.Invariant(_processador != null);
        }
    }
}