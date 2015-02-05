using System;
using System.Reflection;
using Domain.Mapeamentos;
using NHibernate;
using NHibernate.Impl;

namespace Domain.Tests.Mapeamentos {
    public class GestorTransacoesTestes : GestorTransacoes {
        private const string _cnnString = @"Data Source=(LocalDB)\v11.0;AttachDbFileName=|DataDirectory|tests.mdf;Integrated Security=true";

        public override string ObtemCnnString() {
            return _cnnString;
        }

        private static object _locker = new object();
        private static ISessionFactory _sessionFactory;

        public ISessionFactory SessionFactory {
            get {
                if (_sessionFactory == null) {
                    lock (_locker) {
                        if (_sessionFactory == null) {
                            _sessionFactory = this.ObtemFabricaSessoes();
                        }
                    }
                }
                return _sessionFactory;
            }
        }
        public void ExecutaTransacao(Action<ISession, ITransaction> operacao) {
            using (var session = SessionFactory.OpenSession()) {
                using (var tran = session.BeginTransaction()) {
                    operacao(session, tran);
                    tran.Rollback();
                }
            }
        }
    }
}