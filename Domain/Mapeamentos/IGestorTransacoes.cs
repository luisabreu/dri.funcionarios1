using NHibernate;

namespace Domain.Mapeamentos {
    public interface IGestorTransacoes {
        ISessionFactory ObtemFabricaSessoes();
    }
}