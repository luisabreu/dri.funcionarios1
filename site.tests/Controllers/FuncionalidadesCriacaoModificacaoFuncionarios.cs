using System.Collections.Generic;
using System.Web.Mvc;
using Domain.Handlers;
using Domain.Messages;
using Domain.Messages.Comandos;
using Domain.Messages.Handlers;
using FluentAssertions;
using Moq;
using NHibernate;
using site.Controllers;
using site.Models;
using Xbehave;
using FuncionarioDto = Domain.Messages.Relatorios.Funcionario;

namespace site.tests.Controllers {
    public class FuncionalidadesCriacaoModificacaoFuncionarios {
        private readonly AutoMockContainer _container = new AutoMockContainer(new MockRepository(MockBehavior.Strict));

        [Scenario]
        public void Cria_novo_funcionario_com_sucesso(FuncionariosController controlador, int id, int versao, string nome, string nif, int tipoFuncionario, ActionResult resultado) {
            var funcionarioDto = new FuncionarioDto();
            var tipos = new List<TipoFuncionario> {new TipoFuncionario(1, "teste")};

            "Dado um controlador"
                .Given(() => controlador = _container.Create<FuncionariosController>());

            "E um id, versao e tipoFuncionario"
                .And(() => {
                         id = versao = 0;
                         tipoFuncionario = 1;
                     });

            "E um nome"
                .And(() => nome = "Luis");

            "E um NIF"
                .And(() => nif = "123456789");

            "E um conjunto de mocks"
                .And(() => {
                         var query = _container.GetMock<IQueryOver<TipoFuncionario, TipoFuncionario>>();
                         var tran = _container.GetMock<ITransaction>();
                         _container.GetMock<ISession>()
                             .Setup(s => s.BeginTransaction())
                             .Returns(tran.Object);
                         tran.Setup(t => t.Commit());
                         _container.GetMock<ISession>()
                             .Setup(s => s.QueryOver<TipoFuncionario>())
                             .Returns(query.Object);
                         query.Setup(q => q.List<TipoFuncionario>())
                             .Returns(tipos);

                         _container.GetMock<IProcessador>()
                             .Setup(p => p.Trata(It.IsAny<CriaFuncionario>()))
                             .Returns(new MsgGravacao {Id = 1, Versao = 1});

                         _container.GetMock<ISession>()
                             .Setup(s => s.Load<FuncionarioDto>(1))
                             .Returns(funcionarioDto);
                         _container.GetMock<ITransaction>()
                             .Setup(t => t.Dispose());
                     });

            "Quando tentarmos criar um novo funcionário"
                .When(() => resultado = controlador.DadosGerais(id, versao, nome, nif, tipoFuncionario));

            "Então devemos ter uma mensagem com dados esperados"
                .Then(() => {
                          var rv = (ViewResult) resultado;
                          rv.ViewName.Should().Be("Funcionario");
                          var model = (DadosFormularioFuncionario) rv.Model;
                          model.Funcionario.Should().BeSameAs(funcionarioDto);
                          model.Novo.Should().BeFalse();
                          model.TiposFuncionario.Should().BeEquivalentTo(tipos);
                      });
        }

        [Scenario]
        public void Trata_erro_criacao_novo_funcionario(FuncionariosController controlador, int id, int versao, string nome, string nif, int tipoFuncionario, ActionResult resultado) {
            var funcionarioDto = new FuncionarioDto();
            var tipos = new List<TipoFuncionario> {new TipoFuncionario(1, "teste")};

            "Dado um controlador"
                .Given(() => controlador = _container.Create<FuncionariosController>());

            "E um id, versao e tipoFuncionario"
                .And(() => {
                         id = versao = 0;
                         tipoFuncionario = 1;
                     });

            "E um nome"
                .And(() => nome = "Luis");

            "E um NIF incorreto"
                .And(() => nif = "111");

            "E um conjunto de mocks"
                .And(() => {
                         var query = _container.GetMock<IQueryOver<TipoFuncionario, TipoFuncionario>>();
                         var tran = _container.GetMock<ITransaction>();
                         _container.GetMock<ISession>()
                             .Setup(s => s.BeginTransaction())
                             .Returns(tran.Object);
                         tran.Setup(t => t.Commit());
                         _container.GetMock<ISession>()
                             .Setup(s => s.QueryOver<TipoFuncionario>())
                             .Returns(query.Object);
                         query.Setup(q => q.List<TipoFuncionario>())
                             .Returns(tipos);

                         _container.GetMock<ITransaction>()
                             .Setup(t => t.Dispose());
                     });

            "Quando tentarmos criar um novo funcionário"
                .When(() => resultado = controlador.DadosGerais(id, versao, nome, nif, tipoFuncionario));

            "Então devemos ter uma mensagem com dados esperados"
                .Then(() => {
                          var rv = (ViewResult) resultado;
                          rv.ViewName.Should().Be("Funcionario");
                          var model = (DadosFormularioFuncionario) rv.Model;
                          model.Novo.Should().BeTrue();
                          model.TiposFuncionario.Should().BeEquivalentTo(tipos);
                          controlador.ModelState.Should().ContainKey("total");
                      });
        }

        [Scenario]
        public void Modifica_dados_gerais_com_sucesso(FuncionariosController controlador, int id, int versao, string nome, string nif, int tipoFuncionario, ActionResult resultado) {
            var funcionarioDto = new FuncionarioDto();
            var tipos = new List<TipoFuncionario> {new TipoFuncionario(1, "teste")};

            "Dado um controlador"
                .Given(() => controlador = _container.Create<FuncionariosController>());

            "E um id, versao e tipoFuncionario"
                .And(() => { id = versao = tipoFuncionario = 1; });

            "E um nome"
                .And(() => nome = "Luis");

            "E um NIF"
                .And(() => nif = "123456789");

            "E um conjunto de mocks"
                .And(() => {
                         var query = _container.GetMock<IQueryOver<TipoFuncionario, TipoFuncionario>>();
                         var tran = _container.GetMock<ITransaction>();
                         _container.GetMock<ISession>()
                             .Setup(s => s.BeginTransaction())
                             .Returns(tran.Object);
                         tran.Setup(t => t.Commit());
                         _container.GetMock<ISession>()
                             .Setup(s => s.QueryOver<TipoFuncionario>())
                             .Returns(query.Object);
                         query.Setup(q => q.List<TipoFuncionario>())
                             .Returns(tipos);

                         _container.GetMock<IProcessador>()
                             .Setup(p => p.Trata(It.IsAny<ModificaDadosGeraisFuncionario>()))
                             .Returns(new MsgGravacao {Id = 1, Versao = 1});

                         _container.GetMock<ISession>()
                             .Setup(s => s.Load<FuncionarioDto>(1))
                             .Returns(funcionarioDto);
                         _container.GetMock<ITransaction>()
                             .Setup(t => t.Dispose());
                     });

            "Quando tentarmos criar um novo funcionário"
                .When(() => resultado = controlador.DadosGerais(id, versao, nome, nif, tipoFuncionario));

            "Então devemos ter uma mensagem com dados esperados"
                .Then(() => {
                          var rv = (ViewResult) resultado;
                          rv.ViewName.Should().Be("Funcionario");
                          var model = (DadosFormularioFuncionario) rv.Model;
                          model.Funcionario.Should().BeSameAs(funcionarioDto);
                          model.Novo.Should().BeFalse();
                          model.TiposFuncionario.Should().BeEquivalentTo(tipos);
                      });
        }

        [Scenario]
        public void Trata_erro_modificacao_dados_gerais(FuncionariosController controlador, int id, int versao, string nome, string nif, int tipoFuncionario, ActionResult resultado) {
            var funcionarioDto = new FuncionarioDto();
            var tipos = new List<TipoFuncionario> {new TipoFuncionario(1, "teste")};

            "Dado um controlador"
                .Given(() => controlador = _container.Create<FuncionariosController>());

            "E um id, versao e tipoFuncionario"
                .And(() => { id = versao = tipoFuncionario = 1; });

            "E um nome errado"
                .And(() => nome = null);

            "E um NIF"
                .And(() => nif = "123456789");

            "E um conjunto de mocks"
                .And(() => {
                         var query = _container.GetMock<IQueryOver<TipoFuncionario, TipoFuncionario>>();
                         var tran = _container.GetMock<ITransaction>();
                         _container.GetMock<ISession>()
                             .Setup(s => s.BeginTransaction())
                             .Returns(tran.Object);
                         tran.Setup(t => t.Commit());
                         _container.GetMock<ISession>()
                             .Setup(s => s.QueryOver<TipoFuncionario>())
                             .Returns(query.Object);
                         query.Setup(q => q.List<TipoFuncionario>())
                             .Returns(tipos);

                         _container.GetMock<ISession>()
                             .Setup(s => s.Load<FuncionarioDto>(1))
                             .Returns(funcionarioDto);
                         _container.GetMock<ITransaction>()
                             .Setup(t => t.Dispose());
                     });

            "Quando tentarmos criar um novo funcionário"
                .When(() => resultado = controlador.DadosGerais(id, versao, nome, nif, tipoFuncionario));

            "Então devemos ter uma mensagem com dados esperados"
                .Then(() => {
                          var rv = (ViewResult) resultado;
                          rv.ViewName.Should().Be("Funcionario");
                          var modelo = (DadosFormularioFuncionario) rv.Model;
                          modelo.Funcionario.Should().BeSameAs(funcionarioDto);
                          modelo.Novo.Should().BeFalse();
                          modelo.TiposFuncionario.Should().BeEquivalentTo(tipos);
                      });
        }

        [Scenario]
        public void Adiciona_contacto_com_sucesso(FuncionariosController controlador, int id, int versao, string contacto, ActionResult resultado) {
            var funcionarioDto = new FuncionarioDto();
            var tipos = new List<TipoFuncionario> {new TipoFuncionario(1, "teste")};

            "Quando recebemos um controlador"
                .Given(() => controlador = _container.Create<FuncionariosController>());

            "E um id e versão inicializados"
                .And(() => id = versao = 1);

            "E um contacto válido"
                .And(() => contacto = "123456789");

            "E um conjunto de mocks"
                .And(() => {
                         var query = _container.GetMock<IQueryOver<TipoFuncionario, TipoFuncionario>>();
                         var tran = _container.GetMock<ITransaction>();
                         _container.GetMock<ISession>()
                             .Setup(s => s.BeginTransaction())
                             .Returns(tran.Object);
                         tran.Setup(t => t.Commit());
                         _container.GetMock<ISession>()
                             .Setup(s => s.QueryOver<TipoFuncionario>())
                             .Returns(query.Object);
                         query.Setup(q => q.List<TipoFuncionario>())
                             .Returns(tipos);

                         _container.GetMock<IProcessador>()
                             .Setup(p => p.Trata(It.IsAny<ModificaContactosFuncionario>()))
                             .Returns(new MsgGravacao {Id = 1, Versao = 2});

                         _container.GetMock<ISession>()
                             .Setup(s => s.Load<FuncionarioDto>(1))
                             .Returns(funcionarioDto);
                         _container.GetMock<ITransaction>()
                             .Setup(t => t.Dispose());
                     });

            "Quando tentamos adicionar o contacto"
                .When(() => resultado = controlador.AdicionaContacto(id, versao, contacto));

            "Então devemos ser redirecionados para vista funcionários"
                .Then(() => ((ViewResult) resultado).ViewName.ToLower().Should().Be("funcionario"));

            "E devemos obter os valores corretos no modelo"
                .And(() => {
                         var modelo = (DadosFormularioFuncionario) ((ViewResult) resultado).Model;
                         modelo.Funcionario.Should().BeSameAs(funcionarioDto);
                         modelo.Novo.Should().BeFalse();
                         modelo.TiposFuncionario.Should().BeEquivalentTo(tipos);
                     });
        }
        
        [Scenario]
        public void Adiciona_contacto_sem_sucesso(FuncionariosController controlador, int id, int versao, string contacto, ActionResult resultado) {
            var funcionarioDto = new FuncionarioDto();
            var tipos = new List<TipoFuncionario> {new TipoFuncionario(1, "teste")};

            "Quando recebemos um controlador"
                .Given(() => controlador = _container.Create<FuncionariosController>());

            "E um id e versão inicializados"
                .And(() => id = versao = 1);

            "E um contacto inválido"
                .And(() => contacto = "222");

            "E um conjunto de mocks"
                .And(() => {
                         var query = _container.GetMock<IQueryOver<TipoFuncionario, TipoFuncionario>>();
                         var tran = _container.GetMock<ITransaction>();
                         _container.GetMock<ISession>()
                             .Setup(s => s.BeginTransaction())
                             .Returns(tran.Object);
                         tran.Setup(t => t.Commit());
                         _container.GetMock<ISession>()
                             .Setup(s => s.QueryOver<TipoFuncionario>())
                             .Returns(query.Object);
                         query.Setup(q => q.List<TipoFuncionario>())
                             .Returns(tipos);

                         _container.GetMock<IProcessador>()
                             .Setup(p => p.Trata(It.IsAny<ModificaContactosFuncionario>()))
                             .Returns(new MsgGravacao {Id = 1, Versao = 2});

                         _container.GetMock<ISession>()
                             .Setup(s => s.Load<FuncionarioDto>(1))
                             .Returns(funcionarioDto);
                         _container.GetMock<ITransaction>()
                             .Setup(t => t.Dispose());
                     });

            "Quando tentamos adicionar o contacto"
                .When(() => resultado = controlador.AdicionaContacto(id, versao, contacto));

            "Então devemos ser redirecionados para vista funcionários"
                .Then(() => ((ViewResult) resultado).ViewName.ToLower().Should().Be("funcionario"));

            "E devemos obter os valores corretos no modelo"
                .And(() => {
                         var modelo = (DadosFormularioFuncionario) ((ViewResult) resultado).Model;
                         modelo.Funcionario.Should().BeSameAs(funcionarioDto);
                         modelo.Novo.Should().BeFalse();
                         modelo.TiposFuncionario.Should().BeEquivalentTo(tipos);
                     });

            "E ainda devemos ter o erro"
                .And(() => controlador.ModelState.Should().ContainKey("total"));
        }

        [Scenario]
        public void Remove_contacto_com_sucesso(FuncionariosController controlador, int id, int versao, string contacto, ActionResult resultado) {
            var funcionarioDto = new FuncionarioDto();
            var tipos = new List<TipoFuncionario> {new TipoFuncionario(1, "teste")};

            "Quando recebemos um controlador"
                .Given(() => controlador = _container.Create<FuncionariosController>());

            "E um id e versão inicializados"
                .And(() => id = versao = 1);

            "E um contacto válido"
                .And(() => contacto = "123456789");

            "E um conjunto de mocks"
                .And(() => {
                         var query = _container.GetMock<IQueryOver<TipoFuncionario, TipoFuncionario>>();
                         var tran = _container.GetMock<ITransaction>();
                         _container.GetMock<ISession>()
                             .Setup(s => s.BeginTransaction())
                             .Returns(tran.Object);
                         tran.Setup(t => t.Commit());
                         _container.GetMock<ISession>()
                             .Setup(s => s.QueryOver<TipoFuncionario>())
                             .Returns(query.Object);
                         query.Setup(q => q.List<TipoFuncionario>())
                             .Returns(tipos);

                         _container.GetMock<IProcessador>()
                             .Setup(p => p.Trata(It.IsAny<ModificaContactosFuncionario>()))
                             .Returns(new MsgGravacao {Id = 1, Versao = 2});

                         _container.GetMock<ISession>()
                             .Setup(s => s.Load<FuncionarioDto>(1))
                             .Returns(funcionarioDto);
                         _container.GetMock<ITransaction>()
                             .Setup(t => t.Dispose());
                     });

            "Quando tentamos remover o contacto"
                .When(() => resultado = controlador.EliminaContacto(id, versao, contacto));

            "Então devemos ser redirecionados para vista funcionários"
                .Then(() => ((ViewResult) resultado).ViewName.ToLower().Should().Be("funcionario"));

            "E devemos obter os valores corretos no modelo"
                .And(() => {
                         var modelo = (DadosFormularioFuncionario) ((ViewResult) resultado).Model;
                         modelo.Funcionario.Should().BeSameAs(funcionarioDto);
                         modelo.Novo.Should().BeFalse();
                         modelo.TiposFuncionario.Should().BeEquivalentTo(tipos);
                     });
        }
        
        [Scenario]
        public void Remove_contacto_sem_sucesso(FuncionariosController controlador, int id, int versao, string contacto, ActionResult resultado) {
            var funcionarioDto = new FuncionarioDto();
            var tipos = new List<TipoFuncionario> {new TipoFuncionario(1, "teste")};

            "Quando recebemos um controlador"
                .Given(() => controlador = _container.Create<FuncionariosController>());

            "E um id e versão inicializados"
                .And(() => id = versao = 1);

            "E um contacto inválido"
                .And(() => contacto = "333");

            "E um conjunto de mocks"
                .And(() => {
                         var query = _container.GetMock<IQueryOver<TipoFuncionario, TipoFuncionario>>();
                         var tran = _container.GetMock<ITransaction>();
                         _container.GetMock<ISession>()
                             .Setup(s => s.BeginTransaction())
                             .Returns(tran.Object);
                         tran.Setup(t => t.Commit());
                         _container.GetMock<ISession>()
                             .Setup(s => s.QueryOver<TipoFuncionario>())
                             .Returns(query.Object);
                         query.Setup(q => q.List<TipoFuncionario>())
                             .Returns(tipos);

                         _container.GetMock<IProcessador>()
                             .Setup(p => p.Trata(It.IsAny<ModificaContactosFuncionario>()))
                             .Returns(new MsgGravacao {Id = 1, Versao = 2});

                         _container.GetMock<ISession>()
                             .Setup(s => s.Load<FuncionarioDto>(1))
                             .Returns(funcionarioDto);
                         _container.GetMock<ITransaction>()
                             .Setup(t => t.Dispose());
                     });

            "Quando tentamos remover o contacto"
                .When(() => resultado = controlador.EliminaContacto(id, versao, contacto));

            "Então devemos ser redirecionados para vista funcionários"
                .Then(() => ((ViewResult) resultado).ViewName.ToLower().Should().Be("funcionario"));

            "E devemos obter os valores corretos no modelo"
                .And(() => {
                         var modelo = (DadosFormularioFuncionario) ((ViewResult) resultado).Model;
                         modelo.Funcionario.Should().BeSameAs(funcionarioDto);
                         modelo.Novo.Should().BeFalse();
                         modelo.TiposFuncionario.Should().BeEquivalentTo(tipos);
                     });

            "E ainda devemos ter o erro no model state"
                .And(() => controlador.ModelState.Should().ContainKey("total"));
        }
    }
}